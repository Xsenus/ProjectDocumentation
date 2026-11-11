using Core.NET.Controllers;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ProjectDocumentation.NET.Services
{
    internal static class SchemaXmlDocument
    {
        public const int MaxXmlCharacters = 8 * 1024 * 1024;

        public static string ReadVersion(string xml, string rootName)
        {
            if (string.IsNullOrWhiteSpace(xml) || xml.Length > MaxXmlCharacters)
            {
                return null;
            }

            try
            {
                using (var source = new StringReader(xml))
                using (var reader = XmlReader.Create(source, ReaderSettings()))
                {
                    return reader.MoveToContent() == XmlNodeType.Element && reader.Name == rootName && reader.NamespaceURI.Length == 0
                        ? reader.GetAttribute("SchemaVersion") : null;
                }
            }
            catch (XmlException)
            {
                return null;
            }
        }

        public static string ReadFileVersion(string path, string rootName)
        {
            using (var source = File.OpenRead(path))
            using (var reader = XmlReader.Create(source, ReaderSettings()))
            {
                if (reader.MoveToContent() != XmlNodeType.Element || reader.Name != rootName || reader.NamespaceURI.Length != 0)
                {
                    throw new InvalidOperationException($"Корневой элемент XML должен быть {rootName}.");
                }

                return reader.GetAttribute("SchemaVersion");
            }
        }

        public static XDocument ReadFile(string path, string rootName)
        {
            using (var source = File.OpenRead(path))
            using (var reader = XmlReader.Create(source, ReaderSettings()))
            {
                return RequireRoot(XDocument.Load(reader, LoadOptions.PreserveWhitespace), rootName);
            }
        }

        public static XDocument Parse(string xml, string rootName)
        {
            if (string.IsNullOrWhiteSpace(xml) || xml.Length > MaxXmlCharacters)
            {
                throw new InvalidOperationException("Документ пустой или превышает допустимый размер XML.");
            }

            using (var source = new StringReader(xml))
            using (var reader = XmlReader.Create(source, ReaderSettings()))
            {
                return RequireRoot(XDocument.Load(reader, LoadOptions.PreserveWhitespace), rootName);
            }
        }

        public static Guid? ReadGuid(XDocument document, string attribute)
        {
            return Guid.TryParseExact((string)document.Root.Attribute(attribute), "D", out var guid) ? (Guid?)guid : null;
        }

        public static string WithGuid(XDocument document, string attribute, Guid guid)
        {
            document.Root.SetAttributeValue(attribute, guid.ToString("D"));
            return document.ToString(SaveOptions.DisableFormatting);
        }

        public static async Task<T> SaveAsync<T>(ConclusionSchemaService service, string xml, string name,
            Guid guid, string rootName, string version, string guidAttribute, Func<Session, T> create, T existing) where T : XmlObjXPO
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidOperationException("Укажите наименование документа.");
            }

            var previousName = existing?.Name;
            var previousXml = existing?.GetObj();
            var document = Parse(xml, rootName);
            if ((string)document.Root.Attribute("SchemaVersion") != version)
            {
                throw new InvalidOperationException($"Сохранить в этом редакторе можно только документ версии {version}.");
            }

            if (ReadGuid(document, guidAttribute) != guid)
            {
                xml = WithGuid(document, guidAttribute, guid);
            }

            RequireValid(await service.ValidateAsync(xml), version);
            var session = SessionController.GetSession();
            int oid;
            using (var unit = new UnitOfWork(session.DataLayer))
            {
                var matching = await new XPQuery<XmlObjXPO>(unit).FirstOrDefaultAsync(candidate => candidate.Guid == guid);
                T record;
                if (existing == null)
                {
                    if (matching != null)
                    {
                        throw new InvalidOperationException($"Документ с идентификатором {guid:D} уже существует.");
                    }

                    record = create(unit);
                    record.SetGuid(guid);
                }
                else
                {
                    record = matching as T;
                    if (record == null || record.Oid != existing.Oid || record.Name != previousName || record.GetObj() != previousXml)
                    {
                        throw new InvalidOperationException("Документ был изменён или удалён. Откройте его заново перед сохранением.");
                    }
                }

                record.Name = name.Trim();
                record.SetObj(xml);
                unit.Save(record);
                await unit.CommitChangesAsync();
                oid = record.Oid;
            }

            return session.GetObjectByKey<T>(oid, true);
        }

        public static void RequireValid(ConclusionValidation result, string version)
        {
            if (result.Valid && result.Version == version)
            {
                return;
            }

            var text = new StringBuilder($"XML не прошёл проверку по схеме {version}.");
            if (result.Errors != null)
            {
                foreach (var error in result.Errors.Take(12))
                {
                    text.AppendLine().Append(error.Path).Append(' ').Append(error.Message);
                }

                if (result.Errors.Count > 12)
                {
                    text.AppendLine().Append($"Ещё ошибок: {result.Errors.Count - 12}.");
                }
            }

            throw new InvalidOperationException(text.ToString());
        }

        public static void WriteExport(string path, string xml, string rootName, string schema)
        {
            var document = Parse(xml, rootName);
            var xsi = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
            document.Root.SetAttributeValue(XNamespace.Xmlns + "xsi", xsi.NamespaceName);
            document.Root.SetAttributeValue(xsi + "noNamespaceSchemaLocation", schema + ".xsd");
            foreach (var instruction in document.Nodes().OfType<XProcessingInstruction>()
                .Where(node => node.Target == "xml-stylesheet").ToList())
            {
                instruction.Remove();
            }

            document.Root.AddBeforeSelf(new XProcessingInstruction("xml-stylesheet", $"type=\"text/xsl\" href=\"{schema}.xsl\""));
            document.Declaration = new XDeclaration("1.0", "utf-8", null);
            var settings = new XmlWriterSettings { Encoding = new UTF8Encoding(false), NewLineHandling = NewLineHandling.None };
            var temporary = Path.Combine(Path.GetDirectoryName(path), Guid.NewGuid().ToString("N") + ".tmp");
            try
            {
                using (var stream = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    document.Save(writer);
                }

                if (File.Exists(path))
                {
                    File.Replace(temporary, path, null);
                }
                else
                {
                    File.Move(temporary, path);
                }
            }
            finally
            {
                File.Delete(temporary);
            }
        }

        private static XDocument RequireRoot(XDocument document, string rootName)
        {
            if (document.Root?.Name != XName.Get(rootName))
            {
                throw new InvalidOperationException($"Корневой элемент XML должен быть {rootName}.");
            }

            return document;
        }

        private static XmlReaderSettings ReaderSettings()
        {
            return new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null,
                MaxCharactersInDocument = MaxXmlCharacters
            };
        }
    }
}

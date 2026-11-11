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
    internal static class Conclusion0104Document
    {
        public const string CurrentVersion = "01.04";
        public const string LegacyVersion = "01.03";
        public const int MaxXmlCharacters = 8 * 1024 * 1024;

        public static string ReadVersion(string xml)
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
                    return reader.MoveToContent() == XmlNodeType.Element && reader.Name == "Conclusion" && reader.NamespaceURI.Length == 0
                        ? reader.GetAttribute("SchemaVersion") : null;
                }
            }
            catch (XmlException)
            {
                return null;
            }
        }

        public static XDocument ReadFile(string path)
        {
            using (var source = File.OpenRead(path))
            using (var reader = XmlReader.Create(source, ReaderSettings()))
            {
                return RequireConclusion(XDocument.Load(reader, LoadOptions.PreserveWhitespace));
            }
        }

        public static XDocument Parse(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml) || xml.Length > MaxXmlCharacters)
            {
                throw new InvalidOperationException("Заключение пустое или превышает допустимый размер XML.");
            }

            using (var source = new StringReader(xml))
            using (var reader = XmlReader.Create(source, ReaderSettings()))
            {
                return RequireConclusion(XDocument.Load(reader, LoadOptions.PreserveWhitespace));
            }
        }

        public static Guid? ReadGuid(XDocument document)
        {
            return Guid.TryParseExact((string)document.Root.Attribute("ConclusionGUID"), "D", out var guid) ? (Guid?)guid : null;
        }

        public static string WithGuid(XDocument document, Guid guid)
        {
            document.Root.SetAttributeValue("ConclusionGUID", guid.ToString("D"));
            return document.ToString(SaveOptions.DisableFormatting);
        }

        public static async Task<ConclusionXPO> SaveAsync(ConclusionSchemaService service, string xml, string name,
            Guid guid, ConclusionXPO existing = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidOperationException("Укажите наименование заключения.");
            }

            var previousName = existing?.Name;
            var previousXml = existing?.GetObj();
            var document = Parse(xml);
            if ((string)document.Root.Attribute("SchemaVersion") != CurrentVersion)
            {
                throw new InvalidOperationException("Сохранить в этом редакторе можно только заключение версии 01.04.");
            }

            if (ReadGuid(document) != guid)
            {
                xml = WithGuid(document, guid);
            }

            RequireValid(await service.ValidateAsync(xml), CurrentVersion);
            var session = SessionController.GetSession();
            int oid;
            using (var unit = new UnitOfWork(session.DataLayer))
            {
                var matching = await new XPQuery<XmlObjXPO>(unit).FirstOrDefaultAsync(candidate => candidate.Guid == guid);
                ConclusionXPO record;
                if (existing == null)
                {
                    if (matching != null)
                    {
                        throw new InvalidOperationException($"Документ с идентификатором {guid:D} уже существует.");
                    }

                    record = new ConclusionXPO(unit);
                    record.SetGuid(guid);
                }
                else
                {
                    record = matching as ConclusionXPO;
                    if (record == null || record.Oid != existing.Oid || record.Name != previousName || record.GetObj() != previousXml)
                    {
                        throw new InvalidOperationException("Заключение было изменено или удалено. Откройте его заново перед сохранением.");
                    }
                }

                record.Name = name.Trim();
                record.SetObj(xml);
                unit.Save(record);
                await unit.CommitChangesAsync();
                oid = record.Oid;
            }

            return session.GetObjectByKey<ConclusionXPO>(oid, true);
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

        public static void WriteExport(string path, string xml, string version)
        {
            var document = Parse(xml);
            var schema = version == CurrentVersion ? "conclusion-01-04" : "conclusion-01-03";
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

        private static XDocument RequireConclusion(XDocument document)
        {
            if (document.Root?.Name != XName.Get("Conclusion"))
            {
                throw new InvalidOperationException("Корневой элемент XML должен быть Conclusion.");
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

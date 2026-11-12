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
        public const int MaxXmlBytes = 32 * 1024 * 1024;
        private const int UnableToRemoveReplacedFile = unchecked((int)0x80070497);

        public static string ReadVersion(string xml, string rootName)
        {
            if (string.IsNullOrWhiteSpace(xml))
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

        public static (string RootName, string Version) ReadFileHeader(string path)
        {
            using (var source = File.OpenRead(path))
            using (var reader = XmlReader.Create(source, ReaderSettings()))
            {
                if (reader.MoveToContent() != XmlNodeType.Element || reader.NamespaceURI.Length != 0)
                {
                    throw new InvalidOperationException("Ожидается заключение или пояснительная записка XML без пространства имён.");
                }

                return (reader.LocalName, reader.GetAttribute("SchemaVersion"));
            }
        }

        public static XDocument ReadFile(string path)
        {
            using (var source = File.OpenRead(path))
            using (var reader = XmlReader.Create(source, ReaderSettings()))
            {
                return XDocument.Load(reader, LoadOptions.PreserveWhitespace);
            }
        }

        public static XDocument Parse(string xml, string rootName)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                throw new InvalidOperationException("Документ XML пустой.");
            }

            if (xml.Length > MaxXmlBytes || Encoding.UTF8.GetByteCount(xml) > MaxXmlBytes)
            {
                throw new InvalidOperationException("Размер XML превышает 32 МиБ в UTF-8.");
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
            Guid guid, string rootName, string version, string guidAttribute, Func<Session, T> create, T existing,
            DocumentStatus initialStatus = DocumentStatus.New, bool allowIncomplete = false) where T : XmlObjXPO
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

            if (allowIncomplete)
                initialStatus = DocumentStatus.InProgress;
            else
                RequireValid(await service.ValidateAsync(xml), version);
            return await PersistAsync(xml, name, guid, create, existing, previousName, previousXml, initialStatus);
        }

        internal static async Task<T> PersistAsync<T>(string xml, string name, Guid guid, Func<Session, T> create,
            T existing = null, string previousName = null, string previousXml = null,
            DocumentStatus initialStatus = DocumentStatus.New) where T : XmlObjXPO
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("Укажите наименование документа.");

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
                    record.SetStatus(initialStatus);
                }
                else
                {
                    record = matching as T;
                    if (record == null || record.Oid != existing.Oid || record.Name != previousName || record.GetObj() != previousXml)
                    {
                        throw new InvalidOperationException("Документ был изменён или удалён. Откройте его заново перед сохранением.");
                    }
                }

                record.SetDocument(name?.Trim(), xml);
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

            var text = new StringBuilder(version == null ? "XML не прошёл проверку." : $"XML не прошёл проверку по схеме {version}.");
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
            => WriteExportAsync(path, xml, rootName, schema).GetAwaiter().GetResult();

        private static async Task WriteExportAsync(string path, string xml, string rootName, string schema, bool includeStylesheet = true)
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

            if (includeStylesheet)
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
                    // Win32 error 1175 leaves both filenames intact, so the same atomic replacement can be retried.
                    for (var attempt = 0; ; attempt++)
                    {
                        try
                        {
                            File.Replace(temporary, path, null);
                            break;
                        }
                        catch (IOException error) when (error.HResult == UnableToRemoveReplacedFile && attempt < 4)
                        {
                            await Task.Delay(100 * (attempt + 1)).ConfigureAwait(false);
                        }
                    }
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

        public static async Task WriteExportWithAssetsAsync(ConclusionSchemaService service, string path, string xml, string rootName,
            string schema, bool includeStylesheet = true)
        {
            path = Path.GetFullPath(path);
            var directory = Path.GetDirectoryName(path);
            var names = includeStylesheet ? new[] { schema + ".xsd", schema + ".xsl" } : new[] { schema + ".xsd" };
            var paths = names.Select(name => Path.Combine(directory, name)).ToArray();
            if (paths.Contains(path, StringComparer.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Выберите для XML имя, отличное от имени файла схемы или стиля.");
            }

            var assets = await Task.WhenAll(names.Select(service.GetSchemaAssetAsync));
            for (var index = 0; index < paths.Length; index++)
            {
                CheckExistingAsset(paths[index], assets[index]);
            }

            for (var index = 0; index < paths.Length; index++)
            {
                await WriteMissingAssetAsync(paths[index], assets[index]);
            }

            await WriteExportAsync(path, xml, rootName, schema, includeStylesheet);
        }

        private static void CheckExistingAsset(string path, byte[] expected)
        {
            var info = new FileInfo(path);
            if (info.Exists && (info.Length != expected.Length || !File.ReadAllBytes(path).SequenceEqual(expected)))
            {
                throw new InvalidOperationException($"В папке уже есть файл «{info.Name}», который отличается от комплектного. Выберите другую папку для выгрузки.");
            }
        }

        private static async Task WriteMissingAssetAsync(string path, byte[] asset)
        {
            if (File.Exists(path))
            {
                CheckExistingAsset(path, asset);
                return;
            }

            var temporary = Path.Combine(Path.GetDirectoryName(path), Guid.NewGuid().ToString("N") + ".tmp");
            try
            {
                using (var stream = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    await stream.WriteAsync(asset, 0, asset.Length);
                }

                try
                {
                    File.Move(temporary, path);
                }
                catch (IOException) when (File.Exists(path))
                {
                    CheckExistingAsset(path, asset);
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
                MaxCharactersInDocument = MaxXmlBytes
            };
        }
    }
}

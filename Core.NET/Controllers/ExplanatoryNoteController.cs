using Core.NET.AutoModels.ExplanatoryNoteObj;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Core.NET.Controllers
{
    public static class ExplanatoryNoteController
    {
        public static async Task<ExplanatoryNoteXPO> CreateExplanatoryNoteByPath(string path, string name = default)
        {
            return (await ImportExplanatoryNoteAsync(path, name)).Document as ExplanatoryNoteXPO;
        }

        /// <summary>
        /// Импорт пояснительной записки из файла. GUID берётся из имени файла, иначе создаётся новый.
        /// Если документ с таким GUID уже есть, решение принимает <paramref name="resolveConflict"/>
        /// (по умолчанию — копия с новым GUID).
        /// </summary>
        public static async Task<ImportResult> ImportExplanatoryNoteAsync(string path, string name = default, Func<XmlObjXPO, ImportConflictAction> resolveConflict = null)
        {
            var result = new ImportResult();
            var explanatoryNote = GetExplanatoryNoteByPath(path, result.Warnings);
            if (explanatoryNote is null)
            {
                return result;
            }

            return await ImportController.SaveAsync(result, name, GetGuidByFile(path), resolveConflict,
                session => new ExplanatoryNoteXPO(session),
                newGuid => explanatoryNote.SerializeObject());
        }

        private static Guid? GetGuidByFile(string path)
        {
            var fileInfo = new FileInfo(path);
            var splits = fileInfo.Name.Replace(fileInfo.Extension, string.Empty).Split('_');
            foreach (var item in splits)
            {
                if (Guid.TryParse(item, out Guid result))
                {
                    return result;
                }
            }

            return default;
        }

        public static ExplanatoryNote GetExplanatoryNoteByPath(string path, ICollection<string> warnings = null)
        {
            if (FileController.CheckFile(path))
            {
                try
                {
                    using (var source = File.OpenRead(path))
                    using (var reader = XmlReader.Create(source, LegacyReaderSettings()))
                    {
                        return DeserializeLegacyNote(reader, warnings);
                    }
                }
                catch (IOException)
                {
                    return default;
                }
                catch (UnauthorizedAccessException)
                {
                    return default;
                }
            }

            return default;
        }

        public static ExplanatoryNote GetExplanatoryNoteByString(string obj)
        {
            if (string.IsNullOrWhiteSpace(obj))
            {
                return default;
            }

            using (var source = new StringReader(obj))
            using (var reader = XmlReader.Create(source, LegacyReaderSettings()))
            {
                return DeserializeLegacyNote(reader);
            }
        }

        private static ExplanatoryNote DeserializeLegacyNote(XmlReader reader, ICollection<string> warnings = null)
        {
            if (!IsLegacyXml(reader))
            {
                return default;
            }

            try
            {
                var xml = XDocument.Load(reader, LoadOptions.PreserveWhitespace).ToString(SaveOptions.DisableFormatting);
                return Utf8StringSerialize.DeSerializeObjectByString<ExplanatoryNote>(xml, warnings: warnings);
            }
            catch (XmlException)
            {
                return default;
            }
        }

        /// <summary>Only send legacy XML to the 01.03 serializer, which ignores elements of newer schemas.</summary>
        public static bool IsLegacyXml(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                return false;
            }

            using (var source = new StringReader(xml))
            using (var reader = XmlReader.Create(source, LegacyReaderSettings()))
            {
                return IsLegacyXml(reader);
            }
        }

        private static bool IsLegacyXml(XmlReader reader)
        {
            try
            {
                if (reader.MoveToContent() != XmlNodeType.Element || reader.Name != "ExplanatoryNote" || reader.NamespaceURI.Length != 0)
                {
                    return false;
                }

                var version = reader.GetAttribute("SchemaVersion");
                var versionless = string.IsNullOrWhiteSpace(version);
                return (versionless || version == "01.03")
                    && IsLegacyHint(reader.GetAttribute("SchemaLink"), versionless)
                    && IsLegacyHint(reader.GetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance"), versionless);
            }
            catch (XmlException)
            {
                return false;
            }
        }

        private static bool IsLegacyHint(string hint, bool versionless)
        {
            if (string.IsNullOrWhiteSpace(hint))
            {
                return true;
            }

            var parts = hint.Trim().Split('?', '#')[0].Split('/', '\\');
            var filename = parts[parts.Length - 1];
            return versionless
                ? filename.Equals("explanatorynote-01-03.xsd", StringComparison.OrdinalIgnoreCase)
                    || filename.Equals("explanatorynote-wire-01-03.xsd", StringComparison.OrdinalIgnoreCase)
                : !Regex.IsMatch(filename, @"\b0?1[._-]0?[4-9]\b");
        }

        private static XmlReaderSettings LegacyReaderSettings() => new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Prohibit,
            XmlResolver = null
        };

        public static string GetSerializeExplanatoryNote(this ExplanatoryNote explanatoryNote)
        {
            return Utf8StringSerialize.SerializeObject(explanatoryNote);
        }
    }
}

using Core.NET.Controllers;
using Core.NET.Models.XPO;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjectDocumentation.NET.Services
{
    internal static class ExplanatoryNote0107Document
    {
        public const string Family = "explanatorynote";
        public const string CurrentVersion = "01.07";
        public const string CompatibleVersion = "01.05";
        public const string LegacyVersion = "01.03";
        internal const string LegacySchemaAsset = "explanatorynote-wire-01-03";
        internal const string RootName = "ExplanatoryNote";
        private const string GuidAttribute = "id";

        public static string ReadVersion(string xml) => SchemaXmlDocument.ReadVersion(xml, RootName);

        public static XDocument Parse(string xml) => SchemaXmlDocument.Parse(xml, RootName);

        public static Guid? ReadGuid(XDocument document) => SchemaXmlDocument.ReadGuid(document, GuidAttribute);

        public static Guid? ReadFileGuid(string path) => Path.GetFileNameWithoutExtension(path).Split('_')
            .Select(part => Guid.TryParse(part, out var guid) ? (Guid?)guid : null).FirstOrDefault(value => value != null);

        public static string WithGuid(XDocument document, Guid guid) => SchemaXmlDocument.WithGuid(document, GuidAttribute, guid);

        public static Task<ExplanatoryNoteXPO> SaveAsync(ConclusionSchemaService service, string xml, string name,
            Guid guid, ExplanatoryNoteXPO existing = null, string version = CurrentVersion)
        {
            if (version != CurrentVersion && version != CompatibleVersion)
                throw new ArgumentOutOfRangeException(nameof(version));
            return SchemaXmlDocument.SaveAsync(service, xml, name, guid, RootName, version, GuidAttribute,
                session => new ExplanatoryNoteXPO(session), existing);
        }

        public static string WithMigrationVersion(string xml)
        {
            if (string.IsNullOrWhiteSpace(ReadVersion(xml)) && ExplanatoryNoteController.IsLegacyXml(xml))
            {
                var document = Parse(xml);
                document.Root.SetAttributeValue("SchemaVersion", LegacyVersion);
                return document.ToString(SaveOptions.DisableFormatting);
            }

            return xml;
        }
    }
}

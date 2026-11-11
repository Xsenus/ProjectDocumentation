using Core.NET.Models.XPO;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjectDocumentation.NET.Services
{
    internal static class Conclusion0104Document
    {
        public const string CurrentVersion = "01.04";
        public const string LegacyVersion = "01.03";
        internal const string RootName = "Conclusion";
        private const string GuidAttribute = "ConclusionGUID";

        public static bool IsHistoricalVersion(string version) => version == "01.00" || version == "01.02";

        public static bool CanMigrate(string version) => IsHistoricalVersion(version) || version == LegacyVersion;

        public static bool CanUseLegacyEditor(string version) => version == null || version == LegacyVersion;

        public static string ReadVersion(string xml) => SchemaXmlDocument.ReadVersion(xml, RootName);

        public static XDocument Parse(string xml) => SchemaXmlDocument.Parse(xml, RootName);

        public static Guid? ReadGuid(XDocument document) => SchemaXmlDocument.ReadGuid(document, GuidAttribute);

        public static string WithGuid(XDocument document, Guid guid) => SchemaXmlDocument.WithGuid(document, GuidAttribute, guid);

        public static Task<ConclusionXPO> SaveAsync(ConclusionSchemaService service, string xml, string name, Guid guid, ConclusionXPO existing = null)
            => SchemaXmlDocument.SaveAsync(service, xml, name, guid, RootName, CurrentVersion, GuidAttribute, session => new ConclusionXPO(session), existing);

        public static void RequireValid(ConclusionValidation result, string version) => SchemaXmlDocument.RequireValid(result, version);

        public static void WriteExport(string path, string xml, string version)
            => SchemaXmlDocument.WriteExport(path, xml, RootName, "conclusion-" + version.Replace('.', '-'));
    }
}

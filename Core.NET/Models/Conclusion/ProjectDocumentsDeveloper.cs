using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "ProjectDocumentsDeveloper")]
    public class ProjectDocumentsDeveloper
    {
        [XmlElement(ElementName = "IP")]
        public IP IP;
    }
}
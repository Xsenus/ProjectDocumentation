using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "ProjectDocumentsTechnicalCustomer")]
    public class ProjectDocumentsTechnicalCustomer
    {

        [XmlElement(ElementName = "Organization")]
        public Organization Organization;
    }
}
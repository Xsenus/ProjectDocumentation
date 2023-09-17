using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "EngineeringSurveyTechnicalCustomer")]
    public class EngineeringSurveyTechnicalCustomer
    {
        [XmlElement(ElementName = "Organization")]
        public Organization Organization;
    }
}
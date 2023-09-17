using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "EngineeringSurveyAddress")]
    public class EngineeringSurveyAddress
    {
        [XmlElement(ElementName = "EngineeringSurveyRegion")]
        public string EngineeringSurveyRegion;

        [XmlElement(ElementName = "EngineeringSurveyDistrict")]
        public string EngineeringSurveyDistrict;
    }
}
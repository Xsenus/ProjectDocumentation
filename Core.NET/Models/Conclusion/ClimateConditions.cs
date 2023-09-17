using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "ClimateConditions")]
    public class ClimateConditions
    {

        [XmlElement(ElementName = "ClimateDistrict")]
        public string ClimateDistrict;

        [XmlElement(ElementName = "GeologicalConditions")]
        public string GeologicalConditions;

        [XmlElement(ElementName = "WindDistrict")]
        public string WindDistrict;

        [XmlElement(ElementName = "SnowDistrict")]
        public string SnowDistrict;

        [XmlElement(ElementName = "SeismicActivity")]
        public string SeismicActivity;
    }
}
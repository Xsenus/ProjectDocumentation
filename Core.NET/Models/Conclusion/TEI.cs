using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "TEI")]
    public class TEI
    {
        [XmlElement(ElementName = "Name")]
        public string Name;

        [XmlElement(ElementName = "Measure")]
        public string Measure;

        [XmlElement(ElementName = "Value")]
        public string Value;
    }
}
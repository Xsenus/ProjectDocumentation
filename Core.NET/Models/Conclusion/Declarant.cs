using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "Declarant")]
    public class Declarant
    {
        [XmlElement(ElementName = "Organization")]
        public Organization Organization;

        [XmlElement(ElementName = "IP")]
        public IP IP;
    }
}
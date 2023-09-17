using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "Designer")]
    public class Designer
    {
        [XmlElement(ElementName = "Organization")]
        public Organization Organization;

        [XmlAttribute(AttributeName = "General")]
        public string General;

        [XmlText]
        public string Text;
    }
}
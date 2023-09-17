using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "Number")]
    public class Number
    {
        [XmlElement(ElementName = "EGRZ")]
        public string EGRZ;
    }
}
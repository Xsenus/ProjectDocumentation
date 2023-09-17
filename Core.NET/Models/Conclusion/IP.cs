using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "IP")]
    public class IP
    {
        [XmlElement(ElementName = "FamilyName")]
        public string FamilyName;

        [XmlElement(ElementName = "FirstName")]
        public string FirstName;

        [XmlElement(ElementName = "SecondName")]
        public string SecondName;

        [XmlElement(ElementName = "OGRNIP")]
        public string OGRNIP;

        [XmlElement(ElementName = "PostAddress")]
        public PostAddress PostAddress;
    }
}
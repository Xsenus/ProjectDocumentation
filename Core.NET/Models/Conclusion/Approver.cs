using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "Approver")]
    public class Approver
    {
        [XmlElement(ElementName = "FamilyName")]
        public string FamilyName;

        [XmlElement(ElementName = "FirstName")]
        public string FirstName;

        [XmlElement(ElementName = "SecondName")]
        public string SecondName;

        [XmlElement(ElementName = "Position")]
        public string Position;
    }
}
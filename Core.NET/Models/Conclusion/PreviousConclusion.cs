using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "PreviousConclusion")]
    public class PreviousConclusion
    {
        [XmlElement(ElementName = "Date")]
        public string Date;

        [XmlElement(ElementName = "Number")]
        public Number Number;

        [XmlElement(ElementName = "ExaminationObjectType")]
        public string ExaminationObjectType;

        [XmlElement(ElementName = "Name")]
        public string Name;

        [XmlElement(ElementName = "Result")]
        public string Result;
    }
}
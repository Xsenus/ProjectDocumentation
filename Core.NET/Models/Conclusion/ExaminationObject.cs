using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "ExaminationObject")]
    public class ExaminationObject
    {
        [XmlElement(ElementName = "ExaminationForm")]
        public string ExaminationForm;

        [XmlElement(ElementName = "ExaminationResult")]
        public string ExaminationResult;

        [XmlElement(ElementName = "ExaminationObjectType")]
        public string ExaminationObjectType;

        [XmlElement(ElementName = "ExaminationType")]
        public string ExaminationType;

        [XmlElement(ElementName = "ConstructionType")]
        public string ConstructionType;

        [XmlElement(ElementName = "ExaminationStage")]
        public string ExaminationStage;

        [XmlElement(ElementName = "Name")]
        public string Name;
    }
}
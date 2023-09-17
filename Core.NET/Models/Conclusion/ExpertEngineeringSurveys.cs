using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "ExpertEngineeringSurveys")]
    public class ExpertEngineeringSurveys
    {
        [XmlElement(ElementName = "EngineeringSurveyConditions")]
        public string EngineeringSurveyConditions;

        [XmlElement(ElementName = "EngineeringSurveyProgramNote")]
        public string EngineeringSurveyProgramNote;

        [XmlElement(ElementName = "EngineeringSurveyMethods")]
        public string EngineeringSurveyMethods;

        [XmlElement(ElementName = "EngineeringSurveyChanges")]
        public string EngineeringSurveyChanges;

        [XmlAttribute(AttributeName = "EngineeringSurveyType")]
        public string EngineeringSurveyType;

        [XmlText]
        public string Text;
    }
}
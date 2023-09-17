using System.Collections.Generic;
using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "Summary")]
    public class Summary
    {
        [XmlElement(ElementName = "EngineeringSurveySummary")]
        public string EngineeringSurveySummary;

        [XmlElement(ElementName = "EngineeringSurveySummaryDate")]
        public string EngineeringSurveySummaryDate;

        [XmlElement(ElementName = "EngineeringSurveyType")]
        public List<string> EngineeringSurveyType;

        [XmlElement(ElementName = "ProjectDocumentsSummary")]
        public string ProjectDocumentsSummary;

        [XmlElement(ElementName = "ProjectDocumentsSummaryDate")]
        public string ProjectDocumentsSummaryDate;

        [XmlElement(ElementName = "ExaminationSummary")]
        public string ExaminationSummary;
    }
}
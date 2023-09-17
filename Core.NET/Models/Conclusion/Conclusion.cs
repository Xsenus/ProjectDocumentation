using System.Collections.Generic;
using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "Conclusion")]
    public class Conclusion
    {
        [XmlElement(ElementName = "ExpertOrganization")]
        public ExpertOrganization ExpertOrganization;

        [XmlElement(ElementName = "Approver")]
        public List<Approver> Approver;

        [XmlElement(ElementName = "ExaminationObject")]
        public ExaminationObject ExaminationObject;

        [XmlElement(ElementName = "Documents")]
        public Documents Documents;

        [XmlElement(ElementName = "PreviousConclusions")]
        public PreviousConclusions PreviousConclusions;

        [XmlElement(ElementName = "Object")]
        public Object Object;

        [XmlElement(ElementName = "CadastralNumber")]
        public string CadastralNumber;

        [XmlElement(ElementName = "Declarant")]
        public List<Declarant> Declarant;

        [XmlElement(ElementName = "ProjectDocumentsTechnicalCustomer")]
        public List<ProjectDocumentsTechnicalCustomer> ProjectDocumentsTechnicalCustomer;

        [XmlElement(ElementName = "ProjectDocumentsDeveloper")]
        public ProjectDocumentsDeveloper ProjectDocumentsDeveloper;

        [XmlElement(ElementName = "Finance")]
        public Finance Finance;

        [XmlElement(ElementName = "ClimateConditions")]
        public ClimateConditions ClimateConditions;

        [XmlElement(ElementName = "ClimateConditionsNote")]
        public string ClimateConditionsNote;

        [XmlElement(ElementName = "Designer")]
        public List<Designer> Designer;

        [XmlElement(ElementName = "ExpertProjectDocuments")]
        public List<ExpertProjectDocuments> ExpertProjectDocuments;

        [XmlElement(ElementName = "EngineeringSurveyAddress")]
        public EngineeringSurveyAddress EngineeringSurveyAddress;

        [XmlElement(ElementName = "EngineeringSurveyTechnicalCustomer")]
        public EngineeringSurveyTechnicalCustomer EngineeringSurveyTechnicalCustomer;

        [XmlElement(ElementName = "ExpertEngineeringSurveys")]
        public List<ExpertEngineeringSurveys> ExpertEngineeringSurveys;

        [XmlElement(ElementName = "Summary")]
        public Summary Summary;

        [XmlElement(ElementName = "Experts")]
        public Experts Experts;

        [XmlAttribute(AttributeName = "ConclusionGUID")]
        public string ConclusionGUID;

        [XmlAttribute(AttributeName = "SchemaVersion")]
        public string SchemaVersion;

        [XmlAttribute(AttributeName = "xsi")]
        public string Xsi;

        [XmlAttribute(AttributeName = "noNamespaceSchemaLocation")]
        public string NoNamespaceSchemaLocation;

        [XmlText]
        public string Text;
    }
}
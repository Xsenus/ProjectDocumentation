using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "ExpertProjectDocuments")]
    public class ExpertProjectDocuments
    {
        [XmlElement(ElementName = "ProjectDocumentsReview")]
        public string ProjectDocumentsReview;

        [XmlElement(ElementName = "ProjectDocumentsChanges")]
        public string ProjectDocumentsChanges;

        [XmlAttribute(AttributeName = "ExpertType")]
        public string ExpertType;

        [XmlText]
        public string Text;
    }
}
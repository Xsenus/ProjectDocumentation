using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "FullDocIssueAuthor")]
    public class FullDocIssueAuthor
    {
        [XmlElement(ElementName = "Organization")]
        public Organization Organization;
    }
}
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "Documents")]
    public class Documents
    {
        [XmlElement(ElementName = "Document")]
        public List<Document> Document;
    }
}
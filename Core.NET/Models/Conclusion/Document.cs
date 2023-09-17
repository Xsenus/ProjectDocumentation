using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "Document")]
    public class Document
    {
        [XmlElement(ElementName = "DocType")]
        public string DocType;

        [XmlElement(ElementName = "DocName")]
        public string DocName;

        [XmlElement(ElementName = "DocNumber")]
        public string DocNumber;

        [XmlElement(ElementName = "DocDate")]
        public string DocDate;

        [XmlElement(ElementName = "DocIssueAuthor")]
        public string DocIssueAuthor;

        [XmlElement(ElementName = "File")]
        public List<File> File;

        [XmlElement(ElementName = "FullDocIssueAuthor")]
        public FullDocIssueAuthor FullDocIssueAuthor;
    }
}
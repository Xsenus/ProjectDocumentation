using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "SignFile")]
    public class SignFile
    {
        [XmlElement(ElementName = "FileName")]
        public string FileName;

        [XmlElement(ElementName = "FileFormat")]
        public string FileFormat;

        [XmlElement(ElementName = "FileChecksum")]
        public string FileChecksum;
    }
}
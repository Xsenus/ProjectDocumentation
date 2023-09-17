using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "File")]
    public class File
    {
        [XmlElement(ElementName = "FileName")]
        public string FileName;

        [XmlElement(ElementName = "FileFormat")]
        public string FileFormat;

        [XmlElement(ElementName = "FileChecksum")]
        public string FileChecksum;

        [XmlElement(ElementName = "SignFile")]
        public SignFile SignFile;
    }
}
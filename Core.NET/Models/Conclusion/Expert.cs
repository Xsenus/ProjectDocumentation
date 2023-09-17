using System;
using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "Expert")]
    public class Expert
    {
        [XmlElement(ElementName = "FamilyName")]
        public string FamilyName;

        [XmlElement(ElementName = "FirstName")]
        public string FirstName;

        [XmlElement(ElementName = "SecondName")]
        public string SecondName;

        [XmlElement(ElementName = "ExpertType")]
        public string ExpertType;

        [XmlElement(ElementName = "ExpertCertificate")]
        public string ExpertCertificate;

        [XmlElement(ElementName = "ExpertCertificateBeginDate")]
        public string ExpertCertificateBeginDate;

        [XmlElement(ElementName = "ExpertCertificateEndDate")]
        public string ExpertCertificateEndDate;
    }
}
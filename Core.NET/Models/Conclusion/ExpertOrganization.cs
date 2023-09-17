using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "ExpertOrganization")]
    public class ExpertOrganization
    {
        [XmlElement(ElementName = "OrgFullName")]
        public string OrgFullName;

        [XmlElement(ElementName = "OrgOGRN")]
        public string OrgOGRN;

        [XmlElement(ElementName = "OrgINN")]
        public string OrgINN;

        [XmlElement(ElementName = "OrgKPP")]
        public string OrgKPP;

        [XmlElement(ElementName = "Address")]
        public Address Address;
    }
}
using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "Finance")]
    public class Finance
    {
        [XmlElement(ElementName = "FinanceType")]
        public string FinanceType;

        [XmlElement(ElementName = "FinanceSize")]
        public string FinanceSize;
    }
}
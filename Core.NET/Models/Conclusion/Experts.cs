using System.Collections.Generic;
using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "Experts")]
    public class Experts
    {
        [XmlElement(ElementName = "Expert")]
        public List<Expert> Expert;
    }
}
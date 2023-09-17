using System.Collections.Generic;
using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "PreviousConclusions")]
    public class PreviousConclusions
    {
        [XmlElement(ElementName = "PreviousConclusion")]
        public List<PreviousConclusion> PreviousConclusion;
    }
}
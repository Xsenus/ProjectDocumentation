using System.Collections.Generic;
using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "Object")]
    public class Object
    {
        [XmlElement(ElementName = "Name")]
        public string Name;

        [XmlElement(ElementName = "Address")]
        public Address Address;

        [XmlElement(ElementName = "Type")]
        public string Type;

        [XmlElement(ElementName = "Functions")]
        public string Functions; 
        
        [XmlElement(ElementName = "TEI")]
        public List<TEI> TEI;
    }
}
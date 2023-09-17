using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "PostAddress")]
    public class PostAddress
    {
        [XmlElement(ElementName = "Country")]
        public string Country;

        [XmlElement(ElementName = "Region")]
        public string Region;

        [XmlElement(ElementName = "PostIndex")]
        public string PostIndex;

        [XmlElement(ElementName = "City")]
        public string City;

        [XmlElement(ElementName = "Street")]
        public string Street;

        [XmlElement(ElementName = "Building")]
        public string Building;

        [XmlElement(ElementName = "Room")]
        public string Room;
    }
}
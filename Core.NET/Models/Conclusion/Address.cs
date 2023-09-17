using System.Xml.Serialization;

namespace Core.NET.Models.Conclusion
{
    [XmlRoot(ElementName = "Address")]
    public class Address
    {
        [XmlElement(ElementName = "Region")]
        public string Region;

        [XmlElement(ElementName = "City")]
        public string City;

        [XmlElement(ElementName = "Street")]
        public string Street;

        [XmlElement(ElementName = "Building")]
        public string Building;

        [XmlElement(ElementName = "Room")]
        public string Room;

        [XmlElement(ElementName = "Country")]
        public string Country;

        [XmlElement(ElementName = "District")]
        public string District;
    }
}
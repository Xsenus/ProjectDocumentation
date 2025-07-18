using System;
using System.Xml.Serialization;

namespace Core.NET.AutoModels
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    public class tEcologyExpertise
    {
        [XmlElement("Required")]
        public tYesNo Required { get; set; }

        [XmlElement("Note")]
        public string Note { get; set; }
    }
}
// Ignore Spelling: Cadastral Declarant Org OGRNIP TEI SNILS Constraction

using Core.NET.Extensions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Core.NET.AutoModels
{
    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tPostAddress
    {
        [XmlElement(DataType = "normalizedString")]
        public string Country { get; set; }

        public tRegionsRF Region { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string PostIndex { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string District { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string City { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string Settlement { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string Street { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string Building { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string Room { get; set; }

        public string Note { get; set; }

        public override string ToString()
        {
            var result = default(string);

            if (string.IsNullOrWhiteSpace(Note))
            {
                result += CheckObjValue(PostIndex);
                result += CheckObjValue(Country);
                result += CheckObjValue(Region.GetEnumXmlAttributeValue()?.ToString());
                result += CheckObjValue(District);
                result += CheckObjValue(City);
                result += CheckObjValue(Settlement);
                result += CheckObjValue(Street);
                result += CheckObjValue(Building);
                result += CheckObjValue(Room);
                result = result?.Replace("  ", " ")?.Trim();
            }
            else
            {
                result = Note;
            }

            return result ?? "Не задан";
        }

        private string CheckObjValue(string obj)
        {
            if (!string.IsNullOrWhiteSpace(Country))
            {
                return $"{obj} ";
            }

            return default;
        }
    }
}
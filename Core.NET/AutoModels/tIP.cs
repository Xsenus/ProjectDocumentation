// Ignore Spelling: Cadastral Declarant Org OGRNIP TEI SNILS Constraction

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
    public partial class tIP
    {
        [XmlElement(DataType = "normalizedString")]
        public string FamilyName { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string FirstName { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string SecondName { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string OGRNIP { get; set; }

        public tPostAddress PostAddress { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string Email { get; set; }

        public override string ToString()
        {
            return OGRNIP ?? base.ToString();
        }
    }
}
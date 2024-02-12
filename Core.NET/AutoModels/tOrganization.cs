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
    public partial class tOrganization
    {
        [XmlElement(DataType = "normalizedString")]
        public string OrgFullName { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string OrgOGRN { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string OrgINN { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string OrgKPP { get; set; }

        public tAddress Address { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string Email { get; set; }

        public override string ToString()
        {
            return OrgINN ?? base.ToString();
        }
    }
}
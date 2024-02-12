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
    public partial class tPerson
    {
        [XmlElement(DataType = "normalizedString")]
        public string FamilyName { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string FirstName { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string SecondName { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string SNILS { get; set; }

        public tPostAddress PostAddress { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string Email { get; set; }

        [XmlIgnore()]
        public string FullName
        {
            get
            {
                var result = default(string);

                if (!string.IsNullOrWhiteSpace(FamilyName))
                {
                    result += FamilyName;

                    if (!string.IsNullOrWhiteSpace(FirstName))
                    {
                        result += $" {FirstName.Substring(0, 1).ToUpper()}.";
                    }

                    if (!string.IsNullOrWhiteSpace(SecondName))
                    {
                        result += $" {SecondName.Substring(0, 1).ToUpper()}.";
                    }
                }

                return result;
            }
        }

        public override string ToString()
        {
            return $"{FullName} ({SNILS})" ?? base.ToString();
        }
    }
}
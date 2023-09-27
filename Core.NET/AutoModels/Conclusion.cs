using Core.NET.Extensions;
using System.ComponentModel;

namespace Core.NET.AutoModels
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Conclusion
    {
        private tOrganization expertOrganizationField;

        private tWorkPerson approverField;

        private tExaminationObject examinationObjectField;

        private tDocument[] documentsField;

        private tPreviousConclusion[] previousConclusionsField;

        private tPreviousSimpleConclusion[] previousSimpleConclusionsField;

        private tObject objectField;

        private string[] cadastralNumberField;

        private tDeclarant declarantField;

        private object[] itemsField;

        private tFinance[] financeField;

        private string financeCommentField;

        private tEstimatedCost estimatedCostField;

        private tClimateConditions climateConditionsField;

        private string climateConditionsNoteField;

        private tDesigner[] designerField;

        private tEEPDUse[] eEPDUseField;

        private tEngineeringSurveyAddress[] engineeringSurveyAddressField;

        private object[] items1Field;

        private tExpertEngineeringSurveys[] expertEngineeringSurveysField;

        private tExpertProjectDocuments[] expertProjectDocumentsField;

        private tExpertEstimate expertEstimateField;

        private tSummary summaryField;

        private tExpert[] expertsField;

        private string conclusionGUIDField;

        private string schemaVersionField;

        private string schemaLinkField;

        public Conclusion()
        {
            this.schemaVersionField = "01.02";
        }

        /// <remarks/>
        public tOrganization ExpertOrganization
        {
            get
            {
                return this.expertOrganizationField;
            }
            set
            {
                this.expertOrganizationField = value;
            }
        }

        /// <remarks/>
        public tWorkPerson Approver
        {
            get
            {
                return this.approverField;
            }
            set
            {
                this.approverField = value;
            }
        }

        /// <remarks/>
        public tExaminationObject ExaminationObject
        {
            get
            {
                return this.examinationObjectField;
            }
            set
            {
                this.examinationObjectField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Document", IsNullable = false)]
        public tDocument[] Documents
        {
            get
            {
                return this.documentsField;
            }
            set
            {
                this.documentsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("PreviousConclusion", IsNullable = false)]
        public tPreviousConclusion[] PreviousConclusions
        {
            get
            {
                return this.previousConclusionsField;
            }
            set
            {
                this.previousConclusionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("PreviousSimpleConclusion", IsNullable = false)]
        public tPreviousSimpleConclusion[] PreviousSimpleConclusions
        {
            get
            {
                return this.previousSimpleConclusionsField;
            }
            set
            {
                this.previousSimpleConclusionsField = value;
            }
        }

        /// <remarks/>
        public tObject Object
        {
            get
            {
                return this.objectField;
            }
            set
            {
                this.objectField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CadastralNumber")]
        public string[] CadastralNumber
        {
            get
            {
                return this.cadastralNumberField;
            }
            set
            {
                this.cadastralNumberField = value;
            }
        }

        /// <remarks/>
        public tDeclarant Declarant
        {
            get
            {
                return this.declarantField;
            }
            set
            {
                this.declarantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ProjectDocumentsDeveloper", typeof(tDeclarant))]
        [System.Xml.Serialization.XmlElementAttribute("ProjectDocumentsTechnicalCustomer", typeof(tTechnicalCustomer))]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Finance")]
        public tFinance[] Finance
        {
            get
            {
                return this.financeField;
            }
            set
            {
                this.financeField = value;
            }
        }

        /// <remarks/>
        public string FinanceComment
        {
            get
            {
                return this.financeCommentField;
            }
            set
            {
                this.financeCommentField = value;
            }
        }

        /// <remarks/>
        public tEstimatedCost EstimatedCost
        {
            get
            {
                return this.estimatedCostField;
            }
            set
            {
                this.estimatedCostField = value;
            }
        }

        /// <remarks/>
        public tClimateConditions ClimateConditions
        {
            get
            {
                return this.climateConditionsField;
            }
            set
            {
                this.climateConditionsField = value;
            }
        }

        /// <remarks/>
        public string ClimateConditionsNote
        {
            get
            {
                return this.climateConditionsNoteField;
            }
            set
            {
                this.climateConditionsNoteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Designer")]
        public tDesigner[] Designer
        {
            get
            {
                return this.designerField;
            }
            set
            {
                this.designerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("EEPDUse")]
        public tEEPDUse[] EEPDUse
        {
            get
            {
                return this.eEPDUseField;
            }
            set
            {
                this.eEPDUseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("EngineeringSurveyAddress")]
        public tEngineeringSurveyAddress[] EngineeringSurveyAddress
        {
            get
            {
                return this.engineeringSurveyAddressField;
            }
            set
            {
                this.engineeringSurveyAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("EngineeringSurveyDeveloper", typeof(tDeclarant))]
        [System.Xml.Serialization.XmlElementAttribute("EngineeringSurveyTechnicalCustomer", typeof(tTechnicalCustomer))]
        public object[] Items1
        {
            get
            {
                return this.items1Field;
            }
            set
            {
                this.items1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ExpertEngineeringSurveys")]
        public tExpertEngineeringSurveys[] ExpertEngineeringSurveys
        {
            get
            {
                return this.expertEngineeringSurveysField;
            }
            set
            {
                this.expertEngineeringSurveysField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ExpertProjectDocuments")]
        public tExpertProjectDocuments[] ExpertProjectDocuments
        {
            get
            {
                return this.expertProjectDocumentsField;
            }
            set
            {
                this.expertProjectDocumentsField = value;
            }
        }

        /// <remarks/>
        public tExpertEstimate ExpertEstimate
        {
            get
            {
                return this.expertEstimateField;
            }
            set
            {
                this.expertEstimateField = value;
            }
        }

        /// <remarks/>
        public tSummary Summary
        {
            get
            {
                return this.summaryField;
            }
            set
            {
                this.summaryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Expert", IsNullable = false)]
        public tExpert[] Experts
        {
            get
            {
                return this.expertsField;
            }
            set
            {
                this.expertsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ConclusionGUID
        {
            get
            {
                return this.conclusionGUIDField;
            }
            set
            {
                this.conclusionGUIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SchemaVersion
        {
            get
            {
                return this.schemaVersionField;
            }
            set
            {
                this.schemaVersionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SchemaLink
        {
            get
            {
                return this.schemaLinkField;
            }
            set
            {
                this.schemaLinkField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tOrganization
    {

        private string orgFullNameField;

        private string orgOGRNField;

        private string orgINNField;

        private string orgKPPField;

        private tAddress addressField;

        private string emailField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string OrgFullName
        {
            get
            {
                return this.orgFullNameField;
            }
            set
            {
                this.orgFullNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string OrgOGRN
        {
            get
            {
                return this.orgOGRNField;
            }
            set
            {
                this.orgOGRNField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string OrgINN
        {
            get
            {
                return this.orgINNField;
            }
            set
            {
                this.orgINNField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string OrgKPP
        {
            get
            {
                return this.orgKPPField;
            }
            set
            {
                this.orgKPPField = value;
            }
        }

        /// <remarks/>
        public tAddress Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tAddress
    {

        private string countryField;

        private tRegionsRF regionField;

        private string districtField;

        private string cityField;

        private string settlementField;

        private string streetField;

        private string buildingField;

        private string roomField;

        private string noteField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        public tRegionsRF Region
        {
            get
            {
                return this.regionField;
            }
            set
            {
                this.regionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string District
        {
            get
            {
                return this.districtField;
            }
            set
            {
                this.districtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Settlement
        {
            get
            {
                return this.settlementField;
            }
            set
            {
                this.settlementField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Street
        {
            get
            {
                return this.streetField;
            }
            set
            {
                this.streetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Building
        {
            get
            {
                return this.buildingField;
            }
            set
            {
                this.buildingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Room
        {
            get
            {
                return this.roomField;
            }
            set
            {
                this.roomField = value;
            }
        }

        /// <remarks/>
        public string Note
        {
            get
            {
                return this.noteField;
            }
            set
            {
                this.noteField = value;
            }
        }

        public override string ToString()
        {
            var result = default(string);

            if (string.IsNullOrWhiteSpace(Note))
            {
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
            if (!string.IsNullOrWhiteSpace(obj))
            {
                return $"{obj} ";
            }

            return default;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    public enum tRegionsRF
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("00")]
        Item00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("01")]
        Item01,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("02")]
        Item02,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("03")]
        Item03,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("04")]
        Item04,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("05")]
        Item05,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("06")]
        Item06,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07")]
        Item07,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08")]
        Item08,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("09")]
        Item09,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("10")]
        Item10,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("11")]
        Item11,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("12")]
        Item12,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("13")]
        Item13,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("14")]
        Item14,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("15")]
        Item15,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("16")]
        Item16,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("17")]
        Item17,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("18")]
        Item18,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("19")]
        Item19,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("20")]
        Item20,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("21")]
        Item21,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("22")]
        Item22,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("23")]
        Item23,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("24")]
        Item24,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("25")]
        Item25,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("26")]
        Item26,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("27")]
        Item27,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("28")]
        Item28,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("29")]
        Item29,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("30")]
        Item30,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("31")]
        Item31,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("32")]
        Item32,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("33")]
        Item33,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("34")]
        Item34,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("35")]
        Item35,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("36")]
        Item36,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("37")]
        Item37,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("38")]
        Item38,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("39")]
        Item39,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("40")]
        Item40,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("41")]
        Item41,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("42")]
        Item42,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("43")]
        Item43,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("44")]
        Item44,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("45")]
        Item45,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("46")]
        Item46,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("47")]
        Item47,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("48")]
        Item48,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("49")]
        Item49,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("50")]
        Item50,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("51")]
        Item51,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("52")]
        Item52,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("53")]
        Item53,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("54")]
        Item54,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("55")]
        Item55,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("56")]
        Item56,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("57")]
        Item57,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("58")]
        Item58,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("59")]
        Item59,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("60")]
        Item60,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("61")]
        Item61,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("62")]
        Item62,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("63")]
        Item63,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("64")]
        Item64,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("65")]
        Item65,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("66")]
        Item66,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("67")]
        Item67,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("68")]
        Item68,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("69")]
        Item69,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("70")]
        Item70,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("71")]
        Item71,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("72")]
        Item72,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("73")]
        Item73,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("74")]
        Item74,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("75")]
        Item75,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("76")]
        Item76,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("77")]
        Item77,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("78")]
        Item78,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("79")]
        Item79,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("83")]
        Item83,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("86")]
        Item86,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("87")]
        Item87,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("89")]
        Item89,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("91")]
        Item91,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("92")]
        Item92,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("80")]
        Item80,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("81")]
        Item81,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("84")]
        Item84,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("85")]
        Item85,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tExpert
    {

        private string familyNameField;

        private string firstNameField;

        private string secondNameField;

        private tExpertType expertTypeField;

        private string expertCertificateField;

        private System.DateTime expertCertificateBeginDateField;

        private System.DateTime expertCertificateEndDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string FamilyName
        {
            get
            {
                return this.familyNameField;
            }
            set
            {
                this.familyNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string SecondName
        {
            get
            {
                return this.secondNameField;
            }
            set
            {
                this.secondNameField = value;
            }
        }

        public string ExpertTypeString => ExpertType.GetEnumXmlAttributeValue()?.ToString();

        /// <remarks/>
        public tExpertType ExpertType
        {
            get
            {
                return this.expertTypeField;
            }
            set
            {
                this.expertTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string ExpertCertificate
        {
            get
            {
                return this.expertCertificateField;
            }
            set
            {
                this.expertCertificateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime ExpertCertificateBeginDate
        {
            get
            {
                return this.expertCertificateBeginDateField;
            }
            set
            {
                this.expertCertificateBeginDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime ExpertCertificateEndDate
        {
            get
            {
                return this.expertCertificateEndDateField;
            }
            set
            {
                this.expertCertificateEndDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    public enum tExpertType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1.1.")]
        Item11,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1.2.")]
        Item12,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1.3.")]
        Item13,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1.4.")]
        Item14,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1.5.")]
        Item15,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2.1.")]
        Item21,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2.1.1.")]
        Item211,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2.1.2.")]
        Item212,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2.1.3.")]
        Item213,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2.1.4.")]
        Item214,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2.2.")]
        Item22,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2.2.1.")]
        Item221,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2.2.2.")]
        Item222,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2.2.3.")]
        Item223,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2.3.")]
        Item23,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2.3.1.")]
        Item231,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2.3.2.")]
        Item232,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2.4.")]
        Item24,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2.4.1.")]
        Item241,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2.4.2.")]
        Item242,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2.5.")]
        Item25,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3.1.")]
        Item31,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4.1.")]
        Item41,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4.2.")]
        Item42,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4.3.")]
        Item43,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4.4.")]
        Item44,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4.5.")]
        Item45,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.1.1.")]
        Item511,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.1.2.")]
        Item512,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.1.3.")]
        Item513,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.1.4.")]
        Item514,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.1.5.")]
        Item515,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.1.6.")]
        Item516,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.1.7.")]
        Item517,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.1.")]
        Item521,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.2.")]
        Item522,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.3.")]
        Item523,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.4.")]
        Item524,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.4.1.")]
        Item5241,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.4.2.")]
        Item5242,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.4.3.")]
        Item5243,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.4.4.")]
        Item5244,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.4.5.")]
        Item5245,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.4.6.")]
        Item5246,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.4.7.")]
        Item5247,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.5.")]
        Item525,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.6.")]
        Item526,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.7.")]
        Item527,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.8.")]
        Item528,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.9.")]
        Item529,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.10.")]
        Item5210,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.11.")]
        Item5211,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.12.")]
        Item5212,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.12.1.")]
        Item52121,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.12.2.")]
        Item52122,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.12.3.")]
        Item52123,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.12.4.")]
        Item52124,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.12.5.")]
        Item52125,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.12.6.")]
        Item52126,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.13.")]
        Item5213,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.13.1.")]
        Item52131,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.13.2.")]
        Item52132,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.13.3.")]
        Item52133,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.13.4.")]
        Item52134,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.13.5.")]
        Item52135,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.13.6.")]
        Item52136,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.13.7.")]
        Item52137,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.14.")]
        Item5214,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.14.1.")]
        Item52141,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.14.2.")]
        Item52142,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.14.3.")]
        Item52143,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.14.4.")]
        Item52144,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.15.")]
        Item5215,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.2.16.")]
        Item5216,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.3.1.")]
        Item531,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1.")]
        Item1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2.")]
        Item2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3.")]
        Item3,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4.")]
        Item4,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5.")]
        Item5,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("6.")]
        Item6,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("7.")]
        Item7,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("8.")]
        Item8,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("9.")]
        Item9,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("10.")]
        Item10,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("11.")]
        Item111,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("12.")]
        Item121,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("13.")]
        Item131,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("14.")]
        Item141,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("15.")]
        Item151,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("16.")]
        Item16,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("17.")]
        Item17,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("17.1.")]
        Item171,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("18.")]
        Item18,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("19.")]
        Item19,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("20.")]
        Item20,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("21.")]
        Item215,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("22.")]
        Item224,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("23.")]
        Item233,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("24.")]
        Item243,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("25.")]
        Item251,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("26.")]
        Item26,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("27.")]
        Item27,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("28.")]
        Item28,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("29.")]
        Item29,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("30.")]
        Item30,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("31.")]
        Item311,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("32.")]
        Item32,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("33.")]
        Item33,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("34.")]
        Item34,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("35.")]
        Item35,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("35.1.")]
        Item351,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("36.")]
        Item36,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("37.")]
        Item37,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("38.")]
        Item38,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("39.")]
        Item39,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("40.")]
        Item40,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("41.")]
        Item411,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("42.")]
        Item421,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("43.")]
        Item431,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("44.")]
        Item441,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("45.")]
        Item451,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("46.")]
        Item46,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("47.")]
        Item47,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("48.")]
        Item48,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("49.")]
        Item49,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("50.")]
        Item50,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("51.")]
        Item51,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("52.")]
        Item52,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("53.")]
        Item53,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("54.")]
        Item54,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("55.")]
        Item55,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("56.")]
        Item56,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("57.")]
        Item57,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("58.")]
        Item58,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("59.")]
        Item59,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("60.")]
        Item60,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("61.")]
        Item61,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("62.")]
        Item62,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("63.")]
        Item63,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.1.")]
        Item9551,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.2.")]
        Item9552,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.3.")]
        Item9553,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.4.")]
        Item9554,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.5.")]
        Item9555,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.6.")]
        Item9556,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.7.")]
        Item9557,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.8.")]
        Item9558,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.9.")]
        Item9559,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.10.")]
        Item95510,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.11.")]
        Item95511,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.12.")]
        Item95512,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.13.")]
        Item95513,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.14.")]
        Item95514,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.15.")]
        Item95515,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.16.")]
        Item95516,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.17.")]
        Item95517,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.18.")]
        Item95518,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.19.")]
        Item95519,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.20.")]
        Item95520,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.21.")]
        Item95521,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.22.")]
        Item95522,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.23.")]
        Item95523,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.24.")]
        Item95524,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.25.")]
        Item95525,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.26.")]
        Item95526,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.27.")]
        Item95527,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.28.")]
        Item95528,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.29.")]
        Item95529,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.30.")]
        Item95530,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.31.")]
        Item95531,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.32.")]
        Item95532,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.33.")]
        Item95533,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.34.")]
        Item95534,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.35.")]
        Item95535,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.36.")]
        Item95536,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.37.")]
        Item95537,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.38.")]
        Item95538,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.39.")]
        Item95539,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.40.")]
        Item95540,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.41.")]
        Item95541,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.42.")]
        Item95542,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("955.43.")]
        Item95543,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tSummary
    {

        private string engineeringSurveySummaryField;

        private string engineeringSurveySummaryDateField;

        private tEngineeringSurveyType[] engineeringSurveyTypeField;

        private string projectDocumentsSummaryField;

        private string projectDocumentsSummaryDateField;

        private string estimateNormsAndWorksSummaryField;

        private string estimateSummaryField;

        private string examinationSummaryField;

        /// <remarks/>
        public string EngineeringSurveySummary
        {
            get
            {
                return this.engineeringSurveySummaryField;
            }
            set
            {
                this.engineeringSurveySummaryField = value;
            }
        }

        /// <remarks/>
        public string EngineeringSurveySummaryDate
        {
            get
            {
                return this.engineeringSurveySummaryDateField;
            }
            set
            {
                this.engineeringSurveySummaryDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("EngineeringSurveyType")]
        public tEngineeringSurveyType[] EngineeringSurveyType
        {
            get
            {
                return this.engineeringSurveyTypeField;
            }
            set
            {
                this.engineeringSurveyTypeField = value;
            }
        }

        /// <remarks/>
        public string ProjectDocumentsSummary
        {
            get
            {
                return this.projectDocumentsSummaryField;
            }
            set
            {
                this.projectDocumentsSummaryField = value;
            }
        }

        /// <remarks/>
        public string ProjectDocumentsSummaryDate
        {
            get
            {
                return this.projectDocumentsSummaryDateField;
            }
            set
            {
                this.projectDocumentsSummaryDateField = value;
            }
        }

        /// <remarks/>
        public string EstimateNormsAndWorksSummary
        {
            get
            {
                return this.estimateNormsAndWorksSummaryField;
            }
            set
            {
                this.estimateNormsAndWorksSummaryField = value;
            }
        }

        /// <remarks/>
        public string EstimateSummary
        {
            get
            {
                return this.estimateSummaryField;
            }
            set
            {
                this.estimateSummaryField = value;
            }
        }

        /// <remarks/>
        public string ExaminationSummary
        {
            get
            {
                return this.examinationSummaryField;
            }
            set
            {
                this.examinationSummaryField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    public enum tEngineeringSurveyType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        Item4,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5")]
        Item5,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("6")]
        Item6,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("7")]
        Item7,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("8")]
        Item8,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("9")]
        Item9,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("10")]
        Item10,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("11")]
        Item11,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tMismatchExtended
    {

        private string summaryField;

        private string partField;

        private string linkField;

        private tExpertType expertTypeField;

        /// <remarks/>
        public string Summary
        {
            get
            {
                return this.summaryField;
            }
            set
            {
                this.summaryField = value;
            }
        }

        /// <remarks/>
        public string Part
        {
            get
            {
                return this.partField;
            }
            set
            {
                this.partField = value;
            }
        }

        /// <remarks/>
        public string Link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public tExpertType ExpertType
        {
            get
            {
                return this.expertTypeField;
            }
            set
            {
                this.expertTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tEstimateMismatches
    {

        private tMismatch[] commonMismatchField;

        private tMismatch[] fullCalculationMismatchField;

        private tMismatch[] localCalculationMismatchField;

        private tMismatchExtended[] projectDocumentsMismatchField;

        private tMismatch[] basicMismatchField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CommonMismatch")]
        public tMismatch[] CommonMismatch
        {
            get
            {
                return this.commonMismatchField;
            }
            set
            {
                this.commonMismatchField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("FullCalculationMismatch")]
        public tMismatch[] FullCalculationMismatch
        {
            get
            {
                return this.fullCalculationMismatchField;
            }
            set
            {
                this.fullCalculationMismatchField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("LocalCalculationMismatch")]
        public tMismatch[] LocalCalculationMismatch
        {
            get
            {
                return this.localCalculationMismatchField;
            }
            set
            {
                this.localCalculationMismatchField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ProjectDocumentsMismatch")]
        public tMismatchExtended[] ProjectDocumentsMismatch
        {
            get
            {
                return this.projectDocumentsMismatchField;
            }
            set
            {
                this.projectDocumentsMismatchField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("BasicMismatch")]
        public tMismatch[] BasicMismatch
        {
            get
            {
                return this.basicMismatchField;
            }
            set
            {
                this.basicMismatchField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tMismatch
    {

        private string summaryField;

        private string partField;

        private string linkField;

        /// <remarks/>
        public string Summary
        {
            get
            {
                return this.summaryField;
            }
            set
            {
                this.summaryField = value;
            }
        }

        /// <remarks/>
        public string Part
        {
            get
            {
                return this.partField;
            }
            set
            {
                this.partField = value;
            }
        }

        /// <remarks/>
        public string Link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tExpertEstimate
    {

        private string estimateNormsField;

        private tEstimateMismatches mismatchesField;

        /// <remarks/>
        public string EstimateNorms
        {
            get
            {
                return this.estimateNormsField;
            }
            set
            {
                this.estimateNormsField = value;
            }
        }

        /// <remarks/>
        public tEstimateMismatches Mismatches
        {
            get
            {
                return this.mismatchesField;
            }
            set
            {
                this.mismatchesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tProjectDocumentsMismatches
    {

        private tMismatch[] engineeringSurveyMismatchField;

        private tMismatch[] projectTaskMismatchField;

        private tMismatch[] normsMismatchField;

        private string dangerMismatchField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("EngineeringSurveyMismatch")]
        public tMismatch[] EngineeringSurveyMismatch
        {
            get
            {
                return this.engineeringSurveyMismatchField;
            }
            set
            {
                this.engineeringSurveyMismatchField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ProjectTaskMismatch")]
        public tMismatch[] ProjectTaskMismatch
        {
            get
            {
                return this.projectTaskMismatchField;
            }
            set
            {
                this.projectTaskMismatchField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("NormsMismatch")]
        public tMismatch[] NormsMismatch
        {
            get
            {
                return this.normsMismatchField;
            }
            set
            {
                this.normsMismatchField = value;
            }
        }

        /// <remarks/>
        public string DangerMismatch
        {
            get
            {
                return this.dangerMismatchField;
            }
            set
            {
                this.dangerMismatchField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tExpertProjectDocuments
    {

        private string dangerSolutionsField;

        private tProjectDocumentsMismatches mismatchesField;

        private tExpertType expertTypeField;

        /// <remarks/>
        public string DangerSolutions
        {
            get
            {
                return this.dangerSolutionsField;
            }
            set
            {
                this.dangerSolutionsField = value;
            }
        }

        /// <remarks/>
        public tProjectDocumentsMismatches Mismatches
        {
            get
            {
                return this.mismatchesField;
            }
            set
            {
                this.mismatchesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public tExpertType ExpertType
        {
            get
            {
                return this.expertTypeField;
            }
            set
            {
                this.expertTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tExpertEngineeringSurveys
    {

        private tMismatch[] mismatchesField;

        private tEngineeringSurveyType engineeringSurveyTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("NormsMismatch", IsNullable = false)]
        public tMismatch[] Mismatches
        {
            get
            {
                return this.mismatchesField;
            }
            set
            {
                this.mismatchesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public tEngineeringSurveyType EngineeringSurveyType
        {
            get
            {
                return this.engineeringSurveyTypeField;
            }
            set
            {
                this.engineeringSurveyTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tEngineeringSurveyAddress
    {

        private tRegionsRF engineeringSurveyRegionField;

        private string engineeringSurveyDistrictField;

        /// <remarks/>
        public tRegionsRF EngineeringSurveyRegion
        {
            get
            {
                return this.engineeringSurveyRegionField;
            }
            set
            {
                this.engineeringSurveyRegionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string EngineeringSurveyDistrict
        {
            get
            {
                return this.engineeringSurveyDistrictField;
            }
            set
            {
                this.engineeringSurveyDistrictField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tEEPDUse
    {

        private string eEPDNoteField;

        private tConclusionNumber eEPDNumberField;

        private System.DateTime eEPDDateField;

        /// <remarks/>
        public string EEPDNote
        {
            get
            {
                return this.eEPDNoteField;
            }
            set
            {
                this.eEPDNoteField = value;
            }
        }

        /// <remarks/>
        public tConclusionNumber EEPDNumber
        {
            get
            {
                return this.eEPDNumberField;
            }
            set
            {
                this.eEPDNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime EEPDDate
        {
            get
            {
                return this.eEPDDateField;
            }
            set
            {
                this.eEPDDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tConclusionNumber
    {

        private string itemField;

        private ItemChoiceType itemElementNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("EGRZ", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("noEGRZ", typeof(string), DataType = "normalizedString")]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema = false)]
    public enum ItemChoiceType
    {
        [Description("В формате ЕГРЗ")]
        EGRZ,

        [Description("В произвольном формате")]
        noEGRZ,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tDesigner
    {

        private object itemField;

        private tYesNo generalField;

        private bool generalFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ForeignOrganization", typeof(tForeignOrganization))]
        [System.Xml.Serialization.XmlElementAttribute("IP", typeof(tIP))]
        [System.Xml.Serialization.XmlElementAttribute("Organization", typeof(tOrganization))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public tYesNo General
        {
            get
            {
                return this.generalField;
            }
            set
            {
                this.generalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool GeneralSpecified
        {
            get
            {
                return this.generalFieldSpecified;
            }
            set
            {
                this.generalFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tForeignOrganization
    {

        private string orgFullNameField;

        private string orgINNField;

        private string orgKPPField;

        private tAddress addressField;

        private string emailField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string OrgFullName
        {
            get
            {
                return this.orgFullNameField;
            }
            set
            {
                this.orgFullNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string OrgINN
        {
            get
            {
                return this.orgINNField;
            }
            set
            {
                this.orgINNField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string OrgKPP
        {
            get
            {
                return this.orgKPPField;
            }
            set
            {
                this.orgKPPField = value;
            }
        }

        /// <remarks/>
        public tAddress Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tIP
    {

        private string familyNameField;

        private string firstNameField;

        private string secondNameField;

        private string oGRNIPField;

        private tPostAddress postAddressField;

        private string emailField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string FamilyName
        {
            get
            {
                return this.familyNameField;
            }
            set
            {
                this.familyNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string SecondName
        {
            get
            {
                return this.secondNameField;
            }
            set
            {
                this.secondNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string OGRNIP
        {
            get
            {
                return this.oGRNIPField;
            }
            set
            {
                this.oGRNIPField = value;
            }
        }

        /// <remarks/>
        public tPostAddress PostAddress
        {
            get
            {
                return this.postAddressField;
            }
            set
            {
                this.postAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tPostAddress
    {

        private string countryField;

        private tRegionsRF regionField;

        private string postIndexField;

        private string districtField;

        private string cityField;

        private string settlementField;

        private string streetField;

        private string buildingField;

        private string roomField;

        private string noteField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        public tRegionsRF Region
        {
            get
            {
                return this.regionField;
            }
            set
            {
                this.regionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string PostIndex
        {
            get
            {
                return this.postIndexField;
            }
            set
            {
                this.postIndexField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string District
        {
            get
            {
                return this.districtField;
            }
            set
            {
                this.districtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Settlement
        {
            get
            {
                return this.settlementField;
            }
            set
            {
                this.settlementField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Street
        {
            get
            {
                return this.streetField;
            }
            set
            {
                this.streetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Building
        {
            get
            {
                return this.buildingField;
            }
            set
            {
                this.buildingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Room
        {
            get
            {
                return this.roomField;
            }
            set
            {
                this.roomField = value;
            }
        }

        /// <remarks/>
        public string Note
        {
            get
            {
                return this.noteField;
            }
            set
            {
                this.noteField = value;
            }
        }


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

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    public enum tYesNo
    {

        /// <remarks/>
        да,

        /// <remarks/>
        нет,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tSeismicActivityCalculatedValue
    {

        private string minValueField;

        private string maxValueField;

        /// <remarks/>
        public string MinValue
        {
            get
            {
                return this.minValueField;
            }
            set
            {
                this.minValueField = value;
            }
        }

        /// <remarks/>
        public string MaxValue
        {
            get
            {
                return this.maxValueField;
            }
            set
            {
                this.maxValueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tClimateConditions
    {

        private tClimateDistrict[] climateDistrictField;

        private tGeologicalConditions[] geologicalConditionsField;

        private tWindDistrict[] windDistrictField;

        private tSnowDistrict[] snowDistrictField;

        private tSeismicActivity[] seismicActivityField;

        private tSeismicActivityCalculatedValue seismicActivityCalculatedValueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ClimateDistrict")]
        public tClimateDistrict[] ClimateDistrict
        {
            get
            {
                return this.climateDistrictField;
            }
            set
            {
                this.climateDistrictField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GeologicalConditions")]
        public tGeologicalConditions[] GeologicalConditions
        {
            get
            {
                return this.geologicalConditionsField;
            }
            set
            {
                this.geologicalConditionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("WindDistrict")]
        public tWindDistrict[] WindDistrict
        {
            get
            {
                return this.windDistrictField;
            }
            set
            {
                this.windDistrictField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SnowDistrict")]
        public tSnowDistrict[] SnowDistrict
        {
            get
            {
                return this.snowDistrictField;
            }
            set
            {
                this.snowDistrictField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SeismicActivity")]
        public tSeismicActivity[] SeismicActivity
        {
            get
            {
                return this.seismicActivityField;
            }
            set
            {
                this.seismicActivityField = value;
            }
        }

        /// <remarks/>
        public tSeismicActivityCalculatedValue SeismicActivityCalculatedValue
        {
            get
            {
                return this.seismicActivityCalculatedValueField;
            }
            set
            {
                this.seismicActivityCalculatedValueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    public enum tClimateDistrict
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Сведения не представлены")]
        Сведениянепредставлены,

        /// <remarks/>
        I,

        /// <remarks/>
        IА,

        /// <remarks/>
        IБ,

        /// <remarks/>
        IВ,

        /// <remarks/>
        IГ,

        /// <remarks/>
        IД,

        /// <remarks/>
        II,

        /// <remarks/>
        IIА,

        /// <remarks/>
        IIБ,

        /// <remarks/>
        IIВ,

        /// <remarks/>
        IIГ,

        /// <remarks/>
        III,

        /// <remarks/>
        IIIА,

        /// <remarks/>
        IIIБ,

        /// <remarks/>
        IIIВ,

        /// <remarks/>
        IV,

        /// <remarks/>
        IVА,

        /// <remarks/>
        IVБ,

        /// <remarks/>
        IVВ,

        /// <remarks/>
        IVГ,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    public enum tGeologicalConditions
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Сведения не представлены")]
        Сведениянепредставлены,

        /// <remarks/>
        I,

        /// <remarks/>
        II,

        /// <remarks/>
        III,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    public enum tWindDistrict
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Сведения не представлены")]
        Сведениянепредставлены,

        /// <remarks/>
        Iа,

        /// <remarks/>
        I,

        /// <remarks/>
        II,

        /// <remarks/>
        III,

        /// <remarks/>
        IV,

        /// <remarks/>
        V,

        /// <remarks/>
        VI,

        /// <remarks/>
        VII,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    public enum tSnowDistrict
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Сведения не представлены")]
        Сведениянепредставлены,

        /// <remarks/>
        I,

        /// <remarks/>
        II,

        /// <remarks/>
        III,

        /// <remarks/>
        IV,

        /// <remarks/>
        V,

        /// <remarks/>
        VI,

        /// <remarks/>
        VII,

        /// <remarks/>
        VIII,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    public enum tSeismicActivity
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Сведения не представлены")]
        Сведениянепредставлены,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5")]
        Item5,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("6")]
        Item6,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("7")]
        Item7,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("8")]
        Item8,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("9")]
        Item9,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("10")]
        Item10,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tComplexEstimatedCost
    {

        private string costBasicField;

        private string worksCostBasicField;

        private string hardwareCostBasicField;

        private string otherCostBasicField;

        private string projectWorksCostBasicField;

        private string backSumCostBasicField;

        private string costField;

        private string worksCostField;

        private string hardwareCostField;

        private string otherCostField;

        private string projectWorksCostField;

        private string nDSCostField;

        private string backSumCostField;

        private string costBasicCommentField;

        private string costCommentField;

        /// <remarks/>
        public string CostBasic
        {
            get
            {
                return this.costBasicField;
            }
            set
            {
                this.costBasicField = value;
            }
        }

        /// <remarks/>
        public string WorksCostBasic
        {
            get
            {
                return this.worksCostBasicField;
            }
            set
            {
                this.worksCostBasicField = value;
            }
        }

        /// <remarks/>
        public string HardwareCostBasic
        {
            get
            {
                return this.hardwareCostBasicField;
            }
            set
            {
                this.hardwareCostBasicField = value;
            }
        }

        /// <remarks/>
        public string OtherCostBasic
        {
            get
            {
                return this.otherCostBasicField;
            }
            set
            {
                this.otherCostBasicField = value;
            }
        }

        /// <remarks/>
        public string ProjectWorksCostBasic
        {
            get
            {
                return this.projectWorksCostBasicField;
            }
            set
            {
                this.projectWorksCostBasicField = value;
            }
        }

        /// <remarks/>
        public string BackSumCostBasic
        {
            get
            {
                return this.backSumCostBasicField;
            }
            set
            {
                this.backSumCostBasicField = value;
            }
        }

        /// <remarks/>
        public string Cost
        {
            get
            {
                return this.costField;
            }
            set
            {
                this.costField = value;
            }
        }

        /// <remarks/>
        public string WorksCost
        {
            get
            {
                return this.worksCostField;
            }
            set
            {
                this.worksCostField = value;
            }
        }

        /// <remarks/>
        public string HardwareCost
        {
            get
            {
                return this.hardwareCostField;
            }
            set
            {
                this.hardwareCostField = value;
            }
        }

        /// <remarks/>
        public string OtherCost
        {
            get
            {
                return this.otherCostField;
            }
            set
            {
                this.otherCostField = value;
            }
        }

        /// <remarks/>
        public string ProjectWorksCost
        {
            get
            {
                return this.projectWorksCostField;
            }
            set
            {
                this.projectWorksCostField = value;
            }
        }

        /// <remarks/>
        public string NDSCost
        {
            get
            {
                return this.nDSCostField;
            }
            set
            {
                this.nDSCostField = value;
            }
        }

        /// <remarks/>
        public string BackSumCost
        {
            get
            {
                return this.backSumCostField;
            }
            set
            {
                this.backSumCostField = value;
            }
        }

        /// <remarks/>
        public string CostBasicComment
        {
            get
            {
                return this.costBasicCommentField;
            }
            set
            {
                this.costBasicCommentField = value;
            }
        }

        /// <remarks/>
        public string CostComment
        {
            get
            {
                return this.costCommentField;
            }
            set
            {
                this.costCommentField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tEstimatedCost
    {

        private string currencyField;

        private object[] itemsField;

        private ItemsChoiceType[] itemsElementNameField;

        /// <remarks/>
        public string Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("EstimatedCompleteCostBefore", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("EstimatedCompleteCostPost", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("EstimatedComplexCostBefore", typeof(tComplexEstimatedCost))]
        [System.Xml.Serialization.XmlElementAttribute("EstimatedComplexCostPost", typeof(tComplexEstimatedCost))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema = false)]
    public enum ItemsChoiceType
    {

        /// <remarks/>
        EstimatedCompleteCostBefore,

        /// <remarks/>
        EstimatedCompleteCostPost,

        /// <remarks/>
        EstimatedComplexCostBefore,

        /// <remarks/>
        EstimatedComplexCostPost,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tFinance
    {

        private tFinanceType financeTypeField;

        private tBudgetType budgetTypeField;

        private bool budgetTypeFieldSpecified;

        private decimal financeSizeField;

        private bool financeSizeFieldSpecified;

        private tTechnicalCustomer financeOwnerField;

        /// <remarks/>
        public tFinanceType FinanceType
        {
            get
            {
                return this.financeTypeField;
            }
            set
            {
                this.financeTypeField = value;
            }
        }

        /// <remarks/>
        public tBudgetType BudgetType
        {
            get
            {
                return this.budgetTypeField;
            }
            set
            {
                this.budgetTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BudgetTypeSpecified
        {
            get
            {
                return this.budgetTypeFieldSpecified;
            }
            set
            {
                this.budgetTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal FinanceSize
        {
            get
            {
                return this.financeSizeField;
            }
            set
            {
                this.financeSizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FinanceSizeSpecified
        {
            get
            {
                return this.financeSizeFieldSpecified;
            }
            set
            {
                this.financeSizeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public tTechnicalCustomer FinanceOwner
        {
            get
            {
                return this.financeOwnerField;
            }
            set
            {
                this.financeOwnerField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    public enum tFinanceType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    public enum tBudgetType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        Item4,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5")]
        Item5,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tTechnicalCustomer
    {
        private object itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ForeignOrganization", typeof(tForeignOrganization))]
        [System.Xml.Serialization.XmlElementAttribute("Organization", typeof(tOrganization))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tObjectPart
    {

        private string nameField;

        private tAddress[] addressField;

        private string itemField;

        private ItemChoiceType2 itemElementNameField;

        private tTEI[] tEIField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Address")]
        public tAddress[] Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Functions", typeof(string), DataType = "normalizedString")]
        [System.Xml.Serialization.XmlElementAttribute("FunctionsClass", typeof(string))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType2 ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("TEI")]
        public tTEI[] TEI
        {
            get
            {
                return this.tEIField;
            }
            set
            {
                this.tEIField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema = false)]
    public enum ItemChoiceType2
    {

        /// <remarks/>
        Functions,

        /// <remarks/>
        FunctionsClass,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tTEI
    {

        private string nameField;

        private string measureField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Measure
        {
            get
            {
                return this.measureField;
            }
            set
            {
                this.measureField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tObject
    {

        private string nameField;

        private tAddress[] addressField;

        private tObjectType typeField;

        private string itemField;

        private ItemChoiceType1 itemElementNameField;

        private tTEI[] tEIField;

        private tObjectPart[] objectPartField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Address")]
        public tAddress[] Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        public tObjectType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Functions", typeof(string), DataType = "normalizedString")]
        [System.Xml.Serialization.XmlElementAttribute("FunctionsClass", typeof(string))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType1 ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("TEI")]
        public tTEI[] TEI
        {
            get
            {
                return this.tEIField;
            }
            set
            {
                this.tEIField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ObjectPart")]
        public tObjectPart[] ObjectPart
        {
            get
            {
                return this.objectPartField;
            }
            set
            {
                this.objectPartField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    public enum tObjectType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema = false)]
    public enum ItemChoiceType1
    {

        /// <remarks/>
        Functions,

        /// <remarks/>
        FunctionsClass,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tPreviousSimpleConclusion
    {

        private System.DateTime dateField;

        private string numberField;

        private tExaminationResult resultField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }

        /// <remarks/>
        public tExaminationResult Result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    public enum tExaminationResult
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tPreviousConclusion
    {

        private System.DateTime dateField;

        private tConclusionNumber numberField;

        private tExaminationObjectType examinationObjectTypeField;

        private string nameField;

        private tExaminationResult resultField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        /// <remarks/>
        public tConclusionNumber Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }

        /// <remarks/>
        public tExaminationObjectType ExaminationObjectType
        {
            get
            {
                return this.examinationObjectTypeField;
            }
            set
            {
                this.examinationObjectTypeField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public tExaminationResult Result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    public enum tExaminationObjectType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tSignFile
    {

        private string fileNameField;

        private string fileFormatField;

        private string fileChecksumField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string FileName
        {
            get
            {
                return this.fileNameField;
            }
            set
            {
                this.fileNameField = value;
            }
        }

        /// <remarks/>
        public string FileFormat
        {
            get
            {
                return this.fileFormatField;
            }
            set
            {
                this.fileFormatField = value;
            }
        }

        /// <remarks/>
        public string FileChecksum
        {
            get
            {
                return this.fileChecksumField;
            }
            set
            {
                this.fileChecksumField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tFile
    {

        private string fileNameField;

        private string fileFormatField;

        private string fileChecksumField;

        private tSignFile[] signFileField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string FileName
        {
            get
            {
                return this.fileNameField;
            }
            set
            {
                this.fileNameField = value;
            }
        }

        /// <remarks/>
        public string FileFormat
        {
            get
            {
                return this.fileFormatField;
            }
            set
            {
                this.fileFormatField = value;
            }
        }

        /// <remarks/>
        public string FileChecksum
        {
            get
            {
                return this.fileChecksumField;
            }
            set
            {
                this.fileChecksumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SignFile")]
        public tSignFile[] SignFile
        {
            get
            {
                return this.signFileField;
            }
            set
            {
                this.signFileField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tPerson
    {

        private string familyNameField;

        private string firstNameField;

        private string secondNameField;

        private string sNILSField;

        private tPostAddress postAddressField;

        private string emailField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string FamilyName
        {
            get
            {
                return this.familyNameField;
            }
            set
            {
                this.familyNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string SecondName
        {
            get
            {
                return this.secondNameField;
            }
            set
            {
                this.secondNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string SNILS
        {
            get
            {
                return this.sNILSField;
            }
            set
            {
                this.sNILSField = value;
            }
        }

        /// <remarks/>
        public tPostAddress PostAddress
        {
            get
            {
                return this.postAddressField;
            }
            set
            {
                this.postAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tDeclarant
    {

        private object itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ForeignOrganization", typeof(tForeignOrganization))]
        [System.Xml.Serialization.XmlElementAttribute("IP", typeof(tIP))]
        [System.Xml.Serialization.XmlElementAttribute("Organization", typeof(tOrganization))]
        [System.Xml.Serialization.XmlElementAttribute("Person", typeof(tPerson))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tDocument
    {

        private tDocumentType docTypeField;

        private string docNameField;

        private string docNumberField;

        private System.DateTime docDateField;

        private bool docDateFieldSpecified;

        private object[] itemsField;

        private string docChangesField;

        private object[] items1Field;

        /// <remarks/>
        public tDocumentType DocType
        {
            get
            {
                return this.docTypeField;
            }
            set
            {
                this.docTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string DocName
        {
            get
            {
                return this.docNameField;
            }
            set
            {
                this.docNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string DocNumber
        {
            get
            {
                return this.docNumberField;
            }
            set
            {
                this.docNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime DocDate
        {
            get
            {
                return this.docDateField;
            }
            set
            {
                this.docDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DocDateSpecified
        {
            get
            {
                return this.docDateFieldSpecified;
            }
            set
            {
                this.docDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DocIssueAuthor", typeof(string), DataType = "normalizedString")]
        [System.Xml.Serialization.XmlElementAttribute("FullDocIssueAuthor", typeof(tDeclarant))]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string DocChanges
        {
            get
            {
                return this.docChangesField;
            }
            set
            {
                this.docChangesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("File", typeof(tFile))]
        [System.Xml.Serialization.XmlElementAttribute("Included", typeof(string), DataType = "normalizedString")]
        public object[] Items1
        {
            get
            {
                return this.items1Field;
            }
            set
            {
                this.items1Field = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    public enum tDocumentType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("01.01")]
        Item0101,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("01.02")]
        Item0102,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("01.03")]
        Item0103,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("01.99")]
        Item0199,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("02.01")]
        Item0201,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("02.02")]
        Item0202,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("02.04")]
        Item0204,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("02.05")]
        Item0205,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("02.06")]
        Item0206,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("02.07")]
        Item0207,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("02.08")]
        Item0208,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("02.09")]
        Item0209,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("02.99")]
        Item0299,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("03.01")]
        Item0301,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("03.02")]
        Item0302,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("03.03")]
        Item0303,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("03.04")]
        Item0304,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("03.05")]
        Item0305,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("03.06")]
        Item0306,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("03.99")]
        Item0399,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("04.01")]
        Item0401,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("04.02")]
        Item0402,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("04.03")]
        Item0403,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("04.99")]
        Item0499,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("05.01")]
        Item0501,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("05.02")]
        Item0502,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("05.03")]
        Item0503,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("05.99")]
        Item0599,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("06.01")]
        Item0601,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("06.02")]
        Item0602,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("06.03")]
        Item0603,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("06.04")]
        Item0604,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("06.05")]
        Item0605,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("06.06")]
        Item0606,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("06.07")]
        Item0607,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("06.08")]
        Item0608,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("06.09")]
        Item0609,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("06.10")]
        Item0610,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("06.11")]
        Item0611,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("06.99")]
        Item0699,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.01")]
        Item0701,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.02")]
        Item0702,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.03")]
        Item0703,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.04")]
        Item0704,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.05")]
        Item0705,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.06")]
        Item0706,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.07")]
        Item0707,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.08")]
        Item0708,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.09")]
        Item0709,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.10")]
        Item0710,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.11")]
        Item0711,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.12")]
        Item0712,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.13")]
        Item0713,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.14")]
        Item0714,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.15")]
        Item0715,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.16")]
        Item0716,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.17")]
        Item0717,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.19")]
        Item0719,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.20")]
        Item0720,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.21")]
        Item0721,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.22")]
        Item0722,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.23")]
        Item0723,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.24")]
        Item0724,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.25")]
        Item0725,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.26")]
        Item0726,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.27")]
        Item0727,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.28")]
        Item0728,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.29")]
        Item0729,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.30")]
        Item0730,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.31")]
        Item0731,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.32")]
        Item0732,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.33")]
        Item0733,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.34")]
        Item0734,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.35")]
        Item0735,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07.37")]
        Item0737,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08.01")]
        Item0801,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08.02")]
        Item0802,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08.03")]
        Item0803,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08.04")]
        Item0804,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08.05")]
        Item0805,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08.06")]
        Item0806,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08.07")]
        Item0807,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08.08")]
        Item0808,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08.10")]
        Item0810,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08.11")]
        Item0811,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08.12")]
        Item0812,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08.13")]
        Item0813,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08.14")]
        Item0814,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08.15")]
        Item0815,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08.16")]
        Item0816,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08.17")]
        Item0817,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08.18")]
        Item0818,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08.20")]
        Item0820,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("09.01")]
        Item0901,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("09.02")]
        Item0902,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("09.99")]
        Item0999,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("10.01")]
        Item1001,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("10.99")]
        Item1099,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("11.01")]
        Item1101,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("11.02")]
        Item1102,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("11.03")]
        Item1103,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("11.04")]
        Item1104,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("11.05")]
        Item1105,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("11.06")]
        Item1106,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("11.99")]
        Item1199,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("12.01")]
        Item1201,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("12.02")]
        Item1202,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("12.03")]
        Item1203,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("12.99")]
        Item1299,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("13.01")]
        Item1301,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("13.02")]
        Item1302,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("13.03")]
        Item1303,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("13.04")]
        Item1304,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("13.05")]
        Item1305,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("13.06")]
        Item1306,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("13.99")]
        Item1399,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("14.01")]
        Item1401,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("14.02")]
        Item1402,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("14.03")]
        Item1403,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("14.04")]
        Item1404,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("14.99")]
        Item1499,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("15.01")]
        Item1501,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("15.02")]
        Item1502,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("15.03")]
        Item1503,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("15.04")]
        Item1504,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("15.05")]
        Item1505,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("15.06")]
        Item1506,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("15.07")]
        Item1507,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("15.08")]
        Item1508,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("15.99")]
        Item1599,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("16.01")]
        Item1601,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("16.02")]
        Item1602,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("16.99")]
        Item1699,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("17.01")]
        Item1701,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("17.99")]
        Item1799,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("18.01")]
        Item1801,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("18.02")]
        Item1802,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("18.03")]
        Item1803,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("18.99")]
        Item1899,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("19.01")]
        Item1901,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("19.02")]
        Item1902,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("19.03")]
        Item1903,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("19.99")]
        Item1999,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("20.01")]
        Item2001,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("20.02")]
        Item2002,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("20.03")]
        Item2003,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("20.04")]
        Item2004,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("20.05")]
        Item2005,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("20.06")]
        Item2006,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("20.07")]
        Item2007,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("20.99")]
        Item2099,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("21.01")]
        Item2101,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("21.02")]
        Item2102,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("21.03")]
        Item2103,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("21.04")]
        Item2104,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("21.05")]
        Item2105,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("21.06")]
        Item2106,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("21.07")]
        Item2107,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("21.08")]
        Item2108,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("21.09")]
        Item2109,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("21.10")]
        Item2110,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("21.11")]
        Item2111,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("21.12")]
        Item2112,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("21.13")]
        Item2113,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("21.99")]
        Item2199,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("22.01")]
        Item2201,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("22.02")]
        Item2202,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("22.03")]
        Item2203,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("22.04")]
        Item2204,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("22.05")]
        Item2205,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("22.99")]
        Item2299,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("23.01")]
        Item2301,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("23.02")]
        Item2302,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("23.99")]
        Item2399,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("24.01")]
        Item2401,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("25.01")]
        Item2501,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("25.02")]
        Item2502,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("25.03")]
        Item2503,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("25.04")]
        Item2504,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("25.05")]
        Item2505,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("25.06")]
        Item2506,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("99.01")]
        Item9901,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("99.02")]
        Item9902,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("99.03")]
        Item9903,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("99.99")]
        Item9999,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tExaminationObject
    {

        private tExaminationForm examinationFormField;

        private tExaminationResult examinationResultField;

        private tExaminationObjectType examinationObjectTypeField;

        private tExaminationType[] examinationTypeField;

        private tConstractionType constructionTypeField;

        private tExaminationStage examinationStageField;

        private string examinationStageNoteField;

        private string nameField;

        private tYesNo projectDocumentationIMField;

        private bool projectDocumentationIMFieldSpecified;

        private tYesNo engineeringSurveysIMField;

        private bool engineeringSurveysIMFieldSpecified;

        /// <remarks/>
        public tExaminationForm ExaminationForm
        {
            get
            {
                return this.examinationFormField;
            }
            set
            {
                this.examinationFormField = value;
            }
        }

        /// <remarks/>
        public tExaminationResult ExaminationResult
        {
            get
            {
                return this.examinationResultField;
            }
            set
            {
                this.examinationResultField = value;
            }
        }

        /// <remarks/>
        public tExaminationObjectType ExaminationObjectType
        {
            get
            {
                return this.examinationObjectTypeField;
            }
            set
            {
                this.examinationObjectTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ExaminationType")]
        public tExaminationType[] ExaminationType
        {
            get
            {
                return this.examinationTypeField;
            }
            set
            {
                this.examinationTypeField = value;
            }
        }

        /// <remarks/>
        public tConstractionType ConstructionType
        {
            get
            {
                return this.constructionTypeField;
            }
            set
            {
                this.constructionTypeField = value;
            }
        }

        /// <remarks/>
        public tExaminationStage ExaminationStage
        {
            get
            {
                return this.examinationStageField;
            }
            set
            {
                this.examinationStageField = value;
            }
        }

        /// <remarks/>
        public string ExaminationStageNote
        {
            get
            {
                return this.examinationStageNoteField;
            }
            set
            {
                this.examinationStageNoteField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public tYesNo ProjectDocumentationIM
        {
            get
            {
                return this.projectDocumentationIMField;
            }
            set
            {
                this.projectDocumentationIMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ProjectDocumentationIMSpecified
        {
            get
            {
                return this.projectDocumentationIMFieldSpecified;
            }
            set
            {
                this.projectDocumentationIMFieldSpecified = value;
            }
        }

        /// <remarks/>
        public tYesNo EngineeringSurveysIM
        {
            get
            {
                return this.engineeringSurveysIMField;
            }
            set
            {
                this.engineeringSurveysIMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EngineeringSurveysIMSpecified
        {
            get
            {
                return this.engineeringSurveysIMFieldSpecified;
            }
            set
            {
                this.engineeringSurveysIMFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    public enum tExaminationForm
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    public enum tExaminationType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    public enum tConstractionType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        Item4,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5")]
        Item5,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    public enum tExaminationStage
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tWorkPerson
    {

        private string familyNameField;

        private string firstNameField;

        private string secondNameField;

        private string positionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string FamilyName
        {
            get
            {
                return this.familyNameField;
            }
            set
            {
                this.familyNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string SecondName
        {
            get
            {
                return this.secondNameField;
            }
            set
            {
                this.secondNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Position
        {
            get
            {
                return this.positionField;
            }
            set
            {
                this.positionField = value;
            }
        }
    }
}
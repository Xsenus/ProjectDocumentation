// Ignore Spelling: Cadastral Declarant Org OGRNIP TEI SNILS Constraction

using Core.NET.Extensions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Core.NET.AutoModels.ConclusionObj
{
    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public partial class Conclusion
    {
        public Conclusion()
        {
            SchemaVersion = "01.03";
        }

        public tOrganization ExpertOrganization { get; set; }
        public tWorkPerson Approver { get; set; }
        public tExaminationObject ExaminationObject { get; set; }

        [XmlArrayItem("Document", IsNullable = false)]
        public tDocument[] Documents { get; set; }

        [XmlArrayItem("PreviousConclusion", IsNullable = false)]
        public tPreviousConclusion[] PreviousConclusions { get; set; }

        [XmlArrayItem("PreviousSimpleConclusion", IsNullable = false)]
        public tPreviousSimpleConclusion[] PreviousSimpleConclusions { get; set; }

        public tObject Object { get; set; }

        [XmlElement("CadastralNumber")]
        public string[] CadastralNumber { get; set; }

        public tDeclarant Declarant { get; set; }

        [XmlElement("ProjectDocumentsDeveloper", typeof(tDeclarant))]
        [XmlElement("ProjectDocumentsTechnicalCustomer", typeof(tTechnicalCustomer))]
        public object[] Items { get; set; }

        [XmlElement("Finance")]
        public tFinance[] Finance { get; set; }

        public string FinanceComment { get; set; }
        public tEstimatedCost EstimatedCost { get; set; }
        public tClimateConditions ClimateConditions { get; set; }
        public string ClimateConditionsNote { get; set; }

        [XmlElement("Designer")]
        public tDesigner[] Designer { get; set; }

        [XmlElement("EEPDUse")]
        public tEEPDUse[] EEPDUse { get; set; }

        [XmlElement("EngineeringSurveyAddress")]
        public tEngineeringSurveyAddress[] EngineeringSurveyAddress { get; set; }

        [XmlElement("EngineeringSurveyDeveloper", typeof(tDeclarant))]
        [XmlElement("EngineeringSurveyTechnicalCustomer", typeof(tTechnicalCustomer))]
        public object[] Items1 { get; set; }

        [XmlElement("ExpertEngineeringSurveys")]
        public tExpertEngineeringSurveys[] ExpertEngineeringSurveys { get; set; }

        [XmlElement("ExpertProjectDocuments")]
        public tExpertProjectDocuments[] ExpertProjectDocuments { get; set; }

        public tExpertEstimate ExpertEstimate { get; set; }
        public tSummary Summary { get; set; }

        [XmlArrayItem("Expert", IsNullable = false)]
        public tExpert[] Experts { get; set; }

        [XmlAttribute()]
        public string ConclusionGUID { get; set; }

        [XmlAttribute()]
        public string SchemaVersion { get; set; }

        [XmlAttribute()]
        public string SchemaLink { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tExpert
    {
        [XmlElement(DataType = "normalizedString")]
        public string FamilyName { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string FirstName { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string SecondName { get; set; }

        public string ExpertTypeString => ExpertType.GetEnumXmlAttributeValue()?.ToString();

        public tExpertType ExpertType { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string ExpertCertificate { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime ExpertCertificateBeginDate { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime ExpertCertificateEndDate { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum tExpertType
    {
        [XmlEnum("1.1.")]
        Item11,

        [XmlEnum("1.2.")]
        Item12,

        [XmlEnum("1.3.")]
        Item13,

        [XmlEnum("1.4.")]
        Item14,

        [XmlEnum("1.5.")]
        Item15,

        [XmlEnum("2.1.")]
        Item21,

        [XmlEnum("2.1.1.")]
        Item211,

        [XmlEnum("2.1.2.")]
        Item212,

        [XmlEnum("2.1.3.")]
        Item213,

        [XmlEnum("2.1.4.")]
        Item214,

        [XmlEnum("2.2.")]
        Item22,

        [XmlEnum("2.2.1.")]
        Item221,

        [XmlEnum("2.2.2.")]
        Item222,

        [XmlEnum("2.2.3.")]
        Item223,

        [XmlEnum("2.3.")]
        Item23,

        [XmlEnum("2.3.1.")]
        Item231,

        [XmlEnum("2.3.2.")]
        Item232,

        [XmlEnum("2.4.")]
        Item24,

        [XmlEnum("2.4.1.")]
        Item241,

        [XmlEnum("2.4.2.")]
        Item242,

        [XmlEnum("2.5.")]
        Item25,

        [XmlEnum("3.1.")]
        Item31,

        [XmlEnum("4.1.")]
        Item41,

        [XmlEnum("4.2.")]
        Item42,

        [XmlEnum("4.3.")]
        Item43,

        [XmlEnum("4.4.")]
        Item44,

        [XmlEnum("4.5.")]
        Item45,

        [XmlEnum("5.1.1.")]
        Item511,

        [XmlEnum("5.1.2.")]
        Item512,

        [XmlEnum("5.1.3.")]
        Item513,

        [XmlEnum("5.1.4.")]
        Item514,

        [XmlEnum("5.1.5.")]
        Item515,

        [XmlEnum("5.1.6.")]
        Item516,

        [XmlEnum("5.1.7.")]
        Item517,

        [XmlEnum("5.2.1.")]
        Item521,

        [XmlEnum("5.2.2.")]
        Item522,

        [XmlEnum("5.2.3.")]
        Item523,

        [XmlEnum("5.2.4.")]
        Item524,

        [XmlEnum("5.2.4.1.")]
        Item5241,

        [XmlEnum("5.2.4.2.")]
        Item5242,

        [XmlEnum("5.2.4.3.")]
        Item5243,

        [XmlEnum("5.2.4.4.")]
        Item5244,

        [XmlEnum("5.2.4.5.")]
        Item5245,

        [XmlEnum("5.2.4.6.")]
        Item5246,

        [XmlEnum("5.2.4.7.")]
        Item5247,

        [XmlEnum("5.2.5.")]
        Item525,

        [XmlEnum("5.2.6.")]
        Item526,

        [XmlEnum("5.2.7.")]
        Item527,

        [XmlEnum("5.2.8.")]
        Item528,

        [XmlEnum("5.2.9.")]
        Item529,

        [XmlEnum("5.2.10.")]
        Item5210,

        [XmlEnum("5.2.11.")]
        Item5211,

        [XmlEnum("5.2.12.")]
        Item5212,

        [XmlEnum("5.2.12.1.")]
        Item52121,

        [XmlEnum("5.2.12.2.")]
        Item52122,

        [XmlEnum("5.2.12.3.")]
        Item52123,

        [XmlEnum("5.2.12.4.")]
        Item52124,

        [XmlEnum("5.2.12.5.")]
        Item52125,

        [XmlEnum("5.2.12.6.")]
        Item52126,

        [XmlEnum("5.2.13.")]
        Item5213,

        [XmlEnum("5.2.13.1.")]
        Item52131,

        [XmlEnum("5.2.13.2.")]
        Item52132,

        [XmlEnum("5.2.13.3.")]
        Item52133,

        [XmlEnum("5.2.13.4.")]
        Item52134,

        [XmlEnum("5.2.13.5.")]
        Item52135,

        [XmlEnum("5.2.13.6.")]
        Item52136,

        [XmlEnum("5.2.13.7.")]
        Item52137,

        [XmlEnum("5.2.14.")]
        Item5214,

        [XmlEnum("5.2.14.1.")]
        Item52141,

        [XmlEnum("5.2.14.2.")]
        Item52142,

        [XmlEnum("5.2.14.3.")]
        Item52143,

        [XmlEnum("5.2.14.4.")]
        Item52144,

        [XmlEnum("5.2.15.")]
        Item5215,

        [XmlEnum("5.2.16.")]
        Item5216,

        [XmlEnum("5.3.1.")]
        Item531,

        [XmlEnum("1.")]
        Item1,

        [XmlEnum("2.")]
        Item2,

        [XmlEnum("3.")]
        Item3,

        [XmlEnum("4.")]
        Item4,

        [XmlEnum("5.")]
        Item5,

        [XmlEnum("6.")]
        Item6,

        [XmlEnum("7.")]
        Item7,

        [XmlEnum("8.")]
        Item8,

        [XmlEnum("9.")]
        Item9,

        [XmlEnum("10.")]
        Item10,

        [XmlEnum("11.")]
        Item111,

        [XmlEnum("12.")]
        Item121,

        [XmlEnum("13.")]
        Item131,

        [XmlEnum("14.")]
        Item141,

        [XmlEnum("15.")]
        Item151,

        [XmlEnum("16.")]
        Item16,

        [XmlEnum("17.")]
        Item17,

        [XmlEnum("17.1.")]
        Item171,

        [XmlEnum("18.")]
        Item18,

        [XmlEnum("19.")]
        Item19,

        [XmlEnum("20.")]
        Item20,

        [XmlEnum("21.")]
        Item215,

        [XmlEnum("22.")]
        Item224,

        [XmlEnum("23.")]
        Item233,

        [XmlEnum("24.")]
        Item243,

        [XmlEnum("25.")]
        Item251,

        [XmlEnum("26.")]
        Item26,

        [XmlEnum("27.")]
        Item27,

        [XmlEnum("28.")]
        Item28,

        [XmlEnum("29.")]
        Item29,

        [XmlEnum("30.")]
        Item30,

        [XmlEnum("31.")]
        Item311,

        [XmlEnum("32.")]
        Item32,

        [XmlEnum("33.")]
        Item33,

        [XmlEnum("34.")]
        Item34,

        [XmlEnum("35.")]
        Item35,

        [XmlEnum("35.1.")]
        Item351,

        [XmlEnum("36.")]
        Item36,

        [XmlEnum("37.")]
        Item37,

        [XmlEnum("38.")]
        Item38,

        [XmlEnum("39.")]
        Item39,

        [XmlEnum("40.")]
        Item40,

        [XmlEnum("41.")]
        Item411,

        [XmlEnum("42.")]
        Item421,

        [XmlEnum("43.")]
        Item431,

        [XmlEnum("44.")]
        Item441,

        [XmlEnum("45.")]
        Item451,

        [XmlEnum("46.")]
        Item46,

        [XmlEnum("47.")]
        Item47,

        [XmlEnum("48.")]
        Item48,

        [XmlEnum("49.")]
        Item49,

        [XmlEnum("50.")]
        Item50,

        [XmlEnum("51.")]
        Item51,

        [XmlEnum("52.")]
        Item52,

        [XmlEnum("53.")]
        Item53,

        [XmlEnum("54.")]
        Item54,

        [XmlEnum("55.")]
        Item55,

        [XmlEnum("56.")]
        Item56,

        [XmlEnum("57.")]
        Item57,

        [XmlEnum("58.")]
        Item58,

        [XmlEnum("59.")]
        Item59,

        [XmlEnum("60.")]
        Item60,

        [XmlEnum("61.")]
        Item61,

        [XmlEnum("62.")]
        Item62,

        [XmlEnum("63.")]
        Item63,

        [XmlEnum("955.1.")]
        Item9551,

        [XmlEnum("955.2.")]
        Item9552,

        [XmlEnum("955.3.")]
        Item9553,

        [XmlEnum("955.4.")]
        Item9554,

        [XmlEnum("955.5.")]
        Item9555,

        [XmlEnum("955.6.")]
        Item9556,

        [XmlEnum("955.7.")]
        Item9557,

        [XmlEnum("955.8.")]
        Item9558,

        [XmlEnum("955.9.")]
        Item9559,

        [XmlEnum("955.10.")]
        Item95510,

        [XmlEnum("955.11.")]
        Item95511,

        [XmlEnum("955.12.")]
        Item95512,

        [XmlEnum("955.13.")]
        Item95513,

        [XmlEnum("955.14.")]
        Item95514,

        [XmlEnum("955.15.")]
        Item95515,

        [XmlEnum("955.16.")]
        Item95516,

        [XmlEnum("955.17.")]
        Item95517,

        [XmlEnum("955.18.")]
        Item95518,

        [XmlEnum("955.19.")]
        Item95519,

        [XmlEnum("955.20.")]
        Item95520,

        [XmlEnum("955.21.")]
        Item95521,

        [XmlEnum("955.22.")]
        Item95522,

        [XmlEnum("955.23.")]
        Item95523,

        [XmlEnum("955.24.")]
        Item95524,

        [XmlEnum("955.25.")]
        Item95525,

        [XmlEnum("955.26.")]
        Item95526,

        [XmlEnum("955.27.")]
        Item95527,

        [XmlEnum("955.28.")]
        Item95528,

        [XmlEnum("955.29.")]
        Item95529,

        [XmlEnum("955.30.")]
        Item95530,

        [XmlEnum("955.31.")]
        Item95531,

        [XmlEnum("955.32.")]
        Item95532,

        [XmlEnum("955.33.")]
        Item95533,

        [XmlEnum("955.34.")]
        Item95534,

        [XmlEnum("955.35.")]
        Item95535,

        [XmlEnum("955.36.")]
        Item95536,

        [XmlEnum("955.37.")]
        Item95537,

        [XmlEnum("955.38.")]
        Item95538,

        [XmlEnum("955.39.")]
        Item95539,

        [XmlEnum("955.40.")]
        Item95540,

        [XmlEnum("955.41.")]
        Item95541,

        [XmlEnum("955.42.")]
        Item95542,

        [XmlEnum("955.43.")]
        Item95543,
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tSummary
    {
        public string EngineeringSurveySummary { get; set; }
        public string EngineeringSurveySummaryDate { get; set; }

        [XmlElement("EngineeringSurveyType")]
        public tEngineeringSurveyType[] EngineeringSurveyType { get; set; }

        public string ProjectDocumentsSummary { get; set; }
        public string ProjectDocumentsSummaryDate { get; set; }
        public string EstimateNormsAndWorksSummary { get; set; }
        public string EstimateSummary { get; set; }
        public string ExaminationSummary { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum tEngineeringSurveyType
    {
        [XmlEnum("1")]
        Item1,

        [XmlEnum("2")]
        Item2,

        [XmlEnum("3")]
        Item3,

        [XmlEnum("4")]
        Item4,

        [XmlEnum("5")]
        Item5,

        [XmlEnum("6")]
        Item6,

        [XmlEnum("7")]
        Item7,

        [XmlEnum("8")]
        Item8,

        [XmlEnum("9")]
        Item9,

        [XmlEnum("10")]
        Item10,

        [XmlEnum("11")]
        Item11,
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tMismatchExtended
    {
        public string Summary { get; set; }

        public string Part { get; set; }

        public string Link { get; set; }

        [XmlAttribute()]
        public tExpertType ExpertType { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tEstimateMismatches
    {
        [XmlElement("CommonMismatch")]
        public tMismatch[] CommonMismatch { get; set; }

        [XmlElement("FullCalculationMismatch")]
        public tMismatch[] FullCalculationMismatch { get; set; }

        [XmlElement("LocalCalculationMismatch")]
        public tMismatch[] LocalCalculationMismatch { get; set; }

        [XmlElement("ProjectDocumentsMismatch")]
        public tMismatchExtended[] ProjectDocumentsMismatch { get; set; }

        [XmlElement("BasicMismatch")]
        public tMismatch[] BasicMismatch { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tMismatch
    {
        public string Summary { get; set; }
        public string Part { get; set; }
        public string Link { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tExpertEstimate
    {
        public string EstimateNorms { get; set; }

        public tEstimateMismatches Mismatches { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tProjectDocumentsMismatches
    {
        [XmlElement("EngineeringSurveyMismatch")]
        public tMismatch[] EngineeringSurveyMismatch { get; set; }

        [XmlElement("ProjectTaskMismatch")]
        public tMismatch[] ProjectTaskMismatch { get; set; }

        [XmlElement("NormsMismatch")]
        public tMismatch[] NormsMismatch { get; set; }

        public string DangerMismatch { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tExpertProjectDocuments
    {
        [XmlAttribute()]
        public tExpertType ExpertType { get; set; }
        public string ProjectDocumentsReview { get; set; }
        public string ProjectDocumentsChanges { get; set; }
        public string DangerSolutions { get; set; }
        public tProjectDocumentsMismatches Mismatches { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tExpertEngineeringSurveys
    {
        [XmlArrayItem("NormsMismatch", IsNullable = true)]
        public tMismatch[] Mismatches { get; set; }

        [XmlAttribute()]
        public tEngineeringSurveyType EngineeringSurveyType { get; set; }
        public string EngineeringSurveyConditions { get; set; }
        public string EngineeringSurveyProgramNote { get; set; }
        public string EngineeringSurveyMethods { get; set; }
        public string EngineeringSurveyChanges { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tEngineeringSurveyAddress
    {
        public tRegionsRF EngineeringSurveyRegion { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string EngineeringSurveyDistrict { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tEEPDUse
    {
        public string EEPDNote { get; set; }

        public tConclusionNumber EEPDNumber { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime EEPDDate { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tConclusionNumber
    {
        [XmlElement("EGRZ", typeof(string))]
        [XmlElement("noEGRZ", typeof(string), DataType = "normalizedString")]
        [XmlChoiceIdentifier("ItemElementName")]
        public string Item { get; set; }

        [XmlIgnore()]
        public ItemChoiceType ItemElementName { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(IncludeInSchema = false)]
    public enum ItemChoiceType
    {
        [Description("В формате ЕГРЗ")]
        EGRZ,

        [Description("В произвольном формате")]
        noEGRZ,
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tDesigner
    {
        [XmlElement("ForeignOrganization", typeof(tForeignOrganization))]
        [XmlElement("IP", typeof(tIP))]
        [XmlElement("Organization", typeof(tOrganization))]
        public object Item { get; set; }

        [XmlAttribute()]
        public tYesNo General { get; set; }

        [XmlIgnore()]
        public bool GeneralSpecified { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tSeismicActivityCalculatedValue
    {
        public string MinValue { get; set; }

        public string MaxValue { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tClimateConditions
    {
        [XmlElement("ClimateDistrict")]
        public tClimateDistrict[] ClimateDistrict { get; set; }

        [XmlElement("GeologicalConditions")]
        public tGeologicalConditions[] GeologicalConditions { get; set; }

        [XmlElement("WindDistrict")]
        public tWindDistrict[] WindDistrict { get; set; }

        [XmlElement("SnowDistrict")]
        public tSnowDistrict[] SnowDistrict { get; set; }

        [XmlElement("SeismicActivity")]
        public tSeismicActivity[] SeismicActivity { get; set; }

        public tSeismicActivityCalculatedValue SeismicActivityCalculatedValue { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum tClimateDistrict
    {
        [XmlEnum("Сведения не представлены")]
        Сведениянепредставлены,

        I,

        IА,

        IБ,

        IВ,

        IГ,

        IД,

        II,

        IIА,

        IIБ,

        IIВ,

        IIГ,

        III,

        IIIА,

        IIIБ,

        IIIВ,

        IV,

        IVА,

        IVБ,

        IVВ,

        IVГ,
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum tGeologicalConditions
    {
        [XmlEnum("Сведения не представлены")]
        Сведениянепредставлены,

        I,

        II,

        III,
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum tWindDistrict
    {
        [XmlEnum("Сведения не представлены")]
        Сведениянепредставлены,

        Iа,

        I,

        II,

        III,

        IV,

        V,

        VI,

        VII,
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum tSnowDistrict
    {
        [XmlEnum("Сведения не представлены")]
        Сведениянепредставлены,

        I,

        II,

        III,

        IV,

        V,

        VI,

        VII,

        VIII,
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum tSeismicActivity
    {
        [XmlEnum("Сведения не представлены")]
        Сведениянепредставлены,

        [XmlEnum("5")]
        Item5,

        [XmlEnum("6")]
        Item6,

        [XmlEnum("7")]
        Item7,

        [XmlEnum("8")]
        Item8,

        [XmlEnum("9")]
        Item9,

        [XmlEnum("10")]
        Item10,
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tComplexEstimatedCost
    {
        public string CostBasic { get; set; }

        public string WorksCostBasic { get; set; }

        public string HardwareCostBasic { get; set; }

        public string OtherCostBasic { get; set; }

        public string ProjectWorksCostBasic { get; set; }

        public string BackSumCostBasic { get; set; }

        public string Cost { get; set; }

        public string WorksCost { get; set; }

        public string HardwareCost { get; set; }

        public string OtherCost { get; set; }

        public string ProjectWorksCost { get; set; }

        public string NDSCost { get; set; }

        public string BackSumCost { get; set; }

        public string CostBasicComment { get; set; }

        public string CostComment { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tEstimatedCost
    {
        public string Currency { get; set; }

        [XmlElement("EstimatedCompleteCostBefore", typeof(string))]
        [XmlElement("EstimatedCompleteCostPost", typeof(string))]
        [XmlElement("EstimatedComplexCostBefore", typeof(tComplexEstimatedCost))]
        [XmlElement("EstimatedComplexCostPost", typeof(tComplexEstimatedCost))]
        [XmlChoiceIdentifier("ItemsElementName")]
        public object[] Items { get; set; }

        [XmlElement("ItemsElementName")]
        [XmlIgnore()]
        public ItemsChoiceType[] ItemsElementName { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(IncludeInSchema = false)]
    public enum ItemsChoiceType
    {
        EstimatedCompleteCostBefore,

        EstimatedCompleteCostPost,

        EstimatedComplexCostBefore,

        EstimatedComplexCostPost,
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tFinance
    {
        public tFinanceType FinanceType { get; set; }

        public tBudgetType BudgetType { get; set; }

        [XmlIgnore()]
        public bool BudgetTypeSpecified { get; set; }

        public decimal FinanceSize { get; set; }

        [XmlIgnore()]
        public bool FinanceSizeSpecified { get; set; }

        public tTechnicalCustomer FinanceOwner { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum tFinanceType
    {
        [XmlEnum("1")]
        Item1,

        [XmlEnum("2")]
        Item2,

        [XmlEnum("3")]
        Item3,
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum tBudgetType
    {
        [XmlEnum("1")]
        Item1,

        [XmlEnum("2")]
        Item2,

        [XmlEnum("3")]
        Item3,

        [XmlEnum("4")]
        Item4,

        [XmlEnum("5")]
        Item5,
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tTechnicalCustomer : ITechnicalCustomer
    {
        [XmlElement("ForeignOrganization", typeof(tForeignOrganization))]
        [XmlElement("Organization", typeof(tOrganization))]
        public object Item { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tObjectPart
    {
        [XmlElement(DataType = "normalizedString")]
        public string Name { get; set; }

        [XmlElement("Address")]
        public tAddress[] Address { get; set; }

        [XmlElement("Functions", typeof(string), DataType = "normalizedString")]
        [XmlElement("FunctionsClass", typeof(string))]
        [XmlChoiceIdentifier("ItemElementName")]
        public string Item { get; set; }

        [XmlIgnore()]
        public ItemChoiceType2 ItemElementName { get; set; }

        [XmlElement("TEI")]
        public tTEI[] TEI { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(IncludeInSchema = false)]
    public enum ItemChoiceType2
    {
        Functions,

        FunctionsClass,
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tTEI
    {
        [XmlElement(DataType = "normalizedString")]
        public string Name { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string Measure { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string Value { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tObject
    {
        public string Name { get; set; }

        [XmlElement("Address")]
        public tAddress[] Address { get; set; }

        public tObjectType Type { get; set; }

        [XmlElement("Functions", typeof(string), DataType = "normalizedString")]
        [XmlElement("FunctionsClass", typeof(string))]
        [XmlChoiceIdentifier("ItemElementName")]
        public string Item { get; set; }

        [XmlIgnore()]
        public ItemChoiceType1 ItemElementName { get; set; }

        [XmlElement("TEI")]
        public tTEI[] TEI { get; set; }

        [XmlElement("ObjectPart")]
        public tObjectPart[] ObjectPart { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum tObjectType
    {
        [XmlEnum("1")]
        Item1,

        [XmlEnum("2")]
        Item2,

        [XmlEnum("3")]
        Item3,
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(IncludeInSchema = false)]
    public enum ItemChoiceType1
    {
        Functions,

        FunctionsClass,
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tPreviousSimpleConclusion
    {
        [XmlElement(DataType = "date")]
        public DateTime Date { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string Number { get; set; }

        public tExaminationResult Result { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum tExaminationResult
    {
        [XmlEnum("1")]
        Item1,

        [XmlEnum("2")]
        Item2,
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tPreviousConclusion
    {
        [XmlElement(DataType = "date")]
        public DateTime Date { get; set; }

        public tConclusionNumber Number { get; set; }

        public tExaminationObjectType ExaminationObjectType { get; set; }

        public string Name { get; set; }

        public tExaminationResult Result { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum tExaminationObjectType
    {
        [XmlEnum("1")]
        Item1,

        [XmlEnum("2")]
        Item2,

        [XmlEnum("3")]
        Item3,
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tSignFile
    {
        [XmlElement(DataType = "normalizedString")]
        public string FileName { get; set; }

        public string FileFormat { get; set; }

        public string FileChecksum { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tFile
    {
        [XmlElement(DataType = "normalizedString")]
        public string FileName { get; set; }

        public string FileFormat { get; set; }

        public string FileChecksum { get; set; }

        [XmlElement("SignFile")]
        public tSignFile[] SignFile { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tDeclarant
    {
        [XmlElement("ForeignOrganization", typeof(tForeignOrganization))]
        [XmlElement("IP", typeof(tIP))]
        [XmlElement("Organization", typeof(tOrganization))]
        [XmlElement("Person", typeof(tPerson))]
        public object Item { get; set; }

        public override string ToString()
        {
            return Item?.ToString() ?? base.ToString();
        }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tDocument
    {
        public string DocTypeName => DocType.GetEnumDescription();

        public tDocumentType DocType { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string DocName { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string DocNumber { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime DocDate { get; set; }

        [XmlIgnore()]
        public bool DocDateSpecified { get; set; }

        [XmlElement("DocIssueAuthor", typeof(string), DataType = "normalizedString")]
        [XmlElement("FullDocIssueAuthor", typeof(tDeclarant))]
        public object[] Items { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string DocChanges { get; set; }

        [XmlElement("File", typeof(tFile))]
        [XmlElement("Included", typeof(string), DataType = "normalizedString")]
        public object[] Items1 { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tExaminationObject
    {
        public tExaminationForm ExaminationForm { get; set; }

        public tExaminationResult ExaminationResult { get; set; }

        public tExaminationObjectType ExaminationObjectType { get; set; }

        [XmlElement("ExaminationType")]
        public tExaminationType[] ExaminationType { get; set; }

        public tConstractionType ConstructionType { get; set; }

        public tExaminationStage ExaminationStage { get; set; }

        public string ExaminationStageNote { get; set; }

        public string Name { get; set; }

        public tYesNo ProjectDocumentationIM { get; set; }

        [XmlIgnore()]
        public bool ProjectDocumentationIMSpecified { get; set; }

        public tYesNo EngineeringSurveysIM { get; set; }

        [XmlIgnore()]
        public bool EngineeringSurveysIMSpecified { get; set; }
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum tExaminationForm
    {
        [XmlEnum("1")]
        Item1,

        [XmlEnum("2")]
        Item2,
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum tExaminationType
    {
        [XmlEnum("1")]
        Item1,

        [XmlEnum("2")]
        Item2,

        [XmlEnum("3")]
        Item3,
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum tConstractionType
    {
        [XmlEnum("1")]
        Item1,

        [XmlEnum("2")]
        Item2,

        [XmlEnum("3")]
        Item3,

        [XmlEnum("4")]
        Item4,

        [XmlEnum("5")]
        Item5,
    }

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum tExaminationStage
    {
        [XmlEnum("1")]
        Item1,

        [XmlEnum("2")]
        Item2,

        [XmlEnum("3")]
        Item3,
    }
}
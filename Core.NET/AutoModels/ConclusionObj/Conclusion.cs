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
            SchemaVersion = "01.02";
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

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tAddress
    {
        [XmlElement(DataType = "normalizedString")]
        public string Country { get; set; }

        public tRegionsRF Region { get; set; }

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

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum tRegionsRF
    {
        [XmlEnum("00")]
        Item00,

        [XmlEnum("01")]
        Item01,

        [XmlEnum("02")]
        Item02,

        [XmlEnum("03")]
        Item03,

        [XmlEnum("04")]
        Item04,

        [XmlEnum("05")]
        Item05,

        [XmlEnum("06")]
        Item06,

        [XmlEnum("07")]
        Item07,

        [XmlEnum("08")]
        Item08,

        [XmlEnum("09")]
        Item09,

        [XmlEnum("10")]
        Item10,

        [XmlEnum("11")]
        Item11,

        [XmlEnum("12")]
        Item12,

        [XmlEnum("13")]
        Item13,

        [XmlEnum("14")]
        Item14,

        [XmlEnum("15")]
        Item15,

        [XmlEnum("16")]
        Item16,

        [XmlEnum("17")]
        Item17,

        [XmlEnum("18")]
        Item18,

        [XmlEnum("19")]
        Item19,

        [XmlEnum("20")]
        Item20,

        [XmlEnum("21")]
        Item21,

        [XmlEnum("22")]
        Item22,

        [XmlEnum("23")]
        Item23,

        [XmlEnum("24")]
        Item24,

        [XmlEnum("25")]
        Item25,

        [XmlEnum("26")]
        Item26,

        [XmlEnum("27")]
        Item27,

        [XmlEnum("28")]
        Item28,

        [XmlEnum("29")]
        Item29,

        [XmlEnum("30")]
        Item30,

        [XmlEnum("31")]
        Item31,

        [XmlEnum("32")]
        Item32,

        [XmlEnum("33")]
        Item33,

        [XmlEnum("34")]
        Item34,

        [XmlEnum("35")]
        Item35,

        [XmlEnum("36")]
        Item36,

        [XmlEnum("37")]
        Item37,

        [XmlEnum("38")]
        Item38,

        [XmlEnum("39")]
        Item39,

        [XmlEnum("40")]
        Item40,

        [XmlEnum("41")]
        Item41,

        [XmlEnum("42")]
        Item42,

        [XmlEnum("43")]
        Item43,

        [XmlEnum("44")]
        Item44,

        [XmlEnum("45")]
        Item45,

        [XmlEnum("46")]
        Item46,

        [XmlEnum("47")]
        Item47,

        [XmlEnum("48")]
        Item48,

        [XmlEnum("49")]
        Item49,

        [XmlEnum("50")]
        Item50,

        [XmlEnum("51")]
        Item51,

        [XmlEnum("52")]
        Item52,

        [XmlEnum("53")]
        Item53,

        [XmlEnum("54")]
        Item54,

        [XmlEnum("55")]
        Item55,

        [XmlEnum("56")]
        Item56,

        [XmlEnum("57")]
        Item57,

        [XmlEnum("58")]
        Item58,

        [XmlEnum("59")]
        Item59,

        [XmlEnum("60")]
        Item60,

        [XmlEnum("61")]
        Item61,

        [XmlEnum("62")]
        Item62,

        [XmlEnum("63")]
        Item63,

        [XmlEnum("64")]
        Item64,

        [XmlEnum("65")]
        Item65,

        [XmlEnum("66")]
        Item66,

        [XmlEnum("67")]
        Item67,

        [XmlEnum("68")]
        Item68,

        [XmlEnum("69")]
        Item69,

        [XmlEnum("70")]
        Item70,

        [XmlEnum("71")]
        Item71,

        [XmlEnum("72")]
        Item72,

        [XmlEnum("73")]
        Item73,

        [XmlEnum("74")]
        Item74,

        [XmlEnum("75")]
        Item75,

        [XmlEnum("76")]
        Item76,

        [XmlEnum("77")]
        Item77,

        [XmlEnum("78")]
        Item78,

        [XmlEnum("79")]
        Item79,

        [XmlEnum("83")]
        Item83,

        [XmlEnum("86")]
        Item86,

        [XmlEnum("87")]
        Item87,

        [XmlEnum("89")]
        Item89,

        [XmlEnum("91")]
        Item91,

        [XmlEnum("92")]
        Item92,

        [XmlEnum("80")]
        Item80,

        [XmlEnum("81")]
        Item81,

        [XmlEnum("84")]
        Item84,

        [XmlEnum("85")]
        Item85,
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
    public partial class tForeignOrganization
    {
        [XmlElement(DataType = "normalizedString")]
        public string OrgFullName { get; set; }

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

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum tYesNo
    {
        да,

        нет,
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
    public partial class tTechnicalCustomer
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

        public override string ToString()
        {
            return SNILS ?? base.ToString();
        }
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
    public enum tDocumentType
    {
        [XmlEnum("01.01")]
        Item0101,

        [XmlEnum("01.02")]
        Item0102,

        [XmlEnum("01.03")]
        Item0103,

        [XmlEnum("01.99")]
        Item0199,

        [XmlEnum("02.01")]
        Item0201,

        [XmlEnum("02.02")]
        Item0202,

        [XmlEnum("02.04")]
        Item0204,

        [XmlEnum("02.05")]
        Item0205,

        [XmlEnum("02.06")]
        Item0206,

        [XmlEnum("02.07")]
        Item0207,

        [XmlEnum("02.08")]
        Item0208,

        [XmlEnum("02.09")]
        Item0209,

        [XmlEnum("02.99")]
        Item0299,

        [XmlEnum("03.01")]
        Item0301,

        [XmlEnum("03.02")]
        Item0302,

        [XmlEnum("03.03")]
        Item0303,

        [XmlEnum("03.04")]
        Item0304,

        [XmlEnum("03.05")]
        Item0305,

        [XmlEnum("03.06")]
        Item0306,

        [XmlEnum("03.99")]
        Item0399,

        [XmlEnum("04.01")]
        Item0401,

        [XmlEnum("04.02")]
        Item0402,

        [XmlEnum("04.03")]
        Item0403,

        [XmlEnum("04.99")]
        Item0499,

        [XmlEnum("05.01")]
        Item0501,

        [XmlEnum("05.02")]
        Item0502,

        [XmlEnum("05.03")]
        Item0503,

        [XmlEnum("05.99")]
        Item0599,

        [XmlEnum("06.01")]
        Item0601,

        [XmlEnum("06.02")]
        Item0602,

        [XmlEnum("06.03")]
        Item0603,

        [XmlEnum("06.04")]
        Item0604,

        [XmlEnum("06.05")]
        Item0605,

        [XmlEnum("06.06")]
        Item0606,

        [XmlEnum("06.07")]
        Item0607,

        [XmlEnum("06.08")]
        Item0608,

        [XmlEnum("06.09")]
        Item0609,

        [XmlEnum("06.10")]
        Item0610,

        [XmlEnum("06.11")]
        Item0611,

        [XmlEnum("06.99")]
        Item0699,

        [XmlEnum("07.01")]
        Item0701,

        [XmlEnum("07.02")]
        Item0702,

        [XmlEnum("07.03")]
        Item0703,

        [XmlEnum("07.04")]
        Item0704,

        [XmlEnum("07.05")]
        Item0705,

        [XmlEnum("07.06")]
        Item0706,

        [XmlEnum("07.07")]
        Item0707,

        [XmlEnum("07.08")]
        Item0708,

        [XmlEnum("07.09")]
        Item0709,

        [XmlEnum("07.10")]
        Item0710,

        [XmlEnum("07.11")]
        Item0711,

        [XmlEnum("07.12")]
        Item0712,

        [XmlEnum("07.13")]
        Item0713,

        [XmlEnum("07.14")]
        Item0714,

        [XmlEnum("07.15")]
        Item0715,

        [XmlEnum("07.16")]
        Item0716,

        [XmlEnum("07.17")]
        Item0717,

        [XmlEnum("07.19")]
        Item0719,

        [XmlEnum("07.20")]
        Item0720,

        [XmlEnum("07.21")]
        Item0721,

        [XmlEnum("07.22")]
        Item0722,

        [XmlEnum("07.23")]
        Item0723,

        [XmlEnum("07.24")]
        Item0724,

        [XmlEnum("07.25")]
        Item0725,

        [XmlEnum("07.26")]
        Item0726,

        [XmlEnum("07.27")]
        Item0727,

        [XmlEnum("07.28")]
        Item0728,

        [XmlEnum("07.29")]
        Item0729,

        [XmlEnum("07.30")]
        Item0730,

        [XmlEnum("07.31")]
        Item0731,

        [XmlEnum("07.32")]
        Item0732,

        [XmlEnum("07.33")]
        Item0733,

        [XmlEnum("07.34")]
        Item0734,

        [XmlEnum("07.35")]
        Item0735,

        [XmlEnum("07.37")]
        Item0737,

        [XmlEnum("08.01")]
        Item0801,

        [XmlEnum("08.02")]
        Item0802,

        [XmlEnum("08.03")]
        Item0803,

        [XmlEnum("08.04")]
        Item0804,

        [XmlEnum("08.05")]
        Item0805,

        [XmlEnum("08.06")]
        Item0806,

        [XmlEnum("08.07")]
        Item0807,

        [XmlEnum("08.08")]
        Item0808,

        [XmlEnum("08.10")]
        Item0810,

        [XmlEnum("08.11")]
        Item0811,

        [XmlEnum("08.12")]
        Item0812,

        [XmlEnum("08.13")]
        Item0813,

        [XmlEnum("08.14")]
        Item0814,

        [XmlEnum("08.15")]
        Item0815,

        [XmlEnum("08.16")]
        Item0816,

        [XmlEnum("08.17")]
        Item0817,

        [XmlEnum("08.18")]
        Item0818,

        [XmlEnum("08.20")]
        Item0820,

        [XmlEnum("09.01")]
        Item0901,

        [XmlEnum("09.02")]
        Item0902,

        [XmlEnum("09.99")]
        Item0999,

        [XmlEnum("10.01")]
        Item1001,

        [XmlEnum("10.99")]
        Item1099,

        [XmlEnum("11.01")]
        Item1101,

        [XmlEnum("11.02")]
        Item1102,

        [XmlEnum("11.03")]
        Item1103,

        [XmlEnum("11.04")]
        Item1104,

        [XmlEnum("11.05")]
        Item1105,

        [XmlEnum("11.06")]
        Item1106,

        [XmlEnum("11.99")]
        Item1199,

        [XmlEnum("12.01")]
        Item1201,

        [XmlEnum("12.02")]
        Item1202,

        [XmlEnum("12.03")]
        Item1203,

        [XmlEnum("12.99")]
        Item1299,

        [XmlEnum("13.01")]
        Item1301,

        [XmlEnum("13.02")]
        Item1302,

        [XmlEnum("13.03")]
        Item1303,

        [XmlEnum("13.04")]
        Item1304,

        [XmlEnum("13.05")]
        Item1305,

        [XmlEnum("13.06")]
        Item1306,

        [XmlEnum("13.99")]
        Item1399,

        [XmlEnum("14.01")]
        Item1401,

        [XmlEnum("14.02")]
        Item1402,

        [XmlEnum("14.03")]
        Item1403,

        [XmlEnum("14.04")]
        Item1404,

        [XmlEnum("14.99")]
        Item1499,

        [XmlEnum("15.01")]
        Item1501,

        [XmlEnum("15.02")]
        Item1502,

        [XmlEnum("15.03")]
        Item1503,

        [XmlEnum("15.04")]
        Item1504,

        [XmlEnum("15.05")]
        Item1505,

        [XmlEnum("15.06")]
        Item1506,

        [XmlEnum("15.07")]
        Item1507,

        [XmlEnum("15.08")]
        Item1508,

        [XmlEnum("15.99")]
        Item1599,

        [XmlEnum("16.01")]
        Item1601,

        [XmlEnum("16.02")]
        Item1602,

        [XmlEnum("16.99")]
        Item1699,

        [XmlEnum("17.01")]
        Item1701,

        [XmlEnum("17.99")]
        Item1799,

        [XmlEnum("18.01")]
        Item1801,

        [XmlEnum("18.02")]
        Item1802,

        [XmlEnum("18.03")]
        Item1803,

        [XmlEnum("18.99")]
        Item1899,

        [XmlEnum("19.01")]
        Item1901,

        [XmlEnum("19.02")]
        Item1902,

        [XmlEnum("19.03")]
        Item1903,

        [XmlEnum("19.99")]
        Item1999,

        [XmlEnum("20.01")]
        Item2001,

        [XmlEnum("20.02")]
        Item2002,

        [XmlEnum("20.03")]
        Item2003,

        [XmlEnum("20.04")]
        Item2004,

        [XmlEnum("20.05")]
        Item2005,

        [XmlEnum("20.06")]
        Item2006,

        [XmlEnum("20.07")]
        Item2007,

        [XmlEnum("20.99")]
        Item2099,

        [XmlEnum("21.01")]
        Item2101,

        [XmlEnum("21.02")]
        Item2102,

        [XmlEnum("21.03")]
        Item2103,

        [XmlEnum("21.04")]
        Item2104,

        [XmlEnum("21.05")]
        Item2105,

        [XmlEnum("21.06")]
        Item2106,

        [XmlEnum("21.07")]
        Item2107,

        [XmlEnum("21.08")]
        Item2108,

        [XmlEnum("21.09")]
        Item2109,

        [XmlEnum("21.10")]
        Item2110,

        [XmlEnum("21.11")]
        Item2111,

        [XmlEnum("21.12")]
        Item2112,

        [XmlEnum("21.13")]
        Item2113,

        [XmlEnum("21.99")]
        Item2199,

        [XmlEnum("22.01")]
        Item2201,

        [XmlEnum("22.02")]
        Item2202,

        [XmlEnum("22.03")]
        Item2203,

        [XmlEnum("22.04")]
        Item2204,

        [XmlEnum("22.05")]
        Item2205,

        [XmlEnum("22.99")]
        Item2299,

        [XmlEnum("23.01")]
        Item2301,

        [XmlEnum("23.02")]
        Item2302,

        [XmlEnum("23.99")]
        Item2399,

        [XmlEnum("24.01")]
        Item2401,

        [XmlEnum("25.01")]
        Item2501,

        [XmlEnum("25.02")]
        Item2502,

        [XmlEnum("25.03")]
        Item2503,

        [XmlEnum("25.04")]
        Item2504,

        [XmlEnum("25.05")]
        Item2505,

        [XmlEnum("25.06")]
        Item2506,

        [XmlEnum("99.01")]
        Item9901,

        [XmlEnum("99.02")]
        Item9902,

        [XmlEnum("99.03")]
        Item9903,

        [XmlEnum("99.99")]
        Item9999,
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

    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class tWorkPerson
    {
        [XmlElement(DataType = "normalizedString")]
        public string FamilyName { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string FirstName { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string SecondName { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string Position { get; set; }
    }
}
// Ignore Spelling: TEI Cadastral Geotechnical Technogenic Geodynamical Aerological Hydrogeological Perair Org OGRNIP SNILS Safity

using Core.NET.Extensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Core.NET.AutoModels.ExplanatoryNoteObj
{
    [XmlType(AnonymousType = true)]
    public partial class ExplanatoryNote
    {
        public ExplanatoryNote()
        {
            SchemaVersion = "01.03";
        }

        public string ExplanatoryNoteNumber { get; set; }

        [XmlElement(DataType = "integer")]
        public string ExplanatoryNoteYear { get; set; }

        [XmlArrayItem("Modification", IsNullable = false)]
        public tModification[] ExplanatoryNoteModifications { get; set; }

        public tAuthor IssueAuthor { get; set; }

        [XmlArrayItem("ProjectDocumentationAuthor", IsNullable = false)]
        public tAuthor[] ProjectDocumentationAuthors { get; set; }

        [XmlArrayItem("Signer", IsNullable = false)]
        public tWorkPerson[] Signers { get; set; }

        [XmlElement("Developer", typeof(tDeveloper))]
        [XmlElement("TechnicalCustomer", typeof(tTechnicalCustomer))]
        public object[] Items { get; set; }

        public tTextBlock UsedAbbreviations { get; set; }

        //[XmlArrayItem("UsedNorm", IsNullable = false)]
        //public string[] UsedNorms { get; set; }

        public tUsedNorms UsedNorms { get; set; }

        [XmlArrayItem("Document", IsNullable = false)]
        public tDocument[] ProjectDecisionDocuments { get; set; }

        [XmlArrayItem("Document", IsNullable = false)]
        public tDocument[] ProjectInitialDocuments { get; set; }

        [XmlArrayItem("Document", IsNullable = false)]
        public tEngineeringSurveyDocument[] EngineeringSurveyDocuments { get; set; }

        [XmlArrayItem("Data", IsNullable = false)]
        public tData[] AdditionalData { get; set; }

        public tFinanceSources FinanceSources { get; set; }

        [XmlElement("IndustrialObject", typeof(tIndustrialObject))]
        [XmlElement("LinearObject", typeof(tLinearObject))]
        [XmlElement("NonIndustrialObject", typeof(tNonIndustrialObject))]
        public object Item { get; set; }

        [XmlElement("DesignerNote")]
        public tTextBlock[] DesignerNote { get; set; }

        public tDesignerAssurance DesignerAssurance { get; set; }

        [XmlAttribute()]
        public string SchemaVersion { get; set; }

        [XmlAttribute()]
        public string SchemaLink { get; set; }

        [XmlAttribute()]
        public string id { get; set; }

        [XmlAttribute()]
        public string AccessRestriction { get; set; }
    }

    public partial class tNonIndustrialObject
    {
        public string Name { get; set; }

        public tConstructionType ConstructionType { get; set; }

        public decimal ConstructionDuration { get; set; }

        public tConstructionDates ConstructionDates { get; set; }

        public tAddress Address { get; set; }

        public tFunctions Functions { get; set; }

        public tTextBlock FunctionsFeatures { get; set; }

        [XmlElement("PowerIndicator")]
        public tTEI[] PowerIndicator { get; set; }

        [XmlElement("TEI")]
        public tTEI[] TEI { get; set; }

        public tEnergyEfficiency EnergyEfficiency { get; set; }

        [XmlElement("FireDangerCategory", typeof(tFireDangerCategory))]
        [XmlElement("ObjectParts", typeof(tObjectParts1))]
        [XmlElement("PeoplePermanentStay", typeof(tTextBlock))]
        [XmlElement("ResponsibilityLevel", typeof(tResponsibilityLevel))]
        public object[] Items { get; set; }

        public tClimateConditions ClimateConditions { get; set; }

        public tResources Resources { get; set; }

        public tTextBlock RenewableSourceInfo { get; set; }

        public tLandCategory LandCategory { get; set; }

        public tLandInfo LandInfo { get; set; }

        public tTextBlock LandFunds { get; set; }

        public tTextBlock PatentsUsed { get; set; }

        public tTextBlock SpecialRequirements { get; set; }

        public tTextBlock StaffNote { get; set; }

        public tSoftwareUsed SoftwareUsed { get; set; }

        public tStagesInfo StagesInfo { get; set; }

        public tTextBlock DestroyFunds { get; set; }

        [XmlArrayItem("ProjectDocumentationLink", IsNullable = false)]
        public tProjectDocumentationLink[] EnergyEfficiencyLinks { get; set; }

        public tTextBlock LandReclamationLink { get; set; }

        public tNonLinearProjectDocumentContent ProjectDocumentation { get; set; }

        public tModel Model { get; set; }

        [XmlAttribute(DataType = "ID")]
        public string ObjectID { get; set; }

        [XmlAttribute()]
        public tPlacement Placement { get; set; }

        [XmlAttribute()]
        public tDangerousAndComplex DangerousAndComplex { get; set; }

        [XmlIgnore()]
        public bool DangerousAndComplexSpecified { get; set; }

        [XmlAttribute()]
        public tUnique Unique { get; set; }

        [XmlIgnore()]
        public bool UniqueSpecified { get; set; }
    }

    public enum tConstructionType
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

    public partial class tConstructionDates
    {
        [XmlElement(DataType = "date")]
        public System.DateTime BeginDate { get; set; }

        [XmlIgnore()]
        public bool BeginDateSpecified { get; set; }

        [XmlElement(DataType = "date")]
        public System.DateTime EndDate { get; set; }

        [XmlIgnore()]
        public bool EndDateSpecified { get; set; }

        [XmlElement(DataType = "date")]
        public System.DateTime OperationDate { get; set; }

        [XmlIgnore()]
        public bool OperationDateSpecified { get; set; }
    }

    [XmlType(TypeName = "tObjectParts")]
    public partial class tObjectParts1
    {
        [XmlElement("ComplexPart", typeof(tComplexPart))]
        [XmlElement("OKS", typeof(tOKS))]
        public object[] Items { get; set; }
    }

    public partial class tComplexPart
    {
        public string Name { get; set; }

        [XmlElement("Address")]
        public tAddress[] Address { get; set; }

        public string FunctionsClass { get; set; }

        public tTextBlock FunctionsFeatures { get; set; }

        [XmlElement("PowerIndicator")]
        public tTEI[] PowerIndicator { get; set; }

        [XmlElement("TEI")]
        public tTEI[] TEI { get; set; }

        public tDangerIndustrialClass DangerousIndustrialObject { get; set; }

        [XmlArrayItem("ComplexPart", typeof(tComplexPart), IsNullable = false)]
        [XmlArrayItem("OKS", typeof(tOKS), IsNullable = false)]
        public object[] ObjectParts { get; set; }

        public tModel Model { get; set; }

        [XmlAttribute(DataType = "ID")]
        public string ObjectID { get; set; }
    }

    public partial class tTextBlock
    {
        [XmlElement("Image", typeof(tImage))]
        [XmlElement("SubTitle", typeof(string))]
        [XmlElement("Table", typeof(tTable))]
        [XmlElement("Text", typeof(string))]
        [XmlChoiceIdentifier("ItemsElementName")]
        public object[] Items { get; set; }

        [XmlElement("ItemsElementName")]
        [XmlIgnore()]
        public ItemsChoiceType[] ItemsElementName { get; set; }

        [XmlAttribute()]
        public string Title { get; set; }
    }

    public partial class tImage
    {
        [XmlElement(DataType = "base64Binary")]
        public byte[] ImageData { get; set; }

        public string Comment { get; set; }

        [XmlAttribute()]
        public tImageTypes Type { get; set; }
    }

    public enum tImageTypes
    {
        jpg,

        gif,

        png,

        jpeg,
    }

    public partial class tTable
    {
        [XmlArrayItem("Cell", typeof(tCell))]
        public List<tCell> TitleRow { get; set; }

        [XmlArrayItem("Cell", typeof(tCell))]
        public List<tCell> Row { get; set; }
    }

    public partial class tCell
    {
        [XmlAttribute()]
        public tAlignTypes align { get; set; }

        [XmlIgnore()]
        public bool alignSpecified { get; set; }

        [XmlText()]
        public string Value { get; set; }
    }

    public enum tAlignTypes
    {
        left,

        right,

        center,
    }

    [XmlType(IncludeInSchema = false)]
    public enum ItemsChoiceType
    {
        Image,

        SubTitle,

        Table,

        Text,
    }

    public partial class tTEI
    {
        public string Name { get; set; }

        public tOKEI Measure { get; set; }

        public string Value { get; set; }

        public string OldValue { get; set; }
    }

    public enum tOKEI
    {
        [XmlEnum("-")]
        Item,

        [XmlEnum("003")]
        Item003,

        [XmlEnum("004")]
        Item004,

        [XmlEnum("005")]
        Item005,

        [XmlEnum("006")]
        Item006,

        [XmlEnum("008")]
        Item008,

        [XmlEnum("009")]
        Item009,

        [XmlEnum("039")]
        Item039,

        [XmlEnum("041")]
        Item041,

        [XmlEnum("043")]
        Item043,

        [XmlEnum("047")]
        Item047,

        [XmlEnum("050")]
        Item050,

        [XmlEnum("051")]
        Item051,

        [XmlEnum("053")]
        Item053,

        [XmlEnum("055")]
        Item055,

        [XmlEnum("058")]
        Item058,

        [XmlEnum("059")]
        Item059,

        [XmlEnum("061")]
        Item061,

        [XmlEnum("071")]
        Item071,

        [XmlEnum("073")]
        Item073,

        [XmlEnum("075")]
        Item075,

        [XmlEnum("109")]
        Item109,

        [XmlEnum("110")]
        Item110,

        [XmlEnum("111")]
        Item111,

        [XmlEnum("112")]
        Item112,

        [XmlEnum("113")]
        Item113,

        [XmlEnum("118")]
        Item118,

        [XmlEnum("122")]
        Item122,

        [XmlEnum("126")]
        Item126,

        [XmlEnum("131")]
        Item131,

        [XmlEnum("132")]
        Item132,

        [XmlEnum("133")]
        Item133,

        [XmlEnum("159")]
        Item159,

        [XmlEnum("160")]
        Item160,

        [XmlEnum("161")]
        Item161,

        [XmlEnum("162")]
        Item162,

        [XmlEnum("163")]
        Item163,

        [XmlEnum("166")]
        Item166,

        [XmlEnum("168")]
        Item168,

        [XmlEnum("170")]
        Item170,

        [XmlEnum("173")]
        Item173,

        [XmlEnum("181")]
        Item181,

        [XmlEnum("185")]
        Item185,

        [XmlEnum("206")]
        Item206,

        [XmlEnum("212")]
        Item212,

        [XmlEnum("214")]
        Item214,

        [XmlEnum("215")]
        Item215,

        [XmlEnum("222")]
        Item222,

        [XmlEnum("223")]
        Item223,

        [XmlEnum("227")]
        Item227,

        [XmlEnum("228")]
        Item228,

        [XmlEnum("230")]
        Item230,

        [XmlEnum("243")]
        Item243,

        [XmlEnum("245")]
        Item245,

        [XmlEnum("246")]
        Item246,

        [XmlEnum("247")]
        Item247,

        [XmlEnum("260")]
        Item260,

        [XmlEnum("263")]
        Item263,

        [XmlEnum("264")]
        Item264,

        [XmlEnum("270")]
        Item270,

        [XmlEnum("271")]
        Item271,

        [XmlEnum("273")]
        Item273,

        [XmlEnum("274")]
        Item274,

        [XmlEnum("280")]
        Item280,

        [XmlEnum("281")]
        Item281,

        [XmlEnum("282")]
        Item282,

        [XmlEnum("283")]
        Item283,

        [XmlEnum("284")]
        Item284,

        [XmlEnum("288")]
        Item288,

        [XmlEnum("289")]
        Item289,

        [XmlEnum("290")]
        Item290,

        [XmlEnum("291")]
        Item291,

        [XmlEnum("292")]
        Item292,

        [XmlEnum("294")]
        Item294,

        [XmlEnum("296")]
        Item296,

        [XmlEnum("297")]
        Item297,

        [XmlEnum("298")]
        Item298,

        [XmlEnum("300")]
        Item300,

        [XmlEnum("301")]
        Item301,

        [XmlEnum("302")]
        Item302,

        [XmlEnum("304")]
        Item304,

        [XmlEnum("305")]
        Item305,

        [XmlEnum("306")]
        Item306,

        [XmlEnum("308")]
        Item308,

        [XmlEnum("309")]
        Item309,

        [XmlEnum("310")]
        Item310,

        [XmlEnum("312")]
        Item312,

        [XmlEnum("314")]
        Item314,

        [XmlEnum("316")]
        Item316,

        [XmlEnum("323")]
        Item323,

        [XmlEnum("324")]
        Item324,

        [XmlEnum("327")]
        Item327,

        [XmlEnum("328")]
        Item328,

        [XmlEnum("330")]
        Item330,

        [XmlEnum("331")]
        Item331,

        [XmlEnum("333")]
        Item333,

        [XmlEnum("335")]
        Item335,

        [XmlEnum("349")]
        Item349,

        [XmlEnum("354")]
        Item354,

        [XmlEnum("355")]
        Item355,

        [XmlEnum("356")]
        Item356,

        [XmlEnum("359")]
        Item359,

        [XmlEnum("360")]
        Item360,

        [XmlEnum("361")]
        Item361,

        [XmlEnum("362")]
        Item362,

        [XmlEnum("364")]
        Item364,

        [XmlEnum("365")]
        Item365,

        [XmlEnum("366")]
        Item366,

        [XmlEnum("368")]
        Item368,

        [XmlEnum("499")]
        Item499,

        [XmlEnum("533")]
        Item533,

        [XmlEnum("596")]
        Item596,

        [XmlEnum("598")]
        Item598,

        [XmlEnum("599")]
        Item599,

        [XmlEnum("616")]
        Item616,

        [XmlEnum("625")]
        Item625,

        [XmlEnum("626")]
        Item626,

        [XmlEnum("630")]
        Item630,

        [XmlEnum("641")]
        Item641,

        [XmlEnum("657")]
        Item657,

        [XmlEnum("683")]
        Item683,

        [XmlEnum("704")]
        Item704,

        [XmlEnum("715")]
        Item715,

        [XmlEnum("730")]
        Item730,

        [XmlEnum("732")]
        Item732,

        [XmlEnum("733")]
        Item733,

        [XmlEnum("734")]
        Item734,

        [XmlEnum("735")]
        Item735,

        [XmlEnum("736")]
        Item736,

        [XmlEnum("737")]
        Item737,

        [XmlEnum("740")]
        Item740,

        [XmlEnum("745")]
        Item745,

        [XmlEnum("778")]
        Item778,

        [XmlEnum("780")]
        Item780,

        [XmlEnum("781")]
        Item781,

        [XmlEnum("796")]
        Item796,

        [XmlEnum("797")]
        Item797,

        [XmlEnum("798")]
        Item798,

        [XmlEnum("799")]
        Item799,

        [XmlEnum("800")]
        Item800,

        [XmlEnum("801")]
        Item801,

        [XmlEnum("802")]
        Item802,

        [XmlEnum("820")]
        Item820,

        [XmlEnum("821")]
        Item821,

        [XmlEnum("831")]
        Item831,

        [XmlEnum("833")]
        Item833,

        [XmlEnum("841")]
        Item841,

        [XmlEnum("845")]
        Item845,

        [XmlEnum("847")]
        Item847,

        [XmlEnum("852")]
        Item852,

        [XmlEnum("859")]
        Item859,

        [XmlEnum("861")]
        Item861,

        [XmlEnum("863")]
        Item863,

        [XmlEnum("865")]
        Item865,

        [XmlEnum("867")]
        Item867,

        [XmlEnum("018")]
        Item018,

        [XmlEnum("019")]
        Item019,

        [XmlEnum("020")]
        Item020,

        [XmlEnum("048")]
        Item048,

        [XmlEnum("049")]
        Item049,

        [XmlEnum("054")]
        Item054,

        [XmlEnum("056")]
        Item056,

        [XmlEnum("057")]
        Item057,

        [XmlEnum("060")]
        Item060,

        [XmlEnum("062")]
        Item062,

        [XmlEnum("063")]
        Item063,

        [XmlEnum("064")]
        Item064,

        [XmlEnum("081")]
        Item081,

        [XmlEnum("082")]
        Item082,

        [XmlEnum("083")]
        Item083,

        [XmlEnum("084")]
        Item084,

        [XmlEnum("085")]
        Item085,

        [XmlEnum("086")]
        Item086,

        [XmlEnum("087")]
        Item087,

        [XmlEnum("088")]
        Item088,

        [XmlEnum("089")]
        Item089,

        [XmlEnum("114")]
        Item114,

        [XmlEnum("115")]
        Item115,

        [XmlEnum("116")]
        Item116,

        [XmlEnum("119")]
        Item119,

        [XmlEnum("120")]
        Item120,

        [XmlEnum("121")]
        Item121,

        [XmlEnum("123")]
        Item123,

        [XmlEnum("124")]
        Item124,

        [XmlEnum("125")]
        Item125,

        [XmlEnum("127")]
        Item127,

        [XmlEnum("128")]
        Item128,

        [XmlEnum("129")]
        Item129,

        [XmlEnum("130")]
        Item130,

        [XmlEnum("165")]
        Item165,

        [XmlEnum("167")]
        Item167,

        [XmlEnum("169")]
        Item169,

        [XmlEnum("171")]
        Item171,

        [XmlEnum("172")]
        Item172,

        [XmlEnum("175")]
        Item175,

        [XmlEnum("176")]
        Item176,

        [XmlEnum("177")]
        Item177,

        [XmlEnum("178")]
        Item178,

        [XmlEnum("179")]
        Item179,

        [XmlEnum("207")]
        Item207,

        [XmlEnum("226")]
        Item226,

        [XmlEnum("231")]
        Item231,

        [XmlEnum("232")]
        Item232,

        [XmlEnum("233")]
        Item233,

        [XmlEnum("234")]
        Item234,

        [XmlEnum("235")]
        Item235,

        [XmlEnum("236")]
        Item236,

        [XmlEnum("237")]
        Item237,

        [XmlEnum("238")]
        Item238,

        [XmlEnum("239")]
        Item239,

        [XmlEnum("241")]
        Item241,

        [XmlEnum("242")]
        Item242,

        [XmlEnum("248")]
        Item248,

        [XmlEnum("249")]
        Item249,

        [XmlEnum("250")]
        Item250,

        [XmlEnum("251")]
        Item251,

        [XmlEnum("252")]
        Item252,

        [XmlEnum("253")]
        Item253,

        [XmlEnum("254")]
        Item254,

        [XmlEnum("255")]
        Item255,

        [XmlEnum("256")]
        Item256,

        [XmlEnum("257")]
        Item257,

        [XmlEnum("258")]
        Item258,

        [XmlEnum("287")]
        Item287,

        [XmlEnum("313")]
        Item313,

        [XmlEnum("317")]
        Item317,

        [XmlEnum("337")]
        Item337,

        [XmlEnum("338")]
        Item338,

        [XmlEnum("339")]
        Item339,

        [XmlEnum("352")]
        Item352,

        [XmlEnum("353")]
        Item353,

        [XmlEnum("383")]
        Item383,

        [XmlEnum("384")]
        Item384,

        [XmlEnum("385")]
        Item385,

        [XmlEnum("386")]
        Item386,

        [XmlEnum("387")]
        Item387,

        [XmlEnum("388")]
        Item388,

        [XmlEnum("414")]
        Item414,

        [XmlEnum("421")]
        Item421,

        [XmlEnum("423")]
        Item423,

        [XmlEnum("424")]
        Item424,

        [XmlEnum("427")]
        Item427,

        [XmlEnum("449")]
        Item449,

        [XmlEnum("450")]
        Item450,

        [XmlEnum("451")]
        Item451,

        [XmlEnum("479")]
        Item479,

        [XmlEnum("510")]
        Item510,

        [XmlEnum("511")]
        Item511,

        [XmlEnum("512")]
        Item512,

        [XmlEnum("513")]
        Item513,

        [XmlEnum("514")]
        Item514,

        [XmlEnum("515")]
        Item515,

        [XmlEnum("516")]
        Item516,

        [XmlEnum("521")]
        Item521,

        [XmlEnum("522")]
        Item522,

        [XmlEnum("534")]
        Item534,

        [XmlEnum("535")]
        Item535,

        [XmlEnum("536")]
        Item536,

        [XmlEnum("537")]
        Item537,

        [XmlEnum("538")]
        Item538,

        [XmlEnum("539")]
        Item539,

        [XmlEnum("540")]
        Item540,

        [XmlEnum("541")]
        Item541,

        [XmlEnum("542")]
        Item542,

        [XmlEnum("543")]
        Item543,

        [XmlEnum("544")]
        Item544,

        [XmlEnum("545")]
        Item545,

        [XmlEnum("546")]
        Item546,

        [XmlEnum("547")]
        Item547,

        [XmlEnum("548")]
        Item548,

        [XmlEnum("550")]
        Item550,

        [XmlEnum("552")]
        Item552,

        [XmlEnum("553")]
        Item553,

        [XmlEnum("554")]
        Item554,

        [XmlEnum("555")]
        Item555,

        [XmlEnum("556")]
        Item556,

        [XmlEnum("557")]
        Item557,

        [XmlEnum("558")]
        Item558,

        [XmlEnum("559")]
        Item559,

        [XmlEnum("2541")]
        Item2541,

        [XmlEnum("2543")]
        Item2543,

        [XmlEnum("561")]
        Item561,

        [XmlEnum("562")]
        Item562,

        [XmlEnum("563")]
        Item563,

        [XmlEnum("639")]
        Item639,

        [XmlEnum("640")]
        Item640,

        [XmlEnum("642")]
        Item642,

        [XmlEnum("643")]
        Item643,

        [XmlEnum("644")]
        Item644,

        [XmlEnum("661")]
        Item661,

        [XmlEnum("673")]
        Item673,

        [XmlEnum("698")]
        Item698,

        [XmlEnum("699")]
        Item699,

        [XmlEnum("709")]
        Item709,

        [XmlEnum("724")]
        Item724,

        [XmlEnum("729")]
        Item729,

        [XmlEnum("744")]
        Item744,

        [XmlEnum("746")]
        Item746,

        [XmlEnum("751")]
        Item751,

        [XmlEnum("761")]
        Item761,

        [XmlEnum("762")]
        Item762,

        [XmlEnum("775")]
        Item775,

        [XmlEnum("776")]
        Item776,

        [XmlEnum("779")]
        Item779,

        [XmlEnum("782")]
        Item782,

        [XmlEnum("792")]
        Item792,

        [XmlEnum("793")]
        Item793,

        [XmlEnum("794")]
        Item794,

        [XmlEnum("808")]
        Item808,

        [XmlEnum("810")]
        Item810,

        [XmlEnum("812")]
        Item812,

        [XmlEnum("836")]
        Item836,

        [XmlEnum("837")]
        Item837,

        [XmlEnum("838")]
        Item838,

        [XmlEnum("839")]
        Item839,

        [XmlEnum("840")]
        Item840,

        [XmlEnum("868")]
        Item868,

        [XmlEnum("869")]
        Item869,

        [XmlEnum("870")]
        Item870,

        [XmlEnum("871")]
        Item871,

        [XmlEnum("872")]
        Item872,

        [XmlEnum("873")]
        Item873,

        [XmlEnum("874")]
        Item874,

        [XmlEnum("875")]
        Item875,

        [XmlEnum("876")]
        Item876,

        [XmlEnum("877")]
        Item877,

        [XmlEnum("878")]
        Item878,

        [XmlEnum("879")]
        Item879,

        [XmlEnum("880")]
        Item880,

        [XmlEnum("881")]
        Item881,

        [XmlEnum("882")]
        Item882,

        [XmlEnum("883")]
        Item883,

        [XmlEnum("884")]
        Item884,

        [XmlEnum("885")]
        Item885,

        [XmlEnum("886")]
        Item886,

        [XmlEnum("887")]
        Item887,

        [XmlEnum("888")]
        Item888,

        [XmlEnum("889")]
        Item889,

        [XmlEnum("890")]
        Item890,

        [XmlEnum("891")]
        Item891,

        [XmlEnum("892")]
        Item892,

        [XmlEnum("893")]
        Item893,

        [XmlEnum("894")]
        Item894,

        [XmlEnum("895")]
        Item895,

        [XmlEnum("896")]
        Item896,

        [XmlEnum("897")]
        Item897,

        [XmlEnum("898")]
        Item898,

        [XmlEnum("899")]
        Item899,

        [XmlEnum("900")]
        Item900,

        [XmlEnum("901")]
        Item901,

        [XmlEnum("902")]
        Item902,

        [XmlEnum("903")]
        Item903,

        [XmlEnum("904")]
        Item904,

        [XmlEnum("905")]
        Item905,

        [XmlEnum("906")]
        Item906,

        [XmlEnum("907")]
        Item907,

        [XmlEnum("908")]
        Item908,

        [XmlEnum("909")]
        Item909,

        [XmlEnum("910")]
        Item910,

        [XmlEnum("911")]
        Item911,

        [XmlEnum("912")]
        Item912,

        [XmlEnum("913")]
        Item913,

        [XmlEnum("914")]
        Item914,

        [XmlEnum("915")]
        Item915,

        [XmlEnum("916")]
        Item916,

        [XmlEnum("917")]
        Item917,

        [XmlEnum("918")]
        Item918,

        [XmlEnum("920")]
        Item920,

        [XmlEnum("921")]
        Item921,

        [XmlEnum("922")]
        Item922,

        [XmlEnum("923")]
        Item923,

        [XmlEnum("924")]
        Item924,

        [XmlEnum("925")]
        Item925,

        [XmlEnum("930")]
        Item930,

        [XmlEnum("937")]
        Item937,

        [XmlEnum("949")]
        Item949,

        [XmlEnum("950")]
        Item950,

        [XmlEnum("951")]
        Item951,

        [XmlEnum("952")]
        Item952,

        [XmlEnum("953")]
        Item953,

        [XmlEnum("954")]
        Item954,

        [XmlEnum("955")]
        Item955,

        [XmlEnum("956")]
        Item956,

        [XmlEnum("957")]
        Item957,

        [XmlEnum("958")]
        Item958,

        [XmlEnum("959")]
        Item959,

        [XmlEnum("960")]
        Item960,

        [XmlEnum("961")]
        Item961,

        [XmlEnum("962")]
        Item962,

        [XmlEnum("963")]
        Item963,

        [XmlEnum("964")]
        Item964,

        [XmlEnum("965")]
        Item965,

        [XmlEnum("966")]
        Item966,

        [XmlEnum("967")]
        Item967,

        [XmlEnum("968")]
        Item968,

        [XmlEnum("969")]
        Item969,

        [XmlEnum("970")]
        Item970,

        [XmlEnum("971")]
        Item971,

        [XmlEnum("972")]
        Item972,

        [XmlEnum("973")]
        Item973,

        [XmlEnum("974")]
        Item974,

        [XmlEnum("975")]
        Item975,

        [XmlEnum("976")]
        Item976,

        [XmlEnum("977")]
        Item977,

        [XmlEnum("978")]
        Item978,

        [XmlEnum("979")]
        Item979,

        [XmlEnum("980")]
        Item980,

        [XmlEnum("981")]
        Item981,

        [XmlEnum("982")]
        Item982,

        [XmlEnum("983")]
        Item983,

        [XmlEnum("017")]
        Item017,

        [XmlEnum("045")]
        Item045,

        [XmlEnum("077")]
        Item077,

        [XmlEnum("079")]
        Item079,

        [XmlEnum("135")]
        Item135,

        [XmlEnum("136")]
        Item136,

        [XmlEnum("137")]
        Item137,

        [XmlEnum("138")]
        Item138,

        [XmlEnum("139")]
        Item139,

        [XmlEnum("140")]
        Item140,

        [XmlEnum("141")]
        Item141,

        [XmlEnum("142")]
        Item142,

        [XmlEnum("143")]
        Item143,

        [XmlEnum("144")]
        Item144,

        [XmlEnum("145")]
        Item145,

        [XmlEnum("146")]
        Item146,

        [XmlEnum("147")]
        Item147,

        [XmlEnum("148")]
        Item148,

        [XmlEnum("149")]
        Item149,

        [XmlEnum("150")]
        Item150,

        [XmlEnum("151")]
        Item151,

        [XmlEnum("152")]
        Item152,

        [XmlEnum("153")]
        Item153,

        [XmlEnum("154")]
        Item154,

        [XmlEnum("182")]
        Item182,

        [XmlEnum("183")]
        Item183,

        [XmlEnum("184")]
        Item184,

        [XmlEnum("186")]
        Item186,

        [XmlEnum("187")]
        Item187,

        [XmlEnum("188")]
        Item188,

        [XmlEnum("189")]
        Item189,

        [XmlEnum("190")]
        Item190,

        [XmlEnum("191")]
        Item191,

        [XmlEnum("192")]
        Item192,

        [XmlEnum("193")]
        Item193,

        [XmlEnum("194")]
        Item194,

        [XmlEnum("195")]
        Item195,

        [XmlEnum("196")]
        Item196,

        [XmlEnum("197")]
        Item197,

        [XmlEnum("198")]
        Item198,

        [XmlEnum("199")]
        Item199,

        [XmlEnum("200")]
        Item200,

        [XmlEnum("201")]
        Item201,

        [XmlEnum("202")]
        Item202,

        [XmlEnum("213")]
        Item213,

        [XmlEnum("275")]
        Item275,

        [XmlEnum("638")]
        Item638,

        [XmlEnum("731")]
        Item731,

        [XmlEnum("738")]
        Item738,

        [XmlEnum("835")]
        Item835,

        [XmlEnum("851")]
        Item851,

        [XmlEnum("853")]
        Item853,

        [XmlEnum("2545")]
        Item2545,

        [XmlEnum("2547")]
        Item2547,

        [XmlEnum("2551")]
        Item2551,

        [XmlEnum("2552")]
        Item2552,

        [XmlEnum("2561")]
        Item2561,

        [XmlEnum("2571")]
        Item2571,

        [XmlEnum("2581")]
        Item2581,

        [XmlEnum("276")]
        Item276,

        [XmlEnum("3135")]
        Item3135,

        [XmlEnum("7923")]
        Item7923,

        [XmlEnum("9061")]
        Item9061,

        [XmlEnum("9062")]
        Item9062,

        [XmlEnum("9557")]
        Item9557,

        [XmlEnum("9642")]
        Item9642,

        [XmlEnum("9802")]
        Item9802,

        [XmlEnum("984")]
        Item984,

        [XmlEnum("985")]
        Item985,

        [XmlEnum("986")]
        Item986,

        [XmlEnum("987")]
        Item987,

        [XmlEnum("988")]
        Item988,

        [XmlEnum("989")]
        Item989,

        [XmlEnum("990")]
        Item990,

        [XmlEnum("991")]
        Item991,

        [XmlEnum("2553")]
        Item2553,

        [XmlEnum("2554")]
        Item2554,

        [XmlEnum("2555")]
        Item2555,

        [XmlEnum("2556")]
        Item2556,

        [XmlEnum("2557")]
        Item2557,

        [XmlEnum("2558")]
        Item2558,

        [XmlEnum("3831")]
        Item3831,

        [XmlEnum("5401")]
        Item5401,

        [XmlEnum("5423")]
        Item5423,

        [XmlEnum("5451")]
        Item5451,

        [XmlEnum("5562")]
        Item5562,

        [XmlEnum("6421")]
        Item6421,

        [XmlEnum("6422")]
        Item6422,

        [XmlEnum("6424")]
        Item6424,

        [XmlEnum("8361")]
        Item8361,

        [XmlEnum("8751")]
        Item8751,

        [XmlEnum("9111")]
        Item9111,

        [XmlEnum("9113")]
        Item9113,

        [XmlEnum("9245")]
        Item9245,

        [XmlEnum("9246")]
        Item9246,

        [XmlEnum("9491")]
        Item9491,

        [XmlEnum("9501")]
        Item9501,

        [XmlEnum("9641")]
        Item9641,

        [XmlEnum("9803")]
        Item9803,

        [XmlEnum("9805")]
        Item9805,

        [XmlEnum("6423")]
        Item6423,

        [XmlEnum("508")]
        Item508,

        [XmlEnum("164")]
        Item164,

        [XmlEnum("303")]
        Item303,

        [XmlEnum("307")]
        Item307,

        [XmlEnum("320")]
        Item320,

        [XmlEnum("9910")]
        Item9910,

        [XmlEnum("9911")]
        Item9911,

        [XmlEnum("9912")]
        Item9912,

        [XmlEnum("9913")]
        Item9913,

        [XmlEnum("9914")]
        Item9914,

        [XmlEnum("9915")]
        Item9915,

        [XmlEnum("9916")]
        Item9916,

        [XmlEnum("9917")]
        Item9917,

        [XmlEnum("9918")]
        Item9918,

        [XmlEnum("9920")]
        Item9920,

        [XmlEnum("9921")]
        Item9921,

        [XmlEnum("9922")]
        Item9922,

        [XmlEnum("9923")]
        Item9923,

        [XmlEnum("9924")]
        Item9924,

        [XmlEnum("9925")]
        Item9925,

        [XmlEnum("9926")]
        Item9926,

        [XmlEnum("9927")]
        Item9927,

        [XmlEnum("9930")]
        Item9930,

        [XmlEnum("9931")]
        Item9931,

        [XmlEnum("9940")]
        Item9940,

        [XmlEnum("9941")]
        Item9941,

        [XmlEnum("9950")]
        Item9950,

        [XmlEnum("9951")]
        Item9951,

        [XmlEnum("9960")]
        Item9960,

        [XmlEnum("9961")]
        Item9961,

        [XmlEnum("9962")]
        Item9962,

        [XmlEnum("9970")]
        Item9970,

        [XmlEnum("9971")]
        Item9971,

        [XmlEnum("9980")]
        Item9980,

        [XmlEnum("9981")]
        Item9981,

        [XmlEnum("9982")]
        Item9982,

        [XmlEnum("9983")]
        Item9983,

        [XmlEnum("9985")]
        Item9985,

        [XmlEnum("9986")]
        Item9986,

        [XmlEnum("9987")]
        Item9987,

        [XmlEnum("9988")]
        Item9988,

        [XmlEnum("9990")]
        Item9990,

        [XmlEnum("9991")]
        Item9991,

        [XmlEnum("728")]
        Item728,

        [XmlEnum("509")]
        Item509,

        [XmlEnum("277")]
        Item277,

        [XmlEnum("278")]
        Item278,

        [XmlEnum("279")]
        Item279,

        [XmlEnum("293")]
        Item293,

        [XmlEnum("295")]
        Item295,

        [XmlEnum("351")]
        Item351,

        [XmlEnum("318")]
        Item318,

        [XmlEnum("319")]
        Item319,

        [XmlEnum("321")]
        Item321,

        [XmlEnum("348")]
        Item348,

        [XmlEnum("350")]
        Item350,

        [XmlEnum("2311")]
        Item2311,

        [XmlEnum("2312")]
        Item2312,

        [XmlEnum("2313")]
        Item2313,

        [XmlEnum("2314")]
        Item2314,

        [XmlEnum("2315")]
        Item2315,

        [XmlEnum("2316")]
        Item2316,

        [XmlEnum("2351")]
        Item2351,

        [XmlEnum("2352")]
        Item2352,

        [XmlEnum("2353")]
        Item2353,

        [XmlEnum("2354")]
        Item2354,

        [XmlEnum("2355")]
        Item2355,

        [XmlEnum("2356")]
        Item2356,

        [XmlEnum("2357")]
        Item2357,

        [XmlEnum("426")]
        Item426,

        [XmlEnum("747")]
        Item747,

        [XmlEnum("340")]
        Item340,

        [XmlEnum("341")]
        Item341,

        [XmlEnum("9823")]
        Item9823,

        [XmlEnum("9822")]
        Item9822,

        [XmlEnum("3181")]
        Item3181,

        [XmlEnum("3231")]
        Item3231,
    }

    public enum tDangerIndustrialClass
    {
        [XmlEnum("Не относится к опасным производственным объектам")]
        Неотноситсякопаснымпроизводственнымобъектам,

        I,

        II,

        III,

        IV,
    }

    public partial class tOKS
    {
        public string Name { get; set; }

        [XmlElement("Address")]
        public tAddress[] Address { get; set; }

        public string FunctionsClass { get; set; }

        public tTextBlock FunctionsFeatures { get; set; }

        [XmlElement("PowerIndicator")]
        public tTEI[] PowerIndicator { get; set; }

        [XmlElement("TEI")]
        public tTEI[] TEI { get; set; }

        public tEnergyEfficiency EnergyEfficiency { get; set; }

        public tDangerIndustrialClass DangerousIndustrialObject { get; set; }

        public tFireDangerCategory FireDangerCategory { get; set; }

        public tTextBlock PeoplePermanentStay { get; set; }

        public tResponsibilityLevel ResponsibilityLevel { get; set; }

        public tModel Model { get; set; }

        [XmlAttribute(DataType = "ID")]
        public string ObjectID { get; set; }
    }

    public partial class tEnergyEfficiency
    {
        public tEfficiencyClass EnergyEfficiencyClass { get; set; }

        public tTextBlock EnergyEfficiencyImproving { get; set; }
    }

    public enum tEfficiencyClass
    {

        [XmlEnum("A++")]
        A,

        [XmlEnum("A+")]
        A1,

        [XmlEnum("A")]
        A2,

        B,

        C,

        D,

        E,

        F,

        G,
    }

    public enum tFireDangerCategory
    {
        [XmlEnum("Категория не устанавливается")]
        Категориянеустанавливается,

        А,

        Б,

        В,

        Г,

        Д,
    }

    public enum tResponsibilityLevel
    {
        повышенный,

        нормальный,

        пониженный,
    }

    public partial class tModel
    {
        public string Description { get; set; }

        [XmlElement("ModelFile")]
        public tModelFile[] ModelFile { get; set; }
    }

    public partial class tModelFile
    {
        [XmlElement(DataType = "normalizedString")]
        public string FileName { get; set; }

        public tModelFormat FileFormat { get; set; }

        public string FileRelativePath { get; set; }

        public string FileChecksum { get; set; }

        [XmlElement("SignFile")]
        public tSignFile[] SignFile { get; set; }

        [XmlAttribute()]
        public string description { get; set; }
    }

    public enum tModelFormat
    {
        IFC,

        XML,

        LandXML,
    }

    public partial class tSignFile
    {
        [XmlElement(DataType = "normalizedString")]
        public string FileName { get; set; }

        public string FileFormat { get; set; }

        public string RelativePath { get; set; }

        public string FileChecksum { get; set; }
    }

    public partial class tFunctions
    {
        public string FunctionsNote { get; set; }

        public string FunctionsClass { get; set; }

        public tTextBlock ProductsNote { get; set; }
    }

    public partial class tClimateConditions
    {
        public tTextBlock ClimateNote { get; set; }

        public tTextBlock DangerousNatureProcesses { get; set; }

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
    }

    public enum tClimateDistrict
    {

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

    public enum tGeologicalConditions
    {

        I,

        II,

        III,
    }

    public enum tWindDistrict
    {

        Iа,

        I,

        II,

        III,

        IV,

        V,

        VI,

        VII,
    }

    public enum tSnowDistrict
    {

        I,

        II,

        III,

        IV,

        V,

        VI,

        VII,

        VIII,
    }

    public enum tSeismicActivity
    {

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

    public partial class tResources
    {
        public tTextBlock Note { get; set; }

        [XmlElement("Resource")]
        public tResource[] Resource { get; set; }
    }

    public partial class tResource
    {
        public string Name { get; set; }

        public tOKEI Measure { get; set; }

        public string Value { get; set; }
    }

    public enum tLandCategory
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
    }

    public partial class tLandInfo
    {
        public tTextBlock CommonLandInfo { get; set; }

        [XmlElement("LandAreaInfo")]
        public tLandAreaInfo[] LandAreaInfo { get; set; }
    }

    public partial class tLandAreaInfo
    {
        public string CadastralNumber { get; set; }

        public tLandCategory LandCategory { get; set; }

        public string UsingNote { get; set; }
    }

    public partial class tSoftwareUsed
    {
        public tTextBlock SoftwareNote { get; set; }

        [XmlElement("Software")]
        public tSoftware[] Software { get; set; }
    }

    public partial class tSoftware
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public string UsingNote { get; set; }
    }

    public partial class tStagesInfo
    {
        public tTextBlock ReasonNote { get; set; }

        [XmlElement("Stage")]
        public tStage[] Stage { get; set; }
    }

    public partial class tStage
    {
        public string Number { get; set; }

        [XmlElement(DataType = "date")]
        public System.DateTime BeginDate { get; set; }

        [XmlIgnore()]
        public bool BeginDateSpecified { get; set; }

        [XmlElement(DataType = "date")]
        public System.DateTime EndDate { get; set; }

        [XmlIgnore()]
        public bool EndDateSpecified { get; set; }

        [XmlElement(DataType = "date")]
        public System.DateTime OperationDate { get; set; }

        [XmlIgnore()]
        public bool OperationDateSpecified { get; set; }

        public tTextBlock Note { get; set; }
    }

    public partial class tProjectDocumentationLink
    {
        public string DocNumber { get; set; }

        public string DocSectionLink { get; set; }
    }

    public partial class tNonLinearProjectDocumentContent
    {
        public tProjectDocumentSection Section2 { get; set; }

        public tProjectDocumentSection Section3 { get; set; }

        public tProjectDocumentSection Section4 { get; set; }

        public tSection5 Section5 { get; set; }

        public tProjectDocumentSection Section6 { get; set; }

        public tProjectDocumentSection Section7 { get; set; }

        public tProjectDocumentSection Section8 { get; set; }

        public tProjectDocumentSection Section9 { get; set; }

        public tProjectDocumentSection Section10 { get; set; }

        public tProjectDocumentSection Section11 { get; set; }

        public tSectionEstimate Section12 { get; set; }

        public tOtherDocumentsSection Section13 { get; set; }
    }

    public partial class tProjectDocumentSection
    {
        [XmlElement("Document", typeof(tProjectDocument))]
        [XmlElement("SubSection", typeof(tProjectDocumentSubSection))]
        public object[] Items { get; set; }
    }

    public partial class tProjectDocument
    {
        public string DocName { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string DocNumber { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string DocChanges { get; set; }

        [XmlElement("File")]
        public tFile[] File { get; set; }

        [XmlAttribute(DataType = "IDREFS")]
        public string ObjectIDREFS { get; set; }
    }

    public partial class tFile
    {
        [XmlElement(DataType = "normalizedString")]
        public string FileName { get; set; }

        public string FileFormat { get; set; }

        public string FileRelativePath { get; set; }

        public string FileChecksum { get; set; }

        [XmlElement("SignFile")]
        public tSignFile[] SignFile { get; set; }
    }

    public partial class tProjectDocumentSubSection
    {
        [XmlElement("Document", typeof(tProjectDocument))]
        [XmlElement("SubSection", typeof(tProjectDocumentSubSection))]
        public object[] Items { get; set; }

        [XmlAttribute()]
        public string Name { get; set; }
    }

    public partial class tSection5
    {
        public tProjectDocumentSection ElectricitySupply { get; set; }

        public tProjectDocumentSection WaterSupply { get; set; }

        public tProjectDocumentSection WaterRemoval { get; set; }

        public tProjectDocumentSection HeatingVentilation { get; set; }

        public tProjectDocumentSection Communication { get; set; }

        public tProjectDocumentSection GasSupply { get; set; }
    }

    public partial class tSectionEstimate
    {
        public tProjectDocumentSection EstimateExplanatoryNote { get; set; }

        public tProjectDocumentSection CostSummary { get; set; }

        public tProjectDocumentSection SummaryEstimate { get; set; }

        public tProjectDocumentSection ObjectLocalEstimates { get; set; }

        public tProjectDocumentSection CostsEstimates { get; set; }
    }

    public partial class tOtherDocumentsSection
    {
        public tProjectDocumentSection IndustrialSafetyDeclaration { get; set; }

        public tProjectDocumentSection HydraulicStructuresSafetyDeclaration { get; set; }

        public tProjectDocumentSection EventsList { get; set; }

        public tProjectDocumentSection OtherDocuments { get; set; }
    }

    public enum tPlacement
    {
        [XmlEnum("1")]
        Item1,

        [XmlEnum("2")]
        Item2,
    }

    public enum tDangerousAndComplex
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

        [XmlEnum("12")]
        Item12,

        [XmlEnum("13")]
        Item13,

        [XmlEnum("14")]
        Item14,
    }

    public enum tUnique
    {

        [XmlEnum("1")]
        Item1,

        [XmlEnum("2")]
        Item2,

        [XmlEnum("3")]
        Item3,

        [XmlEnum("4")]
        Item4,
    }

    public partial class tIndustrialObject
    {
        public string Name { get; set; }

        public tConstructionType ConstructionType { get; set; }

        public decimal ConstructionDuration { get; set; }

        public tConstructionDates ConstructionDates { get; set; }

        public tAddress Address { get; set; }

        public tFunctions Functions { get; set; }

        public tTextBlock FunctionsFeatures { get; set; }

        [XmlElement("PowerIndicator")]
        public tTEI[] PowerIndicator { get; set; }

        [XmlElement("TEI")]
        public tTEI[] TEI { get; set; }

        public tEnergyEfficiency EnergyEfficiency { get; set; }

        public tDangerIndustrialClass DangerousIndustrialObject { get; set; }

        [XmlElement("FireDangerCategory", typeof(tFireDangerCategory))]
        [XmlElement("ObjectParts", typeof(tObjectParts1))]
        [XmlElement("PeoplePermanentStay", typeof(tTextBlock))]
        [XmlElement("ResponsibilityLevel", typeof(tResponsibilityLevel))]
        public object[] Items { get; set; }

        public tClimateConditions ClimateConditions { get; set; }

        public tResources Resources { get; set; }

        public tResources RawResources { get; set; }

        public tTextBlock ResourceUseInfo { get; set; }

        public tTextBlock RenewableSourceInfo { get; set; }

        public tLandCategory LandCategory { get; set; }

        public tLandInfo LandInfo { get; set; }

        public tTextBlock LandFunds { get; set; }

        public tTextBlock PatentsUsed { get; set; }

        public tTextBlock SpecialRequirements { get; set; }

        public tSoftwareUsed SoftwareUsed { get; set; }

        public tStagesInfo StagesInfo { get; set; }

        public tTextBlock DestroyFunds { get; set; }

        [XmlArrayItem("ProjectDocumentationLink", IsNullable = false)]
        public tProjectDocumentationLink[] EnergyEfficiencyLinks { get; set; }

        [XmlArrayItem("ProjectDocumentationLink", IsNullable = false)]
        public tProjectDocumentationLink[] IndustrialSafetyLinks { get; set; }

        public tTextBlock LandReclamationLink { get; set; }

        public tNuclearPlantFeatures NuclearPlantFeatures { get; set; }

        public tNonLinearProjectDocumentContent ProjectDocumentation { get; set; }

        public tModel Model { get; set; }

        [XmlAttribute(DataType = "ID")]
        public string ObjectID { get; set; }

        [XmlAttribute()]
        public tPlacement Placement { get; set; }

        [XmlAttribute()]
        public tDangerousAndComplex DangerousAndComplex { get; set; }

        [XmlIgnore()]
        public bool DangerousAndComplexSpecified { get; set; }

        [XmlAttribute()]
        public tUnique Unique { get; set; }

        [XmlIgnore()]
        public bool UniqueSpecified { get; set; }
    }

    public partial class tNuclearPlantFeatures
    {
        public tTextBlock ObjectList { get; set; }

        public tTextBlock NuclearSafety { get; set; }

        public tTextBlock OperationalLimits { get; set; }

        public tTextBlock NuclearPlantControl { get; set; }

        public tTextBlock ReactorPlant { get; set; }

        public tTextBlock NuclearSafitySoftware { get; set; }

        public tMonitoringPrograms MonitoringPrograms { get; set; }
    }

    public partial class tMonitoringPrograms
    {
        public tTextBlock GeotechnicalMonitoring { get; set; }

        public tTextBlock HydrogeologicalMonitoring { get; set; }

        public tTextBlock SeismologicalMonitoring { get; set; }

        public tTextBlock MeteorologicalMonitoring { get; set; }

        public tTextBlock AerologicalMonitoring { get; set; }

        public tTextBlock HydrologicalMonitoring { get; set; }

        public tTextBlock GeodynamicalMonitoring { get; set; }

        public tTextBlock TechnogenicMonitoring { get; set; }
    }

    public partial class tLinearObject
    {
        public string Name { get; set; }

        public tConstructionType ConstructionType { get; set; }

        public decimal ConstructionDuration { get; set; }

        public tConstructionDates ConstructionDates { get; set; }

        public tAddress Address { get; set; }

        public tAddress FinishAddress { get; set; }

        public tFunctions Functions { get; set; }

        public tTextBlock FunctionsFeatures { get; set; }

        [XmlElement("PowerIndicator")]
        public tTEI[] PowerIndicator { get; set; }

        [XmlElement("TEI")]
        public tTEI[] TEI { get; set; }

        public tDangerIndustrialClass DangerousIndustrialObject { get; set; }

        public tTextBlock LinearObjectCategory { get; set; }

        public tTextBlock LinearObjectClass { get; set; }

        public tTextBlock LinearObjectNote { get; set; }

        [XmlElement("FireDangerCategory", typeof(tFireDangerCategory))]
        [XmlElement("ObjectParts", typeof(tObjectParts1))]
        [XmlElement("PeoplePermanentStay", typeof(tTextBlock))]
        [XmlElement("ResponsibilityLevel", typeof(tResponsibilityLevel))]
        public object[] Items { get; set; }

        public tClimateConditions ClimateConditions { get; set; }

        public tRoutes Routes { get; set; }

        public tResources Resources { get; set; }

        public tTextBlock ResourceUseInfo { get; set; }

        public tTextBlock RenewableSourceInfo { get; set; }

        public tLandCategory LandCategory { get; set; }

        public tLandInfo LandInfo { get; set; }

        public tTextBlock PatentsUsed { get; set; }

        public tTextBlock SpecialRequirements { get; set; }

        public tSoftwareUsed SoftwareUsed { get; set; }

        public tStagesInfo StagesInfo { get; set; }

        public tTextBlock DestroyFunds { get; set; }

        [XmlArrayItem("ProjectDocumentationLink", IsNullable = false)]
        public tProjectDocumentationLink[] IndustrialSafetyLinks { get; set; }

        public tTextBlock LandReclamationLink { get; set; }

        public tGasNetworksFeatures GasNetworksFeatures { get; set; }

        public tTextBlock EngineeringTerritoryProtection { get; set; }

        public tLinearProjectDocumentContent ProjectDocumentation { get; set; }

        public tModel Model { get; set; }

        [XmlAttribute(DataType = "ID")]
        public string ObjectID { get; set; }

        [XmlAttribute()]
        public tPlacement Placement { get; set; }

        [XmlAttribute()]
        public tDangerousAndComplex DangerousAndComplex { get; set; }

        [XmlIgnore()]
        public bool DangerousAndComplexSpecified { get; set; }

        [XmlAttribute()]
        public tUnique Unique { get; set; }

        [XmlIgnore()]
        public bool UniqueSpecified { get; set; }
    }

    public partial class tRoutes
    {
        public tTextBlock RoutesNote { get; set; }

        [XmlElement("Route")]
        public tRoute[] Route { get; set; }
    }

    public partial class tRoute
    {
        public string Name { get; set; }

        public tTextBlock Note { get; set; }
    }

    public partial class tGasNetworksFeatures
    {
        public tTextBlock StrengthCalculation { get; set; }

        public tTextBlock EnvironmentalProtection { get; set; }

        public tTextBlock FireSafety { get; set; }

        public tTextBlock OperationalSafety { get; set; }

        public tTextBlock PerairFrequency { get; set; }
    }

    public partial class tLinearProjectDocumentContent
    {
        public tProjectDocumentSection Section2 { get; set; }

        public tProjectDocumentSection Section3 { get; set; }

        public tProjectDocumentSection Section4 { get; set; }

        public tProjectDocumentSection Section5 { get; set; }

        public tProjectDocumentSection Section6 { get; set; }

        public tProjectDocumentSection Section7 { get; set; }

        public tProjectDocumentSection Section8 { get; set; }

        public tSectionEstimate Section9 { get; set; }

        public tOtherDocumentsSection Section10 { get; set; }
    }

    public partial class tAuthor
    {
        [XmlElement("ForeignOrganization", typeof(tForeignOrganization))]
        [XmlElement("IP", typeof(tIP))]
        [XmlElement("Organization", typeof(tOrganization))]
        public object Item { get; set; }

        [XmlArrayItem("SRO", IsNullable = false)]
        public tSRO[] SROMembership { get; set; }

        public override string ToString()
        {
            return Item?.ToString() ?? base.ToString();
        }
    }

    public partial class tSRO
    {
        [XmlAttribute()]
        public tSROType SROType { get; set; }

        [XmlText()]
        public string Value { get; set; }
    }

    public enum tSROType
    {
        [Description("Саморегулируемая организация в области инженерных изысканий")]
        [XmlEnum("Саморегулируемая организация в области инженерных изысканий")]
        Саморегулируемаяорганизациявобластиинженерныхизысканий,

        [Description("Саморегулируемая организация в области архитектурно-строительного проектирования")]
        [XmlEnum("Саморегулируемая организация в области архитектурно-строительного проектирования")]
        Саморегулируемаяорганизациявобластиархитектурностроительногопроектирования,

        [Description("Саморегулируемая организация в области строительства")]
        [XmlEnum("Саморегулируемая организация в области строительства")]
        Саморегулируемаяорганизациявобластистроительства,
    }

    public partial class tProjectDocumentationAuthors
    {
        [XmlElement("ProjectDocumentationAuthor")]
        public tAuthor[] ProjectDocumentationAuthor { get; set; }
    }

    public partial class tDeveloper
    {
        [XmlElement("ForeignOrganization", typeof(tForeignOrganization))]
        [XmlElement("IP", typeof(tIP))]
        [XmlElement("Organization", typeof(tOrganization))]
        [XmlElement("Person", typeof(tPerson))]
        public object Item { get; set; }
    }

    public partial class tTechnicalCustomer : ITechnicalCustomer
    {
        [XmlElement("ForeignOrganization", typeof(tForeignOrganization))]
        [XmlElement("Organization", typeof(tOrganization))]
        public object Item { get; set; }

        [XmlArrayItem("SRO", IsNullable = false)]
        public tSRO[] SROMembership { get; set; }
    }

    public partial class tSROMembership
    {
        [XmlElement("SRO")]
        public tSRO[] SRO { get; set; }
    }

    public partial class tSigners
    {
        [XmlElement("Signer")]
        public tWorkPerson[] Signer { get; set; }
    }

    //public partial class tUsedNorms
    //{
    //    [XmlElement("UsedNorm")]
    //    public string[] UsedNorm { get; set; }
    //}

    public partial class tUsedNorms
    {
        [XmlElement("UsedNorm")]
        public tUsedNorm[] UsedNorms { get; set; }
    }

    public partial class tUsedNorm
    {
        [XmlText]
        public string Value { get; set; }
    }

    public partial class tFinanceSources
    {
        [XmlElement("BudgetSource", typeof(tBudgetSource))]
        [XmlElement("PrivateSource", typeof(tPrivateSource))]
        [XmlElement("StateCustomerSource", typeof(tStateCustomerSource))]
        public object[] Items { get; set; }

        public string FinanceComment { get; set; }
    }

    public partial class tBudgetSource
    {
        public tBudgetLevel BudgetLevel { get; set; }

        public double SourceRatio { get; set; }

        [XmlIgnore()]
        public bool SourceRatioSpecified { get; set; }
    }

    public enum tBudgetLevel
    {
        [XmlEnum("Федеральный бюджет")]
        Федеральныйбюджет,

        [XmlEnum("Бюджет субъекта Российской Федерации")]
        БюджетсубъектаРоссийскойФедерации,

        [XmlEnum("Местный бюджет")]
        Местныйбюджет,

        [XmlEnum("Бюджет государственного внебюджетного фонда Российской Федерации")]
        БюджетгосударственноговнебюджетногофондаРоссийскойФедерации,

        [XmlEnum("Бюджет территориального государственного внебюджетного фонда")]
        Бюджеттерриториальногогосударственноговнебюджетногофонда,
    }

    public partial class tPrivateSource
    {
        public double SourceRatio { get; set; }

        [XmlIgnore()]
        public bool SourceRatioSpecified { get; set; }
    }

    public partial class tStateCustomerSource
    {
        public tTechnicalCustomer Owner { get; set; }

        public double SourceRatio { get; set; }

        [XmlIgnore()]
        public bool SourceRatioSpecified { get; set; }
    }

    public partial class tDocuments
    {
        [XmlElement("Document")]
        public tDocument[] Document { get; set; }
    }

    public partial class tDocument
    {
        public string DocTypeName => DocType.GetEnumDescription();
        public tDocumentType DocType { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string DocName { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string DocNumber { get; set; }

        [XmlElement(DataType = "date")]
        public System.DateTime DocDate { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string DocIssueAuthor { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string DocChanges { get; set; }

        [XmlElement("File", typeof(tFile))]
        [XmlElement("Included", typeof(string))]
        [XmlElement("Link", typeof(string))]
        [XmlChoiceIdentifier("ItemsElementName")]
        public object[] Items { get; set; }

        [XmlElement("ItemsElementName")]
        [XmlIgnore()]
        public ItemsChoiceType1[] ItemsElementName { get; set; }
    }

    [XmlType(IncludeInSchema = false)]
    public enum ItemsChoiceType1
    {
        File,

        Included,

        Link,
    }

    public partial class tEngineeringSurveyDocuments
    {
        [XmlElement("Document")]
        public tEngineeringSurveyDocument[] Document { get; set; }
    }

    public partial class tEngineeringSurveyDocument
    {
        public tEngineeringSurveyDocumentType DocType { get; set; }

        public string DocName { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string DocNumber { get; set; }

        [XmlElement(DataType = "date")]
        public System.DateTime DocDate { get; set; }

        [XmlElement("FullDocIssueAuthor")]
        public tAuthor[] FullDocIssueAuthor { get; set; }

        [XmlElement(DataType = "normalizedString")]
        public string DocChanges { get; set; }

        [XmlElement("File")]
        public tFile[] File { get; set; }
    }

    public enum tEngineeringSurveyDocumentType
    {
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
    }

    public partial class tAdditionalData
    {
        [XmlElement("Data")]
        public tData[] Data { get; set; }
    }

    public partial class tData
    {
        public string DataDescription { get; set; }

        [XmlElement("File")]
        public tFile[] File { get; set; }
    }

    public partial class tProjectDocumentationLinks
    {
        [XmlElement("ProjectDocumentationLink")]
        public tProjectDocumentationLink[] ProjectDocumentationLink { get; set; }
    }

    public partial class tDesignerAssurance
    {
        public tTextBlock Assurance { get; set; }

        public tWorkPerson Signer { get; set; }
    }

    public partial class tExplanatoryNoteModifications
    {
        [XmlElement("Modification")]
        public tModification[] Modification { get; set; }
    }

    public partial class tModification
    {
        [XmlElement(DataType = "integer")]
        public string ModificationNumber { get; set; }

        [XmlElement(DataType = "date")]
        public System.DateTime ModificationDate { get; set; }

        public string ModificationNote { get; set; }
    }

    public partial class tRow
    {
        [XmlElement("Cell")]
        public tCell[] Cell { get; set; }
    }
}
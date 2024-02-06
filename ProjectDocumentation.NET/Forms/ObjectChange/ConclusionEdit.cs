using Core.NET.AutoModels;
using Core.NET.Controllers;
using Core.NET.DaDataObj;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Utils;
using DevExpress.Xpo;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;
using ProjectDocumentation.NET.Models.ConclusionObj;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class ConclusionEdit : XtraForm
    {
        private Session _session => SessionController.GetSession();

        private class CadastralNumber
        {
            [RegExEditMask(@"\d+:\d+:\d+:\d+", AllowBlankInput = true, ShowPlaceholders = false)]
            public string Number { get; set; }
        }

        public string FileName { get; private set; }
        public bool IsSave { get; private set; }
        public Conclusion Conclusion { get; private set; }

        private List<tPreviousConclusion> _tPreviousConclusions = new List<tPreviousConclusion>();
        private List<tDocument> _tDocuments = new List<tDocument>();
        private List<tTEI> _tTEIs = new List<tTEI>();
        private List<tObjectPart> _tObjectParts = new List<tObjectPart>();
        private List<tFinance> _tFinances = new List<tFinance>();
        private List<tDesigner> _tDesigners = new List<tDesigner>();
        private List<CadastralNumber> _cadastralNumbers = new List<CadastralNumber>();
        private List<tDeclarant> _tProjectDocumentsDevelopers = new List<tDeclarant>();
        private List<tTechnicalCustomer> _tProjectDocumentsTechnicalCustomer = new List<tTechnicalCustomer>();
        private List<tDeclarant> _tEngineeringSurveyDeveloper = new List<tDeclarant>();
        private List<tTechnicalCustomer> _tEngineeringSurveyTechnicalCustomer = new List<tTechnicalCustomer>();
        private List<tExpertProjectDocuments> _tExpertProjectDocuments = new List<tExpertProjectDocuments>();
        private List<tEngineeringSurveyAddress> _tEngineeringSurveyAddresses = new List<tEngineeringSurveyAddress>();
        private List<tExpertEngineeringSurveys> _tExpertEngineeringSurveys = new List<tExpertEngineeringSurveys>();

        private List<tMismatch> _mismatch = new List<tMismatch>();

        private List<tExpert> _experts = new List<tExpert>();

        public ConclusionEdit()
        {
            InitializeComponent();
        }

        public ConclusionEdit(Conclusion conclusion) : this()
        {
            Conclusion = conclusion;
        }

        public void SetFileName(string fileName)
        {
            FileName = fileName;
        }

        private async Task FillFormControllersAsync()
        {
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(_session).ToListAsync();

            #region Page I
            cmbExaminationResult.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationResult).ToList());

            cmbExaminationForm.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationForm).ToList());

            cmbExaminationObjectType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationObjectType).ToList());

            cmbExaminationStage.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationStage).ToList());

            cmbConstructionType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ConstructionType).ToList());

            checkCmbExaminationType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationType).ToArray());

            cmbProjectDocumentationIM.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.LogicalAnswer).ToArray());

            cmbEngineeringSurveysIM.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.LogicalAnswer).ToArray());

            #endregion

            #region Page II_1
            cmbObjectType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ObjectType).ToArray());
            #endregion

            #region Page II_3
            checkCmbClimateDistrict.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ClimateDistrict).ToArray());
            checkCmbSeismicActivity.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.SeismicActivity).ToArray());
            checkCmbWindDistrict.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.WindDistrict).ToArray());
            checkCmbGeologicalConditions.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.GeologicalConditions).ToArray());
            checkCmbSnowDistrict.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.SnowDistrict).ToArray());
            #endregion

            #region Page II_5
            radioGroupConclusionNumber.Properties.Items.AddEnum(typeof(ItemChoiceType));
            #endregion

            #region Page V
            checkCmbEngineeringSurveyType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.EngineeringSurveyType).ToArray());
            #endregion
        }

        private async void ConclusionEdit_Load(object sender, EventArgs e)
        {
            txtFileName.EditValue = FileName;

            await FillFormControllersAsync();

            if (Conclusion is null)
            {
                Conclusion = new Conclusion();
            }

            txtVersion.EditValue = Conclusion.SchemaVersion;

            #region Page I
            cmbExaminationResult.EditValue = cmbExaminationResult.Properties.Items
                ?.Cast<GeneralizedDictionaryXPO>()
                ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationResult)
                ?.FirstOrDefault(f => f.ValueInt == Conclusion?.ExaminationObject?.ExaminationResult.GetEnumXmlAttributeValueInt());

            cmbExaminationForm.EditValue = cmbExaminationForm.Properties.Items
                ?.Cast<GeneralizedDictionaryXPO>()
                ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationForm)
                ?.FirstOrDefault(f => f.ValueInt == Conclusion?.ExaminationObject?.ExaminationForm.GetEnumXmlAttributeValueInt());

            cmbExaminationObjectType.EditValue = cmbExaminationObjectType.Properties.Items
                ?.Cast<GeneralizedDictionaryXPO>()
                ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationObjectType)
                ?.FirstOrDefault(f => f.ValueInt == Conclusion?.ExaminationObject?.ExaminationObjectType.GetEnumXmlAttributeValueInt());

            cmbExaminationStage.EditValue = cmbExaminationStage.Properties.Items
               ?.Cast<GeneralizedDictionaryXPO>()
               ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationStage)
               ?.FirstOrDefault(f => f.ValueInt == Conclusion?.ExaminationObject?.ExaminationStage.GetEnumXmlAttributeValueInt());

            cmbConstructionType.EditValue = cmbConstructionType.Properties.Items
              ?.Cast<GeneralizedDictionaryXPO>()
              ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ConstructionType)
              ?.FirstOrDefault(f => f.ValueInt == Conclusion?.ExaminationObject?.ConstructionType.GetEnumXmlAttributeValueInt());

            SetCheckComboBoxByItems(Conclusion?.ExaminationObject?.ExaminationType, checkCmbExaminationType);
            memoName.Text = Conclusion?.ExaminationObject?.Name;

            if (Conclusion?.ExaminationObject?.ProjectDocumentationIMSpecified is true)
            {
                cmbProjectDocumentationIM.EditValue = cmbProjectDocumentationIM.Properties.Items
                  ?.Cast<GeneralizedDictionaryXPO>()
                  ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.LogicalAnswer)
                  ?.FirstOrDefault(f => f.Value == Conclusion?.ExaminationObject?.ProjectDocumentationIM.GetEnumXmlAttributeValue().ToString());
            }            

            if (Conclusion?.ExaminationObject?.EngineeringSurveysIMSpecified is true)
            {
                cmbEngineeringSurveysIM.EditValue = cmbEngineeringSurveysIM.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.LogicalAnswer)
                    ?.FirstOrDefault(f => f.Value == Conclusion?.ExaminationObject?.EngineeringSurveysIM.GetEnumXmlAttributeValue().ToString());
            }

            memoExaminationStageNote.Text = Conclusion?.ExaminationObject?.ExaminationStageNote;
            #endregion

            #region Page I_2
            btnSearch.EditValue = Conclusion?.ExpertOrganization?.OrgINN;
            txtOrgFullName.EditValue = Conclusion?.ExpertOrganization?.OrgFullName;
            txtOrgOGRN.EditValue = Conclusion?.ExpertOrganization?.OrgOGRN;
            txtOrgKPP.EditValue = Conclusion?.ExpertOrganization?.OrgKPP;
            txtOrgINN.EditValue = Conclusion?.ExpertOrganization?.OrgINN;
            txtEmail.EditValue = Conclusion?.ExpertOrganization?.Email;
            btnAddress.EditValue = Conclusion?.ExpertOrganization?.Address;
            txtFamilyName.EditValue = Conclusion?.Approver?.FamilyName;
            txtFirstName.EditValue = Conclusion?.Approver?.FirstName;
            txtSecondName.EditValue = Conclusion?.Approver?.SecondName;
            txtPosition.EditValue = Conclusion?.Approver?.Position;
            #endregion

            #region Page I_3
            btnDeclarant.EditValue = Conclusion?.Declarant;
            #endregion

            #region Page I_5
            _tDocuments = Conclusion.Documents?.ToList() ?? new List<tDocument>();
            gridControlDocuments.DataSource = DocumentObjController.GetDocumentObjs(_tDocuments).ToArray();
            foreach (GridColumn column in gridViewDocuments.Columns)
            {
                column.Visible = false;
            }
            if (gridViewDocuments.Columns[nameof(DocumentObj.DocChanges)] is GridColumn gridDocumentDocChanges)
            {
                gridDocumentDocChanges.Caption = "Примечание";
                gridDocumentDocChanges.Visible = true;
            }
            if (gridViewDocuments.Columns[nameof(DocumentObj.DocDate)] is GridColumn gridDocumentDocDate)
            {
                gridDocumentDocDate.Caption = "Дата документа";
                gridDocumentDocDate.Visible = true;
            }
            if (gridViewDocuments.Columns[nameof(DocumentObj.DocNumber)] is GridColumn gridDocumentDocNumber)
            {
                gridDocumentDocNumber.Caption = "Номер документа";
                gridDocumentDocNumber.Visible = true;
            }
            if (gridViewDocuments.Columns[nameof(DocumentObj.DocName)] is GridColumn gridDocumentDocName)
            {
                gridDocumentDocName.Caption = "Наименование документа";
                gridDocumentDocName.Visible = true;
            }
            if (gridViewDocuments.Columns[nameof(DocumentObj.DocTypeName)] is GridColumn gridDocumentDocTypeName)
            {
                gridDocumentDocTypeName.Caption = "Тип документа";
                gridDocumentDocTypeName.Visible = true;
            }
            #endregion

            #region Page I_6
            _tPreviousConclusions = Conclusion?.PreviousConclusions?.ToList() ?? new List<tPreviousConclusion>();
            gridControlPreviousConclusions.DataSource = _tPreviousConclusions;
            foreach (GridColumn column in gridViewPreviousConclusions.Columns)
            {
                column.Visible = false;
            }
            if (gridViewPreviousConclusions.Columns[nameof(tPreviousConclusion.Name)] is GridColumn gridPreviousConclusionName)
            {
                gridPreviousConclusionName.Caption = "Наименование объекта экспертизы";
                gridPreviousConclusionName.Visible = true;
            }
            #endregion

            #region Page II_1
            memoObjectName.EditValue = Conclusion?.Object?.Name;
            cmbObjectType.EditValue = cmbObjectType.Properties.Items
              ?.Cast<GeneralizedDictionaryXPO>()
              ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ObjectType)
              ?.FirstOrDefault(f => f.ValueInt == Conclusion?.Object?.Type.GetEnumXmlAttributeValueInt());
            cmbFunctionsClass.EditValue = Conclusion?.Object?.Item;
            cmbObjectAddress.EditValue = Conclusion?.Object?.Address?.FirstOrDefault();
            _tTEIs = Conclusion?.Object?.TEI?.ToList() ?? new List<tTEI>();
            gridControlTEI.DataSource = _tTEIs;
            if (gridViewTEI.Columns[nameof(tTEI.Name)] is GridColumn gridTEIName)
            {
                gridTEIName.Caption = "Наименование показателя";
            }
            if (gridViewTEI.Columns[nameof(tTEI.Measure)] is GridColumn gridTEIMeasure)
            {
                gridTEIMeasure.Caption = "Единица измерения показателя";
            }
            if (gridViewTEI.Columns[nameof(tTEI.Value)] is GridColumn gridTEIValue)
            {
                gridTEIValue.Caption = "Значение показателя";
            }
            _tObjectParts = Conclusion?.Object?.ObjectPart?.ToList() ?? new List<tObjectPart>();
            gridControlObjectPart.DataSource = _tObjectParts;
            foreach (GridColumn column in gridViewObjectPart.Columns)
            {
                column.Visible = false;
            }
            if (gridViewObjectPart.Columns[nameof(tObjectPart.Name)] is GridColumn gridObjectPartName)
            {
                gridObjectPartName.Caption = "Наименование объекта";
                gridObjectPartName.Visible = true;
            }
            #endregion

            #region Page II_2
            _tFinances = Conclusion?.Finance?.ToList() ?? new List<tFinance>();
            //gridControlFinances.DataSource = _tFinances;
            gridControlFinances.DataSource = FinanceObjController.GetFinanceObj(_tFinances);
            foreach (GridColumn column in gridViewFinances.Columns)
            {
                column.Visible = false;
            }
            if (gridViewFinances.Columns[nameof(FinanceObj.FinanceSize)] is GridColumn gridFinanceFinanceSize)
            {
                gridFinanceFinanceSize.Caption = "Размер финансирования (в % от общей суммы)";
                gridFinanceFinanceSize.Visible = true;

                gridFinanceFinanceSize.DisplayFormat.FormatType = FormatType.Numeric;
                gridFinanceFinanceSize.DisplayFormat.FormatString = "n5";
            }
            if (gridViewFinances.Columns[nameof(FinanceObj.BudgetType)] is GridColumn gridFinanceBudgetType)
            {
                gridFinanceBudgetType.Caption = "Уровень бюджета (в случае бюджетного финансирования)";
                gridFinanceBudgetType.Visible = true;
            }
            if (gridViewFinances.Columns[nameof(FinanceObj.FinanceType)] is GridColumn gridFinanceFinanceType)
            {
                gridFinanceFinanceType.Caption = "Вид источника финансирования";
                gridFinanceFinanceType.Visible = true;
            }
            memoFinanceComment.EditValue = Conclusion.FinanceComment;
            #endregion

            #region Page II_3
            SetCheckComboBoxByItems(Conclusion?.ClimateConditions?.ClimateDistrict, checkCmbClimateDistrict);
            SetCheckComboBoxByItems(Conclusion?.ClimateConditions?.SeismicActivity, checkCmbSeismicActivity);
            SetCheckComboBoxByItems(Conclusion?.ClimateConditions?.WindDistrict, checkCmbWindDistrict);
            SetCheckComboBoxByItems(Conclusion?.ClimateConditions?.GeologicalConditions, checkCmbGeologicalConditions);
            SetCheckComboBoxByItems(Conclusion?.ClimateConditions?.SnowDistrict, checkCmbSnowDistrict);
            memoClimateConditionsNote.EditValue = Conclusion?.ClimateConditionsNote;
            if (string.IsNullOrWhiteSpace(Conclusion?.ClimateConditionsNote))
            {
                memoClimateConditionsNote.EditValue = "Сведения отсутствуют.";
            }
            #endregion

            #region Page II_4
            _tDesigners = Conclusion?.Designer?.ToList() ?? new List<tDesigner>();
            gridControlDesigners.DataSource = PersonObjController.GetPersonObjByDesigner(_tDesigners)?.ToArray();
            foreach (GridColumn column in gridViewDesigners.Columns)
            {
                column.Visible = false;
            }
            if (gridViewDesigners.Columns[nameof(PersonObj.Name)] is GridColumn gridPersonObjName)
            {
                gridPersonObjName.Caption = "Наименование";
                gridPersonObjName.Visible = true;
            }
            #endregion

            #region Page II_5
            var eEPDUse = Conclusion?.EEPDUse?.FirstOrDefault();
            if (eEPDUse != null)
            {
                var number = eEPDUse.EEPDNumber;
                if (number != null)
                {
                    checkEEPDUse.EditValue = true;
                    switch (number.ItemElementName)
                    {
                        case ItemChoiceType.EGRZ:
                            radioGroupConclusionNumber.SelectedIndex = (int)ItemChoiceType.EGRZ;
                            break;

                        case ItemChoiceType.noEGRZ:
                            radioGroupConclusionNumber.SelectedIndex = (int)ItemChoiceType.noEGRZ;
                            break;
                    }
                    txtEEPDNumber.EditValue = number.Item;
                    dateEEPDDate.EditValue = eEPDUse.EEPDDate;
                    memoEEPDNote.EditValue = eEPDUse.EEPDNote;
                }
            }
            #endregion

            #region Page II_6
            if (Conclusion?.CadastralNumber?.Length > 0)
            {
                _cadastralNumbers.AddRange(Conclusion?.CadastralNumber?.Select(s => new CadastralNumber() { Number = s }));
            }
            else
            {
                _cadastralNumbers = new List<CadastralNumber>();
            }
            gridControlCadastralNumber.DataSource = _cadastralNumbers;
            if (gridViewCadastralNumber.Columns[nameof(CadastralNumber.Number)] is GridColumn gridCadastralNumber)
            {
                gridCadastralNumber.Caption = "Кадастровый номер";
                gridCadastralNumber.Visible = true;
            }
            #endregion

            #region Page II_7
            var engineeringSurveyObjs = Conclusion?.Items1;
            if (engineeringSurveyObjs != null)
            {
                foreach (var item in engineeringSurveyObjs)
                {
                    if (item is tDeclarant tDeclarant)
                    {
                        _tEngineeringSurveyDeveloper.Add(tDeclarant);
                    }
                    else if (item is tTechnicalCustomer tTechnicalCustomer)
                    {
                        _tEngineeringSurveyTechnicalCustomer.Add(tTechnicalCustomer);
                    }
                }
            }
            gridControlEngineeringSurveyDeveloper.DataSource = PersonObjController.GetPersonObjByDeclarant(_tEngineeringSurveyDeveloper).ToArray();
            foreach (GridColumn column in gridViewEngineeringSurveyDeveloper.Columns)
            {
                column.Visible = false;
            }
            if (gridViewEngineeringSurveyDeveloper.Columns[nameof(PersonObj.Name)] is GridColumn gridEngineeringSurveyDeveloperName)
            {
                gridEngineeringSurveyDeveloperName.Caption = "Наименование";
                gridEngineeringSurveyDeveloperName.Visible = true;
            }
            gridControlEngineeringSurveyTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tEngineeringSurveyTechnicalCustomer).ToArray();
            foreach (GridColumn column in gridViewEngineeringSurveyTechnicalCustomer.Columns)
            {
                column.Visible = false;
            }
            if (gridViewEngineeringSurveyTechnicalCustomer.Columns[nameof(PersonObj.Name)] is GridColumn gridEngineeringSurveyTechnicalCustomerName)
            {
                gridEngineeringSurveyTechnicalCustomerName.Caption = "Наименование";
                gridEngineeringSurveyTechnicalCustomerName.Visible = true;
            }
            var projectDocumentsObjs = Conclusion?.Items;
            if (projectDocumentsObjs != null)
            {
                foreach (var item in projectDocumentsObjs)
                {
                    if (item is tDeclarant tDeclarant)
                    {
                        _tProjectDocumentsDevelopers.Add(tDeclarant);
                    }
                    else if (item is tTechnicalCustomer tTechnicalCustomer)
                    {
                        _tProjectDocumentsTechnicalCustomer.Add(tTechnicalCustomer);
                    }
                }
            }
            gridControlProjectDocumentsDeveloper.DataSource = PersonObjController.GetPersonObjByDeclarant(_tProjectDocumentsDevelopers).ToArray();
            foreach (GridColumn column in gridViewProjectDocumentsDeveloper.Columns)
            {
                column.Visible = false;
            }
            if (gridViewProjectDocumentsDeveloper.Columns[nameof(PersonObj.Name)] is GridColumn gridProjectDocumentsDevelopersName)
            {
                gridProjectDocumentsDevelopersName.Caption = "Наименование";
                gridProjectDocumentsDevelopersName.Visible = true;
            }
            gridControlProjectDocumentsTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tProjectDocumentsTechnicalCustomer).ToArray();
            foreach (GridColumn column in gridViewProjectDocumentsTechnicalCustomer.Columns)
            {
                column.Visible = false;
            }
            if (gridViewProjectDocumentsTechnicalCustomer.Columns[nameof(PersonObj.Name)] is GridColumn gridProjectDocumentsTechnicalCustomerName)
            {
                gridProjectDocumentsTechnicalCustomerName.Caption = "Наименование";
                gridProjectDocumentsTechnicalCustomerName.Visible = true;
            }
            #endregion

            #region Page III
            _tExpertProjectDocuments = Conclusion?.ExpertProjectDocuments?.ToList() ?? new List<tExpertProjectDocuments>();
            gridControlExpertProjectDocuments.DataSource = ExpertProjectDocumentController.GetExpertProjectDocuments(_tExpertProjectDocuments).ToArray();
            foreach (GridColumn column in gridViewExpertProjectDocuments.Columns)
            {
                column.Visible = false;
            }
            if (gridViewExpertProjectDocuments.Columns[nameof(ExpertProjectDocument.ExpertType)] is GridColumn gridExpertProjectDocumentExpertType)
            {
                gridExpertProjectDocumentExpertType.Caption = "Направление деятельности";
                gridExpertProjectDocumentExpertType.Visible = true;
            }

            _tEngineeringSurveyAddresses = Conclusion?.EngineeringSurveyAddress?.ToList() ?? new List<tEngineeringSurveyAddress>();
            gridControlEngineeringSurveyAddress.DataSource = EngineeringSurveyAddressController.GetEngineeringSurveyAddress(_tEngineeringSurveyAddresses);
            foreach (GridColumn column in gridViewEngineeringSurveyAddress.Columns)
            {
                column.Visible = false;
            }
            if (gridViewEngineeringSurveyAddress.Columns[nameof(EngineeringSurveyAddress.EngineeringSurveyDistrict)] is GridColumn gridEngineeringSurveyDistrict)
            {
                gridEngineeringSurveyDistrict.Caption = "Описание района изысканий";
                gridEngineeringSurveyDistrict.Visible = true;
            }
            if (gridViewEngineeringSurveyAddress.Columns[nameof(EngineeringSurveyAddress.EngineeringSurveyRegion)] is GridColumn gridEngineeringSurveyRegion)
            {
                gridEngineeringSurveyRegion.Caption = "Регион";
                gridEngineeringSurveyRegion.Visible = true;
                gridEngineeringSurveyRegion.Width = 150;
                gridEngineeringSurveyRegion.OptionsColumn.FixedWidth = true;
            }

            _tExpertEngineeringSurveys = Conclusion.ExpertEngineeringSurveys?.ToList() ?? new List<tExpertEngineeringSurveys>();
            gridControlExpertEngineeringSurveys.DataSource = ExpertEngineeringSurveysController.GetExpertEngineeringSurveys(_tExpertEngineeringSurveys).ToArray();
            foreach (GridColumn column in gridViewExpertEngineeringSurveys.Columns)
            {
                column.Visible = false;
            }
            if (gridViewExpertEngineeringSurveys.Columns[nameof(ExpertEngineeringSurvey.EngineeringSurveyType)] is GridColumn gridEngineeringSurveyType)
            {
                gridEngineeringSurveyType.Caption = "Вид инженерных изысканий";
                gridEngineeringSurveyType.Visible = true;
            }
            #endregion

            #region Page V
            memoEngineeringSurveySummary.EditValue = Conclusion?.Summary?.EngineeringSurveySummary;
            memoEngineeringSurveySummaryDate.EditValue = Conclusion?.Summary?.EngineeringSurveySummaryDate;
            SetCheckComboBoxByItems(Conclusion?.Summary?.EngineeringSurveyType, checkCmbEngineeringSurveyType);
            memoProjectDocumentsSummary.EditValue = Conclusion?.Summary?.ProjectDocumentsSummary;
            memoProjectDocumentsSummaryDate.EditValue = Conclusion?.Summary?.ProjectDocumentsSummaryDate;
            memoEstimateNormsAndWorksSummary.EditValue = Conclusion?.Summary?.EstimateNormsAndWorksSummary;
            memoEstimateSummary.EditValue = Conclusion?.Summary?.EstimateSummary;
            #endregion

            #region Page VI
            memoExaminationSummary.EditValue = Conclusion?.Summary?.ExaminationSummary;
            #endregion

            #region Page VII
            _experts = Conclusion?.Experts?.ToList() ?? new List<tExpert>();
            gridControlExpert.DataSource = _experts;
            if (gridViewExpert.Columns[nameof(tExpert.FamilyName)] is GridColumn gridColumntExpertFamilyName)
            {
                gridColumntExpertFamilyName.Caption = "Фамилия";
            }
            if (gridViewExpert.Columns[nameof(tExpert.FirstName)] is GridColumn gridColumntExpertFirstName)
            {
                gridColumntExpertFirstName.Caption = "Имя";
            }
            if (gridViewExpert.Columns[nameof(tExpert.SecondName)] is GridColumn gridColumntExpertSecondName)
            {
                gridColumntExpertSecondName.Caption = "Отчество";
            }
            if (gridViewExpert.Columns[nameof(tExpert.ExpertType)] is GridColumn gridColumntExpertExpertType)
            {
                gridColumntExpertExpertType.Visible = false;
            }
            if (gridViewExpert.Columns[nameof(tExpert.ExpertTypeString)] is GridColumn gridColumntExpertExpertTypeString)
            {
                gridColumntExpertExpertTypeString.Caption = "Направление деятельности";
            }
            if (gridViewExpert.Columns[nameof(tExpert.ExpertCertificate)] is GridColumn gridColumntExpertExpertCertificate)
            {
                gridColumntExpertExpertCertificate.Caption = "Номер аттестата";
            }
            if (gridViewExpert.Columns[nameof(tExpert.ExpertCertificateBeginDate)] is GridColumn gridColumntExpertExpertCertificateBeginDate)
            {
                gridColumntExpertExpertCertificateBeginDate.Caption = "Дата выдачи";
            }
            if (gridViewExpert.Columns[nameof(tExpert.ExpertCertificateEndDate)] is GridColumn gridColumntExpertExpertCertificateEndDate)
            {
                gridColumntExpertExpertCertificateEndDate.Caption = "Дата окончания";
            }
            #endregion
        }

        public static void SetCheckComboBoxByItems<T>(ICollection<T> collection, CheckedComboBoxEdit comboBoxEdit) where T : Enum
        {
            if (collection is null)
            {
                return;
            }

            foreach (var item in collection)
            {
                var checkItem = comboBoxEdit.Properties.Items
                    .FirstOrDefault(f => f.Value is GeneralizedDictionaryXPO obj && obj.Value == item.GetEnumXmlAttributeValue()?.ToString());
                if (checkItem != null)
                {
                    checkItem.CheckState = CheckState.Checked;
                    checkItem.CheckState = CheckState.Checked;
                }
            }
        }

        public static ICollection<T> GetCheckComboBoxItems<T>(CheckedComboBoxEdit sender, string message = default, bool isNullable = true) where T : Enum
        {
            var result = new List<T>();

            var items = sender.Properties.Items
                .Where(w => w.CheckState == CheckState.Checked);

            foreach (var item in items)
            {
                if (item.Value is GeneralizedDictionaryXPO obj)
                {
                    result.Add(GetEnumByAttributeValue<T>(GetValueGeneralizedDictionary(obj, message: message).Value));
                }
            }

            if (result.Count > 0)
            {
                return result;
            }

            if (isNullable)
            {
                return result;
            }

            if (sender is Control control)
            {
                control.Focus();
            }

            throw new Exception(message ?? "Не заполнен один из параметров.");
        }

        public static GeneralizedDictionaryXPO GetValueGeneralizedDictionary(object sender, string message = default, bool isNullAble = true)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                if (buttonEdit.EditValue is GeneralizedDictionaryXPO result)
                {
                    return result;
                }
            }
            else
            {
                if (sender is GeneralizedDictionaryXPO result)
                {
                    return result;
                }
            }

            if (isNullAble)
            {
                return default;
            }

            if (sender is Control control)
            {
                control.Focus();
            }

            throw new Exception(message ?? "Не заполнен один из параметров.");
        }

        public static T GetEnumByAttributeValue<T>(string value, bool isNullAble = true)
        {
            if (typeof(T).GetTypeInfo().BaseType == typeof(Enum))
            {
                var method = GetMethodName("GetEnumXmlAttributeValue");
                foreach (T type in Enum.GetValues(typeof(T)))
                {
                    var typeValue = method.Invoke(type, new object[] { type })?.ToString();
                    if (!string.IsNullOrWhiteSpace(typeValue) && typeValue.Equals(value))
                    {
                        return type;
                    }
                }
            }

            if (isNullAble)
            {
                return default;
            }

            throw new Exception();
        }

        private static MethodInfo GetMethodName(string methodName)
        {
            var filePath = "Core.NET.dll";
            if (File.Exists(filePath) is false)
            {
                filePath = "dll\\Core.NET.dll";
                if (File.Exists(filePath) is false)
                {
                    throw new Exception("Не найден Core.NET.dll.");
                }
            }

            var targetType = typeof(Enum);
            var fileInfo = new FileInfo(filePath);
            var assemblyPath = fileInfo.FullName;
            var externalAssembly = Assembly.LoadFile(assemblyPath);

            var extensionMethods = externalAssembly
                .GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.IsDefined(typeof(ExtensionAttribute), false))
                .ToArray();

            var matchingExtensions = extensionMethods
                .Where(m => m.GetParameters()[0].ParameterType == targetType)
                .ToArray();

            return matchingExtensions.FirstOrDefault(f => f.Name == methodName);
        }

        public static string GetStringValue(object sender, object control = default, string message = default, bool isNullAble = true, object objPage = null)
        {
            var result = sender?.ToString();
            if (!string.IsNullOrWhiteSpace(result))
            {
                return result;
            }

            if (isNullAble)
            {
                return result;
            }

            if (control is Control obj)
            {
                obj.Focus();
            }

            throw new Exception(message ?? "Не заполнен один из параметров.");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            FileName = txtFileName.Text;

            try
            {
                var schemaVersion = txtVersion.EditValue?.ToString()?.Trim();
                if (string.IsNullOrWhiteSpace(schemaVersion))
                {
                    schemaVersion = "01.02";
                }
                Conclusion.SchemaVersion = schemaVersion;

                #region Page I
                Conclusion.ExaminationObject = new tExaminationObject();
                Conclusion.ExaminationObject.ExaminationResult = GetEnumByAttributeValue<tExaminationResult>(GetValueGeneralizedDictionary(
                    cmbExaminationResult,
                    message: "Заполните поле результат экспертизы")?.Value);
                Conclusion.ExaminationObject.ExaminationForm = GetEnumByAttributeValue<tExaminationForm>(GetValueGeneralizedDictionary(
                    cmbExaminationForm,
                    message: "Заполните поле формы экспертизы")?.Value);
                Conclusion.ExaminationObject.ExaminationObjectType = GetEnumByAttributeValue<tExaminationObjectType>(GetValueGeneralizedDictionary(
                    cmbExaminationObjectType,
                    message: "Заполните поле вида объекта экспертизы")?.Value);
                Conclusion.ExaminationObject.ExaminationStage = GetEnumByAttributeValue<tExaminationStage>(GetValueGeneralizedDictionary(
                   cmbExaminationStage,
                   message: "Заполните поле вид экспертизы")?.Value);
                Conclusion.ExaminationObject.ConstructionType = GetEnumByAttributeValue<tConstractionType>(GetValueGeneralizedDictionary(
                   cmbConstructionType,
                   message: "Заполните поле вид работ")?.Value);
                Conclusion.ExaminationObject.ExaminationType = GetCheckComboBoxItems<tExaminationType>(
                    checkCmbExaminationType,
                    "Заполните поле предмет экспертизы").ToArray();
                Conclusion.ExaminationObject.Name = GetStringValue(memoName.Text, memoName, "Заполните поле наименование объекта экспертизы");

                if (cmbProjectDocumentationIM.EditValue is GeneralizedDictionaryXPO valueProjectDocumentationIM)
                {
                    Conclusion.ExaminationObject.ProjectDocumentationIMSpecified = true;

                    if (valueProjectDocumentationIM.Value == tYesNo.да.GetEnumDescription())
                    {
                        Conclusion.ExaminationObject.ProjectDocumentationIM = tYesNo.да;
                    }
                    else
                    {
                        Conclusion.ExaminationObject.ProjectDocumentationIM = tYesNo.нет;
                    }                    
                }
                else
                {
                    Conclusion.ExaminationObject.ProjectDocumentationIMSpecified = false;
                }

                if (cmbEngineeringSurveysIM.EditValue is GeneralizedDictionaryXPO valueEngineeringSurveysIM)
                {
                    Conclusion.ExaminationObject.EngineeringSurveysIMSpecified = true;

                    if (valueEngineeringSurveysIM.Value == tYesNo.да.GetEnumDescription())
                    {
                        Conclusion.ExaminationObject.EngineeringSurveysIM = tYesNo.да;
                    }
                    else
                    {
                        Conclusion.ExaminationObject.EngineeringSurveysIM = tYesNo.нет;
                    }
                }
                else
                {
                    Conclusion.ExaminationObject.EngineeringSurveysIMSpecified = false;
                }

                Conclusion.ExaminationObject.ExaminationStageNote = GetStringValue(memoExaminationStageNote.Text, isNullAble: true);
                #endregion

                #region Page I_2
                Conclusion.ExpertOrganization = new tOrganization();
                Conclusion.ExpertOrganization.OrgFullName = GetStringValue(txtOrgFullName.EditValue, txtOrgFullName, "Заполните наименование организации");
                Conclusion.ExpertOrganization.OrgOGRN = GetStringValue(txtOrgOGRN.EditValue, txtOrgOGRN, "Заполните ОГРН организации");
                Conclusion.ExpertOrganization.OrgKPP = GetStringValue(txtOrgKPP.EditValue, txtOrgKPP, "Заполните КПП организации");
                Conclusion.ExpertOrganization.OrgINN = GetStringValue(txtOrgINN.EditValue, txtOrgINN, "Заполните ИНН организации");
                Conclusion.ExpertOrganization.Email = GetStringValue(txtEmail.EditValue, txtEmail, isNullAble: true);
                if (btnAddress.EditValue is tAddress address)
                {
                    Conclusion.ExpertOrganization.Address = address;
                }
                else
                {
                    //btnAddress.Focus();
                    //XtraMessageBox.Show("Заполните адрес организации",
                    //    "Информационное сообщение",
                    //    MessageBoxButtons.OK,
                    //    MessageBoxIcon.Error);
                    //return;
                }
                Conclusion.Approver = new tWorkPerson();
                Conclusion.Approver.FamilyName = GetStringValue(txtFamilyName.EditValue, txtFamilyName, "Заполните фамилию подписанта");
                Conclusion.Approver.FirstName = GetStringValue(txtFirstName.EditValue, txtFirstName, "Заполните имя подписанта");
                Conclusion.Approver.SecondName = GetStringValue(txtSecondName.EditValue, txtSecondName, "Заполните отчество подписанта");
                Conclusion.Approver.Position = GetStringValue(txtPosition.EditValue, txtPosition, "Заполните должность подписанта");
                #endregion

                #region Page I_3
                if (btnDeclarant.EditValue is tDeclarant tDeclarant)
                {
                    Conclusion.Declarant = tDeclarant;
                }
                else
                {
                    //XtraMessageBox.Show("Укажите сведения о заявителе.", "Информационное сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //btnDeclarant.Focus();
                    //return;
                }
                #endregion

                #region Page I_5
                if (_tDocuments.Count > 0)
                {
                    Conclusion.Documents = _tDocuments.ToArray();
                }
                else
                {
                    Conclusion.Documents = null;
                }
                #endregion

                #region Page 1_6
                if (_tPreviousConclusions.Count > 0)
                {
                    Conclusion.PreviousConclusions = _tPreviousConclusions.ToArray();
                }
                else
                {
                    Conclusion.PreviousConclusions = null;
                }
                #endregion

                #region Page II_1
                Conclusion.Object = new tObject();
                Conclusion.Object.Name = GetStringValue(memoObjectName.Text, memoObjectName, "Заполните поле наименование объекта капитального строительства");
                Conclusion.Object.Type = GetEnumByAttributeValue<tObjectType>(GetValueGeneralizedDictionary(
                   cmbObjectType,
                   message: "Заполните поле вид объекта капитального строительства")?.Value);
                Conclusion.Object.Item = GetStringValue(cmbFunctionsClass.EditValue?.ToString(), cmbFunctionsClass, "Функциональное назначение объекта капитального строительства (код по классификатору)");
                if (cmbObjectAddress.EditValue is tAddress objectAddress)
                {
                    Conclusion.Object.Address = new tAddress[] { objectAddress };
                }
                else
                {
                    //XtraMessageBox.Show("Укажите адрес объекта.", "Информационное сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //cmbObjectAddress.Focus();
                    //return;
                }
                if (_tTEIs.Count > 0)
                {
                    foreach (var item in _tTEIs)
                    {
                        if (string.IsNullOrWhiteSpace(item.Name))
                        {
                            //XtraMessageBox.Show("Наименование технико-экономического показателя не может быть пустым.",
                            //    "Информационное сообщение",
                            //    MessageBoxButtons.OK,
                            //    MessageBoxIcon.Error);
                            //gridViewTEI.Focus();
                            //return;
                        }

                        if (string.IsNullOrWhiteSpace(item.Measure))
                        {
                            //XtraMessageBox.Show("Единица измерения технико-экономического показателя не может быть пустой.",
                            //    "Информационное сообщение",
                            //    MessageBoxButtons.OK,
                            //    MessageBoxIcon.Error);
                            //gridViewTEI.Focus();
                            //return;
                        }

                        if (string.IsNullOrWhiteSpace(item.Value))
                        {
                            //XtraMessageBox.Show("Значение технико-экономического показателя не может быть пустым.",
                            //    "Информационное сообщение",
                            //    MessageBoxButtons.OK,
                            //    MessageBoxIcon.Error);
                            //gridViewTEI.Focus();
                            //return;
                        }
                    }
                    Conclusion.Object.TEI = _tTEIs.ToArray();
                }
                else
                {
                    if (Conclusion.Object?.TEI != null)
                    {
                        Conclusion.Object.TEI = null;
                    }
                }
                if (_tObjectParts.Count > 0)
                {
                    Conclusion.Object.ObjectPart = _tObjectParts.ToArray();
                }
                else
                {
                    if (Conclusion.Object?.ObjectPart != null)
                    {
                        Conclusion.Object.ObjectPart = null;
                    }
                }
                #endregion

                #region Page II_2
                if (_tFinances.Count > 0)
                {
                    Conclusion.Finance = _tFinances.ToArray();
                }
                else
                {
                    Conclusion.Finance = null;
                }
                Conclusion.FinanceComment = GetStringValue(memoFinanceComment.EditValue, isNullAble: true);
                #endregion

                #region Page II_3
                Conclusion.ClimateConditions = new tClimateConditions();
                Conclusion.ClimateConditions.ClimateDistrict = GetCheckComboBoxItems<tClimateDistrict>(checkCmbClimateDistrict, isNullable: true).ToArray();
                Conclusion.ClimateConditions.SeismicActivity = GetCheckComboBoxItems<tSeismicActivity>(checkCmbSeismicActivity, isNullable: true).ToArray();
                Conclusion.ClimateConditions.WindDistrict = GetCheckComboBoxItems<tWindDistrict>(checkCmbWindDistrict, isNullable: true).ToArray();
                Conclusion.ClimateConditions.GeologicalConditions = GetCheckComboBoxItems<tGeologicalConditions>(checkCmbGeologicalConditions, isNullable: true).ToArray();
                Conclusion.ClimateConditions.SnowDistrict = GetCheckComboBoxItems<tSnowDistrict>(checkCmbSnowDistrict, isNullable: true).ToArray();
                Conclusion.ClimateConditionsNote = GetStringValue(memoClimateConditionsNote.EditValue, memoClimateConditionsNote, "Заполните поле дополнительных сведений о природных и техногенных условиях");
                #endregion

                #region Page II_4
                if (_tDesigners.Count > 0)
                {
                    Conclusion.Designer = _tDesigners.ToArray();
                }
                else
                {
                    Conclusion.Designer = null;
                }
                #endregion

                #region Page II_5

                //var eEPDUse = Conclusion.EEPDUse?.FirstOrDefault();
                //if (eEPDUse != null)
                //{
                //    var number = eEPDUse.EEPDNumber;
                //    if (number != null)
                //    {
                //        checkEEPDUse.EditValue = true;
                //        switch (number.ItemElementName)
                //        {
                //            case ItemChoiceType.EGRZ:
                //                radioGroupConclusionNumber.SelectedIndex = (int)ItemChoiceType.EGRZ;
                //                break;

                //            case ItemChoiceType.noEGRZ:
                //                radioGroupConclusionNumber.SelectedIndex = (int)ItemChoiceType.noEGRZ;
                //                break;
                //        }
                //        txtEEPDNumber.EditValue = number.Item;
                //        dateEEPDDate.EditValue = eEPDUse.EEPDDate;
                //        memoEEPDNote.EditValue = eEPDUse.EEPDNote;
                //    }
                //}

                if (checkEEPDUse.Checked)
                {
                    var eEPDUse = new tEEPDUse();
                    eEPDUse.EEPDDate = dateEEPDDate.DateTime.Date;
                    eEPDUse.EEPDNote = GetStringValue(memoEEPDNote.EditValue, memoEEPDNote, "Заполните поле сведения о разделах представленной на экспертизу проектной документации");
                    eEPDUse.EEPDNumber = new tConclusionNumber();
                    var number = new tConclusionNumber();
                    number.ItemElementName = (ItemChoiceType)radioGroupConclusionNumber.SelectedIndex;
                    number.Item = GetStringValue(txtEEPDNumber.EditValue, txtEEPDNumber, "Заполните поле номер заключения экспертизы *"); ;
                    eEPDUse.EEPDNumber = number;
                    Conclusion.EEPDUse = new tEEPDUse[] { eEPDUse };
                }
                #endregion

                #region Page II_6
                if (_cadastralNumbers?.Count() > 0)
                {
                    var cadastralNumbers = new List<string>();
                    foreach (var obj in _cadastralNumbers)
                    {
                        if (string.IsNullOrWhiteSpace(obj.Number))
                        {
                            //gridControlCadastralNumber.Focus();
                            //XtraMessageBox.Show(
                            //    "Кадастровый номер не может быть пустым.",
                            //    "Информационное сообщение",
                            //    MessageBoxButtons.OK,
                            //    MessageBoxIcon.Error);
                            //return;
                        }
                        else
                        {
                            cadastralNumbers.Add(obj.Number);
                        }
                    }
                    Conclusion.CadastralNumber = cadastralNumbers.ToArray();
                }
                else
                {
                    Conclusion.CadastralNumber = null;
                }
                #endregion

                #region Page II_7
                var listObjItems = new List<object>();
                if (_tProjectDocumentsDevelopers?.Count() > 0)
                {
                    listObjItems.AddRange(_tProjectDocumentsDevelopers);
                }
                if (_tProjectDocumentsTechnicalCustomer?.Count() > 0)
                {
                    listObjItems.AddRange(_tProjectDocumentsTechnicalCustomer);
                }
                if (listObjItems.Count > 0)
                {
                    Conclusion.Items = listObjItems.ToArray();
                }
                else
                {
                    Conclusion.Items = null;
                }
                var listObjItems1 = new List<object>();
                if (_tEngineeringSurveyDeveloper?.Count() > 0)
                {
                    listObjItems1.AddRange(_tEngineeringSurveyDeveloper);
                }
                if (_tEngineeringSurveyTechnicalCustomer?.Count() > 0)
                {
                    listObjItems1.AddRange(_tEngineeringSurveyTechnicalCustomer);
                }
                if (listObjItems1.Count > 0)
                {
                    Conclusion.Items1 = listObjItems1.ToArray();
                }
                else
                {
                    Conclusion.Items1 = null;
                }
                #endregion

                #region Page III
                if (_tExpertProjectDocuments?.Count() > 0)
                {
                    Conclusion.ExpertProjectDocuments = _tExpertProjectDocuments.ToArray();
                }
                else
                {
                    Conclusion.ExpertProjectDocuments = null;
                }

                if (_tEngineeringSurveyAddresses?.Count() > 0)
                {
                    Conclusion.EngineeringSurveyAddress = _tEngineeringSurveyAddresses.ToArray();
                }
                else
                {
                    Conclusion.EngineeringSurveyAddress = null;
                }                

                if (_tExpertEngineeringSurveys?.Count() > 0)
                {
                    Conclusion.ExpertEngineeringSurveys = _tExpertEngineeringSurveys.ToArray();
                }
                else
                {
                    Conclusion.ExpertEngineeringSurveys = null;
                }
                #endregion

                #region Page V
                if (Conclusion.Summary is null)
                {
                    Conclusion.Summary = new tSummary();
                }
                Conclusion.Summary.EngineeringSurveySummary = GetStringValue(memoEngineeringSurveySummary.EditValue, memoEngineeringSurveySummary, "Заполните поле вывод о соответствии результатов инженерных изысканий требованиям технических регламентов", isNullAble: true);
                Conclusion.Summary.EngineeringSurveySummaryDate = GetStringValue(memoEngineeringSurveySummaryDate.EditValue, memoEngineeringSurveySummaryDate, "Заполните поле сведения о дате, по состоянию на которую действовали требования, примененные в соответствии с частью 5.2 статьи 49 Градостроительного кодекса Российской Федерации (в части экспертизы результатов инженерных изысканий)");
                Conclusion.Summary.EngineeringSurveyType = GetCheckComboBoxItems<tEngineeringSurveyType>(checkCmbEngineeringSurveyType, "Заполните поле вид инженерных изысканий, на соответствие которым проводилась экспертиза проектной документации").ToArray();
                Conclusion.Summary.ProjectDocumentsSummary = GetStringValue(memoProjectDocumentsSummary.EditValue, memoProjectDocumentsSummary, "Заполните поле вывод о соответствии технической части проектной документации результатам инженерных изысканий, заданию застройщика или технического заказчика на проектирование и требованиям технических регламентов");
                Conclusion.Summary.ProjectDocumentsSummaryDate = GetStringValue(memoProjectDocumentsSummaryDate.EditValue, memoProjectDocumentsSummaryDate, "Заполните поле сведения о дате, по состоянию на которую действовали требования, примененные в соответствии с частью 5.2 статьи 49 Градостроительного кодекса Российской Федерации (в части экспертизы проектной документации)");
                Conclusion.Summary.EstimateNormsAndWorksSummary = GetStringValue(memoEstimateNormsAndWorksSummary.EditValue, memoEstimateNormsAndWorksSummary, "Заполните поле вывод о соответствии расчетов, содержащихся в сметной документации, утвержденным сметным нормативам, сведения о которых включены в федеральный реестр сметных нормативов, физическим объемам работ, конструктивным, организационно-технологическим и другим решениям, предусмотренным проектной документацией или Выводы о соответствии (несоответствии) расчетов, содержащихся в сметной документации, физическим объемам работ, включенным в ведомость объемов работ, акт, утвержденный застройщиком или техническим заказчиком и содержащий перечень дефектов оснований, строительных конструкций, систем инженерно-технического обеспечения и сетей инженерно-технического обеспечения с указанием качественных и количественных характеристик таких дефектов, при проведении проверки достоверности определения сметной стоимости капитального ремонта", isNullAble: true);
                Conclusion.Summary.EstimateSummary = GetStringValue(memoEstimateSummary.EditValue, memoEstimateSummary, "Заполните поле вывод о достоверности или недостоверности определения сметной стоимости строительства, реконструкции, капитального ремонта, сноса объекта капитального строительства, работ по сохранению объектов культурного наследия (памятников истории и культуры) народов Российской Федерации", isNullAble: true);
                #endregion

                #region Page VI
                if (Conclusion.Summary is null)
                {
                    Conclusion.Summary = new tSummary();
                }
                Conclusion.Summary.ExaminationSummary = GetStringValue(memoExaminationSummary.EditValue, memoExaminationSummary, "Заполните поле общие выводы (итоговые выводы по результатам проведенной экспертизы)");
                #endregion

                #region Page VII
                if (_experts.Count > 0)
                {
                    Conclusion.Experts = _experts.ToArray();
                }
                else
                {
                    Conclusion.Experts = null;
                }
                #endregion

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(
                    ex.Message,
                    "Информационное сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            IsSave = true;
            Close();
        }

        private void gridViewExpert_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuExpert.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnExpertAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var form = new ExpretEdit())
            {
                form.ShowDialog();
                if (form.IsSave && form.Expert != null)
                {
                    _experts.Add(form.Expert);
                }
            };

            gridControlExpert.RefreshDataSource();
        }

        private void barBtnExpertEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewExpert.GetRow(gridViewExpert.FocusedRowHandle) is tExpert expert)
            {
                using (var form = new ExpretEdit(expert))
                {
                    form.ShowDialog();
                };

                gridControlExpert.RefreshDataSource();
            }
        }

        private void barBtnExpertDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewExpert.GetRow(gridViewExpert.FocusedRowHandle) is tExpert expert)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _experts.Remove(expert);
                }
            }
        }


        private void gridViewMismatches_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuMismatches.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnMismatchesAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var form = new EstimateMismatchEdit())
            {
                form.ShowDialog();
                //if (form.IsSave && form.EstimateMismatch != null)
                //{
                //    _experts.Add(form.Expert);
                //}
            };

            gridControlExpert.RefreshDataSource();
        }

        private void barBtnMismatchesEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var form = new EstimateMismatchEdit())
            {
                form.ShowDialog();
                //if (form.IsSave && form.EstimateMismatch != null)
                //{
                //    _experts.Add(form.Expert);
                //}
            };

            gridControlExpert.RefreshDataSource();
        }

        private void barBtnMismatchesDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewExpert.GetRow(gridViewExpert.FocusedRowHandle) is tMismatch tMismatch)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _mismatch.Remove(tMismatch);
                }
            }
        }

        private void cmbObjectAddress_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ComboBoxEdit comboBoxEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Ellipsis)
                {
                    var form = default(AddressEdit);
                    if (comboBoxEdit.EditValue is tAddress address)
                    {
                        form = new AddressEdit(address);
                    }
                    else
                    {
                        form = new AddressEdit();
                    }

                    form.ShowDialog();
                    if (form.IsSave)
                    {
                        comboBoxEdit.EditValue = form.Address;
                    }
                }
                else if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    comboBoxEdit.EditValue = null;
                }
            }
        }

        private void barBtnCadastralNumberAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            _cadastralNumbers.Add(new CadastralNumber());
            gridControlCadastralNumber.RefreshDataSource();
        }

        private void barBtnCadastralNumberDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewCadastralNumber.GetRow(gridViewExpert.FocusedRowHandle) is CadastralNumber obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _cadastralNumbers.Remove(obj);
                    gridControlCadastralNumber.RefreshDataSource();
                }
            }
        }

        private void gridViewCadastralNumber_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuCadastralNumber.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void clearEditValue_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (sender is CheckedComboBoxEdit checkedComboBoxEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    foreach (CheckedListBoxItem item in checkedComboBoxEdit.Properties.Items)
                    {
                        item.CheckState = CheckState.Unchecked;
                    }
                }
            }

            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    buttonEdit.EditValue = null;
                }
            }
        }

        private async void cmbObjectAddress_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Ellipsis)
                {
                    var form = default(AddressEdit);
                    if (buttonEdit.EditValue is tAddress address)
                    {
                        form = new AddressEdit(address);
                    }
                    else
                    {
                        form = new AddressEdit();
                    }
                    form.ShowDialog();

                    if (form.IsSave)
                    {
                        buttonEdit.EditValue = form.Address;
                        buttonEdit.Refresh();
                    }
                }
                else if (e.Button.Kind == ButtonPredefines.Search)
                {
                    var obj = buttonEdit.EditValue?.ToString();
                    if (!string.IsNullOrWhiteSpace(obj))
                    {
                        var addressData = await DaDataObj.GetAddressAsync(obj);
                        if (addressData != null)
                        {
                            var address = new tAddress()
                            {
                                Country = addressData.country,
                                District = addressData.area,
                                City = addressData.city,
                                Settlement = addressData.settlement,
                                Street = addressData.street,
                                Building = addressData.house,
                                Room = addressData.flat,
                                Note = addressData.result,
                            };

                            try
                            {
                                var region = GetEnumByAttributeValue<tRegionsRF>(addressData.region_kladr_id?.Replace("0", string.Empty));
                                address.Region = region;
                            }
                            catch (Exception)
                            {
                                XtraMessageBox.Show("Регион не определен, введите вручную",
                                    "Информационное сообщение",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                            }

                            buttonEdit.EditValue = address;
                        }
                    }
                }
            }
        }

        private void gridViewTEI_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuTEI.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnTEIAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            _tTEIs.Add(new tTEI());
            gridControlTEI.RefreshDataSource();
        }

        private void barBtnTEIDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewTEI.GetRow(gridViewTEI.FocusedRowHandle) is tTEI obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tTEIs.Remove(obj);
                    gridControlTEI.RefreshDataSource();
                }
            }
        }

        private void gridViewObjectPart_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuObjectPart.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnObjectPartAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new ObjPartEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tObjectParts.Add(form.ObjectPart);
                gridControlObjectPart.RefreshDataSource();
            }
        }

        private void barBtnObjectPartEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewObjectPart.GetRow(gridViewObjectPart.FocusedRowHandle) is tObjectPart obj)
            {
                var form = new ObjPartEdit(obj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlObjectPart.RefreshDataSource();
                }
            }
        }

        private void barBtnObjectPartDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewObjectPart.GetRow(gridViewObjectPart.FocusedRowHandle) is tObjectPart obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tObjectParts.Remove(obj);
                    gridControlObjectPart.RefreshDataSource();
                }
            }
        }

        private void gridViewFinances_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuFinance.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnFinanceAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new FundingInformationEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tFinances.Add(form.Finance);
                gridControlFinances.DataSource = FinanceObjController.GetFinanceObj(_tFinances);
                gridControlFinances.RefreshDataSource();
            }
        }

        private void barBtnFinanceEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewFinances.GetRow(gridViewFinances.FocusedRowHandle) is FinanceObj obj)
            {
                var form = new FundingInformationEdit(obj.tFinance);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlFinances.DataSource = FinanceObjController.GetFinanceObj(_tFinances);
                    gridControlFinances.RefreshDataSource();
                }
            }
        }

        private void barBtnFinanceDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewFinances.GetRow(gridViewFinances.FocusedRowHandle) is FinanceObj obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tFinances.Remove(obj.tFinance);
                    gridControlFinances.DataSource = FinanceObjController.GetFinanceObj(_tFinances);
                    gridControlFinances.RefreshDataSource();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void gridViewDesigners_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuDesigners.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnDesignersAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new DesignerEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tDesigners.Add(form.Designer);
                gridControlDesigners.DataSource = PersonObjController.GetPersonObjByDesigner(_tDesigners)?.ToArray();
                gridControlDesigners.RefreshDataSource();
            }
        }

        private void barBtnDesignersEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewDesigners.GetRow(gridViewDesigners.FocusedRowHandle) is PersonObj obj)
            {
                var form = new DesignerEdit(obj.BaseObj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlDesigners.DataSource = PersonObjController.GetPersonObjByDesigner(_tDesigners)?.ToArray();
                    gridControlDesigners.RefreshDataSource();
                }
            }
        }

        private void barBtnDesignersDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewDesigners.GetRow(gridViewDesigners.FocusedRowHandle) is PersonObj obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (obj.BaseObj is tDesigner tDesigner)
                    {
                        _tDesigners.Remove(tDesigner);
                        gridControlDesigners.DataSource = PersonObjController.GetPersonObj(_tDesigners?.Select(s => s.Item)?.ToArray());
                        gridControlDesigners.RefreshDataSource();
                    }
                }
            }
        }

        private void checkEEPDUse_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckEdit checkEdit)
            {
                if (checkEdit.Checked)
                {
                    radioGroupConclusionNumber.SelectedIndex = 0;
                    layoutControlItemEEPDType.Visibility = LayoutVisibility.Always;
                    layoutControlItemEEPDNumber.Visibility = LayoutVisibility.Always;
                    layoutControlItemEEPDDate.Visibility = LayoutVisibility.Always;
                    layoutControlItemEEPDNote.Visibility = LayoutVisibility.Always;
                }
                else
                {
                    radioGroupConclusionNumber.SelectedIndex = -1;
                    layoutControlItemEEPDType.Visibility = LayoutVisibility.Never;
                    layoutControlItemEEPDNumber.Visibility = LayoutVisibility.Never;
                    layoutControlItemEEPDDate.Visibility = LayoutVisibility.Never;
                    layoutControlItemEEPDNote.Visibility = LayoutVisibility.Never;
                }
            }
        }

        private void radioGroupConclusionNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is RadioGroup radioGroup)
            {
                switch (radioGroup.SelectedIndex)
                {
                    case (int)ItemChoiceType.EGRZ:
                        txtEEPDNumber.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegularMaskManager));
                        txtEEPDNumber.Properties.MaskSettings.Set("MaskManagerSignature", "ignoreMaskBlank=True");
                        txtEEPDNumber.Properties.MaskSettings.Set("mask", "[0-9]{2}-[1-2]{1}-[1-2]{1}-[1-3]{1}-[0-9]{6}-[0-9]{4}");
                        break;
                    case (int)ItemChoiceType.noEGRZ:
                        txtEEPDNumber.Properties.ResetMaskSettings();
                        txtEEPDNumber.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
                        break;
                }
            }
        }

        private void gridViewEngineeringSurveyDeveloper_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuEngineeringSurveyDeveloper.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnEngineeringSurveyDeveloperAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new DeclarantEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tEngineeringSurveyDeveloper.Add(form.Declarant);
                gridControlEngineeringSurveyDeveloper.DataSource = PersonObjController.GetPersonObjByDeclarant(_tEngineeringSurveyDeveloper).ToArray();
                gridControlEngineeringSurveyDeveloper.RefreshDataSource();
            }
        }

        private void barBtnEngineeringSurveyDeveloperEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewEngineeringSurveyDeveloper.GetRow(gridViewEngineeringSurveyDeveloper.FocusedRowHandle) is PersonObj obj)
            {
                var form = new DeclarantEdit(obj.BaseObj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlEngineeringSurveyDeveloper.DataSource = PersonObjController.GetPersonObjByDeclarant(_tEngineeringSurveyDeveloper).ToArray();
                    gridControlEngineeringSurveyDeveloper.RefreshDataSource();
                }
            }
        }

        private void barBtnEngineeringSurveyDeveloperDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewEngineeringSurveyDeveloper.GetRow(gridViewEngineeringSurveyDeveloper.FocusedRowHandle) is PersonObj obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (obj.BaseObj is tDeclarant tDeclarant)
                    {
                        _tEngineeringSurveyDeveloper.Remove(tDeclarant);
                        gridControlEngineeringSurveyDeveloper.DataSource = PersonObjController.GetPersonObjByDeclarant(_tEngineeringSurveyDeveloper).ToArray();
                        gridControlEngineeringSurveyDeveloper.RefreshDataSource();
                    }
                }
            }
        }

        private void gridViewEngineeringSurveyTechnicalCustomer_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuEngineeringSurveyTechnicalCustomer.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnEngineeringSurveyTechnicalCustomerAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new TechnicalCustomerEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tEngineeringSurveyTechnicalCustomer.Add(form.TechnicalCustomer);
                gridControlEngineeringSurveyTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tEngineeringSurveyTechnicalCustomer).ToArray();
                gridControlEngineeringSurveyTechnicalCustomer.RefreshDataSource();
            }
        }

        private void barBtnEngineeringSurveyTechnicalCustomerEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewEngineeringSurveyTechnicalCustomer.GetRow(gridViewEngineeringSurveyTechnicalCustomer.FocusedRowHandle) is PersonObj obj)
            {
                var form = new TechnicalCustomerEdit(obj.BaseObj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlEngineeringSurveyTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tEngineeringSurveyTechnicalCustomer).ToArray();
                    gridControlEngineeringSurveyTechnicalCustomer.RefreshDataSource();
                }
            }
        }

        private void barBtnEngineeringSurveyTechnicalCustomerDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewEngineeringSurveyTechnicalCustomer.GetRow(gridViewEngineeringSurveyTechnicalCustomer.FocusedRowHandle) is PersonObj obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (obj.BaseObj is tTechnicalCustomer tTechnicalCustomer)
                    {
                        _tEngineeringSurveyTechnicalCustomer.Remove(tTechnicalCustomer);
                        gridControlEngineeringSurveyTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tEngineeringSurveyTechnicalCustomer).ToArray();
                        gridControlEngineeringSurveyTechnicalCustomer.RefreshDataSource();
                    }
                }
            }
        }

        private void gridViewProjectDocumentsDeveloper_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuProjectDocumentsDeveloper.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnProjectDocumentsDeveloperAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new DeclarantEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tProjectDocumentsDevelopers.Add(form.Declarant);
                gridControlProjectDocumentsDeveloper.DataSource = PersonObjController.GetPersonObjByDeclarant(_tProjectDocumentsDevelopers).ToArray();
                gridControlProjectDocumentsDeveloper.RefreshDataSource();
            }
        }

        private void barBtnProjectDocumentsDeveloperEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewProjectDocumentsDeveloper.GetRow(gridViewProjectDocumentsDeveloper.FocusedRowHandle) is PersonObj obj)
            {
                var form = new DeclarantEdit(obj.BaseObj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlProjectDocumentsDeveloper.DataSource = PersonObjController.GetPersonObjByDeclarant(_tProjectDocumentsDevelopers).ToArray();
                    gridControlProjectDocumentsDeveloper.RefreshDataSource();
                }
            }
        }

        private void barBtnProjectDocumentsDeveloperDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewProjectDocumentsDeveloper.GetRow(gridViewProjectDocumentsDeveloper.FocusedRowHandle) is PersonObj obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (obj.BaseObj is tDeclarant tDeclarant)
                    {
                        _tProjectDocumentsDevelopers.Remove(tDeclarant);
                        gridControlProjectDocumentsDeveloper.DataSource = PersonObjController.GetPersonObjByDeclarant(_tProjectDocumentsDevelopers).ToArray();
                        gridControlProjectDocumentsDeveloper.RefreshDataSource();
                    }
                }
            }
        }

        private void gridViewProjectDocumentsTechnicalCustomer_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuProjectDocumentsTechnicalCustomer.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnProjectDocumentsTechnicalCustomerAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new TechnicalCustomerEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tProjectDocumentsTechnicalCustomer.Add(form.TechnicalCustomer);
                gridControlProjectDocumentsTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tProjectDocumentsTechnicalCustomer).ToArray();
                gridControlProjectDocumentsTechnicalCustomer.RefreshDataSource();
            }
        }

        private void barBtnProjectDocumentsTechnicalCustomerEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewProjectDocumentsTechnicalCustomer.GetRow(gridViewProjectDocumentsTechnicalCustomer.FocusedRowHandle) is PersonObj obj)
            {
                var form = new TechnicalCustomerEdit(obj.BaseObj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlProjectDocumentsTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tProjectDocumentsTechnicalCustomer).ToArray();
                    gridControlProjectDocumentsTechnicalCustomer.RefreshDataSource();
                }
            }
        }

        private void barBtnProjectDocumentsTechnicalCustomerDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewProjectDocumentsTechnicalCustomer.GetRow(gridViewProjectDocumentsTechnicalCustomer.FocusedRowHandle) is PersonObj obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (obj.BaseObj is tTechnicalCustomer tTechnicalCustomer)
                    {
                        _tProjectDocumentsTechnicalCustomer.Remove(tTechnicalCustomer);
                        gridControlProjectDocumentsTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tProjectDocumentsTechnicalCustomer).ToArray();
                        gridControlProjectDocumentsTechnicalCustomer.RefreshDataSource();
                    }
                }
            }
        }

        private async void btnSearch_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Search)
                {
                    var obj = buttonEdit.EditValue?.ToString();

                    if (!string.IsNullOrWhiteSpace(obj))
                    {
                        var party = await DaDataObj.GetPartyAsync(obj);
                        var data = party?.suggestions?.FirstOrDefault()?.data;
                        if (data != null)
                        {
                            txtOrgFullName.EditValue = data.name?.full_with_opf ?? data.name.ToString();
                            txtOrgOGRN.EditValue = data.ogrn;
                            txtOrgKPP.EditValue = data.kpp;
                            txtOrgINN.EditValue = data.inn;

                            var splitsSplitsName = data.management?.name?.Split();
                            if (splitsSplitsName.Length >= 3)
                            {
                                txtFamilyName.EditValue = splitsSplitsName[0];
                                txtFirstName.EditValue = splitsSplitsName[1];
                                txtSecondName.EditValue = splitsSplitsName[2];
                            }
                            txtPosition.EditValue = data.management?.post;

                            var addressData = data.address?.data;
                            if (addressData != null)
                            {
                                var address = new tAddress()
                                {
                                    Country = addressData.country,
                                    District = addressData.city_district,
                                    City = addressData.city,
                                    Settlement = addressData.settlement,
                                    Street = addressData.street,
                                    Building = addressData.house,
                                    Room = addressData.flat,
                                    Note = addressData.result ?? data.address?.unrestricted_value,
                                };

                                try
                                {
                                    var region = ConclusionEdit.GetEnumByAttributeValue<tRegionsRF>(addressData.region_kladr_id?.Replace("0", string.Empty));
                                    address.Region = region;
                                }
                                catch (Exception)
                                {
                                    XtraMessageBox.Show("Регион не определен, введите вручную",
                                        "Информационное сообщение",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation);
                                }

                                btnAddress.EditValue = address;
                            }
                        }
                    }
                    else
                    {
                        btnSearch.Focus();
                        XtraMessageBox.Show("Введите ИНН или ОГРН организации",
                            "Информационное сообщение",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }
        }

        private async void btnAddress_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    buttonEdit.EditValue = null;
                }
                else if (e.Button.Kind == ButtonPredefines.Ellipsis)
                {
                    var form = default(AddressEdit);
                    if (buttonEdit.EditValue is tAddress address)
                    {
                        form = new AddressEdit(address);
                    }
                    else
                    {
                        form = new AddressEdit();
                    }

                    form.ShowDialog();
                    if (form.IsSave)
                    {
                        buttonEdit.EditValue = form.Address;
                    }
                }
                else if (e.Button.Kind == ButtonPredefines.Search)
                {
                    var obj = buttonEdit.EditValue?.ToString();
                    if (!string.IsNullOrWhiteSpace(obj))
                    {
                        var addressData = await DaDataObj.GetAddressAsync(obj);
                        if (addressData != null)
                        {
                            var address = new tAddress()
                            {
                                Country = addressData.country,
                                District = addressData.city_district,
                                City = addressData.city,
                                Settlement = addressData.settlement,
                                Street = addressData.street,
                                Building = addressData.house,
                                Room = addressData.flat,
                                Note = addressData.result,
                            };

                            try
                            {
                                var region = ConclusionEdit.GetEnumByAttributeValue<tRegionsRF>(addressData.region_kladr_id?.Replace("0", string.Empty));
                                address.Region = region;
                            }
                            catch (Exception)
                            {
                                XtraMessageBox.Show("Регион не определен, введите вручную",
                                    "Информационное сообщение",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                            }

                            buttonEdit.EditValue = address;
                        }
                    }
                }
            }
        }

        private void btnDeclarant_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    buttonEdit.EditValue = null;
                }
                else if (e.Button.Kind == ButtonPredefines.Ellipsis)
                {
                    var form = default(DeclarantEdit);
                    if (buttonEdit.EditValue is tDeclarant tDeclarant)
                    {
                        form = new DeclarantEdit(tDeclarant);
                    }
                    else
                    {
                        form = new DeclarantEdit();
                    }
                    form.ShowDialog();
                    if (form.IsSave)
                    {
                        buttonEdit.EditValue = form.Declarant;
                    }
                }
            }
        }

        private void gridViewPreviousConclusions_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuPreviousConclusions.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnPreviousConclusionsAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new PreviousConclusionEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tPreviousConclusions.Add(form.PreviousConclusion);
                gridControlPreviousConclusions.RefreshDataSource();
            }
        }

        private void barBtnPreviousConclusionsEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewPreviousConclusions.GetRow(gridViewPreviousConclusions.FocusedRowHandle) is tPreviousConclusion obj)
            {
                var form = new PreviousConclusionEdit(obj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlPreviousConclusions.RefreshDataSource();
                }
            }
        }

        private void barBtnPreviousConclusionsDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewPreviousConclusions.GetRow(gridViewPreviousConclusions.FocusedRowHandle) is tPreviousConclusion obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tPreviousConclusions.Remove(obj);
                    gridControlPreviousConclusions.RefreshDataSource();
                }
            }
        }

        private void gridViewExpertProjectDocuments_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuExpertProjectDocuments.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnExpertProjectDocumentsAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new ExpertProjectDocumentsEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tExpertProjectDocuments.Add(form.ExpertProjectDocuments);
                gridControlExpertProjectDocuments.DataSource = ExpertProjectDocumentController.GetExpertProjectDocuments(_tExpertProjectDocuments).ToArray();
                gridControlExpertProjectDocuments.RefreshDataSource();
            }
        }

        private void barBtnExpertProjectDocumentsEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewExpertProjectDocuments.GetRow(gridViewExpertProjectDocuments.FocusedRowHandle) is ExpertProjectDocument obj)
            {
                var form = new ExpertProjectDocumentsEdit(obj.ExpertProjectDocuments);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlExpertProjectDocuments.DataSource = ExpertProjectDocumentController.GetExpertProjectDocuments(_tExpertProjectDocuments).ToArray();
                    gridControlExpertProjectDocuments.RefreshDataSource();
                }
            }
        }

        private void barBtnExpertProjectDocumentsDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewExpertProjectDocuments.GetRow(gridViewExpertProjectDocuments.FocusedRowHandle) is ExpertProjectDocument obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tExpertProjectDocuments.Remove(obj.ExpertProjectDocuments);
                    gridControlExpertProjectDocuments.DataSource = ExpertProjectDocumentController.GetExpertProjectDocuments(_tExpertProjectDocuments).ToArray();
                    gridControlExpertProjectDocuments.RefreshDataSource();
                }
            }
        }

        private void gridViewDocuments_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuDocuments.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnDocumentsAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new DocumentEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tDocuments.Add(form.Document);
                gridControlDocuments.DataSource = DocumentObjController.GetDocumentObjs(_tDocuments).ToArray();
                gridControlDocuments.RefreshDataSource();
            }
        }

        private void barBtnDocumentsEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewDocuments.GetRow(gridViewDocuments.FocusedRowHandle) is DocumentObj obj)
            {
                var form = new DocumentEdit(obj.Document);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlDocuments.DataSource = DocumentObjController.GetDocumentObjs(_tDocuments).ToArray();
                    gridControlDocuments.RefreshDataSource();
                }
            }
        }

        private void barBtnDocumentsDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewDocuments.GetRow(gridViewDocuments.FocusedRowHandle) is DocumentObj obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tDocuments.Remove(obj.Document);
                    gridControlDocuments.DataSource = DocumentObjController.GetDocumentObjs(_tDocuments).ToArray();
                    gridControlDocuments.RefreshDataSource();
                }
            }
        }

        private void gridViewExpertEngineeringSurveys_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuExpertEngineeringSurveys.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnExpertEngineeringSurveysAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new ExpertEngineeringSurveyEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tExpertEngineeringSurveys.Add(form.ExpertEngineeringSurvey);
                gridControlExpertEngineeringSurveys.DataSource = ExpertEngineeringSurveysController.GetExpertEngineeringSurveys(_tExpertEngineeringSurveys).ToArray();
                gridControlExpertEngineeringSurveys.RefreshDataSource();
            }
        }

        private void barBtnExpertEngineeringSurveysEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewExpertEngineeringSurveys.GetRow(gridViewExpertEngineeringSurveys.FocusedRowHandle) is ExpertEngineeringSurvey obj)
            {
                var form = new ExpertEngineeringSurveyEdit(obj.ExpertEngineeringSurveys);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlExpertEngineeringSurveys.DataSource = ExpertEngineeringSurveysController.GetExpertEngineeringSurveys(_tExpertEngineeringSurveys).ToArray();
                    gridControlExpertEngineeringSurveys.RefreshDataSource();
                }
            }
        }

        private void barBtnExpertEngineeringSurveysDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewExpertEngineeringSurveys.GetRow(gridViewExpertEngineeringSurveys.FocusedRowHandle) is ExpertEngineeringSurvey obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tExpertEngineeringSurveys.Remove(obj.ExpertEngineeringSurveys);
                    gridControlExpertEngineeringSurveys.DataSource = ExpertEngineeringSurveysController.GetExpertEngineeringSurveys(_tExpertEngineeringSurveys).ToArray();
                    gridControlExpertEngineeringSurveys.RefreshDataSource();
                }
            }
        }

        private void gridViewEngineeringSurveyAddress_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuEngineeringSurveyAddress.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnEngineeringSurveyAddressAdd_ItemClick(object sender, ItemClickEventArgs e)
        {           
            var form = new EngineeringSurveyAddressEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tEngineeringSurveyAddresses.Add(form.EngineeringSurveyAddress);
                gridControlEngineeringSurveyAddress.DataSource = EngineeringSurveyAddressController.GetEngineeringSurveyAddress(_tEngineeringSurveyAddresses);
                gridControlEngineeringSurveyAddress.RefreshDataSource();
            }
        }

        private void barBtnEngineeringSurveyAddressEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewEngineeringSurveyAddress.GetRow(gridViewEngineeringSurveyAddress.FocusedRowHandle) is EngineeringSurveyAddress obj)
            {
                var form = new EngineeringSurveyAddressEdit(obj.EngineeringSurveyAddressObj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlEngineeringSurveyAddress.DataSource = EngineeringSurveyAddressController.GetEngineeringSurveyAddress(_tEngineeringSurveyAddresses);
                    gridControlEngineeringSurveyAddress.RefreshDataSource();
                }
            }            
        }

        private void barBtnEngineeringSurveyAddressDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewEngineeringSurveyAddress.GetRow(gridViewEngineeringSurveyAddress.FocusedRowHandle) is EngineeringSurveyAddress obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tEngineeringSurveyAddresses.Remove(obj.EngineeringSurveyAddressObj);
                    gridControlEngineeringSurveyAddress.DataSource = EngineeringSurveyAddressController.GetEngineeringSurveyAddress(_tEngineeringSurveyAddresses);
                    gridControlEngineeringSurveyAddress.RefreshDataSource();
                }
            }
        }
    }
}
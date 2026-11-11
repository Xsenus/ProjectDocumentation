using Core.NET.AutoModels;
using Core.NET.AutoModels.ConclusionObj;
using Core.NET.AutoModels.ExplanatoryNoteObj;
using Core.NET.Controllers;
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
using ProjectDocumentation.NET.Models.ExplanatoryNoteObj;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TTechnicalCustomer = Core.NET.AutoModels.ExplanatoryNoteObj.TTechnicalCustomer;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class ExplanatoryNoteEdit : XtraForm
    {
        private ExplanatoryNoteObjType _explanatoryNoteObjType;
        private Session Session => SessionController.GetSession();
        public string FileName { get; private set; }
        public bool IsSave { get; private set; }
        internal Func<ExplanatoryNote, string, Task<bool>> SaveNoteAsync { get; set; }
        internal Func<ExplanatoryNote, string, IReadOnlyList<string>, Task<bool>> SaveDraftAsync { get; set; }
        private bool _saving;
        public ExplanatoryNote ExplanatoryNote { get; private set; }

        private List<tModification> _tModifications = new List<tModification>();
        private List<tAuthor> _tProjectDocumentationAuthors = new List<tAuthor>();
        private List<tWorkPerson> _tSigners = new List<tWorkPerson>();
        private List<tDeveloper> _tDevelopers = new List<tDeveloper>();
        private List<TTechnicalCustomer> _tTechnicalCustomers = new List<TTechnicalCustomer>();
        private List<tUsedNorm> _tUsedNorms = new List<tUsedNorm>();
        private List<tDocument> _tProjectDecisionDocuments = new List<tDocument>();
        private List<tDocument> _tProjectInitialDocuments = new List<tDocument>();

        public ExplanatoryNoteEdit()
        {
            InitializeComponent();
            var draftButton = new SimpleButton { Name = "btnSaveDraft", Text = "Черновик XML…" };
            layoutControlExplanatoryNote.AddItem(draftButton.Text, draftButton, layoutControlItemSave, InsertType.Left).TextVisible = false;
            draftButton.Click += async (_, __) => await SaveFormAsync(draft: true);
            GridContextMenus.Attach(gridViewUsedNorms, popupMenuUsedNorms, barBtnUsedNormsAdd);
            GridContextMenus.Attach(gridViewSigners, popupMenuSigners, barBtnSignerAdd);
            GridContextMenus.Attach(gridViewModifications, popupMenuModifications, barBtnExplanatoryNoteModificationsAdd);
            GridContextMenus.Attach(gridViewDeveloper, popupMenuDeveloper, barBtnDeveloperAdd);
            GridContextMenus.Attach(gridViewTechnicalCustomer, popupMenuTechnicalCustomer, barBtnTechnicalCustomerAdd);
            GridContextMenus.Attach(gridViewProjectDecisionDocuments, popupMenuProjectDecisionDocuments, barBtnProjectDecisionDocumentsAdd);
            GridContextMenus.Attach(gridViewProjectInitialDocuments, popupMenuProjectInitialDocuments, barBtnProjectInitialDocumentsAdd);
            txtVersion.Properties.ReadOnly = true;

            foreach (ExplanatoryNoteObjType item in Enum.GetValues(typeof(ExplanatoryNoteObjType)))
            {
                cmbExplanatoryNoteObjType.Properties.Items.Add(item.GetEnumDescription());
            }
        }

        public ExplanatoryNoteEdit(ExplanatoryNote explanatoryNote) : this()
        {
            ExplanatoryNote = explanatoryNote;
        }

        public void SetFileName(string fileName)
        {
            FileName = fileName;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void cmbExplanatoryNoteObjType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                if (string.IsNullOrWhiteSpace(buttonEdit.Text))
                {
                    layoutControlGroupIndustrialObject.Visibility = LayoutVisibility.Never;
                    return;
                }
                ApplyObjType(EnumExtension.GetDefaultEnum<ExplanatoryNoteObjType>(buttonEdit.Text));
            }
        }

        /// <summary>
        /// Запоминает тип объекта и настраивает блок полей объекта под этот тип.
        /// </summary>
        private void ApplyObjType(ExplanatoryNoteObjType objType)
        {
            _explanatoryNoteObjType = objType;

            // Блок общих полей объекта доступен для всех трёх типов; класса опасности ОПО у непроизводственного объекта нет.
            layoutControlGroupIndustrialObject.Visibility = LayoutVisibility.Always;
            switch (objType)
            {
                case ExplanatoryNoteObjType.NonIndustrialObject:
                    layoutControlGroupIndustrialObject.Text = "Описание объекта капитального строительства непроизводственного назначения";
                    break;
                case ExplanatoryNoteObjType.LinearObject:
                    layoutControlGroupIndustrialObject.Text = "Описание линейного объекта капитального строительства";
                    break;
                default:
                    layoutControlGroupIndustrialObject.Text = "Описание объекта капитального строительства производственного назначения";
                    break;
            }
            layoutControlItemDangerIndustrialClass.Visibility = objType == ExplanatoryNoteObjType.NonIndustrialObject
                ? LayoutVisibility.Never
                : LayoutVisibility.Always;
        }

        /// <summary>
        /// Выставляет тип объекта и в списке, и в поле формы: тип при сохранении не должен зависеть от того,
        /// сработало ли событие выбора в списке.
        /// </summary>
        private void SetObjType(ExplanatoryNoteObjType objType)
        {
            cmbExplanatoryNoteObjType.EditValue = objType.GetEnumDescription();
            ApplyObjType(objType);
        }

        private async Task FillFormControllersAsync()
        {
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(Session).ToListAsync();

            cmbPlacement.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.Placement).ToList());
            cmbDangerousAndComplex.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.DangerousAndComplex).ToList());
            cmbUnique.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.Unique).ToList());
            cmbConstructionType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ConstructionType).ToList());
            cmbDangerIndustrialClass.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.DangerIndustrialClass).ToList());
            cmbLandCategory.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.LandCategory).ToList());
            cmbFireDangerCategory.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.FireDangerCategory).ToList()); 
            cmbResponsibilityLevel.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ResponsibilityLevel).ToList());
        }
        
        private async void ExplanatoryNoteEdit_Load(object sender, EventArgs e)
        {
            txtFileName.EditValue = FileName;

            await FillFormControllersAsync();

            if (ExplanatoryNote is null)
            {
                ExplanatoryNote = new ExplanatoryNote();
                cmbExplanatoryNoteObjType.SelectedIndex = 0;
            }

            txtVersion.EditValue = "01.03";

            #region Page General Information
            txtExplanatoryNoteNumber.EditValue = ExplanatoryNote?.ExplanatoryNoteNumber;
            txtExplanatoryNoteYear.EditValue = ExplanatoryNote?.ExplanatoryNoteYear;
            btnIssueAuthor.EditValue = ExplanatoryNote?.IssueAuthor;
            _tSigners = ExplanatoryNote.Signers?.ToList() ?? new List<tWorkPerson>();
            gridControlSigners.DataSource = _tSigners;
            foreach (GridColumn column in gridViewSigners.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewSigners.Columns[nameof(tWorkPerson.FullName)] is GridColumn gridFullName)
            {
                gridFullName.Caption = "ФИО";
                gridFullName.Visible = true;
            }
            var developers = ExplanatoryNote?.Items;
            if (developers != null)
            {
                foreach (var item in developers)
                {
                    if (item is tDeveloper tDeveloper)
                    {
                        _tDevelopers.Add(tDeveloper);
                    }
                    else if (item is TTechnicalCustomer tTechnicalCustomer)
                    {
                        _tTechnicalCustomers.Add(tTechnicalCustomer);
                    }
                }
            }
            gridControlDeveloper.DataSource = PersonObjController.GetPersonObjByDeveloper(_tDevelopers).ToArray();
            foreach (GridColumn column in gridViewDeveloper.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewDeveloper.Columns[nameof(PersonObj.Name)] is GridColumn gridDeveloperName)
            {
                gridDeveloperName.Caption = "Наименование";
                gridDeveloperName.Visible = true;
            }
            gridControlTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tTechnicalCustomers).ToArray();
            foreach (GridColumn column in gridViewTechnicalCustomer.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewTechnicalCustomer.Columns[nameof(PersonObj.Name)] is GridColumn gridTechnicalCustomerName)
            {
                gridTechnicalCustomerName.Caption = "Наименование";
                gridTechnicalCustomerName.Visible = true;
            }
            #endregion

            // Для новой записки тип объекта выбирает пользователь.
            if (ExplanatoryNote?.Item is tNonIndustrialObject)
            {
                SetObjType(ExplanatoryNoteObjType.NonIndustrialObject);
            }
            else if (ExplanatoryNote?.Item is tLinearObject)
            {
                SetObjType(ExplanatoryNoteObjType.LinearObject);
            }
            else if (ExplanatoryNote?.Item is tIndustrialObject)
            {
                SetObjType(ExplanatoryNoteObjType.IndustrialObject);
            }
            else
            {
                cmbExplanatoryNoteObjType.EditValue = null;
                layoutControlGroupIndustrialObject.Visibility = LayoutVisibility.Never;
            }

            // Поля формы общие для всех трёх типов объекта (IExplanatoryNoteObject).
            if (ExplanatoryNote?.Item is IExplanatoryNoteObject noteObject)
            {
                #region NoteObject
                txtIndustrialObjectID.EditValue = noteObject.ObjectID;
                cmbPlacement.EditValue = cmbPlacement.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.Placement)
                    ?.FirstOrDefault(f => f.ValueInt == noteObject.Placement.GetEnumXmlAttributeValueInt());
                if (noteObject.DangerousAndComplexSpecified is true)
                {
                    cmbDangerousAndComplex.EditValue = cmbDangerousAndComplex.Properties.Items
                                        ?.Cast<GeneralizedDictionaryXPO>()
                                        ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.DangerousAndComplex)
                                        ?.FirstOrDefault(f => f.ValueInt == noteObject.DangerousAndComplex.GetEnumXmlAttributeValueInt());
                }
                else
                {
                    cmbDangerousAndComplex.EditValue = null;
                }
                if (noteObject.UniqueSpecified is true)
                {
                    cmbUnique.EditValue = cmbUnique.Properties.Items
                                        ?.Cast<GeneralizedDictionaryXPO>()
                                        ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.Unique)
                                        ?.FirstOrDefault(f => f.ValueInt == noteObject.Unique.GetEnumXmlAttributeValueInt());
                }
                else
                {
                    cmbUnique.EditValue = null;
                }
                txtIndustrialObjectName.EditValue = noteObject.Name;
                cmbConstructionType.EditValue = cmbConstructionType.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ConstructionType)
                    ?.FirstOrDefault(f => f.ValueInt == noteObject.ConstructionType.GetEnumXmlAttributeValueInt());
                txtConstructionDuration.EditValue = noteObject.ConstructionDuration;
                if (noteObject is IDangerousIndustrialObject dangerousIndustrialObject)
                {
                    cmbDangerIndustrialClass.EditValue = cmbDangerIndustrialClass.Properties.Items
                        ?.Cast<GeneralizedDictionaryXPO>()
                        ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.DangerIndustrialClass)
                        ?.FirstOrDefault(f => f.Value == dangerousIndustrialObject.DangerousIndustrialObject.GetEnumXmlAttributeValue()?.ToString());
                }
                cmbLandCategory.EditValue = cmbLandCategory.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.LandCategory)
                    ?.FirstOrDefault(f => f.ValueInt == noteObject.LandCategory.GetEnumXmlAttributeValueInt());
                // GetItem возвращает null, если значения нет (раньше показывалось первое значение перечисления).
                var fireDangerCategory = noteObject.GetItem<tFireDangerCategory>();
                cmbFireDangerCategory.EditValue = fireDangerCategory is null ? null : cmbFireDangerCategory.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.FireDangerCategory)
                    ?.FirstOrDefault(f => f.Value == fireDangerCategory.Value.GetEnumXmlAttributeValue()?.ToString());
                var responsibilityLevel = noteObject.GetItem<tResponsibilityLevel>();
                cmbResponsibilityLevel.EditValue = responsibilityLevel is null ? null : cmbResponsibilityLevel.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ResponsibilityLevel)
                    ?.FirstOrDefault(f => f.Value == responsibilityLevel.Value.GetEnumXmlAttributeValue()?.ToString());
                btnPeoplePermanentStay.EditValue = noteObject.GetPeoplePermanentStay();
                #endregion
            }

            #region Intelligence
            _tModifications = ExplanatoryNote?.ExplanatoryNoteModifications?.ToList() ?? new List<tModification>();
            gridControlModifications.DataSource = _tModifications;
            foreach (GridColumn column in gridViewModifications.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewModifications.Columns[nameof(tModification.ModificationNote)] is GridColumn gridModificationNote)
            {
                gridModificationNote.Caption = "Описание изменения";
                gridModificationNote.Visible = true;
            }
            if (gridViewModifications.Columns[nameof(tModification.ModificationDate)] is GridColumn gridModificationDate)
            {
                gridModificationDate.Caption = "Дата изменения";
                gridModificationDate.Visible = true;
                gridModificationDate.Width = 150;
                gridModificationDate.OptionsColumn.FixedWidth = true;
                gridModificationDate.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            }
            if (gridViewModifications.Columns[nameof(tModification.ModificationNumber)] is GridColumn gridModificationNumber)
            {
                gridModificationNumber.Caption = "Номер изменения";
                gridModificationNumber.Visible = true;
            }
            _tProjectDocumentationAuthors = ExplanatoryNote?.ProjectDocumentationAuthors?.ToList() ?? new List<tAuthor>();
            gridControlProjectDocumentationAuthors.DataSource = _tProjectDocumentationAuthors;
            foreach (GridColumn column in gridViewProjectDocumentationAuthors.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewProjectDocumentationAuthors.Columns[nameof(tAuthor.Item)] is GridColumn gridItem)
            {
                gridItem.Caption = "Сведения об индивидуальном предпринимателе или юридическом лице, подготовившем проектную документацию (кроме генерального проектировщика)";
                gridItem.Visible = true;
            }
            #endregion

            #region Documents
            _tUsedNorms = ExplanatoryNote?.UsedNorms?.UsedNorms?.ToList() ?? new List<tUsedNorm>();
            gridControlUsedNorms.DataSource = _tUsedNorms;
            foreach (GridColumn column in gridViewUsedNorms.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewUsedNorms.Columns[nameof(tUsedNorm.Value)] is GridColumn gridUsedNormValue)
            {
                gridUsedNormValue.Caption = "Описание нормативного требования (документа)";
                gridUsedNormValue.Visible = true;
            }
            _tProjectDecisionDocuments = ExplanatoryNote?.ProjectDecisionDocuments?.ToList() ?? new List<tDocument>();
            gridControlProjectDecisionDocuments.DataSource = DocumentObjEnController.GetDocumentObjS(_tProjectDecisionDocuments).ToArray();
            foreach (GridColumn column in gridViewProjectDecisionDocuments.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewProjectDecisionDocuments.Columns[nameof(DocumentObjEn.DocChanges)] is GridColumn gridDocumentDocChanges)
            {
                gridDocumentDocChanges.Caption = "Примечание";
                gridDocumentDocChanges.Visible = true;
            }
            if (gridViewProjectDecisionDocuments.Columns[nameof(DocumentObjEn.DocDate)] is GridColumn gridDocumentDocDate)
            {
                gridDocumentDocDate.Caption = "Дата документа";
                gridDocumentDocDate.Visible = true;
            }
            if (gridViewProjectDecisionDocuments.Columns[nameof(DocumentObjEn.DocNumber)] is GridColumn gridDocumentDocNumber)
            {
                gridDocumentDocNumber.Caption = "Номер документа";
                gridDocumentDocNumber.Visible = true;
            }
            if (gridViewProjectDecisionDocuments.Columns[nameof(DocumentObjEn.DocName)] is GridColumn gridDocumentDocName)
            {
                gridDocumentDocName.Caption = "Наименование документа";
                gridDocumentDocName.Visible = true;
            }
            if (gridViewProjectDecisionDocuments.Columns[nameof(DocumentObjEn.DocTypeName)] is GridColumn gridDocumentDocTypeName)
            {
                gridDocumentDocTypeName.Caption = "Тип документа";
                gridDocumentDocTypeName.Visible = true;
            }
            _tProjectInitialDocuments = ExplanatoryNote?.ProjectInitialDocuments?.ToList() ?? new List<tDocument>();
            gridControlProjectInitialDocuments.DataSource = DocumentObjEnController.GetDocumentObjS(_tProjectInitialDocuments).ToArray();
            foreach (GridColumn column in gridViewProjectInitialDocuments.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewProjectInitialDocuments.Columns[nameof(DocumentObjEn.DocChanges)] is GridColumn gridProjectInitialDocumentsDocChanges)
            {
                gridProjectInitialDocumentsDocChanges.Caption = "Примечание";
                gridProjectInitialDocumentsDocChanges.Visible = true;
            }
            if (gridViewProjectInitialDocuments.Columns[nameof(DocumentObjEn.DocDate)] is GridColumn gridProjectInitialDocumentsDocDate)
            {
                gridProjectInitialDocumentsDocDate.Caption = "Дата документа";
                gridProjectInitialDocumentsDocDate.Visible = true;
            }
            if (gridViewProjectInitialDocuments.Columns[nameof(DocumentObjEn.DocNumber)] is GridColumn gridProjectInitialDocumentsDocNumber)
            {
                gridProjectInitialDocumentsDocNumber.Caption = "Номер документа";
                gridProjectInitialDocumentsDocNumber.Visible = true;
            }
            if (gridViewProjectInitialDocuments.Columns[nameof(DocumentObjEn.DocName)] is GridColumn gridProjectInitialDocumentsDocName)
            {
                gridProjectInitialDocumentsDocName.Caption = "Наименование документа";
                gridProjectInitialDocumentsDocName.Visible = true;
            }
            if (gridViewProjectInitialDocuments.Columns[nameof(DocumentObjEn.DocTypeName)] is GridColumn gridProjectInitialDocumentsDocTypeName)
            {
                gridProjectInitialDocumentsDocTypeName.Caption = "Тип документа";
                gridProjectInitialDocumentsDocTypeName.Visible = true;
            }
            #endregion
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private async void btnSave_Click(object sender, EventArgs e) => await SaveFormAsync(draft: false);

        private async Task SaveFormAsync(bool draft)
        {
            if (_saving || (draft && SaveDraftAsync == null)) return;
            FileName = txtFileName.Text;
            var omitted = draft ? new List<string>() : null;

            try
            {
                gridViewModifications.PostEditor();
                gridViewModifications.UpdateCurrentRow();
                gridViewUsedNorms.PostEditor();
                gridViewUsedNorms.UpdateCurrentRow();
                ExplanatoryNote.SchemaVersion = "01.03";

                #region Page General Information
                ExplanatoryNote.ExplanatoryNoteNumber = ConclusionEdit.GetStringValue(txtExplanatoryNoteNumber.EditValue, txtExplanatoryNoteNumber, "Заполните шифр пояснительной записки");
                ExplanatoryNote.ExplanatoryNoteYear = ConclusionEdit.GetStringValue(txtExplanatoryNoteYear.EditValue, txtExplanatoryNoteYear, "Заполните год составления пояснительной записки");
                if (btnIssueAuthor.EditValue is tAuthor tAuthor)
                {
                    ExplanatoryNote.IssueAuthor = tAuthor;
                }
                else
                {
                    ExplanatoryNote.IssueAuthor = null;
                }
                if (_tSigners.Count > 0)
                {
                    ExplanatoryNote.Signers = _tSigners.ToArray();
                }
                else
                {
                    ExplanatoryNote.Signers = null;
                }
                var listObjItems = new List<object>();
                if (_tDevelopers?.Count() > 0)
                {
                    listObjItems.AddRange(_tDevelopers);
                }
                if (_tTechnicalCustomers?.Count() > 0)
                {
                    listObjItems.AddRange(_tTechnicalCustomers);
                }
                if (listObjItems.Count > 0)
                {
                    ExplanatoryNote.Items = listObjItems.ToArray();
                }
                else
                {
                    ExplanatoryNote.Items = null;
                }
                #endregion

                // Существующий объект того же типа дополняется, а не создаётся заново: иначе терялись поля,
                // которых нет в форме (адрес, ТЭП, функции, климат, ресурсы, части объекта и т. д.).
                var objectType = string.IsNullOrWhiteSpace(cmbExplanatoryNoteObjType.Text)
                    ? (ExplanatoryNoteObjType?)null : _explanatoryNoteObjType;
                if (!draft && objectType == null)
                {
                    cmbExplanatoryNoteObjType.Focus();
                    throw new InvalidOperationException("Выберите тип объекта строительства.");
                }
                IExplanatoryNoteObject noteObject;
                switch (objectType)
                {
                    case ExplanatoryNoteObjType.IndustrialObject:
                        noteObject = ExplanatoryNote.Item as tIndustrialObject ?? new tIndustrialObject();
                        break;

                    case ExplanatoryNoteObjType.NonIndustrialObject:
                        noteObject = ExplanatoryNote.Item as tNonIndustrialObject ?? new tNonIndustrialObject();
                        break;

                    case ExplanatoryNoteObjType.LinearObject:
                        noteObject = ExplanatoryNote.Item as tLinearObject ?? new tLinearObject();
                        break;

                    default:
                        noteObject = null;
                        break;
                }

                if (noteObject != null)
                {
                    #region NoteObject
                    var objectPath = objectType.Value.ToString();
                    T ReadRequiredEnum<T>(BaseEdit editor, string name, string message) =>
                        ConclusionEdit.ReadEnumForSave<T>(editor, $"{objectPath}/{name}", message, omitted);
                    noteObject.ObjectID = ConclusionEdit.GetStringValue(txtIndustrialObjectID.EditValue, txtIndustrialObjectID, "Заполните идентификатор объекта (части объекта)");
                    noteObject.Placement = ReadRequiredEnum<tPlacement>(cmbPlacement, "@Placement",
                        "Укажите место расположения объекта капитального строительства.");
                    if (cmbDangerousAndComplex.EditValue is GeneralizedDictionaryXPO)
                    {
                        noteObject.DangerousAndComplexSpecified = true;
                        noteObject.DangerousAndComplex = ConclusionEdit.GetEnumByAttributeValue<tDangerousAndComplex>(ConclusionEdit.GetValueGeneralizedDictionary(
                            cmbDangerousAndComplex,
                            message: "Заполните поле сведения об отнесении объекта к особо опасным и технически сложным объектам (номер подпункта пункта 1 статьи 48.1 Градостроительного кодекса Российской Федерации)")?.Value);
                    }
                    else
                    {
                        noteObject.DangerousAndComplexSpecified = false;
                    }
                    if (cmbUnique.EditValue is GeneralizedDictionaryXPO)
                    {
                        noteObject.UniqueSpecified = true;
                        noteObject.Unique = ConclusionEdit.GetEnumByAttributeValue<tUnique>(ConclusionEdit.GetValueGeneralizedDictionary(
                            cmbUnique,
                            message: "Заполните поле сведения об отнесении объекта к уникальным объектам (номер подпункта пункта 2 статьи 48.1 Градостроительного кодекса Российской Федерации)")?.Value);
                    }
                    else
                    {
                        noteObject.UniqueSpecified = false;
                    }
                    noteObject.Name = ConclusionEdit.GetStringValue(txtIndustrialObjectName.EditValue, txtIndustrialObjectName, "Заполните поле наименование объекта капитального строительства (части сложного (составного) объекта капитального строительства)");
                    noteObject.ConstructionType = ReadRequiredEnum<tConstructionType>(cmbConstructionType, "ConstructionType",
                        "Укажите вид работ, на который разрабатывается проектная документация.");
                    var constructionDurationText = txtConstructionDuration.Text.Trim().Replace(',', '.');
                    if (draft && constructionDurationText.Length == 0)
                        omitted.Add($"{objectPath}/ConstructionDuration");
                    else if (decimal.TryParse(constructionDurationText, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                        CultureInfo.InvariantCulture, out var constructionDuration))
                        noteObject.ConstructionDuration = constructionDuration;
                    else
                    {
                        txtConstructionDuration.Focus();
                        throw new InvalidOperationException("Введите продолжительность работ в месяцах. Например, 1,5.");
                    }
                    if (noteObject is IDangerousIndustrialObject dangerousIndustrialObject)
                    {
                        dangerousIndustrialObject.DangerousIndustrialObject = ReadRequiredEnum<tDangerIndustrialClass>(cmbDangerIndustrialClass,
                            "DangerousIndustrialObject", "Укажите класс опасности опасного производственного объекта.");
                    }
                    noteObject.LandCategory = ReadRequiredEnum<tLandCategory>(cmbLandCategory, "LandCategory",
                        "Укажите категорию земель объекта капитального строительства.");

                    // Части объекта и прочие элементы Items сохраняются; меняются только значения из формы.
                    var fireDangerCategory = ConclusionEdit.GetOptionalEnum<tFireDangerCategory>(
                        cmbFireDangerCategory, out bool fireDangerCategorySpecified);
                    var responsibilityLevel = ConclusionEdit.GetOptionalEnum<tResponsibilityLevel>(
                        cmbResponsibilityLevel, out bool responsibilityLevelSpecified);
                    noteObject.SetEditedItems(
                        fireDangerCategorySpecified ? fireDangerCategory : (tFireDangerCategory?)null,
                        responsibilityLevelSpecified ? responsibilityLevel : (tResponsibilityLevel?)null,
                        btnPeoplePermanentStay.EditValue as tTextBlock);
                    #endregion
                }

                ExplanatoryNote.Item = noteObject;

                #region Intelligence
                if (_tModifications.Count > 0)
                {
                    ExplanatoryNote.ExplanatoryNoteModifications = _tModifications.ToArray();
                }
                else
                {
                    ExplanatoryNote.ExplanatoryNoteModifications = null;
                }
                if (_tProjectDocumentationAuthors.Count > 0)
                {
                    ExplanatoryNote.ProjectDocumentationAuthors = _tProjectDocumentationAuthors.ToArray();
                }
                else
                {
                    ExplanatoryNote.ProjectDocumentationAuthors = null;
                }
                #endregion

                #region Documents
                if (_tUsedNorms.Count > 0)
                {
                    ExplanatoryNote.UsedNorms = new tUsedNorms()
                    {
                        UsedNorms = _tUsedNorms.ToArray()
                    };
                }
                else
                {
                    ExplanatoryNote.UsedNorms = null;
                }
                if (_tProjectDecisionDocuments.Count > 0)
                {
                    ExplanatoryNote.ProjectDecisionDocuments = _tProjectDecisionDocuments.ToArray();
                }
                else
                {
                    ExplanatoryNote.ProjectDecisionDocuments = null;
                }
                if (_tProjectInitialDocuments.Count > 0)
                {
                    ExplanatoryNote.ProjectInitialDocuments = _tProjectInitialDocuments.ToArray();
                }
                else
                {
                    ExplanatoryNote.ProjectInitialDocuments = null;
                }
                #endregion

                Func<ExplanatoryNote, string, Task<bool>> save = SaveNoteAsync;
                if (draft) save = (value, name) => SaveDraftAsync(value, name, omitted);
                if (save != null)
                {
                    _saving = true;
                    layoutControlExplanatoryNote.Enabled = false;
                    try
                    {
                        if (!await save(ExplanatoryNote, FileName)) return;
                    }
                    finally
                    {
                        _saving = false;
                        if (!IsDisposed) layoutControlExplanatoryNote.Enabled = true;
                    }
                }
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

            if (draft) return;
            IsSave = true;
            Close();
        }
        
        private void ClearEditValue(object sender, ButtonPressedEventArgs e)
        {
            if (sender is CheckedComboBoxEdit checkedComboBoxEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    foreach (CheckedListBoxItem item in checkedComboBoxEdit.Properties.Items.Cast<CheckedListBoxItem>())
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_saving && e.CloseReason == CloseReason.UserClosing) e.Cancel = true;
            base.OnFormClosing(e);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnExplanatoryNoteModificationsAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            _tModifications.Add(new tModification() { ModificationDate = DateTime.Now.Date });
            gridControlModifications.RefreshDataSource();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnExplanatoryNoteModificationsDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewModifications.GetRow(gridViewModifications.FocusedRowHandle) is tModification obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tModifications.Remove(obj);
                    gridControlModifications.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnIssueAuthor_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    buttonEdit.EditValue = null;
                }
                else if (e.Button.Kind == ButtonPredefines.Ellipsis)
                {
                    AuthorEdit form;
                    if (buttonEdit.EditValue is tAuthor tAuthor)
                    {
                        form = new AuthorEdit(tAuthor);
                    }
                    else
                    {
                        form = new AuthorEdit();
                    }
                    using (form)
                    {
                        form.ShowDialog();
                        if (form.IsSave)
                        {
                            buttonEdit.EditValue = form.Author;
                        }
                    }
                }
            }
        }

        private void gridViewProjectDocumentationAuthors_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuProjectDocumentationAuthor.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnProjectDocumentationAuthorAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var form = new AuthorEdit())
            {
                form.ShowDialog();
                if (form.IsSave)
                {
                    _tProjectDocumentationAuthors.Add(form.Author);
                    gridControlProjectDocumentationAuthors.RefreshDataSource();
                }
            }
        }

        private void barBtnProjectDocumentationAuthorEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewProjectDocumentationAuthors.GetRow(gridViewProjectDocumentationAuthors.FocusedRowHandle) is tAuthor obj)
            {
                using (var form = new AuthorEdit(obj))
                {
                    form.ShowDialog();
                    if (form.IsSave)
                    {
                        gridControlProjectDocumentationAuthors.RefreshDataSource();
                    }
                }
            }
        }

        private void barBtnProjectDocumentationAuthorDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewProjectDocumentationAuthors.GetRow(gridViewProjectDocumentationAuthors.FocusedRowHandle) is tAuthor obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tProjectDocumentationAuthors.Remove(obj);
                    gridControlProjectDocumentationAuthors.RefreshDataSource();
                }
            }
        }

        private void barBtnSignerAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var form = new WorkPersonEdit())
            {
                form.ShowDialog();
                if (form.IsSave)
                {
                    _tSigners.Add(form.WorkPerson);
                    gridControlSigners.RefreshDataSource();
                }
            }
        }

        private void barBtnSignerEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewSigners.GetRow(gridViewSigners.FocusedRowHandle) is tWorkPerson obj)
            {
                using (var form = new WorkPersonEdit(obj))
                {
                    form.ShowDialog();
                    if (form.IsSave)
                    {
                        gridControlSigners.RefreshDataSource();
                    }
                }
            }
        }

        private void barBtnSignerDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewSigners.GetRow(gridViewSigners.FocusedRowHandle) is tWorkPerson obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tSigners.Remove(obj);
                    gridControlSigners.RefreshDataSource();
                }
            }
        }

        private void barBtnDeveloperAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var form = new DeveloperEdit())
            {
                form.ShowDialog();
                if (form.IsSave)
                {
                    _tDevelopers.Add(form.Developer);
                    gridControlDeveloper.DataSource = PersonObjController.GetPersonObjByDeveloper(_tDevelopers).ToArray();
                    gridControlDeveloper.RefreshDataSource();
                }
            }
        }

        private void barBtnDeveloperEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewDeveloper.GetRow(gridViewDeveloper.FocusedRowHandle) is PersonObj obj)
            {
                using (var form = new DeveloperEdit(obj.BaseObj))
                {
                    form.ShowDialog();
                    if (form.IsSave)
                    {
                        gridControlDeveloper.DataSource = PersonObjController.GetPersonObjByDeveloper(_tDevelopers).ToArray();
                        gridControlDeveloper.RefreshDataSource();
                    }
                }
            }
        }

        private void barBtnDeveloperDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewDeveloper.GetRow(gridViewDeveloper.FocusedRowHandle) is PersonObj obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (obj.BaseObj is tDeveloper tDeveloper)
                    {
                        _tDevelopers.Remove(tDeveloper);
                        gridControlDeveloper.DataSource = PersonObjController.GetPersonObjByDeveloper(_tDevelopers).ToArray();
                        gridControlDeveloper.RefreshDataSource();
                    }
                }
            }
        }

        private void barBtnTechnicalCustomerAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var form = new TechnicalCustomerENEdit())
            {
                form.ShowDialog();
                if (form.IsSave)
                {
                    _tTechnicalCustomers.Add(form.TechnicalCustomer);
                    gridControlTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tTechnicalCustomers).ToArray();
                    gridControlTechnicalCustomer.RefreshDataSource();
                }
            }
        }

        private void barBtnTechnicalCustomerEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewTechnicalCustomer.GetRow(gridViewTechnicalCustomer.FocusedRowHandle) is PersonObj obj)
            {
                using (var form = new TechnicalCustomerENEdit(obj.BaseObj))
                {
                    form.ShowDialog();
                    if (form.IsSave)
                    {
                        gridControlTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tTechnicalCustomers).ToArray();
                        gridControlTechnicalCustomer.RefreshDataSource();
                    }
                }
            }
        }

        private void barBtnTechnicalCustomerDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewTechnicalCustomer.GetRow(gridViewTechnicalCustomer.FocusedRowHandle) is PersonObj obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (obj.BaseObj is TTechnicalCustomer tTechnicalCustomer)
                    {
                        _tTechnicalCustomers.Remove(tTechnicalCustomer);
                        gridControlTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tTechnicalCustomers).ToArray();
                        gridControlTechnicalCustomer.RefreshDataSource();
                    }
                }
            }
        }

        private void barBtnUsedNormsAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            _tUsedNorms.Add(new tUsedNorm());
            gridControlUsedNorms.RefreshDataSource();
        }

        private void barBtnUsedNormsDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewUsedNorms.GetFocusedRow() is tUsedNorm obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tUsedNorms.Remove(obj);
                    gridControlUsedNorms.RefreshDataSource();
                }
            }
        }

        private void barBtnProjectDecisionDocumentsAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var form = new DocumentENEdit())
            {
                form.ShowDialog();
                if (form.IsSave)
                {
                    _tProjectDecisionDocuments.Add(form.Document);
                    gridControlProjectDecisionDocuments.DataSource = DocumentObjEnController.GetDocumentObjS(_tProjectDecisionDocuments).ToArray();
                    gridControlProjectDecisionDocuments.RefreshDataSource();
                }
            }
        }

        private void barBtnProjectDecisionDocumentsEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewProjectDecisionDocuments.GetRow(gridViewProjectDecisionDocuments.FocusedRowHandle) is DocumentObjEn obj)
            {
                using (var form = new DocumentENEdit(obj.Document))
                {
                    form.ShowDialog();
                    if (form.IsSave)
                    {
                        gridControlProjectDecisionDocuments.DataSource = DocumentObjEnController.GetDocumentObjS(_tProjectDecisionDocuments).ToArray();
                        gridControlProjectDecisionDocuments.RefreshDataSource();
                    }
                }
            }
        }

        private void barBtnProjectDecisionDocumentsDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewProjectDecisionDocuments.GetRow(gridViewProjectDecisionDocuments.FocusedRowHandle) is DocumentObjEn obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tProjectDecisionDocuments.Remove(obj.Document);
                    gridControlProjectDecisionDocuments.DataSource = DocumentObjEnController.GetDocumentObjS(_tProjectDecisionDocuments).ToArray();
                    gridControlProjectDecisionDocuments.RefreshDataSource();
                }
            }
        }
        
        private void barBtnProjectInitialDocumentsAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var form = new DocumentENEdit())
            {
                form.ShowDialog();
                if (form.IsSave)
                {
                    _tProjectInitialDocuments.Add(form.Document);
                    gridControlProjectInitialDocuments.DataSource = DocumentObjEnController.GetDocumentObjS(_tProjectInitialDocuments).ToArray();
                    gridControlProjectInitialDocuments.RefreshDataSource();
                }
            }
        }

        private void barBtnProjectInitialDocumentsEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewProjectInitialDocuments.GetRow(gridViewProjectInitialDocuments.FocusedRowHandle) is DocumentObjEn obj)
            {
                using (var form = new DocumentENEdit(obj.Document))
                {
                    form.ShowDialog();
                    if (form.IsSave)
                    {
                        gridControlProjectInitialDocuments.DataSource = DocumentObjEnController.GetDocumentObjS(_tProjectInitialDocuments).ToArray();
                        gridControlProjectInitialDocuments.RefreshDataSource();
                    }
                }
            }
        }

        private void barBtnProjectInitialDocumentsDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewProjectInitialDocuments.GetRow(gridViewProjectInitialDocuments.FocusedRowHandle) is DocumentObjEn obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tProjectInitialDocuments.Remove(obj.Document);
                    gridControlProjectInitialDocuments.DataSource = DocumentObjEnController.GetDocumentObjS(_tProjectInitialDocuments).ToArray();
                    gridControlProjectInitialDocuments.RefreshDataSource();
                }
            }
        }

        private void btnPeoplePermanentStay_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                TextBlockEdit form;
                if (buttonEdit.EditValue is tTextBlock tTextBlock)
                {
                    form = new TextBlockEdit(tTextBlock);
                }
                else
                {
                    form = new TextBlockEdit();
                }
                using (form)
                {
                    form.ShowDialog();
                }
            }
        }
    }
}

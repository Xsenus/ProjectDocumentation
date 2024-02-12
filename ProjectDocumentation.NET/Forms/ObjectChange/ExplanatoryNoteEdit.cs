using Core.NET.AutoModels;
using Core.NET.AutoModels.ExplanatoryNoteObj;
using Core.NET.Controllers;
using Core.NET.Models.XPO;
using DevExpress.Utils;
using DevExpress.Xpo;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using ProjectDocumentation.NET.Models.ConclusionObj;
using ProjectDocumentation.NET.Models.ExplanatoryNoteObj;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class ExplanatoryNoteEdit : XtraForm
    {
        private Session Session => SessionController.GetSession();
        public string FileName { get; private set; }
        public bool IsSave { get; private set; }
        public ExplanatoryNote ExplanatoryNote { get; private set; }

        private List<tModification> _tModifications = new List<tModification>();
        private List<tAuthor> _tProjectDocumentationAuthors = new List<tAuthor>();
        private List<tWorkPerson> _tSigners = new List<tWorkPerson>();
        private List<tDeveloper> _tDevelopers = new List<tDeveloper>();
        private List<ITechnicalCustomer> _tTechnicalCustomers = new List<ITechnicalCustomer>();
        private List<tUsedNorm> _tUsedNorms = new List<tUsedNorm>();
        private List<tDocument> _tProjectDecisionDocuments = new List<tDocument>();
        private List<tDocument> _tProjectInitialDocuments = new List<tDocument>();

        public ExplanatoryNoteEdit()
        {
            InitializeComponent();
        }

        public ExplanatoryNoteEdit(ExplanatoryNote explanatoryNote) : this()
        {
            ExplanatoryNote = explanatoryNote;
        }

        public void SetFileName(string fileName)
        {
            FileName = fileName;
        }

        private async Task FillFormControllersAsync()
        {
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(Session).ToListAsync();
        }
        
        private async void ExplanatoryNoteEdit_Load(object sender, EventArgs e)
        {
            txtFileName.EditValue = FileName;

            await FillFormControllersAsync();

            if (ExplanatoryNote is null)
            {
                ExplanatoryNote = new ExplanatoryNote();
            }

            txtVersion.EditValue = ExplanatoryNote.SchemaVersion;

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
                    else if (item is tTechnicalCustomer tTechnicalCustomer)
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
        private void btnSave_Click(object sender, EventArgs e)
        {
            FileName = txtFileName.Text;

            try
            {
                var schemaVersion = txtVersion.EditValue?.ToString()?.Trim();
                if (string.IsNullOrWhiteSpace(schemaVersion))
                {
                    schemaVersion = "01.03";
                }
                ExplanatoryNote.SchemaVersion = schemaVersion;

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridViewExplanatoryNoteModifications_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuModifications.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
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
                    form.ShowDialog();
                    if (form.IsSave)
                    {
                        buttonEdit.EditValue = form.Author;
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
            var form = new AuthorEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tProjectDocumentationAuthors.Add(form.Author);
                gridControlProjectDocumentationAuthors.RefreshDataSource();
            }
        }

        private void barBtnProjectDocumentationAuthorEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewProjectDocumentationAuthors.GetRow(gridViewProjectDocumentationAuthors.FocusedRowHandle) is tAuthor obj)
            {
                var form = new AuthorEdit(obj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlProjectDocumentationAuthors.RefreshDataSource();
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

        private void gridViewSigners_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuSigners.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnSignerAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new WorkPersonEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tSigners.Add(form.WorkPerson);
                gridControlSigners.RefreshDataSource();
            }
        }

        private void barBtnSignerEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewSigners.GetRow(gridViewSigners.FocusedRowHandle) is tWorkPerson obj)
            {
                var form = new WorkPersonEdit(obj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlSigners.RefreshDataSource();
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

        private void gridViewDeveloper_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuDeveloper.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnDeveloperAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new DeveloperEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tDevelopers.Add(form.Developer);
                gridControlDeveloper.DataSource = PersonObjController.GetPersonObjByDeveloper(_tDevelopers).ToArray();
                gridControlDeveloper.RefreshDataSource();
            }
        }

        private void barBtnDeveloperEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewDeveloper.GetRow(gridViewDeveloper.FocusedRowHandle) is PersonObj obj)
            {
                var form = new DeveloperEdit(obj.BaseObj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlDeveloper.DataSource = PersonObjController.GetPersonObjByDeveloper(_tDevelopers).ToArray();
                    gridControlDeveloper.RefreshDataSource();
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

        private void gridViewTechnicalCustomer_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuTechnicalCustomer.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnTechnicalCustomerAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new TechnicalCustomerENEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tTechnicalCustomers.Add(form.TechnicalCustomer);
                gridControlTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tTechnicalCustomers).ToArray();
                gridControlTechnicalCustomer.RefreshDataSource();
            }
        }

        private void barBtnTechnicalCustomerEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewTechnicalCustomer.GetRow(gridViewTechnicalCustomer.FocusedRowHandle) is PersonObj obj)
            {
                var form = new TechnicalCustomerENEdit(obj.BaseObj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tTechnicalCustomers).ToArray();
                    gridControlTechnicalCustomer.RefreshDataSource();
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
                    if (obj.BaseObj is tTechnicalCustomer tTechnicalCustomer)
                    {
                        _tTechnicalCustomers.Remove(tTechnicalCustomer);
                        gridControlTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tTechnicalCustomers).ToArray();
                        gridControlTechnicalCustomer.RefreshDataSource();
                    }
                }
            }
        }

        private void gridViewUsedNorms_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuUsedNorms.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnUsedNormsAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            _tUsedNorms.Add(new tUsedNorm());
            gridControlUsedNorms.RefreshDataSource();
        }

        private void barBtnUsedNormsDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewModifications.GetRow(gridViewModifications.FocusedRowHandle) is tUsedNorm obj)
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

        private void gridViewProjectDecisionDocuments_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuProjectDecisionDocuments.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnProjectDecisionDocumentsAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new DocumentENEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tProjectDecisionDocuments.Add(form.Document);
                gridControlProjectDecisionDocuments.DataSource = DocumentObjEnController.GetDocumentObjS(_tProjectDecisionDocuments).ToArray();
                gridControlProjectDecisionDocuments.RefreshDataSource();
            }
        }

        private void barBtnProjectDecisionDocumentsEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewProjectDecisionDocuments.GetRow(gridViewProjectDecisionDocuments.FocusedRowHandle) is DocumentObjEn obj)
            {
                var form = new DocumentENEdit(obj.Document);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlProjectDecisionDocuments.DataSource = DocumentObjEnController.GetDocumentObjS(_tProjectDecisionDocuments).ToArray();
                    gridControlProjectDecisionDocuments.RefreshDataSource();
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
        
        private void gridViewProjectInitialDocuments_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuProjectInitialDocuments.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnProjectInitialDocumentsAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new DocumentENEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tProjectInitialDocuments.Add(form.Document);
                gridControlProjectInitialDocuments.DataSource = DocumentObjEnController.GetDocumentObjS(_tProjectInitialDocuments).ToArray();
                gridControlProjectInitialDocuments.RefreshDataSource();
            }
        }

        private void barBtnProjectInitialDocumentsEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewProjectInitialDocuments.GetRow(gridViewProjectInitialDocuments.FocusedRowHandle) is DocumentObjEn obj)
            {
                var form = new DocumentENEdit(obj.Document);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlProjectInitialDocuments.DataSource = DocumentObjEnController.GetDocumentObjS(_tProjectInitialDocuments).ToArray();
                    gridControlProjectInitialDocuments.RefreshDataSource();
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
    }
}
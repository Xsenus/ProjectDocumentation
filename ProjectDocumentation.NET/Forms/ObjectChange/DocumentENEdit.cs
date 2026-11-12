using Core.NET.Algorithms;
using Core.NET.AutoModels.ConclusionObj;
using Core.NET.AutoModels.ExplanatoryNoteObj;
using Core.NET.Controllers;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class DocumentENEdit : XtraForm
    {
        public List<tFile> _tFiles = new List<tFile>();

        public DocumentENEdit()
        {
            InitializeComponent();
            ClassicRequiredFields.HighlightExisting(this);
            ClassicRequiredFields.Unmark(txtDocName);
            ClassicRequiredFields.Mark(dateDocDate);
            GridContextMenus.Attach(gridViewItems, popupMenuItems, barBtnItemsAdd);
            layoutControlItemDocIssueAuthor.Text = layoutControlItemDocIssueAuthor.Text.TrimEnd(' ', '*');
        }

        public DocumentENEdit(tDocument tDocument) : this()
        {
            Document = tDocument;
        }

        private void AddFiles(string[] files)
        {
            if (files != null && files.Count() > 0)
            {
                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);

                    var fileObj = new tFile()
                    {
                        FileName = fileInfo.Name,
                        FileFormat = fileInfo.Extension?.Replace(".", string.Empty),
                        FileChecksum = GetCheckSum(fileInfo)?.ToLower()
                    };

                    _tFiles.Add(fileObj);
                }

                gridControlItems.RefreshDataSource();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnItemsAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var ofd = new XtraOpenFileDialog { Multiselect = true, CheckFileExists = true })
            {
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    var files = ofd.FileNames;
                    AddFiles(files);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnItemsDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewItems.GetRow(gridViewItems.FocusedRowHandle) is tFile obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tFiles.Remove(obj);
                    gridControlItems.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var docType = ConclusionEdit.GetEnumByAttributeValue<TDocumentType>(ConclusionEdit.GetValueGeneralizedDictionary(
                    cmbDocType,
                    message: "Заполните поле тип документа")?.Value);
                if (!(dateDocDate.EditValue is DateTime date))
                {
                    dateDocDate.Focus();
                    throw new InvalidOperationException("Укажите дату документа.");
                }

                var items = new List<object>();
                var choices = new List<ItemsChoiceType1>();
                var remainingFiles = new HashSet<tFile>(_tFiles);
                if (Document.Items != null)
                {
                    for (var index = 0; index < Document.Items.Length; index++)
                    {
                        var item = Document.Items[index];
                        if (item is tFile file && !remainingFiles.Remove(file)) continue;
                        items.Add(item);
                        choices.Add(Document.ItemsElementName[index]);
                    }
                }

                foreach (var file in _tFiles.Where(remainingFiles.Contains))
                {
                    items.Add(file);
                    choices.Add(ItemsChoiceType1.File);
                }

                Document.DocType = docType;
                Document.DocName = txtDocName.EditValue?.ToString();
                Document.DocNumber = txtDocNumber.EditValue?.ToString();
                Document.DocDate = date.Date;
                Document.DocIssueAuthor = string.IsNullOrWhiteSpace(txtDocIssueAuthor.Text) ? null : txtDocIssueAuthor.Text;
                Document.DocChanges = memoDocChanges.EditValue?.ToString();
                Document.Items = items.Count > 0 ? items.ToArray() : null;
                Document.ItemsElementName = choices.Count > 0 ? choices.ToArray() : null;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Информационное сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IsSave = true;
            Close();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void buttonEdit_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    buttonEdit.EditValue = null;
                }
            }
        }

        private async Task FillFormControllersAsync()
        {
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(Session).ToListAsync();

            cmbDocType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.DocumentType).ToList());
        }

        private string GetCheckSum(FileInfo fileInfo)
        {
            try
            {
                var computeHash = new CRC32().ComputeHashFile(fileInfo.FullName);
                return computeHash.ValueString;
            }
            catch (Exception) { }
            return default;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridControlItems_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            AddFiles(files);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridControlItems_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private async void ObjPartEdit_Load(object sender, EventArgs e)
        {
            await FillFormControllersAsync();

            if (Document is null)
            {
                Document = new tDocument();
            }

            cmbDocType.EditValue = cmbDocType.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.DocumentType)
                    ?.FirstOrDefault(f => f.Value == Document?.DocType.GetEnumXmlAttributeValue().ToString());
            txtDocName.EditValue = Document.DocName;
            txtDocNumber.EditValue = Document.DocNumber;
            if (Document.DocDate != DateTime.MinValue)
            {
                dateDocDate.EditValue = Document.DocDate;
            }
            memoDocChanges.EditValue = Document.DocChanges;
            txtDocIssueAuthor.EditValue = Document.DocIssueAuthor;

            _tFiles = Document.Items?.OfType<tFile>().ToList() ?? new List<tFile>();
            gridControlItems.DataSource = _tFiles;
            foreach (GridColumn column in gridViewItems.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewItems.Columns[nameof(tFile.FileChecksum)] is GridColumn gridColumntFileFileChecksum)
            {
                gridColumntFileFileChecksum.Caption = "Контрольная сумма";
                gridColumntFileFileChecksum.Visible = true;
            }
            if (gridViewItems.Columns[nameof(tFile.FileFormat)] is GridColumn gridColumntFileFileFormat)
            {
                gridColumntFileFileFormat.Caption = "Формат";
                gridColumntFileFileFormat.Visible = true;
            }
            if (gridViewItems.Columns[nameof(tFile.FileName)] is GridColumn gridColumntFileFileName)
            {
                gridColumntFileFileName.Caption = "Наименование";
                gridColumntFileFileName.Visible = true;
            }
        }

        private Session Session => SessionController.GetSession();
        public tDocument Document { get; private set; }
        public bool IsSave { get; private set; }
    }
}

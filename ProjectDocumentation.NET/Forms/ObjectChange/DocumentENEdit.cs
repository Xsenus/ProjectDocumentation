using Core.NET.Algorithms;
using Core.NET.AutoModels;
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
        private Session Session => SessionController.GetSession();

        public bool IsSave { get; private set; }
        public tDocument Document { get; private set; }
        public List<tFile> _tFiles = new List<tFile>();

        public DocumentENEdit()
        {
            InitializeComponent();
        }

        public DocumentENEdit(tDocument tDocument) : this()
        {
            Document = tDocument;
        }

        private async Task FillFormControllersAsync()
        {
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(Session).ToListAsync();

            cmbDocType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.DocumentType).ToList());
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

            _tFiles = Document.Items?.Cast<tFile>()?.ToList() ?? new List<tFile>();
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Document.DocType = ConclusionEdit.GetEnumByAttributeValue<tDocumentType>(ConclusionEdit.GetValueGeneralizedDictionary(
                    cmbDocType,
                    message: "Заполните поле тип документа")?.Value);
                Document.DocName = ConclusionEdit.GetStringValue(txtDocName.EditValue, txtDocName, "Заполните поле наименование документа.");
                Document.DocNumber = ConclusionEdit.GetStringValue(txtDocNumber.EditValue, txtDocNumber, "Заполните поле номер документа.", isNullAble: true);
                if (dateDocDate.EditValue is DateTime date)
                {
                    Document.DocDate = date;
                }
                else
                {
                    Document.DocDate = default;
                }

                Document.DocChanges = ConclusionEdit.GetStringValue(memoDocChanges.EditValue, memoDocChanges, "Заполните поле примечание об изменении документа.", isNullAble: true);

                if (!string.IsNullOrWhiteSpace(txtDocIssueAuthor.Text))
                {
                    Document.Items = new string[] { txtDocIssueAuthor.Text };
                }
                else
                {
                    txtDocIssueAuthor.Focus();
                    XtraMessageBox.Show("Укажите автора документа.",
                        "Информационное сообщение",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                if (_tFiles.Count > 0)
                {
                    Document.Items = _tFiles.ToArray();
                }
                else
                {
                    Document.Items = null;
                }

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
        private void btnClose_Click(object sender, EventArgs e)
        {
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridViewItems_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuItems.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnItemsAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var ofd = new OpenFileDialog() { Multiselect = true })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var files = ofd.FileNames;
                    AddFiles(files);
                }
            }
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
        private void gridControlItems_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridControlItems_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            AddFiles(files);
        }
    }
}
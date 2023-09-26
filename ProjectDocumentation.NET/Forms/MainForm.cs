using Core.NET.Controllers;
using Core.NET.Models.XPO;
using DevExpress.Utils;
using DevExpress.Xpo;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using ProjectDocumentation.NET.Forms.ObjectChange;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms
{
    public partial class MainForm : XtraForm
    {
        private Session _session => SessionController.GetSession();

        public MainForm()
        {
            InitializeComponent();
        }

        private void gridView_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                if (gridView.GetRow(gridView.FocusedRowHandle) is XmlObjXPO)
                {
                    barBtnView.Enabled = true;
                    barBtnBaseSetting.Enabled = true;
                    barBtnEdit.Enabled = true;
                    barBtnDel.Enabled = true;
                    barBtnFill.Enabled = true;
                }
                else
                {
                    barBtnView.Enabled = false;
                    barBtnBaseSetting.Enabled = false;
                    barBtnEdit.Enabled = false;
                    barBtnDel.Enabled = false;
                    barBtnFill.Enabled = false;
                }

                popupMenu.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private async void barBtnLoad_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var form = new FileLoad())
            {
                form.ShowDialog();
                if (form.ObjXPO != null)
                {
                    await UpdateGridDataSourceAsync(nameof(XmlObjXPO.Guid), form.ObjXPO);
                }
            }
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await DataBaseController.FillingDataBaseAsync();

            await UpdateGridDataSourceAsync();
            InitialSetupGridControl();
        }

        private async Task UpdateGridDataSourceAsync(string columnName = default, object obj = default)
        {
            gridControl.DataSource = await new XPQuery<XmlObjXPO>(_session).ToListAsync();

            if (!string.IsNullOrWhiteSpace(columnName) && obj != null)
            {
                gridView.FocusedRowHandle = gridView.LocateByValue(columnName, obj);
            }
        }

        private async Task UpdateGridDataSource(int focusedRowHandle)
        {
            gridControl.DataSource = await new XPQuery<XmlObjXPO>(_session).ToListAsync();
            gridView.FocusedRowHandle = focusedRowHandle;
        }

        private void InitialSetupGridControl()
        {
            foreach (GridColumn column in gridView.Columns)
            {
                column.Visible = false;
            }

            if (gridView.Columns[nameof(XmlObjXPO.Name)] is GridColumn columnName)
            {
                columnName.Visible = true;
                columnName.Caption = "Наименование";
            }

            if (gridView.Columns[nameof(XmlObjXPO.Guid)] is GridColumn columnGuid)
            {
                columnGuid.Visible = true;
                columnGuid.Caption = "ID";

                columnGuid.Width = 300;
                columnGuid.OptionsColumn.FixedWidth = true;
            }

            if (gridView.Columns[nameof(XmlObjXPO.DateCreate)] is GridColumn columnDateCreate)
            {
                columnDateCreate.Visible = true;
                columnDateCreate.Caption = "Дата создания";

                columnDateCreate.Width = 150;
                columnDateCreate.OptionsColumn.FixedWidth = true;

                columnDateCreate.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                columnDateCreate.AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;

                columnDateCreate.DisplayFormat.FormatType = FormatType.DateTime;
                columnDateCreate.DisplayFormat.FormatString = "dd.MM.yyyy (HH:mm:ss)";
                columnDateCreate.RealColumnEdit.EditFormat.FormatType = FormatType.DateTime;
                columnDateCreate.RealColumnEdit.EditFormat.FormatString = "dd.MM.yyyy (HH:mm:ss)";
            }
        }

        private async void barBtnDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView.GetRow(gridView.FocusedRowHandle) is XmlObjXPO xmlObjXPO)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    await _session.DeleteAsync(xmlObjXPO);
                    await UpdateGridDataSource(gridView.FocusedRowHandle - 1);
                }
            }
        }

        private async void barBtnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView.GetRow(gridView.FocusedRowHandle) is ConclusionXPO conclusionXPO)
            {
                var xml = conclusionXPO.GetObj();
                var conclusion = ConclusionController.GetConclusionByString(xml);
                var form = new ConclusionEdit(conclusion);
                form.SetFileName(conclusionXPO.Name);
                form.ShowDialog();

                if (form.IsSave)
                {
                    conclusionXPO.Name = form.FileName;
                    conclusionXPO.SetObj(form.Conclusion.GetSerializerConclusion());
                    await _session.SaveAsync(conclusionXPO);
                }

                await UpdateGridDataSource(gridView.FocusedRowHandle);
            }
            else if (gridView.GetRow(gridView.FocusedRowHandle) is ExplanatoryNoteXPO explanatoryNoteXPO)
            {

            }
        }

        private void barBtnFill_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var fileName = default(string);
                if (gridView.GetRow(gridView.FocusedRowHandle) is ConclusionXPO conclusionXPO)
                {
                    fileName = "conclusion";
                }
                else if (gridView.GetRow(gridView.FocusedRowHandle) is ExplanatoryNoteXPO explanatoryNoteXPO)
                {

                    fileName = "explanatorynote";
                }

                if (gridView.GetRow(gridView.FocusedRowHandle) is XmlObjXPO xmlObjXPO)
                {
                    var xmlFileName = $"{fileName}_{xmlObjXPO.Guid}.xml";

                    using (var fbd = new FolderBrowserDialog())
                    {
                        if (fbd.ShowDialog() == DialogResult.OK)
                        {
                            var folderPath = fbd.SelectedPath;
                            var outFullPath = Path.Combine(folderPath, xmlFileName);
                            File.WriteAllText(outFullPath, xmlObjXPO.GetObj());

                            XtraMessageBox.Show(
                                $"Файл успешно сформирован и сохранен по следующем пути:{Environment.NewLine}" +
                                    $"{outFullPath}",
                                "Информационное сообщение",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(
                    $"Ошибка сохранения файла XML.{Environment.NewLine}" +
                    $"{ex.Message}",
                    "Информационное сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void barBtnAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new ConclusionEdit();
            form.ShowDialog();

            if (form.IsSave)
            {
                var conclusionXPO = new ConclusionXPO(_session)
                {
                    Name = form.FileName
                };
                conclusionXPO.SetObj(form.Conclusion.GetSerializerConclusion());
                await _session.SaveAsync(conclusionXPO);

                await UpdateGridDataSourceAsync(nameof(XmlObjXPO.Guid), conclusionXPO.Guid);
            }
        }

        private async void gridView_DoubleClick(object sender, EventArgs e)
        {
            if (gridView.GetRow(gridView.FocusedRowHandle) is ConclusionXPO conclusionXPO)
            {
                var xml = conclusionXPO.GetObj();
                var conclusion = ConclusionController.GetConclusionByString(xml);
                var form = new ConclusionEdit(conclusion);
                form.SetFileName(conclusionXPO.Name);
                form.ShowDialog();

                if (form.IsSave)
                {
                    conclusionXPO.Name = form.FileName;
                    conclusionXPO.SetObj(form.Conclusion.GetSerializerConclusion());
                    await _session.SaveAsync(conclusionXPO);
                }

                await UpdateGridDataSource(gridView.FocusedRowHandle);
            }
        }

        private async void barBtnBaseSetting_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView.GetRow(gridView.FocusedRowHandle) is ConclusionXPO conclusionXPO)
            {
                var xml = conclusionXPO.GetObj();
                var conclusion = ConclusionController.GetConclusionByString(xml);
                var form = new ConclusionBaseSettingEdit(conclusion.ExpertOrganization, conclusion.Approver); 
                form.ShowDialog();

                if (form.IsSave)
                {
                    conclusion.ExpertOrganization = form.Organization;
                    conclusion.Approver = form.Person;
                    conclusionXPO.SetObj(conclusion.GetSerializerConclusion());
                    await _session.SaveAsync(conclusionXPO);
                }

                await UpdateGridDataSource(gridView.FocusedRowHandle);
            }
        }

        private void barBtnView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView.GetRow(gridView.FocusedRowHandle) is ConclusionXPO conclusionXPO)
            {
                var xml = conclusionXPO.GetObj();
                var conclusion = ConclusionController.GetConclusionByString(xml);
                var form = new ViewForm(conclusion);
                form.ShowDialog();
            }
        }
    }
}
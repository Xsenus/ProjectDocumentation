using AutoUpdaterDotNET;
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
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms
{
    public partial class MainForm : XtraForm
    {
        private Session Session => SessionController.GetSession();

        public MainForm()
        {
            InitializeComponent();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
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

        private async Task CheckUpdates()
        {
            try
            {
                var path = await new XPQuery<SettingXPO>(Session).FirstOrDefaultAsync(f => f.Name == "UpdatePath");
                if (path is null)
                {
                    path = new SettingXPO(Session)
                    {
                        Name = "UpdatePath",
                        Value = "ftp://90.189.149.59/other/ProjectDocumentation.NET/ProjectDocumentation.NET.Update.xml"
                    };
                    path.Save();
                }

                AutoUpdater.Mandatory = true;
                AutoUpdater.Start(path.Value.ToString());
            }
            catch (Exception) { }
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await DataBaseController.FillingDataBaseAsync();
            await UpdateGridDataSourceAsync();
            InitialSetupGridControl();
            await CheckUpdates();
        }

        private async Task UpdateGridDataSourceAsync(string columnName = default, object obj = default)
        {
            gridControl.DataSource = await new XPQuery<XmlObjXPO>(Session)
                .OrderByDescending(o => o.DateCreate)
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(columnName) && obj != null)
            {
                gridView.FocusedRowHandle = gridView.LocateByValue(columnName, obj);
            }
        }

        private async Task UpdateGridDataSource(int focusedRowHandle)
        {
            gridControl.DataSource = await new XPQuery<XmlObjXPO>(Session).ToListAsync();
            gridView.FocusedRowHandle = focusedRowHandle;
        }

        private void InitialSetupGridControl()
        {
            foreach (GridColumn column in gridView.Columns.Cast<GridColumn>())
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
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
                    await Session.DeleteAsync(xmlObjXPO);
                    await UpdateGridDataSource(gridView.FocusedRowHandle - 1);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private async void barBtnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            await EditGridViewObjectAsync();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
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

                    fileName = "explanatoryNote";
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

                            var result = xmlObjXPO.GetObj();

                            switch (fileName)
                            {
                                case "conclusion":
                                    result = result.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", $"xsi:noNamespaceSchemaLocation=\"{fileName}.xsd\"");
                                    result += "<?xml-stylesheet type=\"text/xsl\" href=\"conclusion.xsl\" ?>";
                                    break;

                                case "explanatoryNote":
                                    result = result.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", $"xsi:noNamespaceSchemaLocation=\"{fileName.ToLower()}-01-03.xsd\"");
                                    //result = result.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n", "");
                                    break;
                            }                            

                            File.WriteAllText(outFullPath, result);

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private async void barBtnAddConclusion_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new ConclusionEdit();
            form.ShowDialog();

            if (form.IsSave)
            {
                var conclusionXPO = new ConclusionXPO(Session)
                {
                    Name = form.FileName
                };

                form.Conclusion.ConclusionGUID = conclusionXPO.Guid?.ToString();
                conclusionXPO.SetObj(form.Conclusion.GetSerializeConclusion());
                await Session.SaveAsync(conclusionXPO);

                await UpdateGridDataSourceAsync(nameof(XmlObjXPO.Guid), conclusionXPO.Guid);
            }
        }

        private async void barBtnAddExplanatoryNote_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new ExplanatoryNoteEdit();
            form.ShowDialog();

            if (form.IsSave)
            {
                var explanatoryNoteXPO = new ExplanatoryNoteXPO(Session)
                {
                    Name = form.FileName
                };

                explanatoryNoteXPO.SetObj(form.ExplanatoryNote.GetSerializeExplanatoryNote());
                await Session.SaveAsync(explanatoryNoteXPO);

                await UpdateGridDataSourceAsync(nameof(XmlObjXPO.Guid), explanatoryNoteXPO.Guid);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private async void gridView_DoubleClick(object sender, EventArgs e)
        {
            await EditGridViewObjectAsync();
        }

        private async Task EditGridViewObjectAsync()
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
                    conclusionXPO.SetObj(form.Conclusion.GetSerializeConclusion());
                    await Session.SaveAsync(conclusionXPO);
                }
            }
            else if (gridView.GetRow(gridView.FocusedRowHandle) is ExplanatoryNoteXPO explanatoryNoteXPO)
            {
                var xml = explanatoryNoteXPO.GetObj();
                var explanatoryNote = ExplanatoryNoteController.GetExplanatoryNoteByString(xml);
                var form = new ExplanatoryNoteEdit(explanatoryNote);
                form.SetFileName(explanatoryNoteXPO.Name);
                form.ShowDialog();

                if (form.IsSave)
                {
                    explanatoryNoteXPO.Name = form.FileName;
                    explanatoryNoteXPO.SetObj(form.ExplanatoryNote.GetSerializeExplanatoryNote());
                    await Session.SaveAsync(explanatoryNoteXPO);
                }
            }
            await UpdateGridDataSource(gridView.FocusedRowHandle);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
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
                    conclusionXPO.SetObj(conclusion.GetSerializeConclusion());
                    await Session.SaveAsync(conclusionXPO);
                }

                await UpdateGridDataSource(gridView.FocusedRowHandle);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
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
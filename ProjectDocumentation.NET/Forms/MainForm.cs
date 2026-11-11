using AutoUpdaterDotNET;
using Core.NET.AutoModels.ConclusionObj;
using Core.NET.Controllers;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Utils;
using DevExpress.Xpo;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using ProjectDocumentation.NET.Forms.ObjectChange;
using ProjectDocumentation.NET.Services;
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
            InitializeConclusionCommands();
            InitializeExplanatoryNoteCommands();
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

                UpdateConclusionCommands(gridView.GetFocusedRow());
                UpdateExplanatoryNoteCommands(gridView.GetFocusedRow());
                popupMenu.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private async void barBtnLoad_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var form = new FileLoad(conclusionSchema))
            {
                form.ShowDialog();
                if (form.ObjXPO != null)
                {
                    await UpdateGridDataSourceAsync(nameof(XmlObjXPO.Guid), form.ObjXPO.Guid);
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

        /// <summary>
        /// Перед выгрузкой заключение проверяется по ограничениям схемы из модели (обязательность, длина, формат).
        /// При замечаниях пользователь решает, сохранять ли файл: ведомственная система может его не принять.
        /// </summary>
        private static bool ConfirmSchemaRemarks(string xml)
        {
            var conclusion = Utf8StringSerialize.DeSerializeObjectByString<Conclusion>(xml, lenient: false);
            if (conclusion is null)
            {
                XtraMessageBox.Show(
                    "Не удалось прочитать XML заключения. Файл не сформирован.",
                    "Проверка заключения",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            var remarks = ModelValidator.Validate(conclusion);
            if (remarks.Count == 0)
            {
                return true;
            }

            const int shown = 15;
            var text = string.Join(Environment.NewLine, remarks.Take(shown).Select(remark => "• " + remark));
            if (remarks.Count > shown)
            {
                text += Environment.NewLine + $"… и ещё {remarks.Count - shown}.";
            }

            return XtraMessageBox.Show(
                $"Заключение не соответствует ограничениям схемы (замечаний: {remarks.Count}):" + Environment.NewLine + Environment.NewLine +
                text + Environment.NewLine + Environment.NewLine +
                "Такой XML может не принять система, в которую он загружается. Всё равно сформировать файл?",
                "Проверка заключения",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        /// <summary>
        /// Старая версия при каждом пересохранении удваивала списки заключения.
        /// Если в документе есть одинаковые записи, предлагаем удалить повторы до открытия формы;
        /// в базу изменения попадут только при сохранении формы.
        /// </summary>
        private static void OfferDuplicateCleanup(Conclusion conclusion)
        {
            var duplicates = conclusion.RemoveDuplicateItems(apply: false);
            if (duplicates == 0)
            {
                return;
            }

            var answer = XtraMessageBox.Show(
                $"В заключении найдено повторяющихся записей: {duplicates} (полностью одинаковые документы, эксперты, финансирование и т. п.)." + Environment.NewLine +
                "Обычно это последствие ошибки сохранения в прежней версии программы." + Environment.NewLine + Environment.NewLine +
                "Удалить повторы? Останется по одной записи; изменения сохранятся в базе, когда вы нажмёте «Сохранить» в форме.",
                "Повторяющиеся записи",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (answer == DialogResult.Yes)
            {
                conclusion.RemoveDuplicateItems();
            }
        }

        /// <summary>
        /// Сериализация возвращает null при ошибке: такую запись сохранять нельзя, иначе документ станет пустым.
        /// </summary>
        private static bool CheckSerialized(string xml)
        {
            if (!string.IsNullOrWhiteSpace(xml))
            {
                return true;
            }

            XtraMessageBox.Show(
                "Не удалось сформировать XML документа. Изменения не сохранены.",
                "Информационное сообщение",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return false;
        }

        /// <summary>
        /// Если XML из базы не прочитался, форма открылась бы пустой и при сохранении перезаписала бы документ.
        /// </summary>
        private static bool CheckDeserialized(object obj)
        {
            if (obj != null)
            {
                return true;
            }

            XtraMessageBox.Show(
                "Не удалось прочитать XML документа из базы. Документ не открыт, чтобы не перезаписать его пустыми данными.",
                "Информационное сообщение",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return false;
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
            gridControl.DataSource = await new XPQuery<XmlObjXPO>(Session)
                .OrderByDescending(o => o.DateCreate)
                .ToListAsync();
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
        private async void barBtnFill_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (gridView.GetFocusedRow() is ConclusionXPO current)
                {
                    var xml = current.GetObj();
                    var version = Conclusion0104Document.ReadVersion(xml);
                    if (version == Conclusion0104Document.CurrentVersion)
                    {
                        await ExportConclusionAsync(xml, version, current.Guid);
                        return;
                    }

                    if (Conclusion0104Document.IsHistoricalVersion(version))
                    {
                        ExportHistoricalConclusion(current);
                        return;
                    }

                    if (!CheckConclusionVersion(version))
                    {
                        return;
                    }
                }

                if (gridView.GetFocusedRow() is ExplanatoryNoteXPO note)
                {
                    var xml = note.GetObj();
                    var version = ExplanatoryNote0107Document.ReadVersion(xml);
                    if (CanPreviewExplanatoryNote(version))
                    {
                        await ExportSchemaDocumentAsync(xml, version, note.Guid, "ExplanatoryNote", ExplanatoryNote0107Document.Family);
                        return;
                    }

                    if (!ExplanatoryNoteController.IsLegacyXml(xml))
                    {
                        throw new InvalidOperationException("Не удалось определить поддерживаемую версию пояснительной записки.");
                    }
                }

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

                    if (xmlObjXPO is ConclusionXPO && !ConfirmSchemaRemarks(xmlObjXPO.GetObj()))
                    {
                        return;
                    }

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
                                    // Инструкция xml-stylesheet должна стоять в прологе: после корневого элемента браузер её игнорирует.
                                    result = result.InsertIntoXmlProlog("<?xml-stylesheet type=\"text/xsl\" href=\"conclusion.xsl\" ?>");
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
            await AddCurrentConclusionAsync();
        }

        private async void barBtnAddExplanatoryNote_ItemClick(object sender, ItemClickEventArgs e)
        {
            await AddCurrentExplanatoryNoteAsync();
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
                var version = Conclusion0104Document.ReadVersion(xml);
                if (version == Conclusion0104Document.CurrentVersion)
                {
                    await EditCurrentConclusionAsync(conclusionXPO);
                    return;
                }

                if (Conclusion0104Document.IsHistoricalVersion(version))
                {
                    await ConvertConclusionAsync(conclusionXPO);
                    return;
                }

                if (!CheckConclusionVersion(version))
                {
                    return;
                }

                var conclusion = ConclusionController.GetConclusionByString(xml);
                if (!CheckDeserialized(conclusion))
                {
                    return;
                }

                OfferDuplicateCleanup(conclusion);

                var form = new ConclusionEdit(conclusion);
                form.SetFileName(conclusionXPO.Name);
                using (form)
                {
                    form.ShowDialog();

                    if (form.IsSave)
                    {
                        var newXml = form.Conclusion.GetSerializeConclusion();
                        if (CheckSerialized(newXml))
                        {
                            conclusionXPO.Name = form.FileName;
                            conclusionXPO.SetObj(newXml);
                            await Session.SaveAsync(conclusionXPO);
                        }
                    }
                }
            }
            else if (gridView.GetRow(gridView.FocusedRowHandle) is ExplanatoryNoteXPO explanatoryNoteXPO)
            {
                var xml = explanatoryNoteXPO.GetObj();
                var version = ExplanatoryNote0107Document.ReadVersion(xml);
                if (version == ExplanatoryNote0107Document.CurrentVersion)
                {
                    await EditCurrentExplanatoryNoteAsync(explanatoryNoteXPO);
                    return;
                }

                if (version == ExplanatoryNote0107Document.CompatibleVersion)
                {
                    await ConvertExplanatoryNoteAsync(explanatoryNoteXPO);
                    return;
                }

                var explanatoryNote = ExplanatoryNoteController.GetExplanatoryNoteByString(xml);
                if (!CheckDeserialized(explanatoryNote))
                {
                    return;
                }

                var form = new ExplanatoryNoteEdit(explanatoryNote);
                form.SetFileName(explanatoryNoteXPO.Name);
                using (form)
                {
                    form.ShowDialog();

                    if (form.IsSave)
                    {
                        var newXml = form.ExplanatoryNote.GetSerializeExplanatoryNote();
                        if (CheckSerialized(newXml))
                        {
                            explanatoryNoteXPO.Name = form.FileName;
                            explanatoryNoteXPO.SetObj(newXml);
                            await Session.SaveAsync(explanatoryNoteXPO);
                        }
                    }
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
                var version = Conclusion0104Document.ReadVersion(xml);
                if (version == Conclusion0104Document.CurrentVersion)
                {
                    await EditCurrentConclusionAsync(conclusionXPO, "ExpertOrganization");
                    return;
                }

                if (Conclusion0104Document.IsHistoricalVersion(version))
                {
                    await ConvertConclusionAsync(conclusionXPO);
                    return;
                }

                if (!CheckConclusionVersion(version))
                {
                    return;
                }

                var conclusion = ConclusionController.GetConclusionByString(xml);
                if (!CheckDeserialized(conclusion))
                {
                    return;
                }

                using (var form = new ConclusionBaseSettingEdit(conclusion.ExpertOrganization, conclusion.Approver))
                {
                    form.ShowDialog();

                    if (form.IsSave)
                    {
                        conclusion.ExpertOrganization = form.Organization;
                        conclusion.Approver = form.Person;
                        var newXml = conclusion.GetSerializeConclusion();
                        if (CheckSerialized(newXml))
                        {
                            conclusionXPO.SetObj(newXml);
                            await Session.SaveAsync(conclusionXPO);
                        }
                    }

                    await UpdateGridDataSource(gridView.FocusedRowHandle);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView.GetFocusedRow() is ExplanatoryNoteXPO note &&
                CanPreviewExplanatoryNote(ExplanatoryNote0107Document.ReadVersion(note.GetObj())))
            {
                ViewExplanatoryNote(note);
                return;
            }

            if (gridView.GetRow(gridView.FocusedRowHandle) is ConclusionXPO conclusionXPO)
            {
                var xml = conclusionXPO.GetObj();
                var version = Conclusion0104Document.ReadVersion(xml);
                if (version == Conclusion0104Document.CurrentVersion)
                {
                    ViewCurrentConclusion(conclusionXPO);
                    return;
                }

                if (Conclusion0104Document.IsHistoricalVersion(version))
                {
                    ViewHistoricalConclusion(conclusionXPO, version);
                    return;
                }

                if (!CheckConclusionVersion(version))
                {
                    return;
                }

                var conclusion = ConclusionController.GetConclusionByString(xml);
                if (!CheckDeserialized(conclusion))
                {
                    return;
                }

                using (var form = new ViewForm(conclusion))
                {
                    form.ShowDialog();
                }
            }
        }
    }
}

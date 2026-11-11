using AutoUpdaterDotNET;
using Core.NET.AutoModels.ConclusionObj;
using Core.NET.AutoModels.ExplanatoryNoteObj;
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ProjectDocumentation.NET.Forms
{
    public partial class MainForm : XtraForm
    {
        private BarButtonItem validateDocument;
        private BarButtonItem editLegacyFields;
        private bool checkingDocument;

        private Session Session => SessionController.GetSession();

        public MainForm()
        {
            InitializeComponent();
            InitializeConclusionCommands();
            InitializeExplanatoryNoteCommands();
            InitializeDocumentToolbar();
        }

        private void InitializeDocumentToolbar()
        {
            var classic = new BarSubItem(barManager, "Классические формы 01.03");
            var addConclusion = new BarButtonItem(barManager, "Добавить заключение 01.03");
            addConclusion.ItemClick += AddClassicConclusion_ItemClick;
            classic.AddItem(addConclusion);
            var addNote = new BarButtonItem(barManager, "Добавить пояснительную записку 01.03");
            addNote.ItemClick += AddClassicExplanatoryNote_ItemClick;
            classic.AddItem(addNote);
            editLegacyFields = new BarButtonItem(barManager, "Все поля выбранного документа 01.03…");
            editLegacyFields.ItemClick += EditLegacyFields_ItemClick;
            classic.AddItem(editLegacyFields).BeginGroup = true;
            popupMenu.AddItem(classic);

            var checks = new BarSubItem(barManager, "Проверка и реестр");
            validateDocument = new BarButtonItem(barManager, "Проверить выбранный XML");
            validateDocument.ItemClick += ValidateSelectedDocument_ItemClick;
            checks.AddItem(validateDocument);
            var externalCheck = new BarButtonItem(barManager, "Открыть проверку Главгосэкспертизы");
            externalCheck.ItemClick += (_, __) => OpenExternalService("https://checkxml.gge.ru/");
            checks.AddItem(externalCheck);
            var registry = new BarButtonItem(barManager, "Открыть поиск в ЕГРЗ");
            registry.ItemClick += (_, __) => OpenExternalService("https://egrz.ru/organisation/reestr-advance/latest");
            checks.AddItem(registry);

            var create = new BarSubItem(barManager, "Создать");
            create.AddItem(barBtnAddConclusion);
            create.AddItem(barBtnAddExplanatoryNote);
            create.AddItem(addConclusion).BeginGroup = true;
            create.AddItem(addNote);

            var versions = new BarSubItem(barManager, "Версии XML");
            versions.AddItem(editLegacyFields);
            versions.AddItem(convertConclusion).BeginGroup = true;
            versions.AddItem(convertExplanatoryNote);
            versions.AddItem(exportLegacyConclusion).BeginGroup = true;
            versions.AddItem(exportCompatibleExplanatoryNote);

            var history = new BarButtonItem(barManager, "История файлов");
            history.ImageOptions.SvgImage = FileOperationHistoryForm.HistoryIcon;
            history.ItemClick += OpenFileOperationHistory_ItemClick;
            var fileCommands = new List<BarItem> { create, barBtnLoad, barBtnFill, history };
            if (ConclusionSchemaService.HasBundledPortal)
            {
                var browser = new BarButtonItem(barManager, "Открыть веб-редактор");
                browser.ItemClick += OpenBrowserPortal_ItemClick;
                fileCommands.Add(browser);
            }
            AddDocumentBar("Документы", 0, fileCommands.ToArray());
            AddDocumentBar("Действия с документом", 1, barBtnView, barBtnEdit, versions, checks);

            gridView.FocusedRowChanged += (_, __) => UpdateDocumentCommands(gridView.GetFocusedRow());
            popupMenu.CloseUp += (_, __) => UpdateDocumentCommands(gridView.GetFocusedRow());
            UpdateDocumentCommands(null);
        }

        private void AddDocumentBar(string name, int row, params BarItem[] commands)
        {
            var toolbar = new Bar { BarName = name };
            toolbar.OptionsBar.AllowQuickCustomization = false;
            toolbar.OptionsBar.DrawDragBorder = false;
            toolbar.OptionsBar.UseWholeRow = true;
            barManager.Bars.Add(toolbar);
            toolbar.DockStyle = BarDockStyle.Top;
            toolbar.DockRow = row;
            foreach (var item in commands)
            {
                item.PaintStyle = BarItemPaintStyle.CaptionGlyph;
                toolbar.AddItem(item);
            }
        }

        private void UpdateDocumentCommands(object row)
        {
            var isDocument = row is XmlObjXPO;
            barBtnView.Enabled = isDocument;
            barBtnBaseSetting.Enabled = row is ConclusionXPO;
            barBtnEdit.Enabled = isDocument;
            barBtnDel.Enabled = isDocument;
            barBtnFill.Enabled = isDocument;
            validateDocument.Enabled = isDocument && !checkingDocument;
            editLegacyFields.Enabled = row is XmlObjXPO record && CanEditLegacyFields(record, record.GetObj());
            UpdateConclusionCommands(row);
            UpdateExplanatoryNoteCommands(row);
        }

        private static bool CanEditLegacyFields(XmlObjXPO record, string xml)
        {
            if (record is ConclusionXPO)
                return Conclusion0104Document.CanUseLegacyEditor(Conclusion0104Document.ReadVersion(xml));

            return record is ExplanatoryNoteXPO && ExplanatoryNoteController.IsLegacyXml(xml);
        }

        private async void EditLegacyFields_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(gridView.GetFocusedRow() is XmlObjXPO record)) return;

            try
            {
                var originalXml = record.GetObj();
                if (!CanEditLegacyFields(record, originalXml)) return;

                var originalName = record.Name;
                var guid = record.Guid ?? throw new InvalidOperationException("У записи отсутствует идентификатор в базе данных.");
                var note = record is ExplanatoryNoteXPO;
                var root = note ? ExplanatoryNote0107Document.RootName : Conclusion0104Document.RootName;
                var document = SchemaXmlDocument.Parse(originalXml, root);
                var version = (string)document.Root.Attribute("SchemaVersion");
                if (string.IsNullOrWhiteSpace(version)) document.Root.SetAttributeValue("SchemaVersion", "01.03");
                var xml = document.ToString(SaveOptions.DisableFormatting);
                using (var editor = new LegacySchemaDocumentForm(conclusionSchema, xml, originalName,
                    note ? ExplanatoryNote0107Document.Family : "conclusion",
                    (value, title) => SchemaXmlDocument.PersistAsync(value, title, guid,
                        create: null, existing: record, previousName: originalName, previousXml: originalXml)))
                {
                    if (editor.ShowDialog(this) == DialogResult.OK && editor.Document != null)
                        await UpdateGridDataSourceAsync(nameof(XmlObjXPO.Guid), guid);
                }
            }
            catch (Exception error)
            {
                if (record is ExplanatoryNoteXPO) ShowExplanatoryNoteError(error);
                else ShowConclusionError(error);
            }
        }

        private async void ValidateSelectedDocument_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (checkingDocument || !(gridView.GetFocusedRow() is XmlObjXPO record)) return;

            var name = record.Name;
            var xml = record.GetObj();
            checkingDocument = true;
            validateDocument.Enabled = false;
            try
            {
                var result = await conclusionSchema.ValidateAsync(xml);
                if (IsDisposed || Disposing) return;
                var basis = result.ValidationBasis == "legacy-wire" ? "локальный формат совместимости" : "комплектная XSD";
                var version = result.Version ?? "не определена";
                var newline = Environment.NewLine;
                var header = $"Документ: {name}{newline}Версия: {version}";
                if (result.ValidationBasis != null) header += $"{newline}Проверка: {basis}";
                var details = result.Valid
                    ? "Ошибок формата не найдено."
                    : string.Join(newline + newline, result.Errors.Select(error => $"{error.Path}{newline}{error.Message}"));
                using (var report = new XtraForm
                {
                    Text = "Результат проверки XML",
                    StartPosition = FormStartPosition.CenterParent,
                    Size = new Size(720, result.Valid ? 220 : 520),
                    MinimumSize = new Size(450, 210)
                })
                {
                    var memo = new MemoEdit { Dock = DockStyle.Fill, Text = header + newline + newline + details };
                    memo.Properties.ReadOnly = true;
                    var close = new SimpleButton { Dock = DockStyle.Bottom, Text = "Закрыть", DialogResult = DialogResult.OK, TabIndex = 0 };
                    memo.TabIndex = 1;
                    report.Controls.Add(memo);
                    report.Controls.Add(close);
                    report.AcceptButton = close;
                    report.CancelButton = close;
                    report.ActiveControl = close;
                    report.ShowDialog(this);
                }
            }
            catch (Exception error)
            {
                if (!IsDisposed && !Disposing)
                    XtraMessageBox.Show(this, error.Message, "Ошибка проверки XML", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                checkingDocument = false;
                if (!IsDisposed && !Disposing) UpdateDocumentCommands(gridView.GetFocusedRow());
            }
        }

        private async void OpenBrowserPortal_ItemClick(object sender, ItemClickEventArgs e)
        {
            e.Item.Enabled = false;
            try
            {
                var address = await conclusionSchema.StartBrowserPortalAsync();
                if (!IsDisposed && !Disposing) OpenExternalService(address);
            }
            catch (Exception error)
            {
                if (!IsDisposed && !Disposing)
                    XtraMessageBox.Show(this, error.Message, "Не удалось открыть веб-редактор", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed && !Disposing) e.Item.Enabled = true;
            }
        }

        private void OpenExternalService(string address)
        {
            try
            {
                Process.Start(new ProcessStartInfo(address) { UseShellExecute = true })?.Dispose();
            }
            catch (Win32Exception error)
            {
                XtraMessageBox.Show(this, error.Message, "Не удалось открыть браузер", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridView_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            var clickedRow = e.HitInfo?.InDataRow == true;
            if (!(sender is GridView view) || (!clickedRow && e.MenuType != GridMenuType.User))
            {
                return;
            }

            if (clickedRow)
            {
                view.FocusedRowHandle = e.HitInfo.RowHandle;
            }

            UpdateDocumentCommands(clickedRow ? view.GetRow(e.HitInfo.RowHandle) : null);
            e.Allow = false;
            popupMenu.ShowPopup(Cursor.Position);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private async void barBtnLoad_ItemClick(object sender, ItemClickEventArgs e)
        {
            await ImportXmlAsync();
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
        private static bool ConfirmSchemaRemarks(string xml, FileOperationEntry attempt)
        {
            var conclusion = Utf8StringSerialize.DeSerializeObjectByString<Conclusion>(xml, lenient: false);
            if (conclusion is null)
            {
                attempt.Status = FileOperationStatus.Rejected;
                attempt.Summary = "Не удалось прочитать исходный XML заключения.";
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

            foreach (var remark in remarks) FileOperationJournal.AddDiagnostic(attempt, remark);
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
            UpdateDocumentCommands(gridView.GetFocusedRow());
        }

        private async Task UpdateGridDataSource(int focusedRowHandle)
        {
            gridControl.DataSource = await new XPQuery<XmlObjXPO>(Session)
                .OrderByDescending(o => o.DateCreate)
                .ToListAsync();
            gridView.FocusedRowHandle = focusedRowHandle;
            UpdateDocumentCommands(gridView.GetFocusedRow());
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
            if (!(gridView.GetFocusedRow() is XmlObjXPO record)) return;
            var xml = record.GetObj();

            try
            {
                if (record is ConclusionXPO)
                {
                    var version = Conclusion0104Document.ReadVersion(xml);
                    await TrackExportAsync(record, "conclusion", version, async attempt =>
                    {
                        if (Conclusion0104Document.IsHistoricalVersion(version))
                        {
                            await ExportHistoricalConclusionAsync(record, xml, attempt);
                        }
                        else if (version == Conclusion0104Document.LegacyVersion)
                        {
                            var normalized = await conclusionSchema.NormalizeLegacyConclusionAsync(xml);
                            await ExportSchemaDocumentAsync(normalized, attempt, Conclusion0104Document.RootName, allowClassicDraft: true);
                        }
                        else if (version == Conclusion0104Document.CurrentVersion)
                        {
                            await ExportSchemaDocumentAsync(xml, attempt, Conclusion0104Document.RootName);
                        }
                        else if (CheckConclusionVersion(version))
                        {
                            if (ConfirmSchemaRemarks(xml, attempt)) await ExportClassicXmlAsync(xml, attempt);
                        }
                        else
                        {
                            attempt.Status = FileOperationStatus.Rejected;
                            attempt.Summary = $"Версия заключения {version} не поддерживается.";
                        }
                    });
                }
                else if (record is ExplanatoryNoteXPO)
                {
                    var version = ExplanatoryNote0107Document.ReadVersion(xml);
                    var legacy = ExplanatoryNoteController.IsLegacyXml(xml);
                    await TrackExportAsync(record, ExplanatoryNote0107Document.Family,
                        legacy ? ExplanatoryNote0107Document.LegacyVersion : version, async attempt =>
                    {
                        if (CanPreviewExplanatoryNote(version))
                        {
                            await ExportSchemaDocumentAsync(xml, attempt, ExplanatoryNote0107Document.RootName);
                        }
                        else if (legacy)
                        {
                            attempt.FileName = $"explanatoryNote_{record.Guid:D}.xml";
                            await ExportClassicXmlAsync(xml, attempt);
                        }
                        else
                        {
                            attempt.Status = FileOperationStatus.Rejected;
                            throw new InvalidOperationException("Не удалось определить поддерживаемую версию пояснительной записки.");
                        }
                    });
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

        private async Task ExportClassicXmlAsync(string xml, FileOperationEntry attempt)
        {
            await FileOperationJournal.ObserveSchemaAsync(attempt, conclusionSchema, xml);
            using (var picker = new FolderBrowserDialog())
            {
                if (picker.ShowDialog(this) != DialogResult.OK) return;
                attempt.FilePath = Path.Combine(picker.SelectedPath, attempt.FileName);
            }

            const string oldNamespace = "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"";
            var isConclusion = attempt.Family == "conclusion";
            var result = xml.Replace(oldNamespace, isConclusion ? "xsi:noNamespaceSchemaLocation=\"conclusion.xsd\""
                : "xsi:noNamespaceSchemaLocation=\"explanatorynote-01-03.xsd\"");
            if (isConclusion)
                result = result.InsertIntoXmlProlog("<?xml-stylesheet type=\"text/xsl\" href=\"conclusion.xsl\" ?>");
            File.WriteAllText(attempt.FilePath, result);
            CompleteExport(attempt, "XML выгружен.");
            XtraMessageBox.Show(this, $"Файл сохранён.{Environment.NewLine}{attempt.FilePath}", "Выгрузка XML",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private async void AddClassicConclusion_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (var form = new ConclusionEdit())
                {
                    var guid = Guid.NewGuid();
                    form.SetFileName("Заключение 01.03");
                    form.SaveConclusionAsync = (conclusion, name) => SaveClassicConclusionAsync(conclusion, name, guid);
                    form.ShowDialog(this);
                    if (form.IsSave) await UpdateGridDataSourceAsync(nameof(XmlObjXPO.Guid), guid);
                }
            }
            catch (Exception error)
            {
                ShowConclusionError(error);
            }
        }

        private async void AddClassicExplanatoryNote_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (var form = new ExplanatoryNoteEdit())
                {
                    var guid = Guid.NewGuid();
                    form.SetFileName("Пояснительная записка 01.03");
                    form.SaveNoteAsync = (note, name) => SaveClassicNoteAsync(note, name, guid);
                    form.ShowDialog(this);
                    if (form.IsSave) await UpdateGridDataSourceAsync(nameof(XmlObjXPO.Guid), guid);
                }
            }
            catch (Exception error)
            {
                ShowExplanatoryNoteError(error);
            }
        }

        private async Task<bool> SaveClassicConclusionAsync(Conclusion conclusion, string name, Guid guid,
            ConclusionXPO existing = null, string previousName = null, string previousXml = null)
        {
            if (existing == null || string.IsNullOrWhiteSpace(conclusion.ConclusionGuid))
            {
                conclusion.ConclusionGuid = guid.ToString("D");
            }

            var xml = conclusion.GetSerializeConclusion();
            if (!CheckSerialized(xml)) return false;

            xml = await conclusionSchema.NormalizeLegacyConclusionAsync(xml);
            if (!await ConfirmClassicSaveAsync(xml, previousXml, Conclusion0104Document.LegacyVersion,
                ClassicConclusionFormat)) return false;

            await SchemaXmlDocument.PersistAsync(xml, name, guid, session => new ConclusionXPO(session),
                existing, previousName, previousXml);
            return true;
        }

        private async Task<bool> SaveClassicNoteAsync(ExplanatoryNote note, string name, Guid guid,
            ExplanatoryNoteXPO existing = null, string previousName = null, string previousXml = null)
        {
            var xml = note.GetSerializeExplanatoryNote();
            if (!CheckSerialized(xml)) return false;
            if (!await ConfirmClassicSaveAsync(xml, previousXml, ExplanatoryNote0107Document.LegacyVersion,
                "локальному формату записки 01.03")) return false;
            await SchemaXmlDocument.PersistAsync(xml, name, guid, session => new ExplanatoryNoteXPO(session),
                existing, previousName, previousXml);
            return true;
        }

        private async Task<bool> ConfirmClassicSaveAsync(string xml, string previousXml, string version, string format)
        {
            var validation = await conclusionSchema.ValidateAsync(xml);
            if (validation.Version != version)
                throw new InvalidOperationException(ClassicValidationMessage(validation, format));
            if (validation.Valid) return true;
            if (previousXml != null && (await conclusionSchema.ValidateAsync(previousXml)).Valid)
                throw new InvalidOperationException(ClassicValidationMessage(validation, format));

            return ConfirmClassicDraft(validation, format, "Сохранить изменения как черновик?");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private async void gridView_DoubleClick(object sender, EventArgs e)
        {
            if (sender is GridView view)
            {
                var hit = view.CalcHitInfo(gridControl.PointToClient(Cursor.Position));
                if (hit.InDataRow)
                {
                    view.FocusedRowHandle = hit.RowHandle;
                    await EditGridViewObjectAsync();
                }
            }
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

                var previousName = conclusionXPO.Name;
                using (var form = new ConclusionEdit(conclusion))
                {
                    form.SetFileName(previousName);
                    form.SaveConclusionAsync = (edited, name) => SaveClassicConclusionAsync(edited, name,
                        conclusionXPO.Guid ?? throw new InvalidOperationException("У заключения отсутствует идентификатор в базе."),
                        conclusionXPO, previousName, xml);
                    form.ShowDialog(this);
                    if (form.IsSave) await UpdateGridDataSourceAsync(nameof(XmlObjXPO.Guid), conclusionXPO.Guid);
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

                var previousName = explanatoryNoteXPO.Name;
                using (var form = new ExplanatoryNoteEdit(explanatoryNote))
                {
                    form.SetFileName(previousName);
                    form.SaveNoteAsync = (edited, name) => SaveClassicNoteAsync(edited, name,
                        explanatoryNoteXPO.Guid ?? throw new InvalidOperationException("У записки отсутствует идентификатор в базе."),
                        explanatoryNoteXPO, previousName, xml);
                    form.ShowDialog(this);
                    if (form.IsSave) await UpdateGridDataSourceAsync(nameof(XmlObjXPO.Guid), explanatoryNoteXPO.Guid);
                }
            }
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

                try
                {
                    var previousName = conclusionXPO.Name;
                    using (var form = new ConclusionBaseSettingEdit(conclusion.ExpertOrganization, conclusion.Approver))
                    {
                        form.ShowDialog(this);
                        if (!form.IsSave) return;

                        conclusion.ExpertOrganization = form.Organization;
                        conclusion.Approver = form.Person;
                        var guid = conclusionXPO.Guid ?? throw new InvalidOperationException("У заключения отсутствует идентификатор в базе.");
                        if (await SaveClassicConclusionAsync(conclusion, previousName, guid, conclusionXPO, previousName, xml))
                        {
                            await UpdateGridDataSourceAsync(nameof(XmlObjXPO.Guid), guid);
                        }
                    }
                }
                catch (Exception error)
                {
                    ShowConclusionError(error);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView.GetFocusedRow() is ExplanatoryNoteXPO note)
            {
                var xml = note.GetObj();
                if (CanPreviewExplanatoryNote(ExplanatoryNote0107Document.ReadVersion(xml)))
                    ViewExplanatoryNote(note);
                else if (ExplanatoryNoteController.IsLegacyXml(xml))
                    ViewSourceXml(note, "Исходная пояснительная записка 01.03");
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

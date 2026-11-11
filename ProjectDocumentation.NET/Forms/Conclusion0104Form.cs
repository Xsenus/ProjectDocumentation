using Core.NET.Controllers;
using Core.NET.Models.XPO;
using DevExpress.Images;
using DevExpress.Office.Services;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraRichEdit;
using DevExpress.XtraTab;
using ProjectDocumentation.NET.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ProjectDocumentation.NET.Forms
{
    internal partial class SchemaDocumentForm : XtraForm
    {
        private static readonly Regex DraftEncoding = new Regex(
            @"\A\uFEFF?<\?xml(?=\s)[^<?]*?\sencoding\s*=\s*(?<quote>['""])(?<encoding>[A-Za-z][A-Za-z0-9._-]*)\k<quote>",
            RegexOptions.CultureInvariant);
        private readonly ConclusionSchemaService service;
        private readonly Func<string, string, Task<XmlObjXPO>> save;
        private readonly string family;
        private readonly string initialXml;
        private readonly string initialSection;
        private readonly string rootName;
        private readonly string idAttribute;
        private readonly bool legacyEditor;
        private readonly TextEdit nameEditor = new TextEdit { Dock = DockStyle.Fill, AccessibleName = "Название документа" };
        private readonly XtraTabControl tabs = new XtraTabControl { Dock = DockStyle.Fill, AccessibleName = "Вкладки редактора" };
        private readonly XtraTabPage sectionsPage = new XtraTabPage { Text = "Разделы" };
        private readonly XtraTabPage fieldsPage = new XtraTabPage { Text = "Все поля" };
        private readonly XtraTabPage xmlPage = new XtraTabPage { Text = "Исходный XML" };
        private readonly XtraTabPage validationPage = new XtraTabPage { Text = "Проверка XML" };
        private readonly XtraTabPage previewPage = new XtraTabPage { Text = "Предпросмотр" };
        private readonly MemoEdit rawEditor = new MemoEdit { Dock = DockStyle.Fill, AccessibleName = "Исходный XML" };
        private readonly GridControl errors = new GridControl { Dock = DockStyle.Fill, AccessibleName = "Результаты проверки XML" };
        private readonly GridView errorView;
        private readonly RichEditControl preview = new RichEditControl { Dock = DockStyle.Fill, ReadOnly = true };
        private readonly LabelControl status = WrappedLabelFor("");
        private readonly LabelControl validationHint = LabelFor("Нажмите «Проверить XML», чтобы проверить документ.");
        private readonly SimpleButton findFieldButton = ActionButton("Найти поле…", "find/find");
        private readonly SimpleButton goToErrorButton = ActionButton("К полю");
        private readonly SimpleButton checkButton = ActionButton("Проверить XML");
        private readonly SimpleButton saveButton = ActionButton("Сохранить");
        private readonly SimpleButton draftButton = ActionButton("Сохранить черновик XML…");
        private readonly SimpleButton applyRawButton = ActionButton("Применить XML");
        private readonly SimpleButton resetRawButton = ActionButton("Вернуть текущий XML");
        private readonly SimpleButton previewButton = ActionButton("Обновить предпросмотр", "xaf/action_refresh");
        private readonly Font xmlFont = new Font(FontFamily.GenericMonospace, 9);
        private readonly ToolTipController fieldToolTips = new ToolTipController();
        private XDocument document;
        private NativeSchemaProfile profile;
        private string documentVersion;
        private string initialId;
        private bool loaded;
        private bool busy;
        private bool dirty;
        private bool rawDirty;
        private bool updatingRaw;
        private int revision;
        private int rawRevision = -1;
        private int previewRevision = -1;
        private int validationRevision = -1;

        public XmlObjXPO Document { get; private set; }
        internal FileOperationEntry ImportAttempt { private get; set; }
        private bool ReadOnlyDocument => save == null;

        protected SchemaDocumentForm(ConclusionSchemaService service, string xml, string name,
            Func<string, string, Task<XmlObjXPO>> save, string family, string title, string section, IReadOnlyList<string> warnings,
            bool legacyEditor = false)
        {
            this.service = service;
            this.family = family ?? "conclusion";
            this.save = save;
            initialXml = xml;
            initialSection = section;
            this.legacyEditor = legacyEditor;
            var note = this.family == ExplanatoryNote0107Document.Family;
            rootName = note ? "ExplanatoryNote" : "Conclusion";
            idAttribute = note ? "id" : "ConclusionGUID";
            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(800, 560);
            ClientSize = new Size(1180, 780);
            AutoScaleMode = AutoScaleMode.Font;
            tabs.AppearancePage.HeaderActive.FontStyleDelta = FontStyle.Bold;
            saveButton.Appearance.FontStyleDelta = FontStyle.Bold;
            findFieldButton.ToolTip = "Найти поле (Ctrl+F)";
            nameEditor.Text = name ?? "";
            nameEditor.Properties.ReadOnly = ReadOnlyDocument;
            nameEditor.TextChanged += (_, __) => { if (loaded) MarkChanged(false); };
            rawEditor.Properties.Appearance.Font = xmlFont;
            rawEditor.Properties.AppearanceReadOnly.Font = xmlFont;
            rawEditor.Properties.ReadOnly = ReadOnlyDocument;
            rawEditor.Properties.AcceptsReturn = true;
            rawEditor.Properties.AcceptsTab = true;
            rawEditor.Properties.ScrollBars = ScrollBars.Both;
            rawEditor.Properties.WordWrap = false;
            rawEditor.Properties.MaxLength = SchemaXmlDocument.MaxXmlBytes;
            rawEditor.TextChanged += (_, __) =>
            {
                if (updatingRaw || ReadOnlyDocument) return;
                rawDirty = true;
                status.Text = "Есть неприменённые изменения XML.";
            };

            fieldsPage.Controls.Add(CreateFieldsPanel());
            sectionsPage.Controls.Add(CreateSectionsPanel());
            fieldToolTips.GetActiveObjectInfo += FieldToolTip;
            xmlPage.Controls.Add(WithToolbar(rawEditor, applyRawButton, resetRawButton));
            errorView = ConfigureListGrid(errors);
            errorView.Columns.AddVisible(nameof(ConclusionValidationError.Path), "Путь XML").Width = 330;
            errorView.Columns.AddVisible(nameof(ConclusionValidationError.Message), "Сообщение").Width = 720;
            WrapListColumns(errorView);
            OnGridActivate(errorView, GoToSelectedError);
            errorView.FocusedRowChanged += (_, __) => UpdateErrorNavigation();
            goToErrorButton.Click += (_, __) => GoToSelectedError();
            goToErrorButton.Enabled = false;
            validationPage.Controls.Add(WithToolbar(errors, goToErrorButton, validationHint));
            preview.ReplaceService<IUriStreamService>(new PreviewImages());
            preview.HyperlinkClick += (_, e) => e.Handled = true;
            previewPage.Controls.Add(WithToolbar(preview, previewButton));
            tabs.TabPages.AddRange(new[] { sectionsPage, fieldsPage, xmlPage, validationPage });
            if (!legacyEditor || !note) tabs.TabPages.Add(previewPage);
            else previewPage.Dispose();
            if (legacyEditor) tabs.SelectedTabPage = fieldsPage;
            if (warnings?.Count > 0)
            {
                var warningsPage = new XtraTabPage { Text = "Замечания преобразования" };
                var warningsText = new MemoEdit
                {
                    Dock = DockStyle.Fill, AccessibleName = "Замечания преобразования",
                    Text = string.Join(Environment.NewLine + Environment.NewLine, warnings)
                };
                warningsText.Properties.ReadOnly = true;
                warningsText.Properties.ScrollBars = ScrollBars.Vertical;
                warningsPage.Controls.Add(WithToolbar(warningsText));
                tabs.TabPages.Add(warningsPage);
            }

            tabs.SelectedPageChanged += async (_, __) =>
            {
                if (!loaded) return;
                if (tabs.SelectedTabPage == sectionsPage || tabs.SelectedTabPage == fieldsPage) ShowFieldsView();
                if (tabs.SelectedTabPage == xmlPage && !rawDirty && rawRevision != revision) ResetRawEditor();
                if (tabs.SelectedTabPage == previewPage && !busy && !rawDirty && previewRevision != revision) await UpdatePreviewAsync();
            };
            tabs.SelectedPageChanging += (_, e) =>
            {
                if (rawDirty && (e.Page == sectionsPage || e.Page == fieldsPage || e.Page == previewPage))
                {
                    e.Cancel = true;
                    status.Text = "Примените или отмените изменения исходного XML.";
                }
            };
            checkButton.Click += async (_, __) => await ValidateOrSaveAsync(false);
            findFieldButton.Click += (_, __) => FindField();
            saveButton.Click += async (_, __) => await ValidateOrSaveAsync(true);
            draftButton.Click += async (_, __) => await SaveDraftAsync();
            applyRawButton.Click += (_, __) => ApplyRaw();
            resetRawButton.Click += (_, __) => ResetRawEditor();
            previewButton.Click += async (_, __) => await UpdatePreviewAsync();
            saveButton.Visible = !ReadOnlyDocument;
            applyRawButton.Visible = !ReadOnlyDocument;
            resetRawButton.Visible = !ReadOnlyDocument;
            draftButton.Text = ReadOnlyDocument ? "Выгрузить исходный XML…" : draftButton.Text;

            var commands = Toolbar(findFieldButton, checkButton, saveButton, draftButton);
            var header = new TableLayoutPanel { AutoSize = true, Dock = DockStyle.Top, ColumnCount = 2, RowCount = 2, Margin = new Padding(0, 0, 0, 8) };
            header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            header.Controls.Add(new LabelControl { Text = "Название документа", Anchor = AnchorStyles.Left, Margin = new Padding(0, 0, 10, 0) }, 0, 0);
            header.Controls.Add(nameEditor, 1, 0);
            header.Controls.Add(commands, 0, 1);
            header.SetColumnSpan(commands, 2);
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(14, 12, 14, 4) };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.Controls.Add(header, 0, 0);
            layout.Controls.Add(tabs, 0, 1);
            layout.Controls.Add(status, 0, 2);
            Controls.Add(layout);
            SetBusy(true);
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);
            status.Text = "Загрузка полей схемы…";
            try
            {
                document = SchemaXmlDocument.Parse(initialXml, rootName);
                documentVersion = DocumentVersionOf(document);
                if (legacyEditor && documentVersion != "01.03")
                    throw new InvalidOperationException("Этот редактор открывает документы версии 01.03.");
                initialId = (string)document.Root.Attribute(idAttribute);
                var schema = await service.GetSchemaAsync(family, documentVersion);
                if (IsDisposed || Disposing) return;
                profile = schema.ToObject<NativeSchemaProfile>();
                if (profile?.Root?.Name != rootName || profile.Types == null)
                {
                    throw new InvalidOperationException("Получено описание полей другого документа.");
                }
                loaded = true;
                ShowFieldsView();
                BuildTree(initialSection);
                status.Text = $"Схема {documentVersion}.";
            }
            catch (Exception error)
            {
                FileOperationJournal.Fail(ImportAttempt, error);
                status.Text = error.Message;
            }
            finally
            {
                if (!IsDisposed && !Disposing) SetBusy(false);
            }

            if (loaded && ReadOnlyDocument && !IsDisposed && !Disposing && tabs.TabPages.Contains(previewPage))
                tabs.SelectedTabPage = previewPage;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && DialogResult != DialogResult.OK)
            {
                e.Cancel = busy || ((dirty || rawDirty) && XtraMessageBox.Show(this, "Закрыть редактор и отбросить несохранённые изменения?",
                    Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes);
            }
            base.OnFormClosing(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                treeMenu?.Dispose();
                fieldToolTips.Dispose();
            }
            base.Dispose(disposing);
            if (disposing) xmlFont.Dispose();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (xmlTree.ContainsFocus && (keyData == Keys.Apps || keyData == (Keys.Shift | Keys.F10)) &&
                ShowFocusedTreeMenu())
                return true;
            if (keyData != (Keys.Control | Keys.F) || !loaded || busy) return base.ProcessCmdKey(ref msg, keyData);
            FindField();
            return true;
        }

        private static SimpleButton ActionButton(string text, string icon = null)
        {
            var button = new SimpleButton
            {
                AutoSize = true, Text = text, Padding = new Padding(6, 0, 6, 0), MinimumSize = new Size(0, 30)
            };
            if (icon != null)
            {
                button.ImageOptions.SvgImage = ImageResourceCache.Default.GetSvgImage($"svgimages/{icon}.svg");
                button.ImageOptions.SvgImageSize = new Size(16, 16);
            }
            return button;
        }

        private static GridView ConfigureListGrid(GridControl grid)
        {
            var view = new GridView(grid);
            grid.ViewCollection.Add(view);
            grid.MainView = view;
            view.OptionsBehavior.Editable = false;
            view.OptionsCustomization.AllowGroup = false;
            view.OptionsView.ShowGroupPanel = false;
            view.OptionsView.ShowIndicator = false;
            view.OptionsView.ShowVerticalLines = DefaultBoolean.False;
            view.OptionsSelection.EnableAppearanceFocusedCell = false;
            view.Appearance.HeaderPanel.FontStyleDelta = FontStyle.Bold;
            view.RowHeight = 29;
            return view;
        }

        private static void OnGridActivate(GridView view, Action activate)
        {
            view.DoubleClick += (_, __) =>
            {
                if (view.CalcHitInfo(view.GridControl.PointToClient(MousePosition)).InRowCell) activate();
            };
            view.KeyDown += (_, e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                e.SuppressKeyPress = true;
                activate();
            };
        }

        private static void WrapListColumns(GridView view)
        {
            var memo = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit { WordWrap = true };
            view.GridControl.RepositoryItems.Add(memo);
            foreach (DevExpress.XtraGrid.Columns.GridColumn column in view.Columns)
                column.ColumnEdit = memo;
            view.OptionsView.RowAutoHeight = true;
        }

        private static FlowLayoutPanel Toolbar(params Control[] controls)
        {
            var panel = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Top, WrapContents = false, Padding = new Padding(0, 2, 0, 2) };
            panel.Controls.AddRange(controls);
            return panel;
        }

        private static Control WithToolbar(Control content, params Control[] controls)
        {
            var panel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Padding = new Padding(8) };
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            if (controls.Length > 0)
                panel.Controls.Add(Toolbar(controls), 0, 0);
            panel.Controls.Add(content, 0, 1);
            return panel;
        }

        private void SetBusy(bool value)
        {
            busy = value;
            tabs.Enabled = loaded && !value;
            nameEditor.Enabled = loaded && !value;
            findFieldButton.Enabled = loaded && !value;
            checkButton.Enabled = loaded && !value;
            saveButton.Enabled = loaded && !value && !ReadOnlyDocument;
            draftButton.Enabled = !value && (loaded || !string.IsNullOrWhiteSpace(initialXml));
            UseWaitCursor = value;
        }

        private string Snapshot()
        {
            var xml = document.ToString(SaveOptions.DisableFormatting);
            return document.Declaration == null ? xml : new XDeclaration("1.0", "utf-8", document.Declaration.Standalone) + Environment.NewLine + xml;
        }

        private string DocumentVersionOf(XDocument xml)
        {
            var version = (string)xml.Root.Attribute("SchemaVersion");
            return legacyEditor && family == ExplanatoryNote0107Document.Family &&
                string.IsNullOrWhiteSpace(version) ? "01.03" : version;
        }

        private void MarkChanged(bool xmlChanged = true)
        {
            dirty = true;
            if (xmlChanged)
            {
                revision++;
                if (validationRevision >= 0)
                    validationHint.Text = "XML изменён. Повторите проверку, чтобы обновить список.";
            }
            status.Text = rawDirty ? "Есть неприменённые изменения XML." : "Есть несохранённые изменения.";
        }

        private void ResetRawEditor()
        {
            if (document == null) return;
            updatingRaw = true;
            try
            {
                rawEditor.Text = Snapshot();
                rawRevision = revision;
                rawDirty = false;
                status.Text = dirty ? "Есть несохранённые изменения." : $"Схема {documentVersion}.";
            }
            finally
            {
                updatingRaw = false;
            }
        }

        private void ApplyRaw()
        {
            if (!rawDirty || ReadOnlyDocument) return;
            try
            {
                var next = SchemaXmlDocument.Parse(rawEditor.Text, rootName);
                if (DocumentVersionOf(next) != documentVersion)
                    throw new InvalidOperationException("Для смены версии используйте преобразование документа.");
                if ((string)next.Root.Attribute(idAttribute) != initialId)
                    throw new InvalidOperationException($"Восстановите исходный идентификатор {idAttribute}.");
                document = next;
                rawDirty = false;
                MarkChanged();
                rawRevision = revision;
                BuildTree();
                status.Text = "Изменения XML применены.";
            }
            catch (Exception error)
            {
                FileOperationJournal.AddDiagnostic(ImportAttempt, $"Редактор. {error.Message}",
                    error is System.Xml.XmlException ? "/" : null);
                status.Text = error.Message;
            }
        }

        private bool CanUseDocument()
        {
            if (!loaded || busy) return false;
            if (!rawDirty) return true;
            tabs.SelectedTabPage = xmlPage;
            status.Text = "Примените изменения исходного XML.";
            return false;
        }

        private async Task ValidateOrSaveAsync(bool commit)
        {
            if (!CanUseDocument() || (commit && ReadOnlyDocument)) return;
            if (commit && string.IsNullOrWhiteSpace(nameEditor.Text))
            {
                status.Text = "Укажите название документа перед сохранением.";
                nameEditor.Focus();
                return;
            }
            await RunAsync(commit ? "Проверка перед сохранением…" : "Проверка XML…", async () =>
            {
                var xml = Snapshot();
                var result = await service.ValidateAsync(xml);
                ShowValidation(result);
                if (ImportAttempt != null && !result.Valid)
                {
                    FileOperationJournal.AddValidation(ImportAttempt, result, "Редактор.");
                    if (commit)
                    {
                        ImportAttempt.Status = FileOperationStatus.Rejected;
                        ImportAttempt.Summary = "Документ не прошёл проверку перед сохранением.";
                    }
                }
                if (!result.Valid || !commit) return;
                Document = await save(xml, nameEditor.Text);
                dirty = false;
                rawDirty = false;
                DialogResult = DialogResult.OK;
                Close();
            }, captureImportFailure: commit);
            if (!IsDisposed && !Disposing && tabs.SelectedTabPage == validationPage && errorView.DataRowCount > 0)
                errors.Focus();
        }

        private void ShowValidation(ConclusionValidation result)
        {
            validationRevision = revision;
            List<ValidationRow> rows;
            if (result.Valid)
            {
                status.Text = result.ValidationBasis == "legacy-wire"
                    ? $"XML соответствует локальному формату совместимости {result.Version}."
                    : $"XML соответствует схеме {result.Version}.";
                rows = new List<ValidationRow> { new ValidationRow(null, status.Text) };
                validationHint.Text = status.Text;
            }
            else
            {
                rows = (result.Errors ?? new List<ConclusionValidationError>())
                    .Select(error => new ValidationRow(error.Path, error.Message)).ToList();
                status.Text = $"XML требует исправлений. Ошибок: {rows.Count}.";
                validationHint.Text = status.Text + " Выберите строку и нажмите «К полю».";
            }
            errors.DataSource = rows;
            tabs.SelectedTabPage = validationPage;
            if (rows.Count > 0) errorView.FocusedRowHandle = 0;
            UpdateErrorNavigation();
        }

        private void UpdateErrorNavigation()
            => goToErrorButton.Enabled = (errorView.GetFocusedRow() as ValidationRow)?.Target != null;

        private void GoToSelectedError()
        {
            if ((errorView.GetFocusedRow() as ValidationRow)?.Target is string path)
                NavigateToField(path);
        }

        private sealed class ValidationRow
        {
            internal ValidationRow(string target, string message)
            {
                Target = target;
                Message = message;
            }
            internal string Target { get; }
            public string Path => Target ?? "/";
            public string Message { get; }
        }

        private async Task UpdatePreviewAsync()
        {
            if (!CanUseDocument()) return;
            await RunAsync("Формирование предпросмотра…", async () =>
            {
                var html = await service.PreviewAsync(Snapshot());
                if (!preview.LoadDocument(Encoding.UTF8.GetBytes(html), DocumentFormat.Html))
                    throw new InvalidOperationException("Не удалось отобразить результат преобразования по XSL.");
                previewRevision = revision;
                status.Text = $"Предпросмотр по XSL {documentVersion}.";
            });
        }

        private async Task RunAsync(string message, Func<Task> action, bool captureImportFailure = false)
        {
            SetBusy(true);
            status.Text = message;
            try
            {
                await action();
            }
            catch (Exception error)
            {
                if (captureImportFailure) FileOperationJournal.Fail(ImportAttempt, error);
                if (!IsDisposed && !Disposing) status.Text = error.Message;
            }
            finally
            {
                if (!IsDisposed && !Disposing) SetBusy(false);
            }
        }

        private async Task SaveDraftAsync()
        {
            if (busy) return;
            var text = rawDirty ? rawEditor.Text : document != null ? Snapshot() : initialXml;
            using (var picker = new SaveFileDialog { Filter = "Документ XML (*.xml)|*.xml", DefaultExt = "xml", AddExtension = true })
            {
                if (picker.ShowDialog(this) != DialogResult.OK) return;
                var path = picker.FileName;
                var entry = new FileOperationEntry
                {
                    Kind = FileOperationKind.Export,
                    FilePath = path,
                    FileName = Path.GetFileName(path),
                    DocumentName = nameEditor.Text,
                    Family = family,
                    Version = documentVersion,
                    DocumentId = Guid.TryParse(initialId, out var guid) ? (Guid?)guid : null
                };
                await RunAsync("Сохранение файла XML…", async () =>
                {
                    var temporary = Path.Combine(Path.GetDirectoryName(path), Guid.NewGuid().ToString("N") + ".tmp");
                    try
                    {
                        var xml = NormalizeDraftEncoding(text);
                        File.WriteAllText(temporary, xml, new UTF8Encoding(false));
                        if (File.Exists(path)) File.Replace(temporary, path, null);
                        else File.Move(temporary, path);
                        await FileOperationJournal.ObserveSchemaAsync(entry, service, xml);
                        entry.Status = ReadOnlyDocument && entry.Diagnostics.Count == 0
                            ? FileOperationStatus.Completed : FileOperationStatus.Warning;
                        entry.Summary = ReadOnlyDocument ? "Выгружен исходный XML." : "Сохранён XML-черновик.";
                        status.Text = "Исходный XML записан в выбранный файл.";
                    }
                    catch (Exception error)
                    {
                        FileOperationJournal.Fail(entry, error);
                        status.Text = error.Message;
                    }
                    finally
                    {
                        try
                        {
                            if (File.Exists(temporary)) File.Delete(temporary);
                        }
                        catch (Exception error)
                        {
                            FileOperationJournal.AddDiagnostic(entry, $"Не удалось удалить временный файл. {error.Message}");
                        }
                        await FileOperationJournal.SaveAsync(this, entry);
                    }
                });
            }
        }

        private static string NormalizeDraftEncoding(string xml)
        {
            // An unapplied draft can be malformed. Rewrite just the encoding of a complete leading declaration.
            if (!xml.StartsWith("<?xml", StringComparison.Ordinal) && !xml.StartsWith("\uFEFF<?xml", StringComparison.Ordinal)) return xml;
            var startOfDeclaration = xml[0] == '\uFEFF' ? 1 : 0;
            var endOfDeclaration = xml.IndexOf('?', startOfDeclaration + 5);
            if (endOfDeclaration < 0 || endOfDeclaration + 1 == xml.Length || xml[endOfDeclaration + 1] != '>' ||
                xml.IndexOf('<', startOfDeclaration + 1, endOfDeclaration - startOfDeclaration - 1) >= 0) return xml;
            var encoding = DraftEncoding.Match(xml, 0, endOfDeclaration).Groups["encoding"];
            if (!encoding.Success || string.Equals(encoding.Value, "utf-8", StringComparison.OrdinalIgnoreCase)) return xml;
            return xml.Substring(0, encoding.Index) + "utf-8" + xml.Substring(encoding.Index + encoding.Length);
        }

        private sealed class PreviewImages : IUriStreamService
        {
            public Stream GetStream(string url)
            {
                var comma = url?.IndexOf(',') ?? -1;
                if (comma < 0) return null;
                var format = url.Substring(0, comma).ToLowerInvariant();
                if (format != "data:image/png;base64" && format != "data:image/jpeg;base64" &&
                    format != "data:image/jpg;base64" && format != "data:image/gif;base64") return null;
                try { return new MemoryStream(Convert.FromBase64String(url.Substring(comma + 1)), writable: false); }
                catch (FormatException) { return null; }
            }

            public void RegisterProvider(IUriStreamProvider provider) { }
            public void UnregisterProvider(IUriStreamProvider provider) { }
        }
    }

    internal sealed class Conclusion0104Form : SchemaDocumentForm
    {
        public Conclusion0104Form(ConclusionSchemaService service, string xml, string name,
            Func<string, string, Task<ConclusionXPO>> save = null, string section = null, IReadOnlyList<string> warnings = null)
            : base(service, xml, name, save == null ? (Func<string, string, Task<XmlObjXPO>>)null : async (document, title) => await save(document, title),
                null, save == null ? "Просмотр заключения экспертизы 01.04" : "Заключение экспертизы 01.04", section, warnings)
        {
        }
    }

    internal sealed class ExplanatoryNote0107Form : SchemaDocumentForm
    {
        public ExplanatoryNote0107Form(ConclusionSchemaService service, string xml, string name,
            Func<string, string, Task<ExplanatoryNoteXPO>> save = null, IReadOnlyList<string> warnings = null)
            : base(service, xml, name, save == null ? (Func<string, string, Task<XmlObjXPO>>)null : async (document, title) => await save(document, title),
                ExplanatoryNote0107Document.Family, $"Пояснительная записка {ExplanatoryNote0107Document.ReadVersion(xml)}", null, warnings)
        {
        }
    }

    internal sealed class LegacySchemaDocumentForm : SchemaDocumentForm
    {
        public LegacySchemaDocumentForm(ConclusionSchemaService service, string xml, string name, string family,
            Func<string, string, Task<XmlObjXPO>> save)
            : base(service, xml, name, save, family, family == ExplanatoryNote0107Document.Family
                ? "Пояснительная записка 01.03 — все поля" : "Заключение экспертизы 01.03 — все поля",
                null, null, legacyEditor: true)
        {
        }
    }
}

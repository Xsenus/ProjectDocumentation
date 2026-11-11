using Core.NET.Models.XPO;
using DevExpress.Office.Services;
using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit;
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
        private readonly TextBox nameEditor = new TextBox { Dock = DockStyle.Fill };
        private readonly TabControl tabs = new TabControl { Dock = DockStyle.Fill };
        private readonly TabPage sectionsPage = new TabPage("Разделы");
        private readonly TabPage fieldsPage = new TabPage("Все поля");
        private readonly TabPage xmlPage = new TabPage("Исходный XML");
        private readonly TabPage validationPage = new TabPage("Проверка XML");
        private readonly TabPage previewPage = new TabPage("Предпросмотр");
        private readonly TextBox rawEditor = new TextBox
        {
            AcceptsReturn = true, AcceptsTab = true, Dock = DockStyle.Fill, Multiline = true,
            ScrollBars = ScrollBars.Both, WordWrap = false, MaxLength = SchemaXmlDocument.MaxXmlBytes
        };
        private readonly ListView errors = new ListView { Dock = DockStyle.Fill, View = View.Details, FullRowSelect = true };
        private readonly RichEditControl preview = new RichEditControl { Dock = DockStyle.Fill, ReadOnly = true };
        private readonly Label status = new Label { AutoSize = true, Dock = DockStyle.Fill, Padding = new Padding(0, 8, 0, 0) };
        private readonly Button checkButton = ActionButton("Проверить XML");
        private readonly Button saveButton = ActionButton("Сохранить");
        private readonly Button draftButton = ActionButton("Сохранить черновик XML…");
        private readonly Button applyRawButton = ActionButton("Применить XML");
        private readonly Button resetRawButton = ActionButton("Вернуть текущий XML");
        private readonly Button previewButton = ActionButton("Обновить предпросмотр");
        private readonly Font xmlFont = new Font(FontFamily.GenericMonospace, 9);
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

        public XmlObjXPO Document { get; private set; }
        private bool ReadOnlyDocument => save == null;

        protected SchemaDocumentForm(ConclusionSchemaService service, string xml, string name,
            Func<string, string, Task<XmlObjXPO>> save, string family, string title, string section, IReadOnlyList<string> warnings)
        {
            this.service = service;
            this.family = family ?? "conclusion";
            this.save = save;
            initialXml = xml;
            initialSection = section;
            var note = this.family == ExplanatoryNote0107Document.Family;
            rootName = note ? "ExplanatoryNote" : "Conclusion";
            idAttribute = note ? "id" : "ConclusionGUID";
            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(800, 560);
            ClientSize = new Size(1180, 780);
            AutoScaleMode = AutoScaleMode.Font;
            nameEditor.Text = name ?? "";
            nameEditor.ReadOnly = ReadOnlyDocument;
            nameEditor.TextChanged += (_, __) => { if (loaded) MarkChanged(false); };
            rawEditor.Font = xmlFont;
            rawEditor.ReadOnly = ReadOnlyDocument;
            rawEditor.TextChanged += (_, __) =>
            {
                if (updatingRaw || ReadOnlyDocument) return;
                rawDirty = true;
                status.Text = "Есть неприменённые изменения XML.";
            };

            fieldsPage.Controls.Add(CreateFieldsPanel());
            sectionsPage.Controls.Add(CreateSectionsPanel());
            xmlPage.Controls.Add(WithToolbar(rawEditor, applyRawButton, resetRawButton));
            errors.Columns.Add("Путь XML", 330);
            errors.Columns.Add("Сообщение", 720);
            errors.ItemActivate += (_, __) =>
            {
                if (errors.SelectedItems.Count == 1 && errors.SelectedItems[0].Tag is string path)
                {
                    NavigateToField(path);
                }
            };
            validationPage.Controls.Add(errors);
            preview.ReplaceService<IUriStreamService>(new PreviewImages());
            preview.HyperlinkClick += (_, e) => e.Handled = true;
            previewPage.Controls.Add(WithToolbar(preview, previewButton));
            tabs.TabPages.AddRange(new[] { sectionsPage, fieldsPage, xmlPage, validationPage, previewPage });
            if (warnings?.Count > 0)
            {
                var warningsPage = new TabPage("Замечания преобразования");
                warningsPage.Controls.Add(new TextBox
                {
                    Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical, ReadOnly = true,
                    Text = string.Join(Environment.NewLine + Environment.NewLine, warnings)
                });
                tabs.TabPages.Add(warningsPage);
            }

            tabs.SelectedIndexChanged += async (_, __) =>
            {
                if (!loaded) return;
                if (tabs.SelectedTab == sectionsPage || tabs.SelectedTab == fieldsPage) ShowFieldsView();
                if (tabs.SelectedTab == xmlPage && !rawDirty && rawRevision != revision) ResetRawEditor();
                if (tabs.SelectedTab == previewPage && !busy && !rawDirty && previewRevision != revision) await UpdatePreviewAsync();
            };
            tabs.Selecting += (_, e) =>
            {
                if (rawDirty && (e.TabPage == sectionsPage || e.TabPage == fieldsPage || e.TabPage == previewPage))
                {
                    e.Cancel = true;
                    status.Text = "Примените или отмените изменения исходного XML.";
                }
            };
            checkButton.Click += async (_, __) => await ValidateOrSaveAsync(false);
            saveButton.Click += async (_, __) => await ValidateOrSaveAsync(true);
            draftButton.Click += (_, __) => SaveDraft();
            applyRawButton.Click += (_, __) => ApplyRaw();
            resetRawButton.Click += (_, __) => ResetRawEditor();
            previewButton.Click += async (_, __) => await UpdatePreviewAsync();
            saveButton.Visible = !ReadOnlyDocument;
            applyRawButton.Visible = !ReadOnlyDocument;
            resetRawButton.Visible = !ReadOnlyDocument;
            draftButton.Text = ReadOnlyDocument ? "Выгрузить исходный XML…" : draftButton.Text;

            var commands = Toolbar(checkButton, saveButton, draftButton);
            var header = new TableLayoutPanel { AutoSize = true, Dock = DockStyle.Top, ColumnCount = 3 };
            header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            header.Controls.Add(new Label { Text = "Название документа", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 0);
            header.Controls.Add(nameEditor, 1, 0);
            header.Controls.Add(commands, 2, 0);
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(12) };
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
                documentVersion = (string)document.Root.Attribute("SchemaVersion");
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
                status.Text = error.Message;
            }
            finally
            {
                if (!IsDisposed && !Disposing) SetBusy(false);
            }

            if (loaded && ReadOnlyDocument && !IsDisposed && !Disposing) tabs.SelectedTab = previewPage;
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
            base.Dispose(disposing);
            if (disposing) xmlFont.Dispose();
        }

        private static Button ActionButton(string text) => new Button { AutoSize = true, Text = text, Padding = new Padding(5, 0, 5, 0) };

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
            panel.Controls.Add(Toolbar(controls), 0, 0);
            panel.Controls.Add(content, 0, 1);
            return panel;
        }

        private void SetBusy(bool value)
        {
            busy = value;
            tabs.Enabled = loaded && !value;
            nameEditor.Enabled = loaded && !value;
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

        private void MarkChanged(bool xmlChanged = true)
        {
            dirty = true;
            if (xmlChanged) revision++;
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
                if ((string)next.Root.Attribute("SchemaVersion") != documentVersion)
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
                status.Text = error.Message;
            }
        }

        private bool CanUseDocument()
        {
            if (!loaded || busy) return false;
            if (!rawDirty) return true;
            tabs.SelectedTab = xmlPage;
            status.Text = "Примените изменения исходного XML.";
            return false;
        }

        private async Task ValidateOrSaveAsync(bool commit)
        {
            if (!CanUseDocument() || (commit && ReadOnlyDocument)) return;
            await RunAsync(commit ? "Проверка перед сохранением…" : "Проверка XML…", async () =>
            {
                var xml = Snapshot();
                var result = await service.ValidateAsync(xml);
                ShowValidation(result);
                if (!result.Valid || !commit) return;
                Document = await save(xml, nameEditor.Text);
                dirty = false;
                rawDirty = false;
                DialogResult = DialogResult.OK;
                Close();
            });
        }

        private void ShowValidation(ConclusionValidation result)
        {
            errors.Items.Clear();
            if (result.Valid)
            {
                errors.Items.Add(new ListViewItem(new[] { "/", $"XML соответствует схеме {result.Version}." }));
                status.Text = $"XML соответствует схеме {result.Version}.";
            }
            else
            {
                foreach (var error in result.Errors ?? new List<ConclusionValidationError>())
                    errors.Items.Add(new ListViewItem(new[] { error.Path ?? "/", error.Message }) { Tag = error.Path });
                status.Text = $"XML требует исправлений. Ошибок: {errors.Items.Count}.";
            }
            tabs.SelectedTab = validationPage;
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

        private async Task RunAsync(string message, Func<Task> action)
        {
            SetBusy(true);
            status.Text = message;
            try
            {
                await action();
            }
            catch (Exception error)
            {
                if (!IsDisposed && !Disposing) status.Text = error.Message;
            }
            finally
            {
                if (!IsDisposed && !Disposing) SetBusy(false);
            }
        }

        private void SaveDraft()
        {
            var text = rawDirty ? rawEditor.Text : document != null ? Snapshot() : initialXml;
            using (var picker = new SaveFileDialog { Filter = "Документ XML (*.xml)|*.xml", DefaultExt = "xml", AddExtension = true })
            {
                if (picker.ShowDialog(this) != DialogResult.OK) return;
                var temporary = Path.Combine(Path.GetDirectoryName(picker.FileName), Guid.NewGuid().ToString("N") + ".tmp");
                try
                {
                    File.WriteAllText(temporary, NormalizeDraftEncoding(text), new UTF8Encoding(false));
                    if (File.Exists(picker.FileName)) File.Replace(temporary, picker.FileName, null);
                    else File.Move(temporary, picker.FileName);
                    status.Text = "Исходный XML записан в выбранный файл.";
                }
                catch (Exception error)
                {
                    status.Text = error.Message;
                }
                finally
                {
                    if (File.Exists(temporary)) File.Delete(temporary);
                }
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
}

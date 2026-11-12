using Core.NET.Controllers;
using Core.NET.Models.XPO;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using ProjectDocumentation.NET.Services;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms
{
    public partial class MainForm
    {
        private const string ClassicConclusionFormat = "официальной схеме заключения 01.03";
        private const string ClassicNoteFormat = "локальному формату записки 01.03";
        private readonly ConclusionSchemaService conclusionSchema = new ConclusionSchemaService();
        private BarButtonItem convertConclusion;
        private BarButtonItem exportLegacyConclusion;

        private void InitializeConclusionCommands()
        {
            barBtnAddConclusion.Caption = "Добавить заключение 01.04";
            convertConclusion = new BarButtonItem(barManager, "Преобразовать заключение в 01.04");
            convertConclusion.ItemClick += ConvertConclusion_ItemClick;
            exportLegacyConclusion = new BarButtonItem(barManager, "Выгрузить совместимую копию 01.03");
            exportLegacyConclusion.ItemClick += ExportLegacyConclusion_ItemClick;
        }

        private void UpdateConclusionCommands(object row)
        {
            var conclusion = row as ConclusionXPO;
            var version = Conclusion0104Document.ReadVersion(conclusion?.GetObj());
            convertConclusion.Enabled = Conclusion0104Document.CanMigrate(version);
            exportLegacyConclusion.Enabled = version == Conclusion0104Document.CurrentVersion;
            if (conclusion != null)
            {
                var canSetParameters = version == Conclusion0104Document.CurrentVersion || Conclusion0104Document.CanUseLegacyEditor(version);
                var canOpen = canSetParameters || Conclusion0104Document.IsHistoricalVersion(version);
                barBtnBaseSetting.Enabled = canSetParameters;
                barBtnEdit.Enabled = canOpen;
                barBtnView.Enabled = canOpen;
                barBtnFill.Enabled = canOpen;
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            conclusionSchema.Dispose();
            base.OnFormClosed(e);
        }

        private async Task AddCurrentConclusionAsync()
        {
            try
            {
                var document = Conclusion0104Document.Parse(await conclusionSchema.CreateAsync());
                var guid = Conclusion0104Document.ReadGuid(document) ?? Guid.NewGuid();
                using (var editor = new Conclusion0104Form(conclusionSchema, Conclusion0104Document.WithGuid(document, guid), "",
                    (xml, name) => Conclusion0104Document.SaveAsync(conclusionSchema, xml, name, guid, allowIncomplete: true)))
                {
                    if (editor.ShowDialog(this) == DialogResult.OK && editor.Document != null)
                    {
                        await UpdateGridDataSourceAsync(nameof(XmlObjXPO.Guid), editor.Document.Guid);
                    }
                }
            }
            catch (Exception error)
            {
                ShowConclusionError(error);
            }
        }

        private async Task EditCurrentConclusionAsync(ConclusionXPO record, string section = null)
        {
            try
            {
                var guid = record.Guid ?? throw new InvalidOperationException("У заключения отсутствует идентификатор в базе.");
                using (var editor = new Conclusion0104Form(conclusionSchema, record.GetObj(), record.Name,
                    (xml, name) => Conclusion0104Document.SaveAsync(conclusionSchema, xml, name, guid, record, allowIncomplete: true), section))
                {
                    if (editor.ShowDialog(this) == DialogResult.OK && editor.Document != null)
                    {
                        await UpdateGridDataSourceAsync(nameof(XmlObjXPO.Guid), editor.Document.Guid);
                    }
                }
            }
            catch (Exception error)
            {
                ShowConclusionError(error);
            }
        }

        private void ViewCurrentConclusion(ConclusionXPO record)
        {
            using (var editor = new Conclusion0104Form(conclusionSchema, record.GetObj(), record.Name))
            {
                editor.ShowDialog(this);
            }
        }

        private async void ConvertConclusion_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView.GetFocusedRow() is ConclusionXPO source)
            {
                await ConvertConclusionAsync(source);
            }
        }

        private async Task ConvertConclusionAsync(ConclusionXPO source)
        {
            var originalXml = source.GetObj();
            if (!Conclusion0104Document.CanMigrate(Conclusion0104Document.ReadVersion(originalXml)))
            {
                return;
            }
            try
            {
                var migration = await conclusionSchema.MigrateAsync(originalXml, Conclusion0104Document.CurrentVersion);
                var document = Conclusion0104Document.Parse(migration.Xml);
                var guid = Guid.NewGuid();
                using (var editor = new Conclusion0104Form(conclusionSchema, Conclusion0104Document.WithGuid(document, guid),
                    $"{source.Name} (01.04)", (xml, name) => Conclusion0104Document.SaveAsync(conclusionSchema, xml, name, guid, allowIncomplete: true),
                    warnings: migration.Warnings))
                {
                    if (editor.ShowDialog(this) == DialogResult.OK && editor.Document != null)
                    {
                        await UpdateGridDataSourceAsync(nameof(XmlObjXPO.Guid), editor.Document.Guid);
                    }
                }
            }
            catch (Exception error)
            {
                ShowConclusionError(error);
            }
        }

        private void ViewHistoricalConclusion(ConclusionXPO record, string version)
            => ViewSourceXml(record, $"Исходное заключение {version}");

        private void ViewSourceXml(XmlObjXPO record, string title)
        {
            using (var viewer = new XtraForm { Text = $"{title} — {record.Name}",
                StartPosition = FormStartPosition.CenterParent, Size = new Size(980, 720) })
            using (var font = new Font(FontFamily.GenericMonospace, 10))
            {
                var source = new MemoEdit { Dock = DockStyle.Fill, Font = font, Text = record.GetObj(), AccessibleName = "Исходный XML" };
                source.Properties.ReadOnly = true;
                source.Properties.WordWrap = false;
                source.Properties.ScrollBars = ScrollBars.Both;
                viewer.Controls.Add(source);
                viewer.ShowDialog(this);
            }
        }

        private async Task ExportHistoricalConclusionAsync(XmlObjXPO record, string xml, FileOperationEntry attempt)
        {
            var original = record.Obj;
            await FileOperationJournal.ObserveSchemaAsync(attempt, conclusionSchema, xml);
            using (var picker = new XtraSaveFileDialog { Filter = "XML (*.xml)|*.xml", DefaultExt = "xml", AddExtension = true, OverwritePrompt = true,
                FileName = attempt.FileName })
            {
                if (picker.ShowDialog(this) == DialogResult.OK)
                {
                    SetExportPath(attempt, picker.FileName);
                    File.WriteAllBytes(attempt.FilePath, original);
                    CompleteExport(attempt, "Исходный XML выгружен без преобразования.");
                }
            }
        }

        private async void ExportLegacyConclusion_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(gridView.GetFocusedRow() is ConclusionXPO record) ||
                Conclusion0104Document.ReadVersion(record.GetObj()) != Conclusion0104Document.CurrentVersion)
            {
                return;
            }

            try
            {
                await TrackExportAsync(record, "conclusion", Conclusion0104Document.LegacyVersion, async attempt =>
                {
                    var migration = await conclusionSchema.MigrateAsync(record.GetObj(), attempt.Version);
                    await ExportSchemaDocumentAsync(migration.Xml, attempt, Conclusion0104Document.RootName, migration.Warnings);
                });
            }
            catch (Exception error)
            {
                ShowConclusionError(error);
            }
        }

        private async Task ExportSchemaDocumentAsync(string xml, FileOperationEntry attempt, string rootName,
            System.Collections.Generic.IReadOnlyList<string> warnings = null, bool allowClassicDraft = false,
            bool saveAsDraft = false, IWin32Window owner = null)
        {
            var version = attempt.Version;
            var classicNote = rootName == ExplanatoryNote0107Document.RootName && version == ExplanatoryNote0107Document.LegacyVersion;
            if (warnings != null)
                foreach (var warning in warnings) FileOperationJournal.AddDiagnostic(attempt, warning);

            var validation = await conclusionSchema.ValidateAsync(xml);
            FileOperationJournal.AddValidation(attempt, validation);
            var offerClassicDraft = allowClassicDraft && validation.Version == version && !validation.Valid;
            var isClassicDraft = saveAsDraft || offerClassicDraft;
            if (offerClassicDraft)
            {
                if (!ConfirmClassicDraft(validation, classicNote ? ClassicNoteFormat : ClassicConclusionFormat, "Выгрузить XML как черновик?"))
                {
                    attempt.Summary = "Выгрузка черновика отменена после проверки XML.";
                    return;
                }
            }
            else if ((!validation.Valid && !saveAsDraft) || validation.Version != version)
            {
                attempt.Status = FileOperationStatus.Rejected;
                attempt.Summary = $"XML не прошёл проверку по схеме {version}.";
                SchemaXmlDocument.RequireValid(validation, version);
            }
            if (warnings != null && warnings.Count > 0)
            {
                var text = string.Join(Environment.NewLine, warnings.Take(10));
                if (warnings.Count > 10)
                {
                    text += Environment.NewLine + $"Ещё замечаний: {warnings.Count - 10}.";
                }

                if (XtraMessageBox.Show(this, text + Environment.NewLine + Environment.NewLine + "Выгрузить совместимую копию?",
                    "Преобразование XML", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    attempt.Summary = "Выгрузка совместимой копии отменена после просмотра замечаний.";
                    return;
                }
            }

            if (isClassicDraft) attempt.FileName = $"{attempt.Family}-draft_{attempt.DocumentId:D}.xml";
            using (var picker = new XtraSaveFileDialog { Filter = "XML (*.xml)|*.xml", DefaultExt = "xml", AddExtension = true, OverwritePrompt = true,
                FileName = attempt.FileName })
            {
                if (picker.ShowDialog(owner ?? this) != DialogResult.OK)
                {
                    return;
                }

                SetExportPath(attempt, picker.FileName);
                var schema = classicNote ? ExplanatoryNote0107Document.LegacySchemaAsset : $"{attempt.Family}-{version.Replace('.', '-')}";
                await SchemaXmlDocument.WriteExportWithAssetsAsync(conclusionSchema, attempt.FilePath, xml, rootName, schema,
                    includeStylesheet: !classicNote);
                if (isClassicDraft)
                {
                    attempt.Status = FileOperationStatus.Warning;
                    attempt.Summary = "Сохранён XML-черновик.";
                }
                else CompleteExport(attempt, "XML выгружен.");
            }
        }

        private static string ClassicValidationMessage(ConclusionValidation validation, string format)
        {
            var errors = validation.Errors;
            var lines = errors?.Take(12).Select(error => $"{error.Path} {error.Message}") ?? Enumerable.Empty<string>();
            var text = string.Join(Environment.NewLine, lines);
            if (errors?.Count > 12)
            {
                text += Environment.NewLine + $"Ещё ошибок: {errors.Count - 12}.";
            }

            return $"XML не прошёл проверку по {format}." + Environment.NewLine + text;
        }

        private bool ConfirmClassicDraft(ConclusionValidation validation, string format, string prompt)
        {
            return XtraMessageBox.Show(Form.ActiveForm ?? this,
                ClassicValidationMessage(validation, format) + Environment.NewLine + Environment.NewLine + prompt,
                "Проверка XML", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        private void ShowConclusionError(Exception error)
        {
            XtraMessageBox.Show(this, error.Message, "Заключение экспертизы", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool CheckConclusionVersion(string version)
        {
            if (Conclusion0104Document.CanUseLegacyEditor(version))
            {
                return true;
            }

            XtraMessageBox.Show(this, $"Версия заключения {version} не поддерживается.", "Заключение экспертизы",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
    }
}

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
        private readonly ConclusionSchemaService conclusionSchema = new ConclusionSchemaService();
        private BarButtonItem convertConclusion;
        private BarButtonItem exportLegacyConclusion;

        private void InitializeConclusionCommands()
        {
            barBtnAddConclusion.Caption = "Добавить заключение 01.04";
            convertConclusion = new BarButtonItem(barManager, "Преобразовать заключение в 01.04");
            convertConclusion.ItemClick += ConvertConclusion_ItemClick;
            popupMenu.AddItem(convertConclusion);
            exportLegacyConclusion = new BarButtonItem(barManager, "Выгрузить совместимую копию 01.03");
            exportLegacyConclusion.ItemClick += ExportLegacyConclusion_ItemClick;
            popupMenu.AddItem(exportLegacyConclusion);
        }

        private void UpdateConclusionCommands(object row)
        {
            var conclusion = row as ConclusionXPO;
            var version = Conclusion0104Document.ReadVersion(conclusion?.GetObj());
            convertConclusion.Enabled = Conclusion0104Document.CanMigrate(version);
            exportLegacyConclusion.Enabled = version == Conclusion0104Document.CurrentVersion;
            if (Conclusion0104Document.IsHistoricalVersion(version))
            {
                barBtnBaseSetting.Enabled = false;
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
                    (xml, name) => Conclusion0104Document.SaveAsync(conclusionSchema, xml, name, guid)))
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
                    (xml, name) => Conclusion0104Document.SaveAsync(conclusionSchema, xml, name, guid, record), section))
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
                    $"{source.Name} (01.04)", (xml, name) => Conclusion0104Document.SaveAsync(conclusionSchema, xml, name, guid),
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
        {
            using (var viewer = new XtraForm { Text = $"Исходное заключение {version} — {record.Name}",
                StartPosition = FormStartPosition.CenterParent, Size = new Size(980, 720) })
            using (var font = new Font(FontFamily.GenericMonospace, 10))
            {
                viewer.Controls.Add(new TextBox { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true,
                    WordWrap = false, ScrollBars = ScrollBars.Both, Font = font, Text = record.GetObj() });
                viewer.ShowDialog(this);
            }
        }

        private void ExportHistoricalConclusion(ConclusionXPO record)
        {
            using (var picker = new SaveFileDialog { Filter = "XML (*.xml)|*.xml", DefaultExt = "xml", AddExtension = true,
                FileName = $"conclusion_{record.Guid:D}.xml" })
            {
                if (picker.ShowDialog(this) == DialogResult.OK)
                {
                    File.WriteAllBytes(picker.FileName, record.Obj);
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
                var migration = await conclusionSchema.MigrateAsync(record.GetObj(), Conclusion0104Document.LegacyVersion);
                await ExportConclusionAsync(migration.Xml, Conclusion0104Document.LegacyVersion, record.Guid, migration.Warnings);
            }
            catch (Exception error)
            {
                ShowConclusionError(error);
            }
        }

        private Task ExportConclusionAsync(string xml, string version, Guid? guid, System.Collections.Generic.IReadOnlyList<string> warnings = null)
            => ExportSchemaDocumentAsync(xml, version, guid, "Conclusion", "conclusion", warnings);

        private async Task ExportSchemaDocumentAsync(string xml, string version, Guid? guid, string rootName, string prefix,
            System.Collections.Generic.IReadOnlyList<string> warnings = null)
        {
            SchemaXmlDocument.RequireValid(await conclusionSchema.ValidateAsync(xml), version);
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
                    return;
                }
            }

            using (var picker = new SaveFileDialog { Filter = "XML (*.xml)|*.xml", DefaultExt = "xml", AddExtension = true,
                FileName = $"{prefix}_{guid:D}.xml" })
            {
                if (picker.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                var schema = $"{prefix}-{version.Replace('.', '-')}";
                await SchemaXmlDocument.WriteExportWithAssetsAsync(conclusionSchema, picker.FileName, xml, rootName, schema);
            }
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

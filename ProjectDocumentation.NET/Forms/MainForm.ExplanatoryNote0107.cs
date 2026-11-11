using Core.NET.Controllers;
using Core.NET.Models.XPO;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using ProjectDocumentation.NET.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms
{
    public partial class MainForm
    {
        private BarButtonItem convertExplanatoryNote;
        private BarButtonItem exportCompatibleExplanatoryNote;

        private void InitializeExplanatoryNoteCommands()
        {
            barBtnAddExplanatoryNote.Caption = "Добавить пояснительную записку 01.07";
            convertExplanatoryNote = new BarButtonItem(barManager, "Преобразовать записку в 01.07");
            convertExplanatoryNote.ItemClick += ConvertExplanatoryNote_ItemClick;
            popupMenu.AddItem(convertExplanatoryNote);
            exportCompatibleExplanatoryNote = new BarButtonItem(barManager, "Выгрузить копию записки 01.05");
            exportCompatibleExplanatoryNote.ItemClick += ExportCompatibleExplanatoryNote_ItemClick;
            popupMenu.AddItem(exportCompatibleExplanatoryNote);
        }

        private void UpdateExplanatoryNoteCommands(object row)
        {
            var note = row as ExplanatoryNoteXPO;
            var xml = note?.GetObj();
            var version = ExplanatoryNote0107Document.ReadVersion(xml);
            var legacy = note != null && ExplanatoryNoteController.IsLegacyXml(xml);
            convertExplanatoryNote.Enabled = version == ExplanatoryNote0107Document.CompatibleVersion || legacy;
            exportCompatibleExplanatoryNote.Enabled = version == ExplanatoryNote0107Document.CurrentVersion;
            if (note != null)
            {
                var previewable = CanPreviewExplanatoryNote(version);
                barBtnView.Enabled = previewable || legacy;
                barBtnEdit.Enabled = previewable || legacy;
                barBtnFill.Enabled = previewable || legacy;
            }
        }

        private static bool CanPreviewExplanatoryNote(string version)
            => version == ExplanatoryNote0107Document.CurrentVersion || version == ExplanatoryNote0107Document.CompatibleVersion;

        private async Task AddCurrentExplanatoryNoteAsync()
        {
            try
            {
                var document = ExplanatoryNote0107Document.Parse(await conclusionSchema.CreateAsync(ExplanatoryNote0107Document.Family));
                var guid = ExplanatoryNote0107Document.ReadGuid(document) ?? Guid.NewGuid();
                await EditExplanatoryNoteXmlAsync(ExplanatoryNote0107Document.WithGuid(document, guid), "", guid);
            }
            catch (Exception error)
            {
                ShowExplanatoryNoteError(error);
            }
        }

        private async Task EditCurrentExplanatoryNoteAsync(ExplanatoryNoteXPO record)
        {
            try
            {
                var guid = record.Guid ?? throw new InvalidOperationException("У записки отсутствует идентификатор в базе.");
                await EditExplanatoryNoteXmlAsync(record.GetObj(), record.Name, guid, record);
            }
            catch (Exception error)
            {
                ShowExplanatoryNoteError(error);
            }
        }

        private async Task EditExplanatoryNoteXmlAsync(string xml, string name, Guid guid, ExplanatoryNoteXPO existing = null,
            System.Collections.Generic.IReadOnlyList<string> warnings = null)
        {
            using (var editor = new ExplanatoryNote0107Form(conclusionSchema, xml, name,
                (document, title) => ExplanatoryNote0107Document.SaveAsync(conclusionSchema, document, title, guid, existing), warnings))
            {
                if (editor.ShowDialog(this) == DialogResult.OK && editor.Document != null)
                {
                    await UpdateGridDataSourceAsync(nameof(XmlObjXPO.Guid), editor.Document.Guid);
                }
            }
        }

        private async void ConvertExplanatoryNote_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView.GetFocusedRow() is ExplanatoryNoteXPO record)
            {
                await ConvertExplanatoryNoteAsync(record);
            }
        }

        private async Task ConvertExplanatoryNoteAsync(ExplanatoryNoteXPO record)
        {
            var xml = record.GetObj();
            if (ExplanatoryNote0107Document.ReadVersion(xml) != ExplanatoryNote0107Document.CompatibleVersion && !ExplanatoryNoteController.IsLegacyXml(xml))
            {
                return;
            }

            try
            {
                var migration = await conclusionSchema.MigrateAsync(ExplanatoryNote0107Document.WithMigrationVersion(xml), ExplanatoryNote0107Document.CurrentVersion);
                var document = ExplanatoryNote0107Document.Parse(migration.Xml);
                var guid = Guid.NewGuid();
                await EditExplanatoryNoteXmlAsync(ExplanatoryNote0107Document.WithGuid(document, guid), $"{record.Name} (01.07)", guid,
                    warnings: migration.Warnings);
            }
            catch (Exception error)
            {
                ShowExplanatoryNoteError(error);
            }
        }

        private void ViewExplanatoryNote(ExplanatoryNoteXPO record)
        {
            using (var editor = new ExplanatoryNote0107Form(conclusionSchema, record.GetObj(), record.Name))
            {
                editor.ShowDialog(this);
            }
        }

        private async void ExportCompatibleExplanatoryNote_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(gridView.GetFocusedRow() is ExplanatoryNoteXPO record) ||
                ExplanatoryNote0107Document.ReadVersion(record.GetObj()) != ExplanatoryNote0107Document.CurrentVersion)
            {
                return;
            }

            try
            {
                await TrackExportAsync(record, ExplanatoryNote0107Document.Family, ExplanatoryNote0107Document.CompatibleVersion, async attempt =>
                {
                    var migration = await conclusionSchema.MigrateAsync(record.GetObj(), attempt.Version);
                    await ExportSchemaDocumentAsync(migration.Xml, attempt, ExplanatoryNote0107Document.RootName, migration.Warnings);
                });
            }
            catch (Exception error)
            {
                ShowExplanatoryNoteError(error);
            }
        }

        private void ShowExplanatoryNoteError(Exception error)
        {
            XtraMessageBox.Show(this, error.Message, "Пояснительная записка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

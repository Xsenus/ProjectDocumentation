using Core.NET.Controllers;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using ProjectDocumentation.NET.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ProjectDocumentation.NET.Forms
{
    public partial class FileLoad
    {
        private async Task ImportExplanatoryNoteAsync(XDocument document, string version, string path, string name, FileOperationEntry attempt)
        {
            if (conclusionSchema == null)
            {
                throw new InvalidOperationException("Редактор пояснительных записок 01.07 недоступен.");
            }

            var warnings = new List<string>();
            var upgrading = version == ExplanatoryNote0107Document.CompatibleVersion;
            if (upgrading)
            {
                var migration = await conclusionSchema.MigrateAsync(document.ToString(SaveOptions.DisableFormatting), ExplanatoryNote0107Document.CurrentVersion);
                document = ExplanatoryNote0107Document.Parse(migration.Xml);
                warnings.Add("Пояснительная записка 01.05 загружается как отдельный документ 01.07.");
                if (migration.Warnings != null)
                {
                    warnings.AddRange(migration.Warnings);
                }
            }

            var incomingGuid = upgrading ? null : ExplanatoryNote0107Document.ReadGuid(document) ?? ExplanatoryNote0107Document.ReadFileGuid(path);
            var guid = incomingGuid ?? Guid.NewGuid();
            ExplanatoryNoteXPO replace = null;
            if (incomingGuid != null)
            {
                var existing = await new XPQuery<XmlObjXPO>(SessionController.GetSession()).FirstOrDefaultAsync(item => item.Guid == guid);
                if (existing != null)
                {
                    var sameVersion = existing is ExplanatoryNoteXPO &&
                        ExplanatoryNote0107Document.ReadVersion(existing.GetObj()) == ExplanatoryNote0107Document.CurrentVersion;
                    switch (ResolveImportConflict(existing, sameVersion, attempt))
                    {
                        case ImportConflictAction.Replace:
                            replace = (ExplanatoryNoteXPO)existing;
                            break;
                        case ImportConflictAction.CreateCopy:
                            guid = Guid.NewGuid();
                            break;
                        default:
                            return;
                    }
                }
            }

            foreach (var warning in warnings) FileOperationJournal.AddDiagnostic(attempt, warning);
            using (var editor = new ExplanatoryNote0107Form(conclusionSchema, ExplanatoryNote0107Document.WithGuid(document, guid), name,
                (xml, title) => ExplanatoryNote0107Document.SaveAsync(conclusionSchema, xml, title, guid, replace), warnings)
                { ImportAttempt = attempt })
            {
                if (editor.ShowDialog(this) == DialogResult.OK && editor.Document != null)
                {
                    ObjXPO = editor.Document;
                    var summary = upgrading ? "Записка преобразована и загружена в версии 01.07."
                        : replace != null ? "Содержимое существующей записки заменено."
                        : incomingGuid.HasValue && guid != incomingGuid.Value ? "Записка загружена как копия."
                        : "Записка загружена.";
                    FinishImport(attempt, summary);
                }
            }
        }
    }
}

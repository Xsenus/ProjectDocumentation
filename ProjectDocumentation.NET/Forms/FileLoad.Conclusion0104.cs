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
        private readonly ConclusionSchemaService conclusionSchema;

        private async Task ImportCurrentConclusionAsync(XDocument document, string name, FileOperationEntry attempt)
        {
            if (conclusionSchema == null)
            {
                throw new InvalidOperationException("Редактор заключений 01.04 недоступен.");
            }

            var warnings = new List<string>();
            var incomingGuid = Conclusion0104Document.ReadGuid(document);
            var guid = incomingGuid ?? Guid.NewGuid();
            ConclusionXPO replace = null;
            if (incomingGuid == null)
            {
                warnings.Add("В импортируемом XML нет корректного ConclusionGUID. Документу назначен новый идентификатор.");
            }
            else
            {
                var existing = await new XPQuery<XmlObjXPO>(SessionController.GetSession()).FirstOrDefaultAsync(item => item.Guid == guid);
                if (existing != null)
                {
                    var sameVersion = existing is ConclusionXPO &&
                        Conclusion0104Document.ReadVersion(existing.GetObj()) == Conclusion0104Document.CurrentVersion;
                    switch (ResolveImportConflict(existing, sameVersion, attempt))
                    {
                        case ImportConflictAction.Replace:
                            replace = (ConclusionXPO)existing;
                            break;
                        case ImportConflictAction.CreateCopy:
                            guid = Guid.NewGuid();
                            break;
                        default:
                            return;
                    }
                }
            }

            var summary = replace != null ? "Содержимое существующего заключения заменено."
                : incomingGuid.HasValue && guid != incomingGuid.Value ? "Заключение загружено как копия."
                : "Заключение загружено.";
            OpenConclusionImport(document, name, guid, warnings, attempt, summary, replace);
        }

        private async Task ImportHistoricalConclusionAsync(XDocument document, string name, FileOperationEntry attempt)
        {
            if (conclusionSchema == null)
            {
                throw new InvalidOperationException("Редактор заключений 01.04 недоступен.");
            }

            var migration = await conclusionSchema.MigrateAsync(document.ToString(SaveOptions.DisableFormatting), Conclusion0104Document.CurrentVersion);
            OpenConclusionImport(Conclusion0104Document.Parse(migration.Xml), $"{name} (01.04)", Guid.NewGuid(),
                migration.Warnings, attempt, "Заключение преобразовано и загружено в версии 01.04.");
        }

        private void OpenConclusionImport(XDocument document, string name, Guid guid, IReadOnlyList<string> warnings,
            FileOperationEntry attempt, string summary, ConclusionXPO replace = null)
        {
            if (warnings != null)
                foreach (var warning in warnings) FileOperationJournal.AddDiagnostic(attempt, warning);

            using (var editor = new Conclusion0104Form(conclusionSchema, Conclusion0104Document.WithGuid(document, guid), name,
                (xml, title) => Conclusion0104Document.SaveAsync(conclusionSchema, xml, title, guid, replace, DocumentStatus.InProgress), warnings: warnings)
                { ImportAttempt = attempt })
            {
                if (editor.ShowDialog(this) == DialogResult.OK && editor.Document != null)
                {
                    ObjXPO = editor.Document;
                    FinishImport(attempt, summary);
                }
            }
        }
    }
}

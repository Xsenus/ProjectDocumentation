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

        private async Task ImportCurrentConclusionAsync(XDocument document, string name)
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
                    switch (AskImportConflict(existing, sameVersion))
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

            using (var editor = new Conclusion0104Form(conclusionSchema, Conclusion0104Document.WithGuid(document, guid), name,
                (xml, title) => Conclusion0104Document.SaveAsync(conclusionSchema, xml, title, guid, replace), warnings: warnings))
            {
                if (editor.ShowDialog(this) == DialogResult.OK && editor.Document != null)
                {
                    ObjXPO = editor.Document;
                    Close();
                }
            }
        }
    }
}

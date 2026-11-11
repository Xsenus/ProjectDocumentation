using Core.NET.Controllers;
using Core.NET.Models.XPO;
using DevExpress.XtraBars;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms
{
    public partial class MainForm
    {
        private async void OpenFileOperationHistory_ItemClick(object sender, ItemClickEventArgs e)
        {
            FileOperationEntry retry;
            using (var history = new FileOperationHistoryForm())
            {
                if (history.ShowDialog(this) != DialogResult.OK) return;
                retry = history.RetryEntry;
            }

            if (retry != null) await ImportXmlAsync(retry.FilePath, retry.DocumentName);
        }

        private async Task ImportXmlAsync(string initialPath = null, string initialName = null)
        {
            using (var form = new FileLoad(conclusionSchema, initialPath, initialName))
            {
                form.ShowDialog(this);
                if (form.ObjXPO != null)
                    await UpdateGridDataSourceAsync(nameof(XmlObjXPO.Guid), form.ObjXPO.Guid);
            }
        }

        private Task TrackExportAsync(XmlObjXPO record, string family, string version, Func<FileOperationEntry, Task> export)
            => TrackExportAsync(family, version, record.Guid, record.Name, export);

        private async Task TrackExportAsync(string family, string version, Guid? guid, string name, Func<FileOperationEntry, Task> export)
        {
            var attempt = new FileOperationEntry
            {
                Kind = FileOperationKind.Export,
                Status = FileOperationStatus.Cancelled,
                FileName = $"{family}_{guid:D}.xml",
                DocumentName = name,
                Family = family,
                Version = version,
                DocumentId = guid,
                Summary = "Выгрузка отменена."
            };
            try
            {
                await export(attempt);
            }
            catch (Exception error)
            {
                if (attempt.Status == FileOperationStatus.Rejected)
                {
                    attempt.Summary = error.Message.Split(new[] { Environment.NewLine }, StringSplitOptions.None)[0];
                    if (attempt.Diagnostics.Count == 0) FileOperationJournal.AddDiagnostic(attempt, error.Message);
                }
                else
                {
                    FileOperationJournal.Fail(attempt, error);
                }
                throw;
            }
            finally
            {
                await FileOperationJournal.SaveAsync(this, attempt);
            }
        }

        private static void SetExportPath(FileOperationEntry attempt, string path)
        {
            attempt.FilePath = path;
            attempt.FileName = Path.GetFileName(path);
        }

        private static void CompleteExport(FileOperationEntry attempt, string summary)
        {
            attempt.Status = attempt.Diagnostics.Count == 0 ? FileOperationStatus.Completed : FileOperationStatus.Warning;
            attempt.Summary = summary;
        }
    }
}

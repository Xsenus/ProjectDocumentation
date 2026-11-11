using Core.NET.Controllers;
using DevExpress.XtraEditors;
using ProjectDocumentation.NET.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms
{
    internal static class FileOperationJournal
    {
        internal static void AddDiagnostic(FileOperationEntry entry, string message, string path = null)
        {
            if (entry == null || string.IsNullOrWhiteSpace(message) ||
                entry.Diagnostics.Any(issue => issue.Path == path && issue.Message == message)) return;

            entry.Diagnostics.Add(new FileOperationDiagnostic { Path = path, Message = message });
        }

        internal static void AddValidation(FileOperationEntry entry, ConclusionValidation validation, string prefix = null)
        {
            if (entry == null || validation?.Errors == null) return;

            foreach (var error in validation.Errors)
                AddDiagnostic(entry, prefix == null ? error.Message : $"{prefix} {error.Message}", error.Path);
        }

        internal static async Task ObserveSchemaAsync(FileOperationEntry entry, ConclusionSchemaService schema, string xml, string prefix = null)
        {
            if (entry == null || schema == null) return;
            try
            {
                var validation = await schema.ValidateAsync(xml);
                if (string.IsNullOrWhiteSpace(entry.Version)) entry.Version = validation.Version;
                AddValidation(entry, validation, prefix);
            }
            catch (Exception error)
            {
                AddDiagnostic(entry, $"Проверка XML недоступна. {error.Message}");
            }
        }

        internal static void Fail(FileOperationEntry entry, Exception error)
        {
            if (entry == null) return;
            entry.Status = FileOperationStatus.Failed;
            entry.Summary = error.Message;
            AddDiagnostic(entry, error.Message, error is System.Xml.XmlException ? "/" : null);
        }

        internal static async Task SaveAsync(Form owner, FileOperationEntry entry)
        {
            if (entry == null) return;

            try
            {
                await FileOperationHistory.AddAsync(entry);
            }
            catch (Exception error)
            {
                var text = $"Результат операции не записан в историю файлов.{Environment.NewLine}{error.Message}";
                if (owner.IsDisposed || owner.Disposing)
                    XtraMessageBox.Show(text, "История файлов", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    XtraMessageBox.Show(owner, text, "История файлов", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

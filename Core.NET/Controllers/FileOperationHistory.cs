using Core.NET.Models.XPO;
using DevExpress.Xpo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.NET.Controllers
{
    public enum FileOperationKind
    {
        Import = 0,
        Export = 1
    }

    public enum FileOperationStatus
    {
        Completed = 0,
        Warning = 1,
        Rejected = 2,
        Failed = 3,
        Cancelled = 4
    }

    public sealed class FileOperationDiagnostic
    {
        public string Path { get; set; }
        public string Message { get; set; }
    }

    public sealed class FileOperationEntry
    {
        public DateTime OccurredAt { get; set; } = DateTime.Now;
        public FileOperationKind Kind { get; set; }
        public FileOperationStatus Status { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string DocumentName { get; set; }
        public string Family { get; set; }
        public string Version { get; set; }
        public string Summary { get; set; }
        public Guid? DocumentId { get; set; }
        public List<FileOperationDiagnostic> Diagnostics { get; set; } = new List<FileOperationDiagnostic>();
    }

    public static class FileOperationHistory
    {
        public static async Task AddAsync(FileOperationEntry entry)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry));

            using (var unit = new UnitOfWork(SessionController.GetSession().DataLayer))
            {
                var row = new FileOperationXPO(unit)
                {
                    OccurredAt = entry.OccurredAt,
                    Kind = entry.Kind,
                    Status = entry.Status,
                    FilePath = entry.FilePath,
                    FileName = entry.FileName,
                    DocumentName = entry.DocumentName,
                    Family = entry.Family,
                    Version = entry.Version,
                    Summary = entry.Summary,
                    DocumentId = entry.DocumentId,
                    DiagnosticsJson = entry.Diagnostics?.Count > 0 ? JsonConvert.SerializeObject(entry.Diagnostics) : null
                };
                unit.Save(row);
                await unit.CommitChangesAsync();
            }
        }

        public static async Task<IReadOnlyList<FileOperationEntry>> ReadAsync()
        {
            using (var unit = new UnitOfWork(SessionController.GetSession().DataLayer))
            {
                var rows = await new XPQuery<FileOperationXPO>(unit)
                    .OrderByDescending(row => row.OccurredAt).ThenByDescending(row => row.Oid)
                    .ToListAsync();
                return rows.Select(row => new FileOperationEntry
                {
                    OccurredAt = row.OccurredAt,
                    Kind = row.Kind,
                    Status = row.Status,
                    FilePath = row.FilePath,
                    FileName = row.FileName,
                    DocumentName = row.DocumentName,
                    Family = row.Family,
                    Version = row.Version,
                    Summary = row.Summary,
                    DocumentId = row.DocumentId,
                    Diagnostics = ReadDiagnostics(row.DiagnosticsJson)
                }).ToArray();
            }
        }

        private static List<FileOperationDiagnostic> ReadDiagnostics(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return new List<FileOperationDiagnostic>();

            try
            {
                return JsonConvert.DeserializeObject<List<FileOperationDiagnostic>>(json)
                    ?? new List<FileOperationDiagnostic>();
            }
            catch (JsonException)
            {
                return new List<FileOperationDiagnostic>
                {
                    new FileOperationDiagnostic { Message = "Не удалось прочитать сохранённый список замечаний." }
                };
            }
        }
    }
}

using Core.NET.Controllers;
using DevExpress.Xpo;
using System;

namespace Core.NET.Models.XPO
{
    public sealed class FileOperationXPO : XPObject
    {
        public FileOperationXPO(Session session) : base(session) { }

        public DateTime OccurredAt { get; set; }
        public FileOperationKind Kind { get; set; }
        public FileOperationStatus Status { get; set; }
        public Guid? DocumentId { get; set; }

        [Size(SizeAttribute.Unlimited)]
        public string FilePath { get; set; }
        [Size(SizeAttribute.Unlimited)]
        public string FileName { get; set; }
        [Size(SizeAttribute.Unlimited)]
        public string DocumentName { get; set; }
        [Size(80)]
        public string Family { get; set; }
        [Size(80)]
        public string Version { get; set; }
        [Size(SizeAttribute.Unlimited)]
        public string Summary { get; set; }
        [Size(SizeAttribute.Unlimited)]
        public string DiagnosticsJson { get; set; }
    }
}

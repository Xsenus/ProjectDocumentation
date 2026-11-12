// Ignore Spelling: XPO

using Core.NET.Extensions;
using DevExpress.Xpo;
using System;

namespace Core.NET.Models.XPO
{
    public enum DocumentStatus
    {
        New,
        InProgress,
        Completed
    }

    public class XmlObjXPO : BaseObjectXPO
    {
        private DateTime? dateModified;
        private DocumentStatus? workflowStatus;
        private bool? completionValidated;

        public XmlObjXPO() : base() { }
        public XmlObjXPO(Session session) : base(session) { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            SetStatus(DocumentStatus.New);
        }

        public string Name { get; set; }
        public virtual string NameXML => Name ?? "NameXML";

        [Persistent]
        public DateTime? DateModified
        {
            get => dateModified;
            private set => SetPropertyValue(nameof(DateModified), ref dateModified, value);
        }

        [Persistent]
        public DocumentStatus? WorkflowStatus
        {
            get => workflowStatus;
            private set => SetPropertyValue(nameof(WorkflowStatus), ref workflowStatus, value);
        }

        [Persistent]
        public bool? CompletionValidated
        {
            get => completionValidated;
            private set => SetPropertyValue(nameof(CompletionValidated), ref completionValidated, value);
        }

        [NonPersistent]
        public DocumentStatus Status => WorkflowStatus == DocumentStatus.Completed && CompletionValidated != true
            ? DocumentStatus.InProgress
            : WorkflowStatus ?? DocumentStatus.InProgress;

        public void SetStatus(DocumentStatus status)
        {
            if (status == DocumentStatus.Completed)
                throw new InvalidOperationException("Чтобы завершить документ, сначала проверьте XML.");

            WorkflowStatus = status;
            CompletionValidated = null;
        }

        public void SetValidatedCompletion(string validatedXml)
        {
            if (string.IsNullOrEmpty(validatedXml) || GetObj() != validatedXml)
                throw new InvalidOperationException("XML изменился. Повторите проверку перед завершением.");

            WorkflowStatus = DocumentStatus.Completed;
            CompletionValidated = true;
        }

        public void SetObj(string obj) => SetDocument(Name, obj);

        public void SetDocument(string name, string xml)
        {
            if (Name == name && GetObj() == xml) return;

            Name = name;
            Obj = xml.StringToByte();
            if (Session.IsNewObject(this)) return;

            DateModified = DateTime.Now;
            SetStatus(DocumentStatus.InProgress);
        }

        public string GetObj()
        {
            return Obj?.ByteToString();
        }

        [Persistent]
        public byte[] Obj { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

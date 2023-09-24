using DevExpress.Xpo;

namespace Core.NET.Models.XPO
{
    public class DocumentTypeXPO : BaseObjectXPO
    {
        public DocumentTypeXPO() { }
        public DocumentTypeXPO(Session session) : base(session) { }

        public string Value { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

using DevExpress.Xpo;

namespace Core.NET.Models.XPO
{
    public class ExpertTypeXPO : BaseObjectXPO
    {
        public ExpertTypeXPO() { }
        public ExpertTypeXPO(Session session) : base(session) { }

        public string Value { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
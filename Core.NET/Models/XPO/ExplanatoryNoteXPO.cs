using DevExpress.Xpo;

namespace Core.NET.Models.XPO
{
    public class ExplanatoryNoteXPO : XmlObjXPO
    {
        public ExplanatoryNoteXPO() : base() { }
        public ExplanatoryNoteXPO(Session session) : base(session) { }

        public override string ToString()
        {
            return Name;
        }
    }
}
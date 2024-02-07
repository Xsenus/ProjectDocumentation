// Ignore Spelling: XPO

using DevExpress.Xpo;

namespace Core.NET.Models.XPO
{
    public class ConclusionXPO : XmlObjXPO
    {
        public ConclusionXPO() : base() { }
        public ConclusionXPO(Session session) : base(session) { }

        public override string ToString()
        {
            return Name;
        }
    }
}
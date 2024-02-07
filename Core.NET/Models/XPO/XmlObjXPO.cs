// Ignore Spelling: XPO

using Core.NET.Extensions;
using DevExpress.Xpo;

namespace Core.NET.Models.XPO
{
    public class XmlObjXPO : BaseObjectXPO
    {
        public XmlObjXPO() : base() { }
        public XmlObjXPO(Session session) : base(session) { }

        public string Name { get; set; }
        public virtual string NameXML => Name ?? "NameXML";

        public void SetObj(string obj)
        {
            Obj = obj.StringToByte();
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
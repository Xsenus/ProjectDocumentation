using DevExpress.Xpo;
using System;

namespace Core.NET.Models.XPO
{
    public class BaseObjectXPO : XPObject
    {
        public BaseObjectXPO() : base() { }
        public BaseObjectXPO(Session session) : base(session) { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Guid = System.Guid.NewGuid();
            DateCreate = DateTime.Now;
        }

        [Persistent]
        public Guid? Guid { get; private set; }

        [Persistent]
        public DateTime DateCreate { get; private set; }

        public string GuidString => Guid?.ToString();
        public string Name { get; set; }

        public void SetGuid(Guid guid)
        {
            Guid = guid;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
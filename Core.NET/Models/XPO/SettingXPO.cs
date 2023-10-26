using DevExpress.Xpo;

namespace Core.NET.Models.XPO
{
    public class SettingXPO : BaseObjectXPO
    {
        public SettingXPO() : base() { }
        public SettingXPO(Session session) : base(session) { }

        [Size(2048)]
        public string Name { get; set; }

        [Size(2048)]
        public string Value { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
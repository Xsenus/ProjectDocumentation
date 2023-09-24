using DevExpress.Xpo;

namespace Core.NET.Models.XPO
{
    public class ClimateDistrictXPO : BaseObjectXPO
    {
        public ClimateDistrictXPO() { }
        public ClimateDistrictXPO(Session session) : base(session) { }

        public string Value { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
// Ignore Spelling: XPO

using DevExpress.Xpo;

namespace Core.NET.Models.XPO
{
    public class GeneralizedDictionaryXPO : BaseObjectXPO
    {
        public GeneralizedDictionaryXPO() { }
        public GeneralizedDictionaryXPO(Session session) : base(session) { }

        public GeneralizedDictionary? GeneralizedDictionary { get; set; }

        public string Value { get; set; }
        public string Name { get; set; }

        public int? ValueInt
        {
            get
            {
                if (int.TryParse(Value, out int result))
                {
                    return result;
                }

                return default;
            }
        }

        public override string ToString()
        {
            return Name;
            //return $"[{Value}] {Name}";
        }
    }
}
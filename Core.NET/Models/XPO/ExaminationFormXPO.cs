using DevExpress.Xpo;

namespace Core.NET.Models.XPO
{
    public enum GeneralizedDictionary
    {
        /// <summary>
        /// Результат экспертизы.
        /// </summary>
        ExaminationResult,

        /// <summary>
        /// Форма экспертизы.
        /// </summary>
        ExaminationForm,

        /// <summary>
        /// Вид объекта экспертизы.
        /// </summary>
        ExaminationObjectType,

        /// <summary>
        /// Вид экспертизы.
        /// </summary>
        ExaminationStage,

        /// <summary>
        /// Вид работ.
        /// </summary>
        ConstructionType,

        /// <summary>
        /// Предмет экспертизы
        /// </summary>
        ExaminationType
    }

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
        }
    }
}
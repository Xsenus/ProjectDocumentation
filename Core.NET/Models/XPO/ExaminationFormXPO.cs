using DevExpress.Xpo;
using System.ComponentModel;

namespace Core.NET.Models.XPO
{
    public enum TypeOrganization
    {
        /// <summary>
        /// Юридическое лицо.
        /// </summary>
        [Description("Юридическое лицо")]
        Entity,

        /// <summary>
        /// Иностранное юридическое лицо.
        /// </summary>
        [Description("Иностранное юридическое лицо (филиал, представительство)")]
        ForeignLegalEntity
    }

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
        /// Предмет экспертизы.
        /// </summary>
        ExaminationType,

        /// <summary>
        /// Вид объекта капитального строительства.
        /// </summary>
        ObjectType,

        /// <summary>
        /// Вид источника финансирования.
        /// </summary>
        FinanceType,

        /// <summary>
        /// Вид бюджета.
        /// </summary>
        BudgetType
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
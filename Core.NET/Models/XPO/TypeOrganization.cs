// Ignore Spelling: XPO

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
        ForeignLegalEntity,

        /// <summary>
        /// Индивидуальный предприниматель.
        /// </summary>
        [Description("Индивидуальный предприниматель")]
        IndividualEntrepreneur,

        /// <summary>
        /// Физическое лицо.
        /// </summary>
        [Description("Физическое лицо")]
        Person
    }
}
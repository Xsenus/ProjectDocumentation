using System.ComponentModel;

namespace Core.NET.AutoModels
{
    public enum ItemChoiceType
    {
        [Description("В формате ЕГРЗ")]
        EGRZ,

        [Description("В произвольном формате")]
        noEGRZ,
    }
}

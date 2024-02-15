using System.ComponentModel;

namespace Core.NET.AutoModels.ExplanatoryNoteObj
{
    public enum ExplanatoryNoteObjType
    {
        [Description("Производственный объект")]
        IndustrialObject,

        [Description("Непроизводственный объект")]
        NonIndustrialObject,

        [Description("Линейный объект")]
        LinearObject
    }
}

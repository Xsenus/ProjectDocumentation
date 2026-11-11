using System.Collections.Generic;
using System.Linq;

namespace Core.NET.AutoModels.ExplanatoryNoteObj
{
    /// <summary>
    /// Поля, общие для производственного, непроизводственного и линейного объекта пояснительной записки,
    /// которые редактирует форма. Реализуется сгенерированными классами через partial без изменения их кода.
    /// </summary>
    public interface IExplanatoryNoteObject
    {
        string ObjectID { get; set; }
        tPlacement Placement { get; set; }
        tDangerousAndComplex DangerousAndComplex { get; set; }
        bool DangerousAndComplexSpecified { get; set; }
        tUnique Unique { get; set; }
        bool UniqueSpecified { get; set; }
        string Name { get; set; }
        tConstructionType ConstructionType { get; set; }
        decimal ConstructionDuration { get; set; }
        tLandCategory LandCategory { get; set; }

        /// <summary>Выбор: FireDangerCategory, ObjectParts, PeoplePermanentStay, ResponsibilityLevel.</summary>
        object[] Items { get; set; }
    }

    /// <summary>Класс опасности опасного производственного объекта (есть у производственного и линейного объекта).</summary>
    public interface IDangerousIndustrialObject
    {
        tDangerIndustrialClass DangerousIndustrialObject { get; set; }
    }

    public partial class tIndustrialObject : IExplanatoryNoteObject, IDangerousIndustrialObject { }

    public partial class tLinearObject : IExplanatoryNoteObject, IDangerousIndustrialObject { }

    public partial class tNonIndustrialObject : IExplanatoryNoteObject { }

    public static class ExplanatoryNoteObjectExtension
    {
        /// <summary>
        /// Значение из Items указанного типа или <c>null</c>, если его нет.
        /// (FirstOrDefault по перечислению вернул бы первое значение перечисления вместо «нет значения».)
        /// </summary>
        public static T? GetItem<T>(this IExplanatoryNoteObject noteObject) where T : struct
        {
            return noteObject?.Items?.OfType<T>().Cast<T?>().FirstOrDefault();
        }

        /// <summary>Сведения о постоянном пребывании людей (tTextBlock в Items) или <c>null</c>.</summary>
        public static tTextBlock GetPeoplePermanentStay(this IExplanatoryNoteObject noteObject)
        {
            return noteObject?.Items?.OfType<tTextBlock>().FirstOrDefault();
        }

        /// <summary>
        /// Обновляет в Items пожарную категорию, уровень ответственности и сведения о постоянном пребывании людей.
        /// Остальные элементы (части объекта и т. п.) и их порядок сохраняются; <c>null</c> удаляет значение.
        /// Новые значения добавляются в конец в порядке: пожарная категория, уровень ответственности, пребывание людей.
        /// </summary>
        public static void SetEditedItems(this IExplanatoryNoteObject noteObject,
            tFireDangerCategory? fireDangerCategory,
            tResponsibilityLevel? responsibilityLevel,
            tTextBlock peoplePermanentStay)
        {
            var result = new List<object>();
            bool fireSet = false, responsibilitySet = false, peopleSet = false;

            foreach (var item in noteObject.Items ?? new object[0])
            {
                switch (item)
                {
                    case tFireDangerCategory _:
                        if (!fireSet && fireDangerCategory != null)
                        {
                            result.Add(fireDangerCategory.Value);
                        }
                        fireSet = true;
                        break;

                    case tResponsibilityLevel _:
                        if (!responsibilitySet && responsibilityLevel != null)
                        {
                            result.Add(responsibilityLevel.Value);
                        }
                        responsibilitySet = true;
                        break;

                    case tTextBlock _:
                        if (!peopleSet && peoplePermanentStay != null)
                        {
                            result.Add(peoplePermanentStay);
                        }
                        peopleSet = true;
                        break;

                    default:
                        result.Add(item);
                        break;
                }
            }

            if (!fireSet && fireDangerCategory != null)
            {
                result.Add(fireDangerCategory.Value);
            }

            if (!responsibilitySet && responsibilityLevel != null)
            {
                result.Add(responsibilityLevel.Value);
            }

            if (!peopleSet && peoplePermanentStay != null)
            {
                result.Add(peoplePermanentStay);
            }

            noteObject.Items = result.Count > 0 ? result.ToArray() : null;
        }
    }
}

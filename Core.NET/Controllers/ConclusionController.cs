using Core.NET.AutoModels.ConclusionObj;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core.NET.Controllers
{
    public static class ConclusionController
    {
        public static async Task<ConclusionXPO> CreateConclusionByPath(string path, string name = default)
        {
            return (await ImportConclusionAsync(path, name)).Document as ConclusionXPO;
        }

        /// <summary>
        /// Импорт заключения из файла. GUID берётся из атрибута ConclusionGUID, иначе из имени файла.
        /// Если документ с таким GUID уже есть, решение принимает <paramref name="resolveConflict"/>
        /// (по умолчанию — копия с новым GUID, который записывается и в ConclusionGUID).
        /// </summary>
        public static async Task<ImportResult> ImportConclusionAsync(string path, string name = default, Func<XmlObjXPO, ImportConflictAction> resolveConflict = null)
        {
            var result = new ImportResult();
            var conclusion = GetConclusionByPath(path, result.Warnings);
            if (conclusion is null)
            {
                return result;
            }

            var guid = Guid.TryParse(conclusion.ConclusionGuid, out Guid resultGuid) ? resultGuid : GetGuidByFile(path);

            return await ImportController.SaveAsync(result, name, guid, resolveConflict,
                session => new ConclusionXPO(session),
                newGuid =>
                {
                    if (newGuid != null)
                    {
                        conclusion.ConclusionGuid = newGuid.ToString();
                    }

                    return conclusion.SerializeObject();
                });
        }

        private static Guid? GetGuidByFile(string path)
        {
            var fileInfo = new FileInfo(path);
            var splits = fileInfo.Name.Replace(fileInfo.Extension, string.Empty).Split('_');
            foreach (var item in splits)
            {
                if (Guid.TryParse(item, out Guid result))
                {
                    return result;
                }
            }

            return default;
        }

        public static Conclusion GetConclusionByPath(string path, ICollection<string> warnings = null)
        {
            if (FileController.CheckFile(path))
            {
                return Utf8StringSerialize.DeSerializeObject<Conclusion>(path, warnings: warnings);
            }

            return default;
        }

        public static Conclusion GetConclusionByString(string obj)
        {
            if (string.IsNullOrWhiteSpace(obj))
            {
                return default;
            }

            return Utf8StringSerialize.DeSerializeObjectByString<Conclusion>(obj);
        }

        /// <summary>
        /// Повторяющиеся (полностью одинаковые) элементы в списках заключения.
        /// Их накапливала старая ошибка сохранения: каждое пересохранение удваивало списки.
        /// </summary>
        /// <param name="apply"><c>true</c> — удалить повторы (остаётся первое вхождение), <c>false</c> — только посчитать.</param>
        /// <returns>Количество повторов.</returns>
        public static int RemoveDuplicateItems(this Conclusion conclusion, bool apply = true)
        {
            if (conclusion is null)
            {
                return 0;
            }

            return RemoveDuplicates(conclusion.Documents, apply)
                + RemoveDuplicates(conclusion.PreviousConclusions, apply)
                + RemoveDuplicates(conclusion.Finance, apply)
                + RemoveDuplicates(conclusion.Designer, apply)
                + RemoveDuplicates(conclusion.EepdUse, apply)
                + RemoveDuplicates(conclusion.CadastralNumber, apply)
                + RemoveDuplicates(conclusion.ProjectDocumentsDeveloper, apply)
                + RemoveDuplicates(conclusion.ProjectDocumentsTechnicalCustomer, apply)
                + RemoveDuplicates(conclusion.EngineeringSurveyDeveloper, apply)
                + RemoveDuplicates(conclusion.EngineeringSurveyTechnicalCustomer, apply)
                + RemoveDuplicates(conclusion.ExpertProjectDocuments, apply)
                + RemoveDuplicates(conclusion.EngineeringSurveyAddress, apply)
                + RemoveDuplicates(conclusion.ExpertEngineeringSurveys, apply)
                + RemoveDuplicates(conclusion.Experts, apply)
                + RemoveDuplicates(conclusion.Summary?.EngineeringSurveyType, apply);
        }

        private static int RemoveDuplicates<T>(Collection<T> collection, bool apply)
        {
            if (collection is null || collection.Count < 2)
            {
                return 0;
            }

            // Элементы сравниваются по XML: так одинаковыми считаются записи с полностью совпадающим содержимым.
            var serializer = typeof(T).IsValueType || typeof(T) == typeof(string) ? null : new XmlSerializer(typeof(T));
            string GetKey(T item)
            {
                if (serializer is null || item == null)
                {
                    return item?.ToString();
                }

                using (var writer = new StringWriter())
                {
                    serializer.Serialize(writer, item);
                    return writer.ToString();
                }
            }

            var seen = new HashSet<string>();
            var duplicates = 0;
            for (var i = 0; i < collection.Count;)
            {
                if (seen.Add(GetKey(collection[i]) ?? string.Empty))
                {
                    i++;
                    continue;
                }

                duplicates++;
                if (apply)
                {
                    collection.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            return duplicates;
        }

        public static string GetSerializeConclusion(this Conclusion conclusion)
        {
            return Utf8StringSerialize.SerializeObject(conclusion);
        }
    }
}
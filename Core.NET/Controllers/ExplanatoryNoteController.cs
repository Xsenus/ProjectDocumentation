using Core.NET.AutoModels.ExplanatoryNoteObj;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Core.NET.Controllers
{
    public static class ExplanatoryNoteController
    {
        public static async Task<ExplanatoryNoteXPO> CreateExplanatoryNoteByPath(string path, string name = default)
        {
            return (await ImportExplanatoryNoteAsync(path, name)).Document as ExplanatoryNoteXPO;
        }

        /// <summary>
        /// Импорт пояснительной записки из файла. GUID берётся из имени файла, иначе создаётся новый.
        /// Если документ с таким GUID уже есть, решение принимает <paramref name="resolveConflict"/>
        /// (по умолчанию — копия с новым GUID).
        /// </summary>
        public static async Task<ImportResult> ImportExplanatoryNoteAsync(string path, string name = default, Func<XmlObjXPO, ImportConflictAction> resolveConflict = null)
        {
            var result = new ImportResult();
            var explanatoryNote = GetExplanatoryNoteByPath(path, result.Warnings);
            if (explanatoryNote is null)
            {
                return result;
            }

            return await ImportController.SaveAsync(result, name, GetGuidByFile(path), resolveConflict,
                session => new ExplanatoryNoteXPO(session),
                newGuid => explanatoryNote.SerializeObject());
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

        public static ExplanatoryNote GetExplanatoryNoteByPath(string path, ICollection<string> warnings = null)
        {
            if (FileController.CheckFile(path))
            {
                return Utf8StringSerialize.DeSerializeObject<ExplanatoryNote>(path, warnings: warnings);
            }

            return default;
        }

        public static ExplanatoryNote GetExplanatoryNoteByString(string obj)
        {
            if (string.IsNullOrWhiteSpace(obj))
            {
                return default;
            }

            return Utf8StringSerialize.DeSerializeObjectByString<ExplanatoryNote>(obj);
        }

        public static string GetSerializeExplanatoryNote(this ExplanatoryNote explanatoryNote)
        {
            return Utf8StringSerialize.SerializeObject(explanatoryNote);
        }
    }
}

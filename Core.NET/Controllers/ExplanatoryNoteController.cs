using Core.NET.AutoModels.ExplanatoryNoteObj;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Core.NET.Controllers
{
    public static class ExplanatoryNoteController
    {
        public static async Task<ExplanatoryNoteXPO> CreateExplanatoryNoteByPath(string path, string name = default)
        {
            var explanatoryNote = GetExplanatoryNoteByPath(path);
            if (explanatoryNote is null)
            {
                return default;
            }

            var fileGuid = GetGuidByFile(path);

            var xpoObj = new ExplanatoryNoteXPO(SessionController.GetSession())
            {
                Name = name
            };

            if (fileGuid != null)
            {
                xpoObj.SetGuid(fileGuid.Value);
            }
            else
            {
                xpoObj.SetGuid(Guid.NewGuid());
            }

            xpoObj.SetObj(explanatoryNote.SerializeObject());
            await SessionController.GetSession().SaveAsync(xpoObj);
            return xpoObj;
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

        public static ExplanatoryNote GetExplanatoryNoteByPath(string path)
        {
            if (FileController.CheckFile(path))
            {
                return Utf8StringSerialize.DeSerializeObject<ExplanatoryNote>(path);
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

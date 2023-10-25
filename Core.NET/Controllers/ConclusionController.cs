using Core.NET.AutoModels;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Core.NET.Controllers
{
    public static class ConclusionController
    {
        public static async Task<ConclusionXPO> CreateConclusionByPath(string path, string name = default)
        {
            var conclusion = GetConclusionByPath(path);
            if (conclusion is null)
            {
                return default;
            }

            var fileGuid = GetGuidByFile(path);

            var xpoObj = new ConclusionXPO(SessionController.GetSession())
            {
                Name = name
            };

            if (Guid.TryParse(conclusion.ConclusionGUID, out Guid resultGuid))
            {
                xpoObj.SetGuid(resultGuid);
            }
            else if (fileGuid != null)
            {
                xpoObj.SetGuid(fileGuid.Value);
            }

            xpoObj.SetObj(conclusion.SerializeObject());
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

        public static Conclusion GetConclusionByPath(string path)
        {
            if (FileController.CheckFile(path))
            {
                return Serializer.DeserializeObject<Conclusion>(path);
            }

            return default;
        }

        public static Conclusion GetConclusionByString(string obj)
        {
            if (string.IsNullOrWhiteSpace(obj))
            {
                return default;
            }

            return Serializer.DeserializeObjectByString<Conclusion>(obj);
        }

        public static string GetSerializerConclusion(this Conclusion conclusion)
        {
            return Serializer.SerializeObject(conclusion);
        }
    }
}

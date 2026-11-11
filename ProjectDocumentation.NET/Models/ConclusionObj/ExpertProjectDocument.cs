using Core.NET.AutoModels.ConclusionObj;
using Core.NET.Controllers;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using System.Collections.Generic;
using System.Linq;

namespace ProjectDocumentation.NET.Models.ConclusionObj
{
    public static class ExpertProjectDocumentController
    {
        public static IEnumerable<ExpertProjectDocument> GetExpertProjectDocuments(ICollection<TExpertProjectDocuments> collection)
        {
            foreach (var obj in collection)
            {
                var result = new ExpertProjectDocument(obj);
                result.SetEnumObj();
                yield return result;
            }
        }
    }

    public class ExpertProjectDocument
    {
        public ExpertProjectDocument(TExpertProjectDocuments tExpertProjectDocuments)
        {
            ExpertProjectDocuments = tExpertProjectDocuments;
        }

        public void SetEnumObj()
        {
            var session = SessionController.GetSession();

            if (ExpertProjectDocuments != null)
            {
                var expertType = new XPQuery<GeneralizedDictionaryXPO>(session)?
                .FirstOrDefault(f => f.GeneralizedDictionary == GeneralizedDictionary.ExpertType
                    && f.Value == ExpertProjectDocuments.ExpertType.GetEnumXmlAttributeValue().ToString());
                ExpertType = expertType?.Name;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public TExpertProjectDocuments ExpertProjectDocuments { get; private set; }
        public string ExpertType { get; private set; }
    }
}
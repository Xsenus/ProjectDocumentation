using Core.NET.AutoModels;
using Core.NET.Controllers;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using System.Collections.Generic;

namespace ProjectDocumentation.NET.Models.ConclusionObj
{
    public static class ExpertProjectDocumentController
    {
        public static IEnumerable<ExpertProjectDocument> GetExpertProjectDocuments(ICollection<tExpertProjectDocuments> collection)
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
        public ExpertProjectDocument(tExpertProjectDocuments tExpertProjectDocuments)
        {
            this.ExpertProjectDocuments = tExpertProjectDocuments;
        }

        public tExpertProjectDocuments ExpertProjectDocuments { get; private set; }

        public string ExpertType { get; private set; }

        public async void SetEnumObj()
        {
            var session = SessionController.GetSession();

            if (ExpertProjectDocuments != null)
            {
                var expertType = await new XPQuery<GeneralizedDictionaryXPO>(session)?
                .FirstOrDefaultAsync(f => f.GeneralizedDictionary == GeneralizedDictionary.ExpertType
                    && f.Value == ExpertProjectDocuments.ExpertType.GetEnumXmlAttributeValue().ToString());
                ExpertType = expertType?.Name;;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}

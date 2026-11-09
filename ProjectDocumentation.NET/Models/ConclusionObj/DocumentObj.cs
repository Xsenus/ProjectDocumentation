using Core.NET.AutoModels.ConclusionObj;
using Core.NET.Controllers;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectDocumentation.NET.Models.ConclusionObj
{
    public static class DocumentObjController
    {
        public static IEnumerable<DocumentObj> GetDocumentObjS(ICollection<TDocument> collection)
        {
            foreach (var obj in collection)
            {
                var result = new DocumentObj(obj);
                result.SetEnumObj();
                yield return result;
            }
        }
    }

    public class DocumentObj
    {
        public DocumentObj(TDocument TDocument)
        {
            this.Document = TDocument;
        }

        public void SetEnumObj()
        {
            var session = SessionController.GetSession();

            if (Document != null)
            {
                var docType = new XPQuery<GeneralizedDictionaryXPO>(session)?
                .FirstOrDefault(f => f.GeneralizedDictionary == GeneralizedDictionary.DocumentType
                    && f.Value == Document.DocType.GetEnumXmlAttributeValue().ToString());
                DocTypeName = docType?.Name;

                DocName = Document.DocName;
                DocNumber = Document.DocNumber;
                if (Document.DocDate != DateTime.MinValue)
                {
                    DocDate = Document.DocDate.ToShortDateString();
                }
                DocChanges = Document.DocChanges;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public string DocChanges { get; private set; }
        public string DocDate { get; private set; }
        public string DocName { get; private set; }
        public string DocNumber { get; private set; }
        public string DocTypeName { get; private set; }
        public TDocument Document { get; private set; }
    }
}

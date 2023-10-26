using Core.NET.AutoModels;
using Core.NET.Controllers;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;

namespace ProjectDocumentation.NET.Models.ConclusionObj
{
    public static class DocumentObjController
    {
        public static IEnumerable<DocumentObj> GetDocumentObjs(ICollection<tDocument> collection)
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
        public DocumentObj(tDocument tDocument)
        {
            this.Document = tDocument;
        }

        public tDocument Document { get; private set; }

        public string DocType { get; private set; }
        public string DocName { get; private set; }
        public string DocNumber { get; private set; }
        public string DocDate { get; private set; }

        public async void SetEnumObj()
        {
            var session = SessionController.GetSession();

            if (Document != null)
            {
                var docType = await new XPQuery<GeneralizedDictionaryXPO>(session)?
                .FirstOrDefaultAsync(f => f.GeneralizedDictionary == GeneralizedDictionary.DocumentType
                    && f.Value == Document.DocType.GetEnumXmlAttributeValue().ToString());
                DocType = docType?.Name;

                DocName = Document.DocName;
                DocNumber = Document.DocNumber;
                if (Document.DocDate != DateTime.MinValue)
                {
                    DocDate = Document.DocDate.ToShortDateString();
                }
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}

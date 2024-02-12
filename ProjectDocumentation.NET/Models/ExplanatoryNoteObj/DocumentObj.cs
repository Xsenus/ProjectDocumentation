using Core.NET.AutoModels.ExplanatoryNoteObj;
using Core.NET.Controllers;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;

namespace ProjectDocumentation.NET.Models.ExplanatoryNoteObj
{
    public static class DocumentObjEnController
    {
        public static IEnumerable<DocumentObjEn> GetDocumentObjS(ICollection<tDocument> collection)
        {
            foreach (var obj in collection)
            {
                var result = new DocumentObjEn(obj);
                result.SetEnumObj();
                yield return result;
            }
        }
    }

    public class DocumentObjEn
    {
        public DocumentObjEn(tDocument tDocument)
        {
            this.Document = tDocument;
        }

        public tDocument Document { get; private set; }

        public string DocTypeName { get; private set; }
        public string DocName { get; private set; }
        public string DocNumber { get; private set; }
        public string DocDate { get; private set; }
        public string DocChanges { get; private set; }

        public async void SetEnumObj()
        {
            var session = SessionController.GetSession();

            if (Document != null)
            {
                var docType = await new XPQuery<GeneralizedDictionaryXPO>(session)?
                .FirstOrDefaultAsync(f => f.GeneralizedDictionary == GeneralizedDictionary.DocumentType
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
    }
}

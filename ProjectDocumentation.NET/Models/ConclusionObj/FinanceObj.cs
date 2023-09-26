using Core.NET.AutoModels;
using Core.NET.Controllers;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using System.Collections.Generic;

namespace ProjectDocumentation.NET.Models.ConclusionObj
{
    public static class FinanceObjController
    {
        public static IEnumerable<FinanceObj> GetFinanceObj(ICollection<tFinance> collection)
        {
            foreach (var obj in collection)
            {
                var result = new FinanceObj(obj);
                result.SetEnumObj();
                yield return result;
            }
        }
    }

    public class FinanceObj
    {
        public FinanceObj(tFinance tFinance)
        {
            this.tFinance = tFinance;
        }

        public tFinance tFinance { get; private set; }

        public string FinanceType { get; set; }
        public string BudgetType { get; set; }
        public string FinanceSize { get; set; }

        public async void SetEnumObj()
        {
            var session = SessionController.GetSession();

            if (tFinance != null)
            {
                var financeType = await new XPQuery<GeneralizedDictionaryXPO>(session)?
                .FirstOrDefaultAsync(f => f.GeneralizedDictionary == GeneralizedDictionary.FinanceType
                    && f.Value == tFinance.FinanceType.GetEnumXmlAttributeValue().ToString());
                FinanceType = financeType?.Name;

                if (tFinance.FinanceType == tFinanceType.Item1)
                {
                    var budgetType = await new XPQuery<GeneralizedDictionaryXPO>(session)?
                        .FirstOrDefaultAsync(f => f.GeneralizedDictionary == GeneralizedDictionary.BudgetType
                            && f.Value == tFinance.BudgetType.GetEnumXmlAttributeValue().ToString());
                    BudgetType = budgetType?.Name;
                }
                else
                {
                    BudgetType = "Нет данных";
                }

                FinanceSize = tFinance.FinanceSize.ToString("n5");
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}

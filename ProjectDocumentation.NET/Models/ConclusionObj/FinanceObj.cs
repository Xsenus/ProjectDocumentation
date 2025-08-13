using Core.NET.AutoModels.ConclusionObj;
using Core.NET.Controllers;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using System.Collections.Generic;

namespace ProjectDocumentation.NET.Models.ConclusionObj
{
    public static class FinanceObjController
    {
        public static IEnumerable<FinanceObj> GetFinanceObj(ICollection<TFinance> collection)
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
        public FinanceObj(TFinance tFinance)
        {
            TFinance = tFinance;
        }

        public async void SetEnumObj()
        {
            var session = SessionController.GetSession();

            if (TFinance != null)
            {
                var financeType = await new XPQuery<GeneralizedDictionaryXPO>(session)?
                .FirstOrDefaultAsync(f => f.GeneralizedDictionary == GeneralizedDictionary.FinanceType
                    && f.Value == TFinance.FinanceType.GetEnumXmlAttributeValue().ToString());
                FinanceType = financeType?.Name;

                if (TFinance.FinanceType == TFinanceType.Item1)
                {
                    var budgetType = await new XPQuery<GeneralizedDictionaryXPO>(session)?
                        .FirstOrDefaultAsync(f => f.GeneralizedDictionary == GeneralizedDictionary.BudgetType
                            && f.Value == TFinance.BudgetType.GetEnumXmlAttributeValue().ToString());
                    BudgetType = budgetType?.Name;
                }
                else
                {
                    BudgetType = "Нет данных";
                }

                FinanceSize = TFinance.FinanceSize.ToString("n5");
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public string BudgetType { get; set; }
        public string FinanceSize { get; set; }
        public string FinanceType { get; set; }
        public TFinance TFinance { get; private set; }
    }
}
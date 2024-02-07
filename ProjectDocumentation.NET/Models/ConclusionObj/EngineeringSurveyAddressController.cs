using Core.NET.AutoModels.ConclusionObj;
using Core.NET.Controllers;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using System.Collections.Generic;

namespace ProjectDocumentation.NET.Models.ConclusionObj
{
    public static class EngineeringSurveyAddressController
    {
        public static IEnumerable<EngineeringSurveyAddress> GetEngineeringSurveyAddress(ICollection<tEngineeringSurveyAddress> collection)
        {
            foreach (var obj in collection)
            {
                var result = new EngineeringSurveyAddress(obj);
                result.SetEnumObj();
                yield return result;
            }
        }
    }

    public class EngineeringSurveyAddress
    {
        public EngineeringSurveyAddress(tEngineeringSurveyAddress tEngineeringSurveyAddress)
        {
            EngineeringSurveyAddressObj = tEngineeringSurveyAddress;
            EngineeringSurveyDistrict = EngineeringSurveyAddressObj.EngineeringSurveyDistrict;
        }

        public tEngineeringSurveyAddress EngineeringSurveyAddressObj { get; private set; }

        public string EngineeringSurveyRegion { get; private set; }

        [Size(1000)]
        public string EngineeringSurveyDistrict { get; private set; }

        public async void SetEnumObj()
        {
            var session = SessionController.GetSession();

            if (EngineeringSurveyAddressObj != null)
            {
                var engineeringSurveyType = await new XPQuery<GeneralizedDictionaryXPO>(session)?
                 .FirstOrDefaultAsync(f => f.GeneralizedDictionary == GeneralizedDictionary.Region
                     && f.Value == EngineeringSurveyAddressObj.EngineeringSurveyRegion.GetEnumXmlAttributeValue().ToString());
                EngineeringSurveyRegion = engineeringSurveyType?.Name;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
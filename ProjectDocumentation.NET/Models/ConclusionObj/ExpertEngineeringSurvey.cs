using Core.NET.AutoModels.ConclusionObj;
using Core.NET.Controllers;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using System.Collections.Generic;

namespace ProjectDocumentation.NET.Models.ConclusionObj
{
    public static class ExpertEngineeringSurveysController
    {
        public static IEnumerable<ExpertEngineeringSurvey> GetExpertEngineeringSurveys(ICollection<tExpertEngineeringSurveys> collection)
        {
            foreach (var obj in collection)
            {
                var result = new ExpertEngineeringSurvey(obj);
                result.SetEnumObj();
                yield return result;
            }
        }
    }

    public class ExpertEngineeringSurvey
    {
        public ExpertEngineeringSurvey(tExpertEngineeringSurveys tExpertEngineeringSurveys)
        {
            ExpertEngineeringSurveys = tExpertEngineeringSurveys;
            EngineeringSurveyType = tExpertEngineeringSurveys.EngineeringSurveyConditions;
            EngineeringSurveyProgramNote = tExpertEngineeringSurveys.EngineeringSurveyProgramNote;
            EngineeringSurveyMethods = tExpertEngineeringSurveys.EngineeringSurveyMethods;
            EngineeringSurveyChanges = tExpertEngineeringSurveys.EngineeringSurveyChanges;
        }

        public tExpertEngineeringSurveys ExpertEngineeringSurveys { get; private set; }
        public string EngineeringSurveyType { get; private set; }
        public string EngineeringSurveyConditions { get; private set; }
        public string EngineeringSurveyProgramNote { get; private set; }
        public string EngineeringSurveyMethods { get; private set; }
        public string EngineeringSurveyChanges { get; private set; }

        public async void SetEnumObj()
        {
            var session = SessionController.GetSession();

            if (ExpertEngineeringSurveys != null)
            {
                var engineeringSurveyType = await new XPQuery<GeneralizedDictionaryXPO>(session)?
                 .FirstOrDefaultAsync(f => f.GeneralizedDictionary == GeneralizedDictionary.EngineeringSurveyType
                     && f.Value == ExpertEngineeringSurveys.EngineeringSurveyType.GetEnumXmlAttributeValue().ToString());
                EngineeringSurveyType = engineeringSurveyType?.Name;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
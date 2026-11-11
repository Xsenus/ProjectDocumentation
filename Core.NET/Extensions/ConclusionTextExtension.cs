using Core.NET.AutoModels.ConclusionObj;

namespace Core.NET.Extensions
{
    /// <summary>
    /// Формулировки предпросмотра заключения, зависящие от формы и стадии экспертизы.
    /// Раньше текст «повторной негосударственной экспертизы» был зашит независимо от данных.
    /// </summary>
    public static class ConclusionTextExtension
    {
        /// <summary>«повторной » для повторной экспертизы, иначе пустая строка (для оборотов «… экспертизы»).</summary>
        public static string GetStageAdjective(this TExaminationStage stage)
        {
            return stage.GetEnumXmlAttributeValue()?.ToString() == "2" ? "повторной " : string.Empty;
        }

        /// <summary>«государственной » / «негосударственной ».</summary>
        public static string GetFormAdjective(this TExaminationForm form)
        {
            switch (form.GetEnumXmlAttributeValue()?.ToString())
            {
                case "1":
                    return "государственной ";
                case "2":
                    return "негосударственной ";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Заголовок, например «Положительное заключение повторной негосударственной экспертизы».
        /// </summary>
        public static string GetConclusionTitle(this TExaminationObject examinationObject)
        {
            if (examinationObject is null)
            {
                return "Заключение экспертизы";
            }

            var result = examinationObject.ExaminationResult.GetEnumXmlAttributeValue()?.ToString() == "2"
                ? "Отрицательное"
                : "Положительное";

            var title = $"{result} заключение {examinationObject.ExaminationStage.GetStageAdjective()}{examinationObject.ExaminationForm.GetFormAdjective()}экспертизы";

            if (examinationObject.ExaminationStage.GetEnumXmlAttributeValue()?.ToString() == "3")
            {
                title += " по результатам экспертного сопровождения";
            }

            return title;
        }
    }
}

using Core.NET.Models.XPO;
using DevExpress.Xpo;
using System.Linq;
using System.Threading.Tasks;

namespace Core.NET.Controllers
{
    public static class DataBaseController
    {
        private static Session _session => SessionController.GetSession();

        public static async Task FillingDataBaseAsync()
        {
            await GeneralizedDictionaryXPOAsync();
            await FillingDocumentTypesXPOAsync();
        }

        private static async Task GeneralizedDictionaryXPOAsync()
        {
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(_session).ToListAsync();

            if (collection.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationResult).Count() == 0)
            {
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ExaminationResult,
                    Value = "1",
                    Name = "Положительный"
                });

                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ExaminationResult,
                    Value = "2",
                    Name = "Отрицательный"
                });
            }

            if (collection.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationForm).Count() == 0)
            {
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ExaminationForm,
                    Value = "1",
                    Name = "Государственная"
                });

                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ExaminationForm,
                    Value = "2",
                    Name = "Негосударственная"
                });
            }

            if (collection.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationObjectType).Count() == 0)
            {
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ExaminationObjectType,
                    Value = "1",
                    Name = "Результаты инженерных изысканий"
                });

                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ExaminationObjectType,
                    Value = "2",
                    Name = "Проектная документация"
                });

                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ExaminationObjectType,
                    Value = "3",
                    Name = "Проектная документация и результаты инженерных изысканий"
                });
            }

            if (collection.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationStage).Count() == 0)
            {
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ExaminationStage,
                    Value = "1",
                    Name = "Первичная"
                });

                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ExaminationStage,
                    Value = "2",
                    Name = "Повторная"
                });

                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ExaminationStage,
                    Value = "3",
                    Name = "По результатам экспертного сопровождения"
                });
            }

            if (collection.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ConstructionType).Count() == 0)
            {
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ConstructionType,
                    Value = "1",
                    Name = "Строительство"
                });

                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ConstructionType,
                    Value = "2",
                    Name = "Реконструкция"
                });

                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ConstructionType,
                    Value = "3",
                    Name = "Капитальный ремонт"
                });

                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ConstructionType,
                    Value = "4",
                    Name = "Снос"
                });

                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ConstructionType,
                    Value = "5",
                    Name = "Сохранение объекта культурного наследия"
                });
            }

            if (collection.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationType).Count() == 0)
            {
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ExaminationType,
                    Value = "1",
                    Name = "Оценка соответствия результатов инженерных изысканий требованиям технических регламентов (абзац 1 пункта 5 статьи 49 Градостроительного кодекса Российской Федерации)"
                });

                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ExaminationType,
                    Value = "2",
                    Name = "Оценка соответствия проектной документации установленным требованиям (подпункт 1 пункт 5 статьи 49 Градостроительного кодекса Российской Федерации)"
                });

                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ExaminationType,
                    Value = "3",
                    Name = "Проверка достоверности определения сметной стоимости (подпункт 2 пункт 5 статьи 49 Градостроительного кодекса Российской Федерации)"
                });
            }

            if (collection.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ObjectType).Count() == 0)
            {
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ObjectType,
                    Value = "1",
                    Name = "Объект производственного назначения"
                });

                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ObjectType,
                    Value = "2",
                    Name = "Объект непроизводственного назначения"
                });

                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.ObjectType,
                    Value = "3",
                    Name = "Линейный объект"
                });
            }

            if (collection.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.FinanceType).Count() == 0)
            {
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.FinanceType,
                    Value = "1",
                    Name = "Бюджетные средства"
                });

                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.FinanceType,
                    Value = "2",
                    Name = "Средства юридических лиц, перечисленных в части 2 статьи 8.3 Градостроительного кодекса Российской Федерации"
                });

                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.FinanceType,
                    Value = "3",
                    Name = "Средства, не входящие в перечень, указанный в части 2 статьи 8.3 Градостроительного кодекса Российской Федерации"
                });
            }

            if (collection.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.BudgetType).Count() == 0)
            {
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.BudgetType,
                    Value = "1",
                    Name = "Федеральный бюджет"
                });

                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.BudgetType,
                    Value = "2",
                    Name = "Бюджет субъекта Российской Федерации"
                });

                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.BudgetType,
                    Value = "3",
                    Name = "Местный бюджет"
                });

                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.BudgetType,
                    Value = "4",
                    Name = "Бюджет государственного внебюджетного фонда Российской Федерации"
                });

                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session)
                {
                    GeneralizedDictionary = GeneralizedDictionary.BudgetType,
                    Value = "5",
                    Name = "Бюджет территориального государственного внебюджетного фонда"
                });
            }

            if (collection.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ClimateDistrict).Count() == 0)
            {
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "Сведения не представлены", Name = "Сведения не представлены", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "I", Name = "климатический район I", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "IА", Name = "климатический подрайон IА", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "IБ", Name = "климатический подрайон IБ", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "IВ", Name = "климатический подрайон IВ", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "IГ", Name = "климатический подрайон IГ", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "IД", Name = "климатический подрайон IД", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "II", Name = "климатический район II", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "IIА", Name = "климатический подрайон IIА", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "IIБ", Name = "климатический подрайон IIБ", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "IIВ", Name = "климатический подрайон IIВ", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "IIГ", Name = "климатический подрайон IIГ", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "III", Name = "климатический район III", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "IIIА", Name = "климатический подрайон IIIА", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "IIIБ", Name = "климатический подрайон IIIБ", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "IIIВ", Name = "климатический подрайон IIIВ", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "IV", Name = "климатический район IV", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "IVА", Name = "климатический подрайон IVА", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "IVБ", Name = "климатический подрайон IVБ", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "IVВ", Name = "климатический подрайон IVВ", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "IVГ", Name = "климатический подрайон IVГ", GeneralizedDictionary = GeneralizedDictionary.ClimateDistrict });
            }

            if (collection.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.SeismicActivity).Count() == 0)
            {
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "Сведения не представлены", Name = "Сведения не представлены", GeneralizedDictionary = GeneralizedDictionary.SeismicActivity });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5", Name = "5 и менее баллов", GeneralizedDictionary = GeneralizedDictionary.SeismicActivity });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "6", Name = "6", GeneralizedDictionary = GeneralizedDictionary.SeismicActivity });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "7", Name = "7", GeneralizedDictionary = GeneralizedDictionary.SeismicActivity });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "8", Name = "8", GeneralizedDictionary = GeneralizedDictionary.SeismicActivity });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "9", Name = "9", GeneralizedDictionary = GeneralizedDictionary.SeismicActivity });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "10", Name = "Более 9 баллов", GeneralizedDictionary = GeneralizedDictionary.SeismicActivity });
            }

            if (collection.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.WindDistrict).Count() == 0)
            {
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "Iа", Name = "Ветровой район Iа", GeneralizedDictionary = GeneralizedDictionary.WindDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "I", Name = "Ветровой район I", GeneralizedDictionary = GeneralizedDictionary.WindDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "II", Name = "Ветровой район II", GeneralizedDictionary = GeneralizedDictionary.WindDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "III", Name = "Ветровой район III", GeneralizedDictionary = GeneralizedDictionary.WindDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "IV", Name = "Ветровой район IV", GeneralizedDictionary = GeneralizedDictionary.WindDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "V", Name = "Ветровой район V", GeneralizedDictionary = GeneralizedDictionary.WindDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "VI", Name = "Ветровой район VI", GeneralizedDictionary = GeneralizedDictionary.WindDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "VII", Name = "Ветровой район VII", GeneralizedDictionary = GeneralizedDictionary.WindDistrict });
            }

            if (collection.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.GeologicalConditions).Count() == 0)
            {
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "I", Name = "I (простые)", GeneralizedDictionary = GeneralizedDictionary.GeologicalConditions });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "II", Name = "II (средней сложности)", GeneralizedDictionary = GeneralizedDictionary.GeologicalConditions });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "III", Name = "III (сложные)", GeneralizedDictionary = GeneralizedDictionary.GeologicalConditions });
            }

            if (collection.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.SnowDistrict).Count() == 0)
            {
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "Сведения не представлены", Name = "Сведения не представлены", GeneralizedDictionary = GeneralizedDictionary.SnowDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "I", Name = "Снеговой район I", GeneralizedDictionary = GeneralizedDictionary.SnowDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "II", Name = "Снеговой район II", GeneralizedDictionary = GeneralizedDictionary.SnowDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "III", Name = "Снеговой район III", GeneralizedDictionary = GeneralizedDictionary.SnowDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "IV", Name = "Снеговой район IV", GeneralizedDictionary = GeneralizedDictionary.SnowDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "V", Name = "Снеговой район V", GeneralizedDictionary = GeneralizedDictionary.SnowDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "VI", Name = "Снеговой район VI", GeneralizedDictionary = GeneralizedDictionary.SnowDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "VII", Name = "Снеговой район VII", GeneralizedDictionary = GeneralizedDictionary.SnowDistrict });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "VIII", Name = "Снеговой район VIII", GeneralizedDictionary = GeneralizedDictionary.SnowDistrict });
            }

            if (collection.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.Region).Count() == 0)
            {
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "00", Name = "Объект расположен за пределами территории Российской Федерации", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "01", Name = "Республика Адыгея (Адыгея)", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "02", Name = "Республика Башкортостан", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "03", Name = "Республика Бурятия", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "04", Name = "Республика Алтай", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "05", Name = "Республика Дагестан", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "06", Name = "Республика Ингушетия", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "07", Name = "Кабардино-Балкарская Республика", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "08", Name = "Республика Калмыкия", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "09", Name = "Карачаево-Черкесская Республика", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "10", Name = "Республика Карелия", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "11", Name = "Республика Коми", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "12", Name = "Республика Марий Эл", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "13", Name = "Республика Мордовия", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "14", Name = "Республика Саха (Якутия)", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "15", Name = "Республика Северная Осетия-Алания", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "16", Name = "Республика Татарстан (Татарстан)", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "17", Name = "Республика Тыва", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "18", Name = "Удмуртская Республика", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "19", Name = "Республика Хакасия", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "20", Name = "Чеченская Республика", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "21", Name = "Чувашская Республика-Чувашия", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "22", Name = "Алтайский край", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "23", Name = "Краснодарский край", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "24", Name = "Красноярский край", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "25", Name = "Приморский край", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "26", Name = "Ставропольский край", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "27", Name = "Хабаровский край", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "28", Name = "Амурская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "29", Name = "Архангельская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "30", Name = "Астраханская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "31", Name = "Белгородская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "32", Name = "Брянская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "33", Name = "Владимирская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "34", Name = "Волгоградская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "35", Name = "Вологодская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "36", Name = "Воронежская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "37", Name = "Ивановская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "38", Name = "Иркутская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "39", Name = "Калининградская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "40", Name = "Калужская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "41", Name = "Камчатский край", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "42", Name = "Кемеровская область - Кузбасс", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "43", Name = "Кировская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "44", Name = "Костромская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "45", Name = "Курганская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "46", Name = "Курская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "47", Name = "Ленинградская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "48", Name = "Липецкая область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "49", Name = "Магаданская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "50", Name = "Московская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "51", Name = "Мурманская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "52", Name = "Нижегородская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "53", Name = "Новгородская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "54", Name = "Новосибирская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "55", Name = "Омская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "56", Name = "Оренбургская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "57", Name = "Орловская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "58", Name = "Пензенская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "59", Name = "Пермский край", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "60", Name = "Псковская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "61", Name = "Ростовская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "62", Name = "Рязанская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "63", Name = "Самарская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "64", Name = "Саратовская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "65", Name = "Сахалинская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "66", Name = "Свердловская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "67", Name = "Смоленская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "68", Name = "Тамбовская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "69", Name = "Тверская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "70", Name = "Томская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "71", Name = "Тульская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "72", Name = "Тюменская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "73", Name = "Ульяновская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "74", Name = "Челябинская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "75", Name = "Забайкальский край", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "76", Name = "Ярославская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "77", Name = "Москва", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "78", Name = "Санкт-Петербург", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "79", Name = "Еврейская автономная область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "83", Name = "Ненецкий автономный округ", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "86", Name = "Ханты-Мансийский автономный округ - Югра", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "87", Name = "Чукотский автономный округ", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "89", Name = "Ямало-Ненецкий автономный округ", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "91", Name = "Республика Крым", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "92", Name = "Севастополь", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "80", Name = "Донецкая Народная Республика", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "81", Name = "Луганская Народная Республика", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "84", Name = "Херсонская область", GeneralizedDictionary = GeneralizedDictionary.Region });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "85", Name = "Запорожская область", GeneralizedDictionary = GeneralizedDictionary.Region });
            }

            if (collection.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExpertType).Count() == 0)
            {
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "1.1.", Name = "1.1. Инженерно-геодезические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "1.2.", Name = "1.2. Инженерно-геологические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "1.3.", Name = "1.3. Инженерно-гидрометеорологические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "1.4.", Name = "1.4. Инженерно-экологические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "1.5.", Name = "1.5. Инженерно-геотехнические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "2.1.", Name = "2.1. Объемно-планировочные, архитектурные и конструктивные решения, планировочная организация земельного участка, организация строительства", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "2.1.1.", Name = "2.1.1. Схемы планировочной организации земельных участков", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "2.1.2.", Name = "2.1.2. Объемно-планировочные и архитектурные решения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "2.1.3.", Name = "2.1.3. Конструктивные решения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "2.1.4.", Name = "2.1.4. Организация строительства", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "2.2.", Name = "2.2. Теплогазоснабжение, водоснабжение, водоотведение, канализация, вентиляция и кондиционирование", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "2.2.1.", Name = "2.2.1. Водоснабжение, водоотведение и канализация", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "2.2.2.", Name = "2.2.2. Теплоснабжение, вентиляция и кондиционирование", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "2.2.3.", Name = "2.2.3. Системы газоснабжения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "2.3.", Name = "2.3. Электроснабжение, связь, сигнализация,  системы автоматизации", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "2.3.1.", Name = "2.3.1. Электроснабжение и электропотребление", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "2.3.2.", Name = "2.3.2. Системы автоматизации, связи и сигнализации", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "2.4.", Name = "2.4. Охрана окружающей среды, санитарно-эпидемиологическая безопасность", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "2.4.1.", Name = "2.4.1. Охрана окружающей среды", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "2.4.2.", Name = "2.4.2. Санитарно-эпидемиологическая безопасность", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "2.5.", Name = "2.5. Пожарная безопасность", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "3.1.", Name = "3.1. Организация экспертизы проектной документации и (или) результатов инженерных изысканий ", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "4.1.", Name = "4.1. Мосты и трубы", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "4.2.", Name = "4.2. Автомобильные дороги", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "4.3.", Name = "4.3. Объекты топливно-энергетического комплекса", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "4.4.", Name = "4.4. Объекты информатизации и связи", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "4.5.", Name = "4.5. Инженерно-технические мероприятия ГО и ЧС", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.1.1.", Name = "5.1.1. Инженерно-геодезические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.1.2.", Name = "5.1.2. Инженерно-геологические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.1.3.", Name = "5.1.3. Инженерно-гидрометеорологические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.1.4.", Name = "5.1.4. Инженерно-экологические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.1.5.", Name = "5.1.5. Инженерно-геотехнические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.1.6.", Name = "5.1.6. Обследования состояния грунтов оснований зданий и сооружений", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.1.7.", Name = "5.1.7. Обследование состояния строительных конструкций зданий и сооружений", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.1.", Name = "5.2.1. Схемы планировочной организации земельных участков", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.2.", Name = "5.2.2. Объемно-планировочные решения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.3.", Name = "5.2.3. Конструктивные решения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.4.", Name = "5.2.4. Сети инженерно-технического обеспечения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.4.1.", Name = "5.2.4.1. Электроснабжение", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.4.2.", Name = "5.2.4.2. Водоснабжение и водоотведение", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.4.3.", Name = "5.2.4.3. Отопление, вентиляция и кондиционирование воздуха, тепловые сети", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.4.4.", Name = "5.2.4.4. Системы связи и сигнализации", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.4.5.", Name = "5.2.4.5. Системы газоснабжения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.4.6.", Name = "5.2.4.6. Системы автоматизации", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.4.7.", Name = "5.2.4.7. Тепловые сети", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.5.", Name = "5.2.5. Охрана окружающей среды", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.6.", Name = "5.2.6. Санитарно-эпидемиологическая безопасность", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.7.", Name = "5.2.7. Пожарная безопасность", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.8.", Name = "5.2.8. Инженерно-технические мероприятия ГО и ЧС", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.9.", Name = "5.2.9. Промышленная безопасность опасных производственных объектов", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.10.", Name = "5.2.10. Ядерная и радиационная безопасность", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.11.", Name = "5.2.11. Организация строительства", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.12.", Name = "5.2.12. Объекты транспортного комплекса", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.12.1.", Name = "5.2.12.1. Воздушный транспорт", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.12.2.", Name = "5.2.12.2. Железнодорожный транспорт", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.12.3.", Name = "5.2.12.3. Мосты и трубы", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.12.4.", Name = "5.2.12.4. Тоннели и метрополитены", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.12.5.", Name = "5.2.12.5. Автомобильные дороги", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.12.6.", Name = "5.2.12.6. Морской транспорт", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.13.", Name = "5.2.13. Объекты топливно-энергетического комплекса", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.13.1.", Name = "5.2.13.1. Объекты химических, нефтехимических и нефтегазоперерабатывающих, взрыво- и пожароопасных производств", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.13.2.", Name = "5.2.13.2. Линии электропередачи и иные объекты электросетевого хозяйства", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.13.3.", Name = "5.2.13.3. Объекты обустройства нефтяных и газовых месторождений", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.13.4.", Name = "5.2.13.4. Строительство скважин", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.13.5.", Name = "5.2.13.5. Строительство магистральных и промысловых трубопроводов", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.13.6.", Name = "5.2.13.6. Объекты использования атомной энергии", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.13.7.", Name = "5.2.13.7. Тепловые электростанции", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.14.", Name = "5.2.14. Объекты производственного назначения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.14.1.", Name = "5.2.14.1. Объекты космической инфраструктуры", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.14.2.", Name = "5.2.14.2. Объекты оборонной промышленности", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.14.3.", Name = "5.2.14.3. Объекты металлургической промышленности", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.14.4.", Name = "5.2.14.4. Объекты горнодобывающей и горно-перерабатывающей промышленности", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.15.", Name = "5.2.15. Объекты информатизации и связи", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.2.16.", Name = "5.2.16. Гидротехнические сооружения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.3.1.", Name = "5.3.1. Организация государственной экспертизы проектной документации и (или) результатов инженерных изысканий", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "1.", Name = "1. Инженерно-геодезические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "2.", Name = "2. Инженерно-геологические изыскания и инженерно-геотехнические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "3.", Name = "3. Инженерно-гидрометеорологические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "4.", Name = "4. Инженерно-экологические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5.", Name = "5. Схемы планировочной организации земельных участков", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "6.", Name = "6. Объемно-планировочные и архитектурные решения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "7.", Name = "7. Конструктивные решения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "8.", Name = "8. Охрана окружающей среды", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "9.", Name = "9. Санитарно-эпидемиологическая безопасность", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "10.", Name = "10. Пожарная безопасность", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "11.", Name = "11. Инженерно-технические мероприятия ГО и ЧС", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "12.", Name = "12. Организация строительства", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "13.", Name = "13. Системы водоснабжения и водоотведения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "14.", Name = "14. Системы отопления, вентиляции, кондиционирования воздуха и холодоснабжения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "15.", Name = "15. Системы газоснабжения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "16.", Name = "16. Системы электроснабжения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "17.", Name = "17. Системы связи и сигнализации", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "17.1.", Name = "17.1. Ценообразование и сметное нормирование", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "18.", Name = "18. Мосты и трубы", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "19.", Name = "19. Автомобильные дороги", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "20.", Name = "20. Объекты топливно-энергетического комплекса", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "21.", Name = "21. Объекты информатизации и связи", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "22.", Name = "22. Инженерно-геодезические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "23.", Name = "23. Инженерно-геологические изыскания и инженерно-геотехнические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "24.", Name = "24. Инженерно-гидрометеорологические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "25.", Name = "25. Инженерно-экологические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "26.", Name = "26. Схемы планировочной организации земельных участков", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "27.", Name = "27. Объемно-планировочные решения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "28.", Name = "28. Конструктивные решения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "29.", Name = "29. Охрана окружающей среды", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "30.", Name = "30. Санитарно-эпидемиологическая безопасность", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "31.", Name = "31. Пожарная безопасность", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "32.", Name = "32. Инженерно-технические мероприятия ГО и ЧС", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "33.", Name = "33. Промышленная безопасность опасных производственных объектов", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "34.", Name = "34. Ядерная и радиационная безопасность", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "35.", Name = "35. Организация строительства", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "35.1.", Name = "35.1. Ценообразование и сметное нормирование", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "36.", Name = "36. Системы электроснабжения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "37.", Name = "37. Системы водоснабжения и водоотведения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "38.", Name = "38. Системы отопления, вентиляции, кондиционирования воздуха и холодоснабжения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "39.", Name = "39. Системы связи и сигнализации", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "40.", Name = "40. Системы газоснабжения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "41.", Name = "41. Системы автоматизации", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "42.", Name = "42. Системы теплоснабжения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "43.", Name = "43. Объекты авиационной инфраструктуры", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "44.", Name = "44. Объекты инфраструктуры железнодорожного транспорта", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "45.", Name = "45. Мосты и трубы", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "46.", Name = "46. Тоннели и метрополитены", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "47.", Name = "47. Автомобильные дороги", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "48.", Name = "48. Объекты морского и речного транспорта", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "49.", Name = "49. Объекты химических, нефтехимических и нефтегазоперерабатывающих, взрыво- и пожароопасных производств", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "50.", Name = "50. Линии электропередачи и иные объекты электросетевого хозяйства", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "51.", Name = "51. Объекты обустройства нефтяных и газовых месторождений", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "52.", Name = "52. Скважины", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "53.", Name = "53. Магистральные и промысловые трубопроводы", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "54.", Name = "54. Объекты использования атомной энергии", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "55.", Name = "55. Тепловые электростанции", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "56.", Name = "56. Объекты оборонной промышленности и иные объекты производственного назначения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "57.", Name = "57. Объекты металлургической промышленности", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "58.", Name = "58. Объекты горнодобывающей и горноперерабатывающей промышленности", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "59.", Name = "59. Объекты информатизации и связи", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "60.", Name = "60. Гидротехнические сооружения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "61.", Name = "61. Объекты обезвреживания и захоронения отходов I-V классов опасности", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "62.", Name = "62. Охрана объектов культурного наследия", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "63.", Name = "63. Объекты социально-культурного назначения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.1.", Name = "1. Инженерно-геодезические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.2.", Name = "2. Инженерно-геологические изыскания и инженерно-геотехнические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.3.", Name = "3. Инженерно-гидрометеорологические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.4.", Name = "4. Инженерно-экологические изыскания", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.5.", Name = "5. Схемы планировочной организации земельных участков", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.6.", Name = "6. Объемно-планировочные и архитектурные решения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.7.", Name = "7. Конструктивные решения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.8.", Name = "8. Охрана окружающей среды", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.9.", Name = "9. Санитарно-эпидемиологическая безопасность", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.10.", Name = "10. Пожарная безопасность", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.11.", Name = "11. Инженерно-технические мероприятия по гражданской обороне и чрезвычайным ситуациям", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.12.", Name = "12. Промышленная безопасность опасных производственных объектов", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.13.", Name = "13. Ядерная и радиационная безопасность", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.14.", Name = "14. Организация строительства", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.15.", Name = "15. Ценообразование и сметное нормирование", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.16.", Name = "16. Системы электроснабжения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.17.", Name = "17. Системы водоснабжения и водоотведения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.18.", Name = "18. Системы отопления, вентиляции, кондиционирования воздуха и холодоснабжения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.19.", Name = "19. Системы связи и сигнализации", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.20.", Name = "20. Системы газоснабжения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.21.", Name = "21. Системы автоматизации", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.22.", Name = "22. Системы теплоснабжения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.23.", Name = "23. Объекты авиационной инфраструктуры", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.24.", Name = "24. Объекты инфраструктуры железнодорожного транспорта", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.25.", Name = "25. Мосты и трубы", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.26.", Name = "26. Тоннели и метрополитены", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.27.", Name = "27. Автомобильные дороги", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.28.", Name = "28. Объекты морского и речного транспорта", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.29.", Name = "29. Объекты химических, нефтехимических и нефтегазоперерабатывающих, взрыво- и пожароопасных производств", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.30.", Name = "30. Линии электропередачи и иные объекты электросетевого хозяйства", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.31.", Name = "31. Объекты обустройства нефтяных и газовых месторождений", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.32.", Name = "32. Скважины", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.33.", Name = "33. Магистральные и промысловые трубопроводы", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.34.", Name = "34. Объекты использования атомной энергии", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.35.", Name = "35. Тепловые электростанции", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.36.", Name = "36. Объекты оборонной промышленности и иные объекты производственного назначения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.37.", Name = "37. Объекты металлургической промышленности", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.38.", Name = "38. Объекты горнодобывающей и горноперерабатывающей промышленности", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.39.", Name = "39. Объекты информатизации и связи", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.40.", Name = "40. Гидротехнические сооружения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.41.", Name = "41. Объекты обезвреживания и захоронения отходов I-V классов опасности", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.42.", Name = "42. Охрана объектов культурного наследия", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "955.43.", Name = "43. Объекты социально-культурного назначения", GeneralizedDictionary = GeneralizedDictionary.ExpertType });
            }

            if (collection.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.EngineeringSurveyType).Count() == 0)
            {
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "1", Name = "Инженерно-геодезические изыскания", GeneralizedDictionary = GeneralizedDictionary.EngineeringSurveyType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "2", Name = "Инженерно-геологические изыскания", GeneralizedDictionary = GeneralizedDictionary.EngineeringSurveyType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "3", Name = "Инженерно-гидрометеорологические изыскания", GeneralizedDictionary = GeneralizedDictionary.EngineeringSurveyType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "4", Name = "Инженерно-экологические изыскания", GeneralizedDictionary = GeneralizedDictionary.EngineeringSurveyType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "5", Name = "Инженерно-геотехнические изыскания", GeneralizedDictionary = GeneralizedDictionary.EngineeringSurveyType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "6", Name = "Геотехнические исследования", GeneralizedDictionary = GeneralizedDictionary.EngineeringSurveyType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "7", Name = "Обследования состояния грунтов оснований зданий и сооружений (их строительных конструкций)", GeneralizedDictionary = GeneralizedDictionary.EngineeringSurveyType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "8", Name = "Поиск и разведка подземных вод для целей водоснабжения", GeneralizedDictionary = GeneralizedDictionary.EngineeringSurveyType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "9", Name = "Локальный мониторинг компонентов окружающей среды", GeneralizedDictionary = GeneralizedDictionary.EngineeringSurveyType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "10", Name = "Разведка грунтовых строительных материалов", GeneralizedDictionary = GeneralizedDictionary.EngineeringSurveyType });
                await _session.SaveAsync(new GeneralizedDictionaryXPO(_session) { Value = "11", Name = "Локальные обследования загрязнения грунтов и грунтовых вод", GeneralizedDictionary = GeneralizedDictionary.EngineeringSurveyType });
            }
        }

        private static async Task FillingDocumentTypesXPOAsync()
        {
            if (await new XPQuery<DocumentTypeXPO>(_session).FirstOrDefaultAsync() != null)
            {
                return;
            }

            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "01.01", Name = "Документы-основания проведения экспертизы. Заявление о проведении экспертизы" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "01.02", Name = "Документы-основания проведения экспертизы. Договор о проведении экспертизы" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "01.03", Name = "Документы-основания проведения экспертизы. Документ, подтверждающий полномочия заявителя действовать от имени застройщика, технического заказчика, лица, обеспечившего выполнение инженерных изысканий и (или) подготовку проектной документации в случаях, предусмотренных частями 1.1 и 1.2 статьи 48 Градостроительного кодекса Российской Федерации" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "01.99", Name = "Документы-основания проведения экспертизы. Иные документы" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "02.01", Name = "Заключение экспертизы. Заключение экспертизы проектной документации и/или результатов инженерных изысканий, выданное ранее в отношении этого же объекта" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "02.02", Name = "Заключение экспертизы. Заключение по результатам оценки соответствия в рамках экспертного сопровождения" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "02.04", Name = "Заключение экспертизы. Заключение государственной экологической экспертизы" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "02.05", Name = "Заключение экспертизы. Заключение государственной историко-культурной экспертизы" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "02.06", Name = "Заключение экспертизы. Сводное заключение о проведении публичного технологического аудита крупного инвестиционного проекта с государственным участием" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "02.07", Name = "Заключение экспертизы. Заключение технологического и ценового аудита обоснования инвестиций" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "02.08", Name = "Заключение экспертизы. Заключение государственной экспертизы запасов подземных вод" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "02.09", Name = "Заключение экспертизы. Заключение государственной экспертизы запасов полезных ископаемых" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "02.99", Name = "Заключение экспертизы. Иные заключения" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "03.01", Name = "Документы размещения объекта строительства. Документ территориального планирования" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "03.02", Name = "Документы размещения объекта строительства. Градостроительный план земельного участка" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "03.03", Name = "Документы размещения объекта строительства. Проект планировки территории" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "03.04", Name = "Документы размещения объекта строительства. Проект межевания территории" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "03.05", Name = "Документы размещения объекта строительства. Согласование включения в границы территории, подлежащей комплексному развитию по инициативе правообладателей земельных участков, для размещения объектов коммунальной, транспортной, социальной инфраструктур, находящихся в государственной и (или) муниципальной собственности и не обремененных правами третьих лиц" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "03.06", Name = "Документы размещения объекта строительства. Согласование при осуществлении строительства помещений и сооружений, необходимых для организации пограничного, таможенного и иных видов контроля в пункте пропуска через Государственную границу" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "03.99", Name = "Документы размещения объекта строительства. Иные документы" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "04.01", Name = "Документы подключения объекта строительства к инженерным сетям. Технические условия на подключение объекта капитального строительства к сетям инженерно-технического обеспечения" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "04.02", Name = "Документы подключения объекта строительства к инженерным сетям. Документ о согласовании отступлений от положений технических условий" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "04.03", Name = "Документы подключения объекта строительства к инженерным сетям. Технические условия на перенос и переустройство линий связи и сооружений связи" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "04.99", Name = "Документы подключения объекта строительства к инженерным сетям. Иные документы" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "05.01", Name = "Задания и программы. Задание на проведение инженерных изысканий" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "05.02", Name = "Задания и программы. Программа инженерных изысканий" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "05.03", Name = "Задания и программы. Задание на проектирование" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "05.99", Name = "Задания и программы. Иные задания" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "06.01", Name = "Результаты инженерных изысканий. Инженерно-геодезические изыскания" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "06.02", Name = "Результаты инженерных изысканий. Инженерно-геологические изыскания" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "06.03", Name = "Результаты инженерных изысканий. Инженерно-гидрометеорологические изыскания" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "06.04", Name = "Результаты инженерных изысканий. Инженерно-экологические изыскания" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "06.05", Name = "Результаты инженерных изысканий. Инженерно-геотехнические изыскания" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "06.06", Name = "Результаты инженерных изысканий. Геотехнические исследования" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "06.07", Name = "Результаты инженерных изысканий. Обследования состояния грунтов оснований зданий и сооружений, их строительных конструкций" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "06.08", Name = "Результаты инженерных изысканий. Поиск и разведка подземных вод для целей водоснабжения" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "06.09", Name = "Результаты инженерных изысканий. Локальный мониторинг компонентов окружающей среды" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "06.10", Name = "Результаты инженерных изысканий. Разведка грунтовых строительных материалов" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "06.11", Name = "Результаты инженерных изысканий. Локальные обследования загрязнения грунтов и грунтовых вод" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "06.99", Name = "Результаты инженерных изысканий. Иной документ" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.01", Name = "Проектная документация. Раздел 1. «Пояснительная записка» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.02", Name = "Проектная документация. Раздел 2. «Схема планировочной организации земельного участка» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.03", Name = "Проектная документация. Раздел 3. «Архитектурные решения» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.04", Name = "Проектная документация. Раздел 4. «Конструктивные и объемно-планировочные решения» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.05", Name = "Проектная документация. Раздел 5. Подраздел «Система электроснабжения» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.06", Name = "Проектная документация. Раздел 5. Подраздел «Система водоснабжения» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.07", Name = "Проектная документация. Раздел 5. Подраздел «Система водоотведения» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.08", Name = "Проектная документация. Раздел 5. Подраздел «Отопление, вентиляция и кондиционирование воздуха, тепловые сети» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.09", Name = "Проектная документация. Раздел 5. Подраздел «Сети связи» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.10", Name = "Проектная документация. Раздел 5. Подраздел «Система газоснабжения» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.11", Name = "Проектная документация. Раздел 5. Подраздел «Технологические решения» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.12", Name = "Проектная документация. Раздел 6. «Проект организации строительства» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.13", Name = "Проектная документация. Раздел 7. «Проект организации работ по сносу или демонтажу объектов капитального строительства» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.14", Name = "Проектная документация. Раздел 8. «Перечень мероприятий по охране окружающей среды» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.15", Name = "Проектная документация. Раздел 9. «Мероприятия по обеспечению пожарной безопасности» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.16", Name = "Проектная документация. Раздел 10. «Мероприятия по обеспечению доступа инвалидов» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.17", Name = "Проектная документация. Раздел 10.1. «Мероприятия по обеспечению соблюдения требований энергетической эффективности и требований оснащенности зданий, строений и сооружений приборами учета используемых энергетических ресурсов» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.19", Name = "Проектная документация. Раздел 12. «Иная документация в случаях, предусмотренных федеральными законами» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.20", Name = "Проектная документация. Раздел 1. «Пояснительная записка» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.21", Name = "Проектная документация. Раздел 2. «Схема планировочной организации земельного участка» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.22", Name = "Проектная документация. Раздел 3. «Объемно-планировочные и архитектурные решения» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.23", Name = "Проектная документация. Раздел 4. «Конструктивные решения» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.24", Name = "Проектная документация. Раздел 5. Подраздел «Система электроснабжения» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.25", Name = "Проектная документация. Раздел 5. Подраздел «Система водоснабжения» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.26", Name = "Проектная документация. Раздел 5. Подраздел «Система водоотведения» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.27", Name = "Проектная документация. Раздел 5. Подраздел «Отопление, вентиляция и кондиционирование воздуха, тепловые сети» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.28", Name = "Проектная документация. Раздел 5. Подраздел «Сети связи» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.29", Name = "Проектная документация. Раздел 5. Подраздел «Система газоснабжения» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.30", Name = "Проектная документация. Раздел 6. «Технологические решения» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.31", Name = "Проектная документация. Раздел 7. «Проект организации строительства» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.32", Name = "Проектная документация. Раздел 8. «Мероприятия по охране окружающей среды» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.33", Name = "Проектная документация. Раздел 9. «Мероприятия по обеспечению пожарной безопасности» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.34", Name = "Проектная документация. Раздел 10. «Требования к обеспечению безопасной эксплуатации объектов капитального строительства» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.35", Name = "Проектная документация. Раздел 11. «Мероприятия по обеспечению доступа инвалидов к объекту капитального строительства» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "07.37", Name = "Проектная документация. Раздел 13. «Иная документация в случаях, предусмотренных законодательными и иными нормативными правовыми актами Российской Федерации» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "08.01", Name = "Проектная документация. Линейный объект. Раздел 1. «Пояснительная записка» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "08.02", Name = "Проектная документация. Линейный объект. Раздел 2. «Проект полосы отвода» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "08.03", Name = "Проектная документация. Линейный объект. Раздел 3. «Технологические и конструктивные решения линейного объекта. Искусственные сооружения» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "08.04", Name = "Проектная документация. Линейный объект. Раздел 4. «Здания, строения и сооружения, входящие в инфраструктуру линейного объекта» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "08.05", Name = "Проектная документация. Линейный объект. Раздел 5. «Проект организации строительства» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "08.06", Name = "Проектная документация. Линейный объект. Раздел 6. «Проект организации работ по сносу (демонтажу) линейного объекта» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "08.07", Name = "Проектная документация. Линейный объект. Раздел 7. «Мероприятия по охране окружающей среды» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "08.08", Name = "Проектная документация. Линейный объект. Раздел 8. «Мероприятия по обеспечению пожарной безопасности» (действовал до 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "08.10", Name = "Проектная документация. Линейный объект. Раздел 10. «Иная документация в случаях, предусмотренных федеральными законами» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "08.11", Name = "Проектная документация. Линейный объект. Раздел 1. «Пояснительная записка» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "08.12", Name = "Проектная документация. Линейный объект. Раздел 2. «Проект полосы отвода» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "08.13", Name = "Проектная документация. Линейный объект. Раздел 3. «Технологические и конструктивные решения линейного объекта. Искусственные сооружения» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "08.14", Name = "Проектная документация. Линейный объект. Раздел 4. «Здания, строения и сооружения, входящие в инфраструктуру линейного объекта» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "08.15", Name = "Проектная документация. Линейный объект. Раздел 5. «Проект организации строительства» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "08.16", Name = "Проектная документация. Линейный объект. Раздел 6. «Мероприятия по охране окружающей среды» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "08.17", Name = "Проектная документация. Линейный объект. Раздел 7. «Мероприятия по обеспечению пожарной безопасности» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "08.18", Name = "Проектная документация. Линейный объект. Раздел 8. «Требования к обеспечению безопасной эксплуатации линейного объекта» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "08.20", Name = "Проектная документация. Линейный объект. Раздел 10. «Иная документация в случаях, предусмотренных законодательными и иными нормативными правовыми актами Российской Федерации» (действует с 01.09.2022)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "09.01", Name = "Переписка по вопросам проведения экспертизы. Уведомление о выявлении недостатков" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "09.02", Name = "Переписка по вопросам проведения экспертизы. Сопроводительное письмо заявителя с откорректированной проектной документацией" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "09.99", Name = "Переписка по вопросам проведения экспертизы. Иной документ" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "10.01", Name = "Документы о членстве в саморегулируемой организации. Выписка из реестра членов саморегулируемой организации" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "10.99", Name = "Документы о членстве в саморегулируемой организации. Иной документ" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "11.01", Name = "Документы для проведения проверки достоверности определения сметной стоимости. Ведомость объемов работ" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "11.02", Name = "Документы для проведения проверки достоверности определения сметной стоимости. Решение по объекту капитального строительства" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "11.03", Name = "Документы для проведения проверки достоверности определения сметной стоимости. Акт, содержащий перечень дефектов оснований, строительных конструкций, систем инженерно-технического обеспечения и сетей инженерно-технического обеспечения" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "11.04", Name = "Документы для проведения проверки достоверности определения сметной стоимости. Решение о разработке и применении индивидуальных сметных нормативов" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "11.05", Name = "Документы для проведения проверки достоверности определения сметной стоимости. Коньюктурный анализ" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "11.06", Name = "Документы для проведения проверки достоверности определения сметной стоимости. Прайс-лист" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "11.99", Name = "Документы для проведения проверки достоверности определения сметной стоимости. Иной документ" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "12.01", Name = "Документы в отношении типовой проектной документации повторного использования. Положительное заключение экспертизы в отношении типовой проектной документации повторного использования" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "12.02", Name = "Документы в отношении типовой проектной документации повторного использования. Справка с указанием разделов, которые не подвергались изменению" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "12.03", Name = "Документы в отношении типовой проектной документации повторного использования. Документ, подтверждающий аналогичность" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "12.99", Name = "Документы в отношении типовой проектной документации повторного использования. Иной документ" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "13.01", Name = "Документы в отношении работ по сносу объектов капитального строительства. Проект организации работ по сносу объекта капитального строительства" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "13.02", Name = "Документы в отношении работ по сносу объектов капитального строительства. Смета на снос объекта капитального строительства" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "13.03", Name = "Документы в отношении работ по сносу объектов капитального строительства. Акт (решение) собственника здания (сооружения, строения) о выведении из эксплуатации и ликвидации объекта капитального строительства" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "13.04", Name = "Документы в отношении работ по сносу объектов капитального строительства. Решение федерального органа исполнительной власти, органа исполнительной власти субъекта Российской Федерации или органа местного самоуправления о признании многоквартирного дома аварийным и подлежащим сносу" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "13.05", Name = "Результаты и материалы обследования объекта капитального строительства в соответствии с требованиями технических регламентов, санитарно-эпидемиологическими требованиями, требованиями в области охраны окружающей среды, требованиями безопасности деятельности в области использования атомной энергии, требованиями к осуществлению деятельности в области промышленной безопасности" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "13.06", Name = "Документ, подтверждающий передачу проекта организации работ по сносу объекта капитального строительства застройщику, техническому заказчику или лицу, обеспечившему выполнение инженерных изысканий и (или) подготовку проектной документации" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "13.99", Name = "Документы в отношении работ по сносу объектов капитального строительства. Иной документ" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "14.01", Name = "Программы развития. Федеральная целевая программа" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "14.02", Name = "Программы развития. Программа развития субъекта Российской Федерации" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "14.03", Name = "Программы развития. Программа развития муниципального образования" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "14.04", Name = "Программы развития. Ведомственная целевая программа" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "14.99", Name = "Программы развития. Иная программа" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "15.01", Name = "Решение о разработке проектной документации. Решение Президента Российской федерации" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "15.02", Name = "Решение о разработке проектной документации. Нормативный правовой акт Правительства Российской Федерации" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "15.03", Name = "Решение о разработке проектной документации. Нормативный правовой акт высшего органа исполнительной власти субъекта Российской Федерации" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "15.04", Name = "Решение о разработке проектной документации. Муниципальный правовой акт местной администрации муниципального образования" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "15.05", Name = "Решение о разработке проектной документации. Решение главного распорядителя средств федерального бюджета о подготовке и реализации бюджетных инвестиций, о предоставлении субсидий на осуществление капитальных вложений в объект капитального строительства" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "15.06", Name = "Решение о разработке проектной документации. Решение руководителя государственной компании и корпорации об осуществлении капитальных вложений в объект капитального строительства" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "15.07", Name = "Решение о разработке проектной документации. Решение по объекту капитального строительства, принятое в порядке, установленном методикой, приведенной в приложении к соответствующей федеральной целевой программе, определяющей порядок детализации мероприятий (укрупненных инвестиционных проектов), содержащее информацию об объекте капитального строительства, входящем в мероприятие (укрупненный инвестиционный проект)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "15.08", Name = "Решение о разработке проектной документации. Решение застройщика" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "15.99", Name = "Решение о разработке проектной документации. Решение иного лица" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "16.01", Name = "Международные договоры. Международный договор" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "16.02", Name = "Международные договоры. Соглашение о разделе продукции" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "16.99", Name = "Международные договоры. Иные договоры" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "17.01", Name = "Правоустанавливающие документы. Выписка из Единого государственного реестра недвижимости" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "17.99", Name = "Правоустанавливающие документы. Иной правоустанавливающий документ" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "18.01", Name = "Документы о безопасности производственных объектов. Обоснование безопасности опасного производственного объекта в случаях, предусмотренных частью 4 статьи 3 Федерального закона «О промышленной безопасности опасных производственных объектов»" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "18.02", Name = "Документы о безопасности производственных объектов. Положительное заключение экспертизы промышленной безопасности такого обоснования, внесенное в реестр заключений экспертизы промышленной безопасности" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "18.03", Name = "Документы о безопасности производственных объектов. Документ о регистрации в государственном реестре опасных производственных объектов" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "18.99", Name = "Документы о безопасности производственных объектов. Иные документы" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "19.01", Name = "Специальные технические условия. Специальные технические условия" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "19.02", Name = "Специальные технические условия. Документ о согласовании отступлений от положений специальных технических условий" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "19.03", Name = "Специальные технические условия. Технические требования и условия, подлежащие обязательному исполнению при подготовке проектной документации в целях реконструкции, капитального ремонта существующих линейного объекта или линейных объектов, а также при осуществлении таких реконструкций, капитального ремонта" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "19.99", Name = "Специальные технические условия. Иные документы" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "20.01", Name = "Разрешения. Разрешение на строительство" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "20.02", Name = "Разрешения. Разрешение на ввод объектов в эксплуатацию" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "20.03", Name = "Разрешения. Акт освидетельствования проведения основных работ по строительству, реконструкции" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "20.04", Name = "Разрешения. Разрешение на условно разрешенный вид использования земельного участка или объекта капитального строительства" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "20.05", Name = "Разрешения. Разрешение на отклонение от предельных параметров разрешенного строительства, реконструкции объектов капитального строительства" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "20.06", Name = "Разрешения. Разрешение на застройку земельных участков, которые расположены за границами населенных пунктов и находятся на площадях залегания полезных ископаемых, а также на размещение за границами населенных пунктов в местах залегания полезных ископаемых подземных сооружений в пределах горного отвода" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "20.07", Name = "Разрешения. Разрешение на установку радиопередающих средств на высотных зданиях" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "20.99", Name = "Разрешения. Иные разрешения" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "21.01", Name = "Документы в сфере охраны окружающей среды и недропользования. Расчет нормативов допустимых выбросов, нормативов допустимых сбросов" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "21.02", Name = "Документы в сфере охраны окружающей среды и недропользования. Горноотводный акт с планом границ горного отвода или его копия" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "21.03", Name = "Документы в сфере охраны окружающей среды и недропользования. Лицензия на пользование недрами" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "21.04", Name = "Документы в сфере охраны окружающей среды и недропользования. Заключение о согласовании строительства и реконструкции объектов капитального строительства, внедрения новых технологических процессов и осуществления иной деятельности, оказывающей воздействие на водные биологические ресурсы и среду их обитания" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "21.05", Name = "Документы в сфере охраны окружающей среды и недропользования. Согласование проектной документации в отношении строительства и эксплуатации в пластах горных пород различных видов хранилищ углеводородного сырья и продуктов его переработки" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "21.06", Name = "Документы в сфере охраны окружающей среды и недропользования. Заключение об отсутствии полезных ископаемых в недрах под участком предстоящей застройки" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "21.07", Name = "Документы в сфере охраны окружающей среды и недропользования. Санитарно-эпидемиологическое заключение о соответствии санитарным правилам проекта зон санитарной охраны источника хозяйственно-питьевого водоснабжения" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "21.08", Name = "Документы в сфере охраны окружающей среды и недропользования. Документ о согласовании сооружения линий связи, электропередач, трубопроводов, дорог и других объектов на мелиорируемых (мелиорированных) землях" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "21.09", Name = "Документы в сфере охраны окружающей среды и недропользования. Документ об утверждении нормативов образования отходов производства и потребления и лимитов на их размещение" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "21.10", Name = "Документы в сфере охраны окружающей среды и недропользования. Согласование плана предупреждения и ликвидации разливов нефти и нефтепродуктов" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "21.11", Name = "Документы в сфере охраны окружающей среды и недропользования. Сведения о размерах водоохранных зон и прибрежных защитных полос, затрагиваемых в ходе строительства, поверхностных водных объектов" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "21.12", Name = "Документы в сфере охраны окружающей среды и недропользования. Сведения о наличии и месторасположении в районе размещения проектируемого объекта зон санитарной охраны источников водоснабжения (поверхностных и подземных)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "21.13", Name = "Документы в сфере охраны окружающей среды и недропользования. Решение об установлении, изменении или о прекращении существования санитарно-защитной зоны" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "21.99", Name = "Документы в сфере охраны окружающей среды и недропользования. Иные документы" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "22.01", Name = "Документы в области организации транспортной инфраструктуры. Сведения о согласовании предварительной категории строящегося объекта транспортной инфраструктуры с компетентным органом в области обеспечения транспортной безопасности" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "22.02", Name = "Документы в области организации транспортной инфраструктуры. Согласование типов и конструкций устройств со специальными проходами, ограждающими опасные участки транспортных магистралей в местах концентрации объектов животного мира и на путях их миграции" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "22.03", Name = "Документы в области организации транспортной инфраструктуры. Решение о согласовании размещения объекта в границах полос воздушных подходов для аэродромов экспериментальной авиации, аэродромов государственной авиации, аэродромов гражданской авиации" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "22.04", Name = "Документы в области организации транспортной инфраструктуры. Решение об установлении приаэродромной территории" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "22.05", Name = "Документы в области организации транспортной инфраструктуры. Сведения о зонах с особыми условиями использования территорий объектов инфраструктуры метрополитена" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "22.99", Name = "Документы в области организации транспортной инфраструктуры. Иные документы" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "23.01", Name = "Документы в отношении объектов культурного наследия. Паспорт объекта культурного наследия" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "23.02", Name = "Документы в отношении объектов культурного наследия. Согласование проектных решений раздела по обеспечению сохранности объектов культурного наследия либо проектной документации по реставрации и приспособлению объекта культурного наследия" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "23.99", Name = "Документы в отношении объектов культурного наследия. Иные документы" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "24.01", Name = "Расчеты. Расчеты конструктивных, технологических и других решений, используемых в проектной документации" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "25.01", Name = "Состав сметной документации. Пояснительная записка к сметной документации" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "25.02", Name = "Состав сметной документации. Сводка затрат" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "25.03", Name = "Состав сметной документации. Сводный сметный расчет" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "25.04", Name = "Состав сметной документации. Объектный сметный расчет" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "25.05", Name = "Состав сметной документации. Локальный сметный расчет" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "25.06", Name = "Состав сметной документации. Сметные расчеты на отдельные виды затрат" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "99.01", Name = "Группа дополнительных документов. Документ, подтверждающий передачу проектной документации и (или) результатов инженерных изысканий застройщику (техническому заказчику)" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "99.02", Name = "Группа дополнительных документов. Справка с описанием изменений, внесенных в проектную документацию и (или) результаты инженерных изысканий" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "99.03", Name = "Группа дополнительных документов. Акт (решение) собственника здания (строения, сооружения), содержащий условия реконструкции, капитального ремонта или сноса объекта капитального строительства или его части" });
            await _session.SaveAsync(new DocumentTypeXPO(_session) { Value = "99.99", Name = "Группа дополнительных документов. Иной документ" });
        }
    }
}

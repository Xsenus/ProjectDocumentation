using Core.NET.Models.XPO;
using DevExpress.Xpo;
using System.Linq;
using System.Threading.Tasks;

namespace Core.NET.Controllers
{
    public static class DataBaseController
    {
        private static Session _session  => SessionController.GetSession();

        public static async Task FillingDataBaseAsync()
        {
            await GeneralizedDictionaryXPOAsync();
            await FillingClimateDistrictXPOAsync();
            await FillingExpertTypesXPOAsync();
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
        }

        private static async Task FillingClimateDistrictXPOAsync()
        {
            if (await new XPQuery<ClimateDistrictXPO>(_session).FirstOrDefaultAsync() != null)
            {
                return;
            }

            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "Сведения не представлены", Name = "Сведения не представлены" });
            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "I", Name = "климатический район I" });
            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "IА", Name = "климатический подрайон IА" });
            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "IБ", Name = "климатический подрайон IБ" });
            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "IВ", Name = "климатический подрайон IВ" });
            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "IГ", Name = "климатический подрайон IГ" });
            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "IД", Name = "климатический подрайон IД" });
            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "II", Name = "климатический район II" });
            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "IIА", Name = "климатический подрайон IIА" });
            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "IIБ", Name = "климатический подрайон IIБ" });
            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "IIВ", Name = "климатический подрайон IIВ" });
            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "IIГ", Name = "климатический подрайон IIГ" });
            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "III", Name = "климатический район III" });
            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "IIIА", Name = "климатический подрайон IIIА" });
            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "IIIБ", Name = "климатический подрайон IIIБ" });
            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "IIIВ", Name = "климатический подрайон IIIВ" });
            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "IV", Name = "климатический район IV" });
            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "IVА", Name = "климатический подрайон IVА" });
            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "IVБ", Name = "климатический подрайон IVБ" });
            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "IVВ", Name = "климатический подрайон IVВ" });
            await _session.SaveAsync(new ClimateDistrictXPO(_session) { Value = "IVГ", Name = "климатический подрайон IVГ" });
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

        private static async Task FillingExpertTypesXPOAsync()
        {
            if (await new XPQuery<ExpertTypeXPO>(_session).FirstOrDefaultAsync() != null)
            {
                return;
            }

            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "1.1.", Name = "1.1. Инженерно-геодезические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "1.2.", Name = "1.2. Инженерно-геологические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "1.3.", Name = "1.3. Инженерно-гидрометеорологические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "1.4.", Name = "1.4. Инженерно-экологические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "1.5.", Name = "1.5. Инженерно-геотехнические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "2.1.", Name = "2.1. Объемно-планировочные, архитектурные и конструктивные решения, планировочная организация земельного участка, организация строительства" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "2.1.1.", Name = "2.1.1. Схемы планировочной организации земельных участков" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "2.1.2.", Name = "2.1.2. Объемно-планировочные и архитектурные решения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "2.1.3.", Name = "2.1.3. Конструктивные решения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "2.1.4.", Name = "2.1.4. Организация строительства" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "2.2.", Name = "2.2. Теплогазоснабжение, водоснабжение, водоотведение, канализация, вентиляция и кондиционирование" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "2.2.1.", Name = "2.2.1. Водоснабжение, водоотведение и канализация" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "2.2.2.", Name = "2.2.2. Теплоснабжение, вентиляция и кондиционирование" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "2.2.3.", Name = "2.2.3. Системы газоснабжения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "2.3.", Name = "2.3. Электроснабжение, связь, сигнализация,  системы автоматизации" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "2.3.1.", Name = "2.3.1. Электроснабжение и электропотребление" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "2.3.2.", Name = "2.3.2. Системы автоматизации, связи и сигнализации" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "2.4.", Name = "2.4. Охрана окружающей среды, санитарно-эпидемиологическая безопасность" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "2.4.1.", Name = "2.4.1. Охрана окружающей среды" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "2.4.2.", Name = "2.4.2. Санитарно-эпидемиологическая безопасность" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "2.5.", Name = "2.5. Пожарная безопасность" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "3.1.", Name = "3.1. Организация экспертизы проектной документации и (или) результатов инженерных изысканий " });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "4.1.", Name = "4.1. Мосты и трубы" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "4.2.", Name = "4.2. Автомобильные дороги" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "4.3.", Name = "4.3. Объекты топливно-энергетического комплекса" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "4.4.", Name = "4.4. Объекты информатизации и связи" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "4.5.", Name = "4.5. Инженерно-технические мероприятия ГО и ЧС" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.1.1.", Name = "5.1.1. Инженерно-геодезические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.1.2.", Name = "5.1.2. Инженерно-геологические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.1.3.", Name = "5.1.3. Инженерно-гидрометеорологические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.1.4.", Name = "5.1.4. Инженерно-экологические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.1.5.", Name = "5.1.5. Инженерно-геотехнические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.1.6.", Name = "5.1.6. Обследования состояния грунтов оснований зданий и сооружений" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.1.7.", Name = "5.1.7. Обследование состояния строительных конструкций зданий и сооружений" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.1.", Name = "5.2.1. Схемы планировочной организации земельных участков" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.2.", Name = "5.2.2. Объемно-планировочные решения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.3.", Name = "5.2.3. Конструктивные решения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.4.", Name = "5.2.4. Сети инженерно-технического обеспечения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.4.1.", Name = "5.2.4.1. Электроснабжение" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.4.2.", Name = "5.2.4.2. Водоснабжение и водоотведение" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.4.3.", Name = "5.2.4.3. Отопление, вентиляция и кондиционирование воздуха, тепловые сети" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.4.4.", Name = "5.2.4.4. Системы связи и сигнализации" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.4.5.", Name = "5.2.4.5. Системы газоснабжения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.4.6.", Name = "5.2.4.6. Системы автоматизации" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.4.7.", Name = "5.2.4.7. Тепловые сети" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.5.", Name = "5.2.5. Охрана окружающей среды" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.6.", Name = "5.2.6. Санитарно-эпидемиологическая безопасность" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.7.", Name = "5.2.7. Пожарная безопасность" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.8.", Name = "5.2.8. Инженерно-технические мероприятия ГО и ЧС" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.9.", Name = "5.2.9. Промышленная безопасность опасных производственных объектов" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.10.", Name = "5.2.10. Ядерная и радиационная безопасность" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.11.", Name = "5.2.11. Организация строительства" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.12.", Name = "5.2.12. Объекты транспортного комплекса" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.12.1.", Name = "5.2.12.1. Воздушный транспорт" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.12.2.", Name = "5.2.12.2. Железнодорожный транспорт" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.12.3.", Name = "5.2.12.3. Мосты и трубы" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.12.4.", Name = "5.2.12.4. Тоннели и метрополитены" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.12.5.", Name = "5.2.12.5. Автомобильные дороги" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.12.6.", Name = "5.2.12.6. Морской транспорт" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.13.", Name = "5.2.13. Объекты топливно-энергетического комплекса" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.13.1.", Name = "5.2.13.1. Объекты химических, нефтехимических и нефтегазоперерабатывающих, взрыво- и пожароопасных производств" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.13.2.", Name = "5.2.13.2. Линии электропередачи и иные объекты электросетевого хозяйства" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.13.3.", Name = "5.2.13.3. Объекты обустройства нефтяных и газовых месторождений" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.13.4.", Name = "5.2.13.4. Строительство скважин" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.13.5.", Name = "5.2.13.5. Строительство магистральных и промысловых трубопроводов" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.13.6.", Name = "5.2.13.6. Объекты использования атомной энергии" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.13.7.", Name = "5.2.13.7. Тепловые электростанции" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.14.", Name = "5.2.14. Объекты производственного назначения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.14.1.", Name = "5.2.14.1. Объекты космической инфраструктуры" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.14.2.", Name = "5.2.14.2. Объекты оборонной промышленности" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.14.3.", Name = "5.2.14.3. Объекты металлургической промышленности" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.14.4.", Name = "5.2.14.4. Объекты горнодобывающей и горно-перерабатывающей промышленности" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.15.", Name = "5.2.15. Объекты информатизации и связи" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.2.16.", Name = "5.2.16. Гидротехнические сооружения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.3.1.", Name = "5.3.1. Организация государственной экспертизы проектной документации и (или) результатов инженерных изысканий" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "1.", Name = "1. Инженерно-геодезические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "2.", Name = "2. Инженерно-геологические изыскания и инженерно-геотехнические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "3.", Name = "3. Инженерно-гидрометеорологические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "4.", Name = "4. Инженерно-экологические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "5.", Name = "5. Схемы планировочной организации земельных участков" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "6.", Name = "6. Объемно-планировочные и архитектурные решения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "7.", Name = "7. Конструктивные решения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "8.", Name = "8. Охрана окружающей среды" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "9.", Name = "9. Санитарно-эпидемиологическая безопасность" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "10.", Name = "10. Пожарная безопасность" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "11.", Name = "11. Инженерно-технические мероприятия ГО и ЧС" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "12.", Name = "12. Организация строительства" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "13.", Name = "13. Системы водоснабжения и водоотведения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "14.", Name = "14. Системы отопления, вентиляции, кондиционирования воздуха и холодоснабжения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "15.", Name = "15. Системы газоснабжения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "16.", Name = "16. Системы электроснабжения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "17.", Name = "17. Системы связи и сигнализации" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "17.1.", Name = "17.1. Ценообразование и сметное нормирование" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "18.", Name = "18. Мосты и трубы" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "19.", Name = "19. Автомобильные дороги" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "20.", Name = "20. Объекты топливно-энергетического комплекса" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "21.", Name = "21. Объекты информатизации и связи" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "22.", Name = "22. Инженерно-геодезические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "23.", Name = "23. Инженерно-геологические изыскания и инженерно-геотехнические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "24.", Name = "24. Инженерно-гидрометеорологические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "25.", Name = "25. Инженерно-экологические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "26.", Name = "26. Схемы планировочной организации земельных участков" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "27.", Name = "27. Объемно-планировочные решения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "28.", Name = "28. Конструктивные решения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "29.", Name = "29. Охрана окружающей среды" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "30.", Name = "30. Санитарно-эпидемиологическая безопасность" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "31.", Name = "31. Пожарная безопасность" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "32.", Name = "32. Инженерно-технические мероприятия ГО и ЧС" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "33.", Name = "33. Промышленная безопасность опасных производственных объектов" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "34.", Name = "34. Ядерная и радиационная безопасность" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "35.", Name = "35. Организация строительства" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "35.1.", Name = "35.1. Ценообразование и сметное нормирование" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "36.", Name = "36. Системы электроснабжения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "37.", Name = "37. Системы водоснабжения и водоотведения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "38.", Name = "38. Системы отопления, вентиляции, кондиционирования воздуха и холодоснабжения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "39.", Name = "39. Системы связи и сигнализации" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "40.", Name = "40. Системы газоснабжения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "41.", Name = "41. Системы автоматизации" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "42.", Name = "42. Системы теплоснабжения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "43.", Name = "43. Объекты авиационной инфраструктуры" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "44.", Name = "44. Объекты инфраструктуры железнодорожного транспорта" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "45.", Name = "45. Мосты и трубы" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "46.", Name = "46. Тоннели и метрополитены" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "47.", Name = "47. Автомобильные дороги" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "48.", Name = "48. Объекты морского и речного транспорта" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "49.", Name = "49. Объекты химических, нефтехимических и нефтегазоперерабатывающих, взрыво- и пожароопасных производств" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "50.", Name = "50. Линии электропередачи и иные объекты электросетевого хозяйства" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "51.", Name = "51. Объекты обустройства нефтяных и газовых месторождений" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "52.", Name = "52. Скважины" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "53.", Name = "53. Магистральные и промысловые трубопроводы" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "54.", Name = "54. Объекты использования атомной энергии" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "55.", Name = "55. Тепловые электростанции" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "56.", Name = "56. Объекты оборонной промышленности и иные объекты производственного назначения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "57.", Name = "57. Объекты металлургической промышленности" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "58.", Name = "58. Объекты горнодобывающей и горноперерабатывающей промышленности" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "59.", Name = "59. Объекты информатизации и связи" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "60.", Name = "60. Гидротехнические сооружения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "61.", Name = "61. Объекты обезвреживания и захоронения отходов I-V классов опасности" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "62.", Name = "62. Охрана объектов культурного наследия" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "63.", Name = "63. Объекты социально-культурного назначения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.1.", Name = "1. Инженерно-геодезические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.2.", Name = "2. Инженерно-геологические изыскания и инженерно-геотехнические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.3.", Name = "3. Инженерно-гидрометеорологические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.4.", Name = "4. Инженерно-экологические изыскания" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.5.", Name = "5. Схемы планировочной организации земельных участков" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.6.", Name = "6. Объемно-планировочные и архитектурные решения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.7.", Name = "7. Конструктивные решения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.8.", Name = "8. Охрана окружающей среды" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.9.", Name = "9. Санитарно-эпидемиологическая безопасность" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.10.", Name = "10. Пожарная безопасность" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.11.", Name = "11. Инженерно-технические мероприятия по гражданской обороне и чрезвычайным ситуациям" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.12.", Name = "12. Промышленная безопасность опасных производственных объектов" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.13.", Name = "13. Ядерная и радиационная безопасность" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.14.", Name = "14. Организация строительства" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.15.", Name = "15. Ценообразование и сметное нормирование" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.16.", Name = "16. Системы электроснабжения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.17.", Name = "17. Системы водоснабжения и водоотведения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.18.", Name = "18. Системы отопления, вентиляции, кондиционирования воздуха и холодоснабжения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.19.", Name = "19. Системы связи и сигнализации" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.20.", Name = "20. Системы газоснабжения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.21.", Name = "21. Системы автоматизации" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.22.", Name = "22. Системы теплоснабжения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.23.", Name = "23. Объекты авиационной инфраструктуры" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.24.", Name = "24. Объекты инфраструктуры железнодорожного транспорта" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.25.", Name = "25. Мосты и трубы" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.26.", Name = "26. Тоннели и метрополитены" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.27.", Name = "27. Автомобильные дороги" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.28.", Name = "28. Объекты морского и речного транспорта" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.29.", Name = "29. Объекты химических, нефтехимических и нефтегазоперерабатывающих, взрыво- и пожароопасных производств" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.30.", Name = "30. Линии электропередачи и иные объекты электросетевого хозяйства" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.31.", Name = "31. Объекты обустройства нефтяных и газовых месторождений" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.32.", Name = "32. Скважины" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.33.", Name = "33. Магистральные и промысловые трубопроводы" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.34.", Name = "34. Объекты использования атомной энергии" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.35.", Name = "35. Тепловые электростанции" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.36.", Name = "36. Объекты оборонной промышленности и иные объекты производственного назначения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.37.", Name = "37. Объекты металлургической промышленности" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.38.", Name = "38. Объекты горнодобывающей и горноперерабатывающей промышленности" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.39.", Name = "39. Объекты информатизации и связи" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.40.", Name = "40. Гидротехнические сооружения" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.41.", Name = "41. Объекты обезвреживания и захоронения отходов I-V классов опасности" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.42.", Name = "42. Охрана объектов культурного наследия" });
            await _session.SaveAsync(new ExpertTypeXPO(_session) { Value = "955.43.", Name = "43. Объекты социально-культурного назначения" });


        }
    }
}

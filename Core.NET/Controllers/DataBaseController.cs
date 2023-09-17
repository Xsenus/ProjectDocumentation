using Core.NET.Models.XPO;
using DevExpress.Xpo;
using System.Threading.Tasks;

namespace Core.NET.Controllers
{
    public static class DataBaseController
    {
        private static Session _session  => SessionController.GetSession();

        public static async Task FillingDataBaseAsync()
        {
            await FillingExpertTypesAsync();
        }

        private static async Task FillingExpertTypesAsync()
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

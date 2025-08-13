using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Core.NET.AutoModels.ConclusionObj
{
    /// <summary>
    /// <para>Адрес</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Адрес")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tAddress", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TAddress
    {

        /// <summary>
        /// <para>Номер здания/сооружения</para>
        /// <para>Текстовая строка длиной 200 символа</para>
        /// <para xml:lang="en">Maximum length: 200.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Номер здания/сооружения")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(200)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("Building")]
        public string Building { get; set; }

        /// <summary>
        /// <para>Город</para>
        /// <para>Текстовая строка длиной 200 символа</para>
        /// <para xml:lang="en">Maximum length: 200.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Город")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(200)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("City")]
        public string City { get; set; }

        /// <summary>
        /// <para>Страна</para>
        /// <para>Текстовая строка длиной 200 символа</para>
        /// <para xml:lang="en">Maximum length: 200.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Страна")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(200)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("Country")]
        public string Country { get; set; }

        /// <summary>
        /// <para>Наименование района</para>
        /// <para>Текстовая строка длиной 200 символа</para>
        /// <para xml:lang="en">Maximum length: 200.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Наименование района")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(200)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("District")]
        public string District { get; set; }

        /// <summary>
        /// <para>Неформализованное описание адреса</para>
        /// <para>Непустая текстовая строка</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Неформализованное описание адреса")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("Note")]
        public string Note { get; set; }

        /// <summary>
        /// <para>Код субъекта Российской Федерации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Код субъекта Российской Федерации")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Region")]
        public TRegionsRf Region { get; set; }

        /// <summary>
        /// <para>Номер помещения</para>
        /// <para>Текстовая строка длиной 200 символа</para>
        /// <para xml:lang="en">Maximum length: 200.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Номер помещения")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(200)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("Room")]
        public string Room { get; set; }

        /// <summary>
        /// <para>Населенный пункт</para>
        /// <para>Текстовая строка длиной 200 символа</para>
        /// <para xml:lang="en">Maximum length: 200.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Населенный пункт")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(200)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("Settlement")]
        public string Settlement { get; set; }

        /// <summary>
        /// <para>Улица</para>
        /// <para>Текстовая строка длиной 200 символа</para>
        /// <para xml:lang="en">Maximum length: 200.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Улица")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(200)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("Street")]
        public string Street { get; set; }

        public override string ToString()
        {
            var parts = new List<string>(8);

            Add(parts, Country);
            Add(parts, RegionToHuman(Region));
            AddPrefixed(parts, District, "р-н");
            AddPrefixed(parts, City, "г.");
            AddPrefixed(parts, Settlement, "н.п.");
            AddPrefixed(parts, Street, "ул.");

            // Дом / помещение
            var b = string.IsNullOrWhiteSpace(Building) ? null : $"д. {Building.Trim()}";
            var r = string.IsNullOrWhiteSpace(Room) ? null : $"пом. {Room.Trim()}";
            if (!string.IsNullOrEmpty(b) && !string.IsNullOrEmpty(r)) Add(parts, $"{b}, {r}");
            else Add(parts, b ?? r);

            // Индекс из Note (остальное игнорируем, чтобы не было дублей)
            var postal = ExtractPostalIndex(Note);
            var line = string.Join(", ", parts.Where(s => !string.IsNullOrWhiteSpace(s)));

            if (!string.IsNullOrEmpty(postal))
                line = $"{line}, {postal}";

            return line;
        }

        private static void Add(List<string> parts, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                parts.Add(value.Trim());
        }

        private static void AddPrefixed(List<string> parts, string value, string prefix)
        {
            if (!string.IsNullOrWhiteSpace(value))
                parts.Add($"{prefix} {value.Trim()}");
        }

        private static string ExtractPostalIndex(string note)
        {
            if (string.IsNullOrWhiteSpace(note)) return null;
            var m = Regex.Match(note, @"\b\d{6}\b");
            return m.Success ? m.Value : null;
        }

        private static string RegionToHuman(TRegionsRf region)
        {
            var mem = typeof(TRegionsRf).GetMember(region.ToString()).FirstOrDefault();
            if (mem != null)
            {
                var xml = mem.GetCustomAttributes(typeof(XmlEnumAttribute), false)
                             .OfType<XmlEnumAttribute>()
                             .FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(xml?.Name) && !xml.Name.All(char.IsDigit))
                    return xml.Name;

                var desc = mem.GetCustomAttributes(typeof(DescriptionAttribute), false)
                              .OfType<DescriptionAttribute>()
                              .FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(desc?.Description))
                    return desc.Description;
            }
            return region.ToString();
        }
    }

    /// <summary>
    /// <para>Коды субъектов Российской Федерации</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Коды субъектов Российской Федерации")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tRegionsRF", Namespace = "")]
    public enum TRegionsRf
    {

        /// <summary>
        /// <para>Объект расположен за пределами территории Российской Федерации, во внутренних морских водах, территориальном море и прилежащей зоне Российской Федерации, в исключительной экономической зоне Российской Федерации, в границах российской части (российском секторе) дна Каспийского моря и континентального шельфа Российской Федерации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(@"Объект расположен за пределами территории Российской Федерации, во внутренних морских водах, территориальном море и прилежащей зоне Российской Федерации, в исключительной экономической зоне Российской Федерации, в границах российской части (российском секторе) дна Каспийского моря и континентального шельфа Российской Федерации")]
        [System.Xml.Serialization.XmlEnumAttribute("00")]
        Item00,

        /// <summary>
        /// <para>Республика Адыгея (Адыгея)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Республика Адыгея (Адыгея)")]
        [System.Xml.Serialization.XmlEnumAttribute("01")]
        Item01,

        /// <summary>
        /// <para>Республика Башкортостан</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Республика Башкортостан")]
        [System.Xml.Serialization.XmlEnumAttribute("02")]
        Item02,

        /// <summary>
        /// <para>Республика Бурятия</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Республика Бурятия")]
        [System.Xml.Serialization.XmlEnumAttribute("03")]
        Item03,

        /// <summary>
        /// <para>Республика Алтай</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Республика Алтай")]
        [System.Xml.Serialization.XmlEnumAttribute("04")]
        Item04,

        /// <summary>
        /// <para>Республика Дагестан</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Республика Дагестан")]
        [System.Xml.Serialization.XmlEnumAttribute("05")]
        Item05,

        /// <summary>
        /// <para>Республика Ингушетия</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Республика Ингушетия")]
        [System.Xml.Serialization.XmlEnumAttribute("06")]
        Item06,

        /// <summary>
        /// <para>Кабардино-Балкарская Республика</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Кабардино-Балкарская Республика")]
        [System.Xml.Serialization.XmlEnumAttribute("07")]
        Item07,

        /// <summary>
        /// <para>Республика Калмыкия</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Республика Калмыкия")]
        [System.Xml.Serialization.XmlEnumAttribute("08")]
        Item08,

        /// <summary>
        /// <para>Карачаево-Черкесская Республика</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Карачаево-Черкесская Республика")]
        [System.Xml.Serialization.XmlEnumAttribute("09")]
        Item09,

        /// <summary>
        /// <para>Республика Карелия</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Республика Карелия")]
        [System.Xml.Serialization.XmlEnumAttribute("10")]
        Item10,

        /// <summary>
        /// <para>Республика Коми</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Республика Коми")]
        [System.Xml.Serialization.XmlEnumAttribute("11")]
        Item11,

        /// <summary>
        /// <para>Республика Марий Эл</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Республика Марий Эл")]
        [System.Xml.Serialization.XmlEnumAttribute("12")]
        Item12,

        /// <summary>
        /// <para>Республика Мордовия</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Республика Мордовия")]
        [System.Xml.Serialization.XmlEnumAttribute("13")]
        Item13,

        /// <summary>
        /// <para>Республика Саха (Якутия)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Республика Саха (Якутия)")]
        [System.Xml.Serialization.XmlEnumAttribute("14")]
        Item14,

        /// <summary>
        /// <para>Республика Северная Осетия-Алания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Республика Северная Осетия-Алания")]
        [System.Xml.Serialization.XmlEnumAttribute("15")]
        Item15,

        /// <summary>
        /// <para>Республика Татарстан (Татарстан)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Республика Татарстан (Татарстан)")]
        [System.Xml.Serialization.XmlEnumAttribute("16")]
        Item16,

        /// <summary>
        /// <para>Республика Тыва</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Республика Тыва")]
        [System.Xml.Serialization.XmlEnumAttribute("17")]
        Item17,

        /// <summary>
        /// <para>Удмуртская Республика</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Удмуртская Республика")]
        [System.Xml.Serialization.XmlEnumAttribute("18")]
        Item18,

        /// <summary>
        /// <para>Республика Хакасия</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Республика Хакасия")]
        [System.Xml.Serialization.XmlEnumAttribute("19")]
        Item19,

        /// <summary>
        /// <para>Чеченская Республика</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Чеченская Республика")]
        [System.Xml.Serialization.XmlEnumAttribute("20")]
        Item20,

        /// <summary>
        /// <para>Чувашская Республика-Чувашия</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Чувашская Республика-Чувашия")]
        [System.Xml.Serialization.XmlEnumAttribute("21")]
        Item21,

        /// <summary>
        /// <para>Алтайский край</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Алтайский край")]
        [System.Xml.Serialization.XmlEnumAttribute("22")]
        Item22,

        /// <summary>
        /// <para>Краснодарский край</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Краснодарский край")]
        [System.Xml.Serialization.XmlEnumAttribute("23")]
        Item23,

        /// <summary>
        /// <para>Красноярский край</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Красноярский край")]
        [System.Xml.Serialization.XmlEnumAttribute("24")]
        Item24,

        /// <summary>
        /// <para>Приморский край</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Приморский край")]
        [System.Xml.Serialization.XmlEnumAttribute("25")]
        Item25,

        /// <summary>
        /// <para>Ставропольский край</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ставропольский край")]
        [System.Xml.Serialization.XmlEnumAttribute("26")]
        Item26,

        /// <summary>
        /// <para>Хабаровский край</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Хабаровский край")]
        [System.Xml.Serialization.XmlEnumAttribute("27")]
        Item27,

        /// <summary>
        /// <para>Амурская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Амурская область")]
        [System.Xml.Serialization.XmlEnumAttribute("28")]
        Item28,

        /// <summary>
        /// <para>Архангельская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Архангельская область")]
        [System.Xml.Serialization.XmlEnumAttribute("29")]
        Item29,

        /// <summary>
        /// <para>Астраханская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Астраханская область")]
        [System.Xml.Serialization.XmlEnumAttribute("30")]
        Item30,

        /// <summary>
        /// <para>Белгородская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Белгородская область")]
        [System.Xml.Serialization.XmlEnumAttribute("31")]
        Item31,

        /// <summary>
        /// <para>Брянская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Брянская область")]
        [System.Xml.Serialization.XmlEnumAttribute("32")]
        Item32,

        /// <summary>
        /// <para>Владимирская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Владимирская область")]
        [System.Xml.Serialization.XmlEnumAttribute("33")]
        Item33,

        /// <summary>
        /// <para>Волгоградская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Волгоградская область")]
        [System.Xml.Serialization.XmlEnumAttribute("34")]
        Item34,

        /// <summary>
        /// <para>Вологодская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вологодская область")]
        [System.Xml.Serialization.XmlEnumAttribute("35")]
        Item35,

        /// <summary>
        /// <para>Воронежская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Воронежская область")]
        [System.Xml.Serialization.XmlEnumAttribute("36")]
        Item36,

        /// <summary>
        /// <para>Ивановская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ивановская область")]
        [System.Xml.Serialization.XmlEnumAttribute("37")]
        Item37,

        /// <summary>
        /// <para>Иркутская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Иркутская область")]
        [System.Xml.Serialization.XmlEnumAttribute("38")]
        Item38,

        /// <summary>
        /// <para>Калининградская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Калининградская область")]
        [System.Xml.Serialization.XmlEnumAttribute("39")]
        Item39,

        /// <summary>
        /// <para>Калужская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Калужская область")]
        [System.Xml.Serialization.XmlEnumAttribute("40")]
        Item40,

        /// <summary>
        /// <para>Камчатский край</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Камчатский край")]
        [System.Xml.Serialization.XmlEnumAttribute("41")]
        Item41,

        /// <summary>
        /// <para>Кемеровская область - Кузбасс</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Кемеровская область - Кузбасс")]
        [System.Xml.Serialization.XmlEnumAttribute("42")]
        Item42,

        /// <summary>
        /// <para>Кировская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Кировская область")]
        [System.Xml.Serialization.XmlEnumAttribute("43")]
        Item43,

        /// <summary>
        /// <para>Костромская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Костромская область")]
        [System.Xml.Serialization.XmlEnumAttribute("44")]
        Item44,

        /// <summary>
        /// <para>Курганская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Курганская область")]
        [System.Xml.Serialization.XmlEnumAttribute("45")]
        Item45,

        /// <summary>
        /// <para>Курская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Курская область")]
        [System.Xml.Serialization.XmlEnumAttribute("46")]
        Item46,

        /// <summary>
        /// <para>Ленинградская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ленинградская область")]
        [System.Xml.Serialization.XmlEnumAttribute("47")]
        Item47,

        /// <summary>
        /// <para>Липецкая область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Липецкая область")]
        [System.Xml.Serialization.XmlEnumAttribute("48")]
        Item48,

        /// <summary>
        /// <para>Магаданская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Магаданская область")]
        [System.Xml.Serialization.XmlEnumAttribute("49")]
        Item49,

        /// <summary>
        /// <para>Московская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Московская область")]
        [System.Xml.Serialization.XmlEnumAttribute("50")]
        Item50,

        /// <summary>
        /// <para>Мурманская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Мурманская область")]
        [System.Xml.Serialization.XmlEnumAttribute("51")]
        Item51,

        /// <summary>
        /// <para>Нижегородская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Нижегородская область")]
        [System.Xml.Serialization.XmlEnumAttribute("52")]
        Item52,

        /// <summary>
        /// <para>Новгородская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Новгородская область")]
        [System.Xml.Serialization.XmlEnumAttribute("53")]
        Item53,

        /// <summary>
        /// <para>Новосибирская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Новосибирская область")]
        [System.Xml.Serialization.XmlEnumAttribute("54")]
        Item54,

        /// <summary>
        /// <para>Омская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Омская область")]
        [System.Xml.Serialization.XmlEnumAttribute("55")]
        Item55,

        /// <summary>
        /// <para>Оренбургская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Оренбургская область")]
        [System.Xml.Serialization.XmlEnumAttribute("56")]
        Item56,

        /// <summary>
        /// <para>Орловская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Орловская область")]
        [System.Xml.Serialization.XmlEnumAttribute("57")]
        Item57,

        /// <summary>
        /// <para>Пензенская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Пензенская область")]
        [System.Xml.Serialization.XmlEnumAttribute("58")]
        Item58,

        /// <summary>
        /// <para>Пермский край</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Пермский край")]
        [System.Xml.Serialization.XmlEnumAttribute("59")]
        Item59,

        /// <summary>
        /// <para>Псковская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Псковская область")]
        [System.Xml.Serialization.XmlEnumAttribute("60")]
        Item60,

        /// <summary>
        /// <para>Ростовская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ростовская область")]
        [System.Xml.Serialization.XmlEnumAttribute("61")]
        Item61,

        /// <summary>
        /// <para>Рязанская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Рязанская область")]
        [System.Xml.Serialization.XmlEnumAttribute("62")]
        Item62,

        /// <summary>
        /// <para>Самарская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Самарская область")]
        [System.Xml.Serialization.XmlEnumAttribute("63")]
        Item63,

        /// <summary>
        /// <para>Саратовская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Саратовская область")]
        [System.Xml.Serialization.XmlEnumAttribute("64")]
        Item64,

        /// <summary>
        /// <para>Сахалинская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сахалинская область")]
        [System.Xml.Serialization.XmlEnumAttribute("65")]
        Item65,

        /// <summary>
        /// <para>Свердловская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Свердловская область")]
        [System.Xml.Serialization.XmlEnumAttribute("66")]
        Item66,

        /// <summary>
        /// <para>Смоленская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Смоленская область")]
        [System.Xml.Serialization.XmlEnumAttribute("67")]
        Item67,

        /// <summary>
        /// <para>Тамбовская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Тамбовская область")]
        [System.Xml.Serialization.XmlEnumAttribute("68")]
        Item68,

        /// <summary>
        /// <para>Тверская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Тверская область")]
        [System.Xml.Serialization.XmlEnumAttribute("69")]
        Item69,

        /// <summary>
        /// <para>Томская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Томская область")]
        [System.Xml.Serialization.XmlEnumAttribute("70")]
        Item70,

        /// <summary>
        /// <para>Тульская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Тульская область")]
        [System.Xml.Serialization.XmlEnumAttribute("71")]
        Item71,

        /// <summary>
        /// <para>Тюменская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Тюменская область")]
        [System.Xml.Serialization.XmlEnumAttribute("72")]
        Item72,

        /// <summary>
        /// <para>Ульяновская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ульяновская область")]
        [System.Xml.Serialization.XmlEnumAttribute("73")]
        Item73,

        /// <summary>
        /// <para>Челябинская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Челябинская область")]
        [System.Xml.Serialization.XmlEnumAttribute("74")]
        Item74,

        /// <summary>
        /// <para>Забайкальский край</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Забайкальский край")]
        [System.Xml.Serialization.XmlEnumAttribute("75")]
        Item75,

        /// <summary>
        /// <para>Ярославская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ярославская область")]
        [System.Xml.Serialization.XmlEnumAttribute("76")]
        Item76,

        /// <summary>
        /// <para>Москва</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Москва")]
        [System.Xml.Serialization.XmlEnumAttribute("77")]
        Item77,

        /// <summary>
        /// <para>Санкт-Петербург</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Санкт-Петербург")]
        [System.Xml.Serialization.XmlEnumAttribute("78")]
        Item78,

        /// <summary>
        /// <para>Еврейская автономная область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Еврейская автономная область")]
        [System.Xml.Serialization.XmlEnumAttribute("79")]
        Item79,

        /// <summary>
        /// <para>Ненецкий автономный округ</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ненецкий автономный округ")]
        [System.Xml.Serialization.XmlEnumAttribute("83")]
        Item83,

        /// <summary>
        /// <para>Ханты-Мансийский автономный округ - Югра</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ханты-Мансийский автономный округ - Югра")]
        [System.Xml.Serialization.XmlEnumAttribute("86")]
        Item86,

        /// <summary>
        /// <para>Чукотский автономный округ</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Чукотский автономный округ")]
        [System.Xml.Serialization.XmlEnumAttribute("87")]
        Item87,

        /// <summary>
        /// <para>Ямало-Ненецкий автономный округ</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ямало-Ненецкий автономный округ")]
        [System.Xml.Serialization.XmlEnumAttribute("89")]
        Item89,

        /// <summary>
        /// <para>Республика Крым</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Республика Крым")]
        [System.Xml.Serialization.XmlEnumAttribute("91")]
        Item91,

        /// <summary>
        /// <para>Севастополь</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Севастополь")]
        [System.Xml.Serialization.XmlEnumAttribute("92")]
        Item92,

        /// <summary>
        /// <para>Донецкая Народная Республика</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Донецкая Народная Республика")]
        [System.Xml.Serialization.XmlEnumAttribute("80")]
        Item80,

        /// <summary>
        /// <para>Луганская Народная Республика</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Луганская Народная Республика")]
        [System.Xml.Serialization.XmlEnumAttribute("81")]
        Item81,

        /// <summary>
        /// <para>Херсонская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Херсонская область")]
        [System.Xml.Serialization.XmlEnumAttribute("84")]
        Item84,

        /// <summary>
        /// <para>Запорожская область</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Запорожская область")]
        [System.Xml.Serialization.XmlEnumAttribute("85")]
        Item85,
    }

    /// <summary>
    /// <para>Почтовый адрес</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Почтовый адрес")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tPostAddress", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TPostAddress
    {

        /// <summary>
        /// <para>Номер здания/сооружения</para>
        /// <para>Текстовая строка длиной 200 символа</para>
        /// <para xml:lang="en">Maximum length: 200.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Номер здания/сооружения")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(200)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("Building")]
        public string Building { get; set; }

        /// <summary>
        /// <para>Город</para>
        /// <para>Текстовая строка длиной 200 символа</para>
        /// <para xml:lang="en">Maximum length: 200.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Город")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(200)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("City")]
        public string City { get; set; }

        /// <summary>
        /// <para>Страна</para>
        /// <para>Текстовая строка длиной 200 символа</para>
        /// <para xml:lang="en">Maximum length: 200.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Страна")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(200)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("Country")]
        public string Country { get; set; }

        /// <summary>
        /// <para>Наименование района</para>
        /// <para>Текстовая строка длиной 200 символа</para>
        /// <para xml:lang="en">Maximum length: 200.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Наименование района")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(200)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("District")]
        public string District { get; set; }

        /// <summary>
        /// <para>Неформализованное описание адреса</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Неформализованное описание адреса")]
        [System.Xml.Serialization.XmlElementAttribute("Note")]
        public string Note { get; set; }

        /// <summary>
        /// <para>Почтовый индекс</para>
        /// <para>Почтовый индекс</para>
        /// <para xml:lang="en">Minimum length: 6.</para>
        /// <para xml:lang="en">Maximum length: 6.</para>
        /// <para xml:lang="en">Pattern: \d{6}.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Почтовый индекс")]
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(6)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(6)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("\\d{6}")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = false)]
        [System.Xml.Serialization.XmlElementAttribute("PostIndex")]
        public string PostIndex { get; set; }

        /// <summary>
        /// <para>Код субъекта Российской Федерации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Код субъекта Российской Федерации")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Region")]
        public TRegionsRf Region { get; set; }

        /// <summary>
        /// <para>Номер помещения</para>
        /// <para>Текстовая строка длиной 200 символа</para>
        /// <para xml:lang="en">Maximum length: 200.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Номер помещения")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(200)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("Room")]
        public string Room { get; set; }

        /// <summary>
        /// <para>Населенный пункт</para>
        /// <para>Текстовая строка длиной 200 символа</para>
        /// <para xml:lang="en">Maximum length: 200.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Населенный пункт")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(200)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("Settlement")]
        public string Settlement { get; set; }

        /// <summary>
        /// <para>Улица</para>
        /// <para>Текстовая строка длиной 200 символа</para>
        /// <para xml:lang="en">Maximum length: 200.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Улица")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(200)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("Street")]
        public string Street { get; set; }
    }

    /// <summary>
    /// <para>Юридическое лицо</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Юридическое лицо")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tOrganization", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TOrganization
    {

        /// <summary>
        /// <para>Адрес (местонахождение) юридического лица</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Адрес (местонахождение) юридического лица")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Address")]
        public TAddress Address { get; set; }

        /// <summary>
        /// <para>Адрес электронной почты юридического лица</para>
        /// <para>Текстовая строка - адрес электронной почты</para>
        /// <para xml:lang="en">Pattern: [a-zA-Zа-яА-Я0-9_.\-]{1,}[@]{1}[a-zA-Zа-яА-Я0-9_.\-]{1,}[.]{1}[a-zA-Zа-яА-Я]{2,}.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Адрес электронной почты юридического лица")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("[a-zA-Zа-яА-Я0-9_.\\-]{1,}[@]{1}[a-zA-Zа-яА-Я0-9_.\\-]{1,}[.]{1}[a-zA-Zа-яА-Я]{2,}")]
        [System.Xml.Serialization.XmlElementAttribute("Email")]
        public string Email { get; set; }

        /// <summary>
        /// <para>Полное наименование юридического лица</para>
        /// <para>Текстовая строка длиной 1000 символа</para>
        /// <para xml:lang="en">Maximum length: 1000.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Полное наименование юридического лица")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(1000)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("OrgFullName")]
        public string OrgFullName { get; set; }

        /// <summary>
        /// <para>Индивидуальный номер налогоплательщика - юридического лица</para>
        /// <para>ИНН организации</para>
        /// <para xml:lang="en">Minimum length: 10.</para>
        /// <para xml:lang="en">Maximum length: 10.</para>
        /// <para xml:lang="en">Pattern: \d{10}.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Индивидуальный номер налогоплательщика - юридического лица")]
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(10)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(10)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("\\d{10}")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = false)]
        [System.Xml.Serialization.XmlElementAttribute("OrgINN")]
        public string OrgInn { get; set; }

        /// <summary>
        /// <para>Код причины постановки на учет юридического лица</para>
        /// <para>КПП организации</para>
        /// <para xml:lang="en">Minimum length: 9.</para>
        /// <para xml:lang="en">Maximum length: 9.</para>
        /// <para xml:lang="en">Pattern: \d{9}.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Код причины постановки на учет юридического лица")]
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(9)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(9)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("\\d{9}")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = false)]
        [System.Xml.Serialization.XmlElementAttribute("OrgKPP")]
        public string OrgKpp { get; set; }

        /// <summary>
        /// <para>Основной государственный регистрационный номер юридического лица</para>
        /// <para>ОГРН организации</para>
        /// <para xml:lang="en">Minimum length: 13.</para>
        /// <para xml:lang="en">Maximum length: 13.</para>
        /// <para xml:lang="en">Pattern: \d{13}.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Основной государственный регистрационный номер юридического лица")]
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(13)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(13)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("\\d{13}")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = false)]
        [System.Xml.Serialization.XmlElementAttribute("OrgOGRN")]
        public string OrgOgrn { get; set; }

        public override string ToString()
        {
            var name = string.IsNullOrWhiteSpace(OrgFullName) ? "Юридическое лицо" : OrgFullName.Trim();

            var ids = new List<string>();
            if (!string.IsNullOrWhiteSpace(OrgInn)) ids.Add($"ИНН {OrgInn}");
            if (!string.IsNullOrWhiteSpace(OrgKpp)) ids.Add($"КПП {OrgKpp}");
            if (!string.IsNullOrWhiteSpace(OrgOgrn)) ids.Add($"ОГРН {OrgOgrn}");

            return ids.Count > 0 ? $"{name} ({string.Join(", ", ids)})" : name;
        }
    }

    /// <summary>
    /// <para>Иностранное юридическое лицо (филиал, представительство)</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Иностранное юридическое лицо (филиал, представительство)")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tForeignOrganization", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TForeignOrganization
    {

        /// <summary>
        /// <para>Адрес (местонахождение) филиала или представительства иностранного юридического лица в Российской Федерации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Адрес (местонахождение) филиала или представительства иностранного юридического л" +
            "ица в Российской Федерации"))]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Address")]
        public TAddress Address { get; set; }

        /// <summary>
        /// <para>Адрес электронной почты юридического лица (филиала, представительства)</para>
        /// <para>Текстовая строка - адрес электронной почты</para>
        /// <para xml:lang="en">Pattern: [a-zA-Zа-яА-Я0-9_.\-]{1,}[@]{1}[a-zA-Zа-яА-Я0-9_.\-]{1,}[.]{1}[a-zA-Zа-яА-Я]{2,}.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Адрес электронной почты юридического лица (филиала, представительства)")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("[a-zA-Zа-яА-Я0-9_.\\-]{1,}[@]{1}[a-zA-Zа-яА-Я0-9_.\\-]{1,}[.]{1}[a-zA-Zа-яА-Я]{2,}")]
        [System.Xml.Serialization.XmlElementAttribute("Email")]
        public string Email { get; set; }

        /// <summary>
        /// <para>Полное наименование иностранного юридического лица (филиала, представительства)</para>
        /// <para>Текстовая строка длиной 1000 символа</para>
        /// <para xml:lang="en">Maximum length: 1000.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Полное наименование иностранного юридического лица (филиала, представительства)")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(1000)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("OrgFullName")]
        public string OrgFullName { get; set; }

        /// <summary>
        /// <para>Индивидуальный номер налогоплательщика – юридического лица (филиала, представительства)</para>
        /// <para>ИНН организации</para>
        /// <para xml:lang="en">Minimum length: 10.</para>
        /// <para xml:lang="en">Maximum length: 10.</para>
        /// <para xml:lang="en">Pattern: \d{10}.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Индивидуальный номер налогоплательщика – юридического лица (филиала, представител" +
            "ьства)"))]
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(10)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(10)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("\\d{10}")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = false)]
        [System.Xml.Serialization.XmlElementAttribute("OrgINN")]
        public string OrgInn { get; set; }

        /// <summary>
        /// <para>Код причины постановки на учет иностранного юридического лица (филиала, представительства)</para>
        /// <para>КПП организации</para>
        /// <para xml:lang="en">Minimum length: 9.</para>
        /// <para xml:lang="en">Maximum length: 9.</para>
        /// <para xml:lang="en">Pattern: \d{9}.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Код причины постановки на учет иностранного юридического лица (филиала, представи" +
            "тельства)"))]
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(9)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(9)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("\\d{9}")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = false)]
        [System.Xml.Serialization.XmlElementAttribute("OrgKPP")]
        public string OrgKpp { get; set; }

        public override string ToString()
        {
            var name = string.IsNullOrWhiteSpace(OrgFullName) ? "Иностранная организация" : OrgFullName.Trim();

            var ids = new List<string>();
            if (!string.IsNullOrWhiteSpace(OrgInn)) ids.Add($"ИНН {OrgInn}");
            if (!string.IsNullOrWhiteSpace(OrgKpp)) ids.Add($"КПП {OrgKpp}");

            return ids.Count > 0 ? $"{name} ({string.Join(", ", ids)})" : name;
        }
    }

    /// <summary>
    /// <para>Индивидуальный предприниматель</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Индивидуальный предприниматель")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tIP", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Tip
    {

        /// <summary>
        /// <para>Адрес электронной почты индивидуального предпринимателя</para>
        /// <para>Текстовая строка - адрес электронной почты</para>
        /// <para xml:lang="en">Pattern: [a-zA-Zа-яА-Я0-9_.\-]{1,}[@]{1}[a-zA-Zа-яА-Я0-9_.\-]{1,}[.]{1}[a-zA-Zа-яА-Я]{2,}.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Адрес электронной почты индивидуального предпринимателя")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("[a-zA-Zа-яА-Я0-9_.\\-]{1,}[@]{1}[a-zA-Zа-яА-Я0-9_.\\-]{1,}[.]{1}[a-zA-Zа-яА-Я]{2,}")]
        [System.Xml.Serialization.XmlElementAttribute("Email")]
        public string Email { get; set; }

        /// <summary>
        /// <para>Фамилия</para>
        /// <para>Текстовая строка длиной 100 символа</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Фамилия")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("FamilyName")]
        public string FamilyName { get; set; }

        /// <summary>
        /// <para>Имя</para>
        /// <para>Текстовая строка длиной 100 символа</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Имя")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// <para>Основной государственный регистрационный номер индивидуального предпринимателя</para>
        /// <para>ОГРНИП</para>
        /// <para xml:lang="en">Minimum length: 15.</para>
        /// <para xml:lang="en">Maximum length: 15.</para>
        /// <para xml:lang="en">Pattern: \d{15}.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Основной государственный регистрационный номер индивидуального предпринимателя")]
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(15)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(15)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("\\d{15}")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = false)]
        [System.Xml.Serialization.XmlElementAttribute("OGRNIP")]
        public string Ogrnip { get; set; }

        /// <summary>
        /// <para>Почтовый адрес индивидуального предпринимателя</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Почтовый адрес индивидуального предпринимателя")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("PostAddress")]
        public TPostAddress PostAddress { get; set; }

        /// <summary>
        /// <para>Отчество</para>
        /// <para>Текстовая строка длиной 100 символа</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Отчество")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("SecondName")]
        public string SecondName { get; set; }
        public override string ToString()
        {
            var fio = string.Join(" ",
                new[] { FamilyName, FirstName, SecondName }
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim()));

            var caption = string.IsNullOrWhiteSpace(fio) ? "ИП" : $"ИП {fio}";
            return !string.IsNullOrWhiteSpace(Ogrnip) ? $"{caption} (ОГРНИП {Ogrnip})" : caption;
        }
    }

    /// <summary>
    /// <para>Физическое лицо</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Физическое лицо")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tPerson", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TPerson
    {

        /// <summary>
        /// <para>Адрес электронной почты</para>
        /// <para>Текстовая строка - адрес электронной почты</para>
        /// <para xml:lang="en">Pattern: [a-zA-Zа-яА-Я0-9_.\-]{1,}[@]{1}[a-zA-Zа-яА-Я0-9_.\-]{1,}[.]{1}[a-zA-Zа-яА-Я]{2,}.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Адрес электронной почты")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("[a-zA-Zа-яА-Я0-9_.\\-]{1,}[@]{1}[a-zA-Zа-яА-Я0-9_.\\-]{1,}[.]{1}[a-zA-Zа-яА-Я]{2,}")]
        [System.Xml.Serialization.XmlElementAttribute("Email")]
        public string Email { get; set; }

        /// <summary>
        /// <para>Фамилия</para>
        /// <para>Текстовая строка длиной 100 символа</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Фамилия")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("FamilyName")]
        public string FamilyName { get; set; }

        /// <summary>
        /// <para>Имя</para>
        /// <para>Текстовая строка длиной 100 символа</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Имя")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// <para>Почтовый адрес физического лица</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Почтовый адрес физического лица")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("PostAddress")]
        public TPostAddress PostAddress { get; set; }

        /// <summary>
        /// <para>Отчество</para>
        /// <para>Текстовая строка длиной 100 символа</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Отчество")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("SecondName")]
        public string SecondName { get; set; }

        /// <summary>
        /// <para>Страховой номер индивидуального лицевого счёта</para>
        /// <para>СНИЛС</para>
        /// <para xml:lang="en">Pattern: [0-9]{3}-[0-9]{3}-[0-9]{3} [0-9]{2}.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Страховой номер индивидуального лицевого счёта")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("[0-9]{3}-[0-9]{3}-[0-9]{3} [0-9]{2}")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("SNILS")]
        public string Snils { get; set; }

        public override string ToString()
        {
            var fio = string.Join(" ",
                new[] { FamilyName, FirstName, SecondName }
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim()));

            return string.IsNullOrWhiteSpace(fio) ? "Физическое лицо" : $"ФЛ {fio}";
        }
    }

    /// <summary>
    /// <para>Должностное лицо</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Должностное лицо")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tWorkPerson", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TWorkPerson
    {

        /// <summary>
        /// <para>Фамилия</para>
        /// <para>Текстовая строка длиной 100 символа</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Фамилия")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("FamilyName")]
        public string FamilyName { get; set; }

        /// <summary>
        /// <para>Имя</para>
        /// <para>Текстовая строка длиной 100 символа</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Имя")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// <para>Должность</para>
        /// <para>Текстовая строка длиной 1000 символа</para>
        /// <para xml:lang="en">Maximum length: 1000.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Должность")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(1000)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Position")]
        public string Position { get; set; }

        /// <summary>
        /// <para>Отчество</para>
        /// <para>Текстовая строка длиной 50 символа</para>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Отчество")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.Xml.Serialization.XmlElementAttribute("SecondName")]
        public string SecondName { get; set; }
    }

    /// <summary>
    /// <para>Описание объекта экспертизы</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Описание объекта экспертизы")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tExaminationObject", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TExaminationObject
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TExaminationType> _examinationType;

        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="TExaminationObject" /> class.</para>
        /// </summary>
        public TExaminationObject()
        {
            this._examinationType = new System.Collections.ObjectModel.Collection<TExaminationType>();
        }

        /// <summary>
        /// <para>Вид работ</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вид работ")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ConstructionType")]
        public TConstractionType ConstructionType { get; set; }

        /// <summary>
        /// <para>Сведения о подготовке отчетной документации о выполнении инженерных изысканий в форме информационной модели</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сведения о подготовке отчетной документации о выполнении инженерных изысканий в ф" +
            "орме информационной модели"))]
        [System.Xml.Serialization.XmlElementAttribute("EngineeringSurveysIM")]
        public TYesNo EngineeringSurveysIm { get; set; }

        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the EngineeringSurveysIm property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EngineeringSurveysImSpecified { get; set; }

        /// <summary>
        /// <para>Форма экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Форма экспертизы")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ExaminationForm")]
        public TExaminationForm ExaminationForm { get; set; }

        /// <summary>
        /// <para>Вид объекта экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вид объекта экспертизы")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ExaminationObjectType")]
        public TExaminationObjectType ExaminationObjectType { get; set; }

        /// <summary>
        /// <para>Результат экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Результат экспертизы")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ExaminationResult")]
        public TExaminationResult ExaminationResult { get; set; }

        /// <summary>
        /// <para>Вид экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вид экспертизы")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ExaminationStage")]
        public TExaminationStage ExaminationStage { get; set; }

        /// <summary>
        /// <para>Дополнительные сведения о виде экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Дополнительные сведения о виде экспертизы")]
        [System.Xml.Serialization.XmlElementAttribute("ExaminationStageNote")]
        public string ExaminationStageNote { get; set; }

        /// <summary>
        /// <para>Предмет экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Предмет экспертизы")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ExaminationType")]
        public System.Collections.ObjectModel.Collection<TExaminationType> ExaminationType
        {
            get
            {
                return _examinationType;
            }
            private set
            {
                _examinationType = value;
            }
        }

        /// <summary>
        /// <para>Наименование объекта экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Наименование объекта экспертизы")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// <para>Сведения о подготовке проектной документации в форме информационной модели</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сведения о подготовке проектной документации в форме информационной модели")]
        [System.Xml.Serialization.XmlElementAttribute("ProjectDocumentationIM")]
        public TYesNo ProjectDocumentationIm { get; set; }

        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the ProjectDocumentationIm property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ProjectDocumentationImSpecified { get; set; }
    }

    /// <summary>
    /// <para>Форма экспертизы</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Форма экспертизы")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tExaminationForm", Namespace = "")]
    public enum TExaminationForm
    {

        /// <summary>
        /// <para>Государственная</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Государственная")]
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <summary>
        /// <para>Негосударственная</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Негосударственная")]
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,
    }

    /// <summary>
    /// <para>Результат экспертизы</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Результат экспертизы")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tExaminationResult", Namespace = "")]
    public enum TExaminationResult
    {

        /// <summary>
        /// <para>Положительный</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Положительный")]
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <summary>
        /// <para>Отрицательный</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Отрицательный")]
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,
    }

    /// <summary>
    /// <para>Вид объекта экспертизы</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Вид объекта экспертизы")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tExaminationObjectType", Namespace = "")]
    public enum TExaminationObjectType
    {

        /// <summary>
        /// <para>Результаты инженерных изысканий</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Результаты инженерных изысканий")]
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <summary>
        /// <para>Проектная документация</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Проектная документация")]
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <summary>
        /// <para>Проектная документация и результаты инженерных изысканий</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Проектная документация и результаты инженерных изысканий")]
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,
    }

    /// <summary>
    /// <para>Предмет экспертизы</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Предмет экспертизы")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tExaminationType", Namespace = "")]
    public enum TExaminationType
    {

        /// <summary>
        /// <para>Оценка соответствия результатов инженерных изысканий требованиям технических регламентов (абзац 1 пункта 5 статьи 49 Градостроительного кодекса Российской Федерации)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Оценка соответствия результатов инженерных изысканий требованиям технических регл" +
            "аментов (абзац 1 пункта 5 статьи 49 Градостроительного кодекса Российской Федера" +
            "ции)"))]
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <summary>
        /// <para>Оценка соответствия проектной документации установленным требованиям (подпункт 1 пункт 5 статьи 49 Градостроительного кодекса Российской Федерации)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Оценка соответствия проектной документации установленным требованиям (подпункт 1 " +
            "пункт 5 статьи 49 Градостроительного кодекса Российской Федерации)"))]
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <summary>
        /// <para>Проверка достоверности определения сметной стоимости (подпункт 2 пункт 5 статьи 49 Градостроительного кодекса Российской Федерации)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Проверка достоверности определения сметной стоимости (подпункт 2 пункт 5 статьи 4" +
            "9 Градостроительного кодекса Российской Федерации)"))]
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,
    }

    /// <summary>
    /// <para>Вид работ</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Вид работ")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tConstractionType", Namespace = "")]
    public enum TConstractionType
    {

        /// <summary>
        /// <para>Строительство</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Строительство")]
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <summary>
        /// <para>Реконструкция</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Реконструкция")]
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <summary>
        /// <para>Капитальный ремонт</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Капитальный ремонт")]
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,

        /// <summary>
        /// <para>Снос</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Снос")]
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        Item4,

        /// <summary>
        /// <para>Сохранение объекта культурного наследия</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сохранение объекта культурного наследия")]
        [System.Xml.Serialization.XmlEnumAttribute("5")]
        Item5,
    }

    /// <summary>
    /// <para>Вид экспертизы</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Вид экспертизы")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tExaminationStage", Namespace = "")]
    public enum TExaminationStage
    {

        /// <summary>
        /// <para>Первичная</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Первичная")]
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <summary>
        /// <para>Повторная</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Повторная")]
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <summary>
        /// <para>По результатам экспертного сопровождения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("По результатам экспертного сопровождения")]
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,
    }

    /// <summary>
    /// <para>Непустая текстовая строка, содержащая одно из значений "да" или "нет"</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Непустая текстовая строка, содержащая одно из значений \"да\" или \"нет\"")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tYesNo", Namespace = "")]
    public enum TYesNo
    {

        /// <summary>
        /// <para>Логическое значение "да"/"истина"</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Логическое значение \"да\"/\"истина\"")]
        [System.Xml.Serialization.XmlEnumAttribute("да")]
        Да,

        /// <summary>
        /// <para>Логическое значение "нет"/"ложь"</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Логическое значение \"нет\"/\"ложь\"")]
        [System.Xml.Serialization.XmlEnumAttribute("нет")]
        Нет,
    }

    /// <summary>
    /// <para>Документы, рассмотренные в рамках экспертизы</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Документы, рассмотренные в рамках экспертизы")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tDocuments", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TDocuments
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TDocument> _document;

        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="TDocuments" /> class.</para>
        /// </summary>
        public TDocuments()
        {
            this._document = new System.Collections.ObjectModel.Collection<TDocument>();
        }

        /// <summary>
        /// <para>Документ</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Документ")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Document")]
        public System.Collections.ObjectModel.Collection<TDocument> Document
        {
            get
            {
                return _document;
            }
            private set
            {
                _document = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tDocument", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TDocument
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TFile> _file;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TDeclarant> _fullDocIssueAuthor;

        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="TDocument" /> class.</para>
        /// </summary>
        public TDocument()
        {
            this._fullDocIssueAuthor = new System.Collections.ObjectModel.Collection<TDeclarant>();
            this._file = new System.Collections.ObjectModel.Collection<TFile>();
        }

        /// <summary>
        /// <para>Примечание об изменении документа</para>
        /// <para>Текстовая строка длиной 50 символа</para>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Примечание об изменении документа")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.Xml.Serialization.XmlElementAttribute("DocChanges")]
        public string DocChanges { get; set; }

        /// <summary>
        /// <para>Дата документа</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Дата документа")]
        [System.Xml.Serialization.XmlElementAttribute("DocDate", DataType = "date")]
        public System.DateTime DocDate { get; set; }

        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the DocDate property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DocDateSpecified { get; set; }

        /// <summary>
        /// <para>Организации, ИП или физическое лицо - автор документа</para>
        /// <para>Текстовая строка длиной 1000 символа</para>
        /// <para xml:lang="en">Maximum length: 1000.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Организации, ИП или физическое лицо - автор документа")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(1000)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("DocIssueAuthor")]
        public string DocIssueAuthor { get; set; }

        /// <summary>
        /// <para>Наименование документа</para>
        /// <para>Текстовая строка длиной 1000 символа</para>
        /// <para xml:lang="en">Maximum length: 1000.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Наименование документа")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(1000)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("DocName")]
        public string DocName { get; set; }

        /// <summary>
        /// <para>Номер (обозначение, шифр) документа</para>
        /// <para>Текстовая строка длиной 100 символа</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Номер (обозначение, шифр) документа")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("DocNumber")]
        public string DocNumber { get; set; }

        /// <summary>
        /// <para>Код типа документа</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Код типа документа")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("DocType")]
        public TDocumentType DocType { get; set; }

        /// <summary>
        /// <para>Файл документа или файл фрагмента документа, поданного для проведения экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Файл документа или файл фрагмента документа, поданного для проведения экспертизы")]
        [System.Xml.Serialization.XmlElementAttribute("File")]
        public System.Collections.ObjectModel.Collection<TFile> File
        {
            get
            {
                return _file;
            }
            private set
            {
                _file = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the File collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FileSpecified
        {
            get
            {
                return (this.File.Count != 0);
            }
        }

        /// <summary>
        /// <para>Организации, ИП или физическое лицо - автор документа</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Организации, ИП или физическое лицо - автор документа")]
        [System.Xml.Serialization.XmlElementAttribute("FullDocIssueAuthor")]
        public System.Collections.ObjectModel.Collection<TDeclarant> FullDocIssueAuthor
        {
            get
            {
                return _fullDocIssueAuthor;
            }
            private set
            {
                _fullDocIssueAuthor = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the FullDocIssueAuthor collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FullDocIssueAuthorSpecified
        {
            get
            {
                return (this.FullDocIssueAuthor.Count != 0);
            }
        }

        /// <summary>
        /// <para>Сведения о документе, приложением к которому является текущий документ и в составе которого поступил на экспертизу</para>
        /// <para>Текстовая строка длиной 1000 символа</para>
        /// <para xml:lang="en">Maximum length: 1000.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сведения о документе, приложением к которому является текущий документ и в состав" +
            "е которого поступил на экспертизу"))]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(1000)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("Included")]
        public string Included { get; set; }
    }

    /// <summary>
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tDocumentType", Namespace = "")]
    public enum TDocumentType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("01.01")]
        Item0101,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("01.02")]
        Item0102,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("01.03")]
        Item0103,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("01.99")]
        Item0199,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("02.01")]
        Item0201,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("02.02")]
        Item0202,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("02.04")]
        Item0204,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("02.05")]
        Item0205,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("02.06")]
        Item0206,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("02.07")]
        Item0207,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("02.08")]
        Item0208,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("02.09")]
        Item0209,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("02.99")]
        Item0299,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("03.01")]
        Item0301,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("03.02")]
        Item0302,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("03.03")]
        Item0303,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("03.04")]
        Item0304,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("03.05")]
        Item0305,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("03.06")]
        Item0306,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("03.99")]
        Item0399,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("04.01")]
        Item0401,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("04.02")]
        Item0402,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("04.03")]
        Item0403,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("04.99")]
        Item0499,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("05.01")]
        Item0501,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("05.02")]
        Item0502,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("05.03")]
        Item0503,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("05.99")]
        Item0599,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("06.01")]
        Item0601,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("06.02")]
        Item0602,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("06.03")]
        Item0603,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("06.04")]
        Item0604,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("06.05")]
        Item0605,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("06.06")]
        Item0606,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("06.07")]
        Item0607,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("06.08")]
        Item0608,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("06.09")]
        Item0609,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("06.10")]
        Item0610,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("06.11")]
        Item0611,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("06.99")]
        Item0699,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.01")]
        Item0701,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.02")]
        Item0702,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.03")]
        Item0703,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.04")]
        Item0704,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.05")]
        Item0705,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.06")]
        Item0706,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.07")]
        Item0707,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.08")]
        Item0708,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.09")]
        Item0709,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.10")]
        Item0710,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.11")]
        Item0711,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.12")]
        Item0712,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.13")]
        Item0713,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.14")]
        Item0714,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.15")]
        Item0715,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.16")]
        Item0716,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.17")]
        Item0717,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.19")]
        Item0719,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.20")]
        Item0720,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.21")]
        Item0721,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.22")]
        Item0722,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.23")]
        Item0723,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.24")]
        Item0724,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.25")]
        Item0725,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.26")]
        Item0726,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.27")]
        Item0727,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.28")]
        Item0728,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.29")]
        Item0729,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.30")]
        Item0730,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.31")]
        Item0731,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.32")]
        Item0732,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.33")]
        Item0733,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.34")]
        Item0734,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.35")]
        Item0735,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("07.37")]
        Item0737,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("08.01")]
        Item0801,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("08.02")]
        Item0802,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("08.03")]
        Item0803,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("08.04")]
        Item0804,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("08.05")]
        Item0805,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("08.06")]
        Item0806,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("08.07")]
        Item0807,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("08.08")]
        Item0808,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("08.10")]
        Item0810,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("08.11")]
        Item0811,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("08.12")]
        Item0812,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("08.13")]
        Item0813,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("08.14")]
        Item0814,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("08.15")]
        Item0815,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("08.16")]
        Item0816,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("08.17")]
        Item0817,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("08.18")]
        Item0818,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("08.20")]
        Item0820,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("09.01")]
        Item0901,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("09.02")]
        Item0902,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("09.99")]
        Item0999,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("10.01")]
        Item1001,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("10.99")]
        Item1099,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("11.01")]
        Item1101,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("11.02")]
        Item1102,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("11.03")]
        Item1103,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("11.04")]
        Item1104,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("11.05")]
        Item1105,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("11.06")]
        Item1106,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("11.99")]
        Item1199,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("12.01")]
        Item1201,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("12.02")]
        Item1202,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("12.03")]
        Item1203,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("12.99")]
        Item1299,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("13.01")]
        Item1301,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("13.02")]
        Item1302,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("13.03")]
        Item1303,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("13.04")]
        Item1304,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("13.05")]
        Item1305,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("13.06")]
        Item1306,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("13.99")]
        Item1399,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("14.01")]
        Item1401,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("14.02")]
        Item1402,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("14.03")]
        Item1403,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("14.04")]
        Item1404,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("14.99")]
        Item1499,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("15.01")]
        Item1501,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("15.02")]
        Item1502,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("15.03")]
        Item1503,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("15.04")]
        Item1504,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("15.05")]
        Item1505,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("15.06")]
        Item1506,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("15.07")]
        Item1507,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("15.08")]
        Item1508,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("15.99")]
        Item1599,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("16.01")]
        Item1601,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("16.02")]
        Item1602,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("16.99")]
        Item1699,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("17.01")]
        Item1701,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("17.99")]
        Item1799,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("18.01")]
        Item1801,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("18.02")]
        Item1802,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("18.03")]
        Item1803,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("18.99")]
        Item1899,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("19.01")]
        Item1901,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("19.02")]
        Item1902,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("19.03")]
        Item1903,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("19.99")]
        Item1999,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("20.01")]
        Item2001,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("20.02")]
        Item2002,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("20.03")]
        Item2003,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("20.04")]
        Item2004,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("20.05")]
        Item2005,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("20.06")]
        Item2006,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("20.07")]
        Item2007,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("20.99")]
        Item2099,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("21.01")]
        Item2101,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("21.02")]
        Item2102,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("21.03")]
        Item2103,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("21.04")]
        Item2104,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("21.05")]
        Item2105,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("21.06")]
        Item2106,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("21.07")]
        Item2107,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("21.08")]
        Item2108,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("21.09")]
        Item2109,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("21.10")]
        Item2110,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("21.11")]
        Item2111,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("21.12")]
        Item2112,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("21.13")]
        Item2113,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("21.99")]
        Item2199,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("22.01")]
        Item2201,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("22.02")]
        Item2202,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("22.03")]
        Item2203,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("22.04")]
        Item2204,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("22.05")]
        Item2205,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("22.99")]
        Item2299,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("23.01")]
        Item2301,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("23.02")]
        Item2302,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("23.99")]
        Item2399,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("24.01")]
        Item2401,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("25.01")]
        Item2501,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("25.02")]
        Item2502,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("25.03")]
        Item2503,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("25.04")]
        Item2504,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("25.05")]
        Item2505,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("25.06")]
        Item2506,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("26.01")]
        Item2601,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("26.02")]
        Item2602,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("26.03")]
        Item2603,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("26.04")]
        Item2604,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("99.01")]
        Item9901,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("99.02")]
        Item9902,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("99.03")]
        Item9903,

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("99.99")]
        Item9999,
    }

    /// <summary>
    /// <para>Тип определяющий лицо, участвующее в процессе подготовки документации, РИИ И ПД и подачи на экспертизу</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute(("Тип определяющий лицо, участвующее в процессе подготовки документации, РИИ И ПД и" +
        " подачи на экспертизу"))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tDeclarant", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TDeclarant
    {

        /// <summary>
        /// <para>Иностранное юридическое лицо (представительство, филиал)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Иностранное юридическое лицо (представительство, филиал)")]
        [System.Xml.Serialization.XmlElementAttribute("ForeignOrganization")]
        public TForeignOrganization ForeignOrganization { get; set; }

        /// <summary>
        /// <para>Индивидуальный предприниматель</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Индивидуальный предприниматель")]
        [System.Xml.Serialization.XmlElementAttribute("IP")]
        public Tip Ip { get; set; }

        /// <summary>
        /// <para>Юридическое лицо</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Юридическое лицо")]
        [System.Xml.Serialization.XmlElementAttribute("Organization")]
        public TOrganization Organization { get; set; }

        /// <summary>
        /// <para>Физическое лицо</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Физическое лицо")]
        [System.Xml.Serialization.XmlElementAttribute("Person")]
        public TPerson Person { get; set; }

        public override string ToString()
        {
            return (Organization as object)?.ToString()
                ?? ForeignOrganization?.ToString()
                ?? Ip?.ToString()
                ?? Person?.ToString()
                ?? string.Empty;
        }
    }

    /// <summary>
    /// <para>Файл документа или файл фрагмента документа</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Файл документа или файл фрагмента документа")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tFile", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TFile
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TSignFile> _signFile;

        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="TFile" /> class.</para>
        /// </summary>
        public TFile()
        {
            this._signFile = new System.Collections.ObjectModel.Collection<TSignFile>();
        }

        /// <summary>
        /// <para>Контрольная сумма CRC32 файла документа</para>
        /// <para>Текстовая строка длиной 8 символов</para>
        /// <para xml:lang="en">Pattern: [0-9a-fA-F]{8}.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Контрольная сумма CRC32 файла документа")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("[0-9a-fA-F]{8}")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("FileChecksum")]
        public string FileChecksum { get; set; }

        /// <summary>
        /// <para>Формат файла документа</para>
        /// <para>Текстовая строка длиной 4 символа</para>
        /// <para xml:lang="en">Maximum length: 4.</para>
        /// <para xml:lang="en">Pattern: [a-z0-9]+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Формат файла документа")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(4)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("[a-z0-9]+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("FileFormat")]
        public string FileFormat { get; set; }

        /// <summary>
        /// <para>Имя файла документа</para>
        /// <para>Текстовая строка длиной до 255 символов - имя файла</para>
        /// <para xml:lang="en">Maximum length: 255.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Имя файла документа")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(255)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("FileName")]
        public string FileName { get; set; }

        /// <summary>
        /// <para>Файл подписи к основному файлу документа</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Файл подписи к основному файлу документа")]
        [System.Xml.Serialization.XmlElementAttribute("SignFile")]
        public System.Collections.ObjectModel.Collection<TSignFile> SignFile
        {
            get
            {
                return _signFile;
            }
            private set
            {
                _signFile = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the SignFile collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SignFileSpecified
        {
            get
            {
                return (this.SignFile.Count != 0);
            }
        }
    }

    /// <summary>
    /// <para>Файл подписи к документу</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Файл подписи к документу")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tSignFile", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TSignFile
    {

        /// <summary>
        /// <para>Контрольная сумма CRC32 файла подписи документа</para>
        /// <para>Текстовая строка длиной 8 символов</para>
        /// <para xml:lang="en">Pattern: [0-9a-fA-F]{8}.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Контрольная сумма CRC32 файла подписи документа")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("[0-9a-fA-F]{8}")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("FileChecksum")]
        public string FileChecksum { get; set; }

        /// <summary>
        /// <para>Формат файла подписи к документу</para>
        /// <para>Текстовая строка длиной 4 символа</para>
        /// <para xml:lang="en">Maximum length: 4.</para>
        /// <para xml:lang="en">Pattern: [a-z0-9]+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Формат файла подписи к документу")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(4)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("[a-z0-9]+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("FileFormat")]
        public string FileFormat { get; set; }

        /// <summary>
        /// <para>Имя файла подписи к документу</para>
        /// <para>Текстовая строка длиной до 255 символов - имя файла</para>
        /// <para xml:lang="en">Maximum length: 255.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Имя файла подписи к документу")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(255)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("FileName")]
        public string FileName { get; set; }
    }

    /// <summary>
    /// <para>Сведения о ранее выданных заключениях экспертизы в отношении объекта капитального строительства, проектная документация и (или) результаты инженерных изысканий по которому представлены для проведения (повторной) экспертизы</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute(("Сведения о ранее выданных заключениях экспертизы в отношении объекта капитального" +
        " строительства, проектная документация и (или) результаты инженерных изысканий п" +
        "о которому представлены для проведения (повторной) экспертизы"))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tPreviousConclusions", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TPreviousConclusions
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TPreviousConclusion> _previousConclusion;

        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="TPreviousConclusions" /> class.</para>
        /// </summary>
        public TPreviousConclusions()
        {
            this._previousConclusion = new System.Collections.ObjectModel.Collection<TPreviousConclusion>();
        }

        /// <summary>
        /// <para>Сведения о ранее выданном заключении экспертизы в отношении объекта капитального строительства, проектная документация и (или) результаты инженерных изысканий по которому представлены для проведения (повторной) экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сведения о ранее выданном заключении экспертизы в отношении объекта капитального " +
            "строительства, проектная документация и (или) результаты инженерных изысканий по" +
            " которому представлены для проведения (повторной) экспертизы"))]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("PreviousConclusion")]
        public System.Collections.ObjectModel.Collection<TPreviousConclusion> PreviousConclusion
        {
            get
            {
                return _previousConclusion;
            }
            private set
            {
                _previousConclusion = value;
            }
        }
    }

    /// <summary>
    /// <para>Сведения о ранее выданном заключении экспертизы в отношении объекта капитального строительства, проектная документация и (или) результаты инженерных изысканий по которому представлены для проведения (повторной) экспертизы</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute(("Сведения о ранее выданном заключении экспертизы в отношении объекта капитального " +
        "строительства, проектная документация и (или) результаты инженерных изысканий по" +
        " которому представлены для проведения (повторной) экспертизы"))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tPreviousConclusion", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TPreviousConclusion
    {

        /// <summary>
        /// <para>Дата заключения экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Дата заключения экспертизы")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Date", DataType = "date")]
        public System.DateTime Date { get; set; }

        /// <summary>
        /// <para>Вид объекта экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вид объекта экспертизы")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ExaminationObjectType")]
        public TExaminationObjectType ExaminationObjectType { get; set; }

        /// <summary>
        /// <para>Наименование объекта экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Наименование объекта экспертизы")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// <para>Номер заключения экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Номер заключения экспертизы")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Number")]
        public TConclusionNumber Number { get; set; }

        /// <summary>
        /// <para>Результат экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Результат экспертизы")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Result")]
        public TExaminationResult Result { get; set; }
    }

    /// <summary>
    /// <para>Номер заключения экспертизы</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Номер заключения экспертизы")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tConclusionNumber", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TConclusionNumber
    {

        /// <summary>
        /// <para>Номер заключения экспертизы в формате ЕГРЗ</para>
        /// <para>Номер заключения экспертизы (формат ЕГРЗ)</para>
        /// <para xml:lang="en">Pattern: [0-9]{2}-[1-2]{1}-[1-2]{1}-[1-3]{1}-[0-9]{6}-[0-9]{4}.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Номер заключения экспертизы в формате ЕГРЗ")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("[0-9]{2}-[1-2]{1}-[1-2]{1}-[1-3]{1}-[0-9]{6}-[0-9]{4}")]
        [System.Xml.Serialization.XmlElementAttribute("EGRZ")]
        public string Egrz { get; set; }

        /// <summary>
        /// <para>Номер заключения экспертизы в произвольном формате</para>
        /// <para>Текстовая строка длиной 50 символа</para>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Номер заключения экспертизы в произвольном формате")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.Xml.Serialization.XmlElementAttribute("noEGRZ")]
        public string NoEgrz { get; set; }
    }

    /// <summary>
    /// <para>Сведения о ранее выданных заключениях по результатам оценки соответствия в рамках экспертного сопровождения в отношении объекта капитального строительства, проектная документация по которому представлена для проведения оценки соответствия в рамках экспертного сопровождения</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute(@"Сведения о ранее выданных заключениях по результатам оценки соответствия в рамках экспертного сопровождения в отношении объекта капитального строительства, проектная документация по которому представлена для проведения оценки соответствия в рамках экспертного сопровождения")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tPreviousSimpleConclusions", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TPreviousSimpleConclusions
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TPreviousSimpleConclusion> _previousSimpleConclusion;

        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="TPreviousSimpleConclusions" /> class.</para>
        /// </summary>
        public TPreviousSimpleConclusions()
        {
            this._previousSimpleConclusion = new System.Collections.ObjectModel.Collection<TPreviousSimpleConclusion>();
        }

        /// <summary>
        /// <para>Сведения о ранее выданном заключении по результатам оценки соответствия в рамках экспертного сопровождения в отношении объекта капитального строительства, проектная документация по которому представлена для проведения оценки соответствия в рамках экспертного сопровождения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(@"Сведения о ранее выданном заключении по результатам оценки соответствия в рамках экспертного сопровождения в отношении объекта капитального строительства, проектная документация по которому представлена для проведения оценки соответствия в рамках экспертного сопровождения")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("PreviousSimpleConclusion")]
        public System.Collections.ObjectModel.Collection<TPreviousSimpleConclusion> PreviousSimpleConclusion
        {
            get
            {
                return _previousSimpleConclusion;
            }
            private set
            {
                _previousSimpleConclusion = value;
            }
        }
    }

    /// <summary>
    /// <para>Сведения о ранее выданном заключении по результатам оценки соответствия в рамках экспертного сопровождения в отношении объекта капитального строительства, проектная документация по которому представлена для проведения оценки соответствия в рамках экспертного сопровождения</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute(@"Сведения о ранее выданном заключении по результатам оценки соответствия в рамках экспертного сопровождения в отношении объекта капитального строительства, проектная документация по которому представлена для проведения оценки соответствия в рамках экспертного сопровождения")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tPreviousSimpleConclusion", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TPreviousSimpleConclusion
    {

        /// <summary>
        /// <para>Дата заключения по результатам оценки в рамках экспертного сопровождения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Дата заключения по результатам оценки в рамках экспертного сопровождения")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Date", DataType = "date")]
        public System.DateTime Date { get; set; }

        /// <summary>
        /// <para>Вид объекта экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вид объекта экспертизы")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ExaminationObjectType")]
        public TExaminationObjectType ExaminationObjectType { get; set; }

        /// <summary>
        /// <para>Номер заключения по результатам оценки в рамках экспертного сопровождения</para>
        /// <para>Текстовая строка длиной 100 символа</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Номер заключения по результатам оценки в рамках экспертного сопровождения")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Number")]
        public string Number { get; set; }

        /// <summary>
        /// <para>Результат оценки соответствия в рамках экспертного сопровождения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Результат оценки соответствия в рамках экспертного сопровождения")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Result")]
        public TExaminationResult Result { get; set; }
    }

    /// <summary>
    /// <para>Описание объекта</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Описание объекта")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tObject", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TObject
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TAddress> _address;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TObjectPart> _objectPart;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<Ttei> _tei;

        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="TObject" /> class.</para>
        /// </summary>
        public TObject()
        {
            this._address = new System.Collections.ObjectModel.Collection<TAddress>();
            this._tei = new System.Collections.ObjectModel.Collection<Ttei>();
            this._objectPart = new System.Collections.ObjectModel.Collection<TObjectPart>();
        }

        /// <summary>
        /// <para>Почтовый (строительный) адрес (местоположение) объекта капитального строительства</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Почтовый (строительный) адрес (местоположение) объекта капитального строительства")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Address")]
        public System.Collections.ObjectModel.Collection<TAddress> Address
        {
            get
            {
                return _address;
            }
            private set
            {
                _address = value;
            }
        }

        /// <summary>
        /// <para>Код классификатора объектов капитального строительства по их назначению и функционально-технологическим особенностям</para>
        /// <para>Код классификатора функционального назначения объектов капитального строительства</para>
        /// <para xml:lang="en">Pattern: ([0-9]{1,2}\.[0-9]{1,2}\.[0-9]{1,3}\.[0-9]{1,3})|([0-9]{1,2}\.[0-9]{1,2}\.[0-9]{1,3}).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Код классификатора объектов капитального строительства по их назначению и функцио" +
            "нально-технологическим особенностям"))]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute(("([0-9]{1,2}\\.[0-9]{1,2}\\.[0-9]{1,3}\\.[0-9]{1,3})|([0-9]{1,2}\\.[0-9]{1,2}\\.[0-9]{1" +
            ",3})"))]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("FunctionsClass")]
        public string FunctionsClass { get; set; }

        /// <summary>
        /// <para>Наименование объекта капитального строительства</para>
        /// <para>Непустая текстовая строка</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Наименование объекта капитального строительства")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// <para>Описание составной части сложного объекта</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Описание составной части сложного объекта")]
        [System.Xml.Serialization.XmlElementAttribute("ObjectPart")]
        public System.Collections.ObjectModel.Collection<TObjectPart> ObjectPart
        {
            get
            {
                return _objectPart;
            }
            private set
            {
                _objectPart = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the ObjectPart collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ObjectPartSpecified
        {
            get
            {
                return (this.ObjectPart.Count != 0);
            }
        }

        /// <summary>
        /// <para>Технико-экономические показатели объекта капитального строительства</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Технико-экономические показатели объекта капитального строительства")]
        [System.Xml.Serialization.XmlElementAttribute("TEI")]
        public System.Collections.ObjectModel.Collection<Ttei> Tei
        {
            get
            {
                return _tei;
            }
            private set
            {
                _tei = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the Tei collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TeiSpecified
        {
            get
            {
                return (this.Tei.Count != 0);
            }
        }

        /// <summary>
        /// <para>Вид объекта</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вид объекта")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Type")]
        public TObjectType Type { get; set; }
    }

    /// <summary>
    /// <para>Вид объекта капитального строительства</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Вид объекта капитального строительства")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tObjectType", Namespace = "")]
    public enum TObjectType
    {

        /// <summary>
        /// <para>Объект производственного назначения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Объект производственного назначения")]
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <summary>
        /// <para>Объект непроизводственного назначения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Объект непроизводственного назначения")]
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <summary>
        /// <para>Линейный объект</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Линейный объект")]
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,
    }

    /// <summary>
    /// <para>Технико-экономический показатель и его значение</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Технико-экономический показатель и его значение")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tTEI", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Ttei
    {

        /// <summary>
        /// <para>Единица измерения показателя</para>
        /// <para>Текстовая строка длиной 100 символа</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Единица измерения показателя")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Measure")]
        public string Measure { get; set; }

        /// <summary>
        /// <para>Наименование показателя</para>
        /// <para>Текстовая строка длиной 500 символа</para>
        /// <para xml:lang="en">Maximum length: 500.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Наименование показателя")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(500)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// <para>Значение показателя</para>
        /// <para>Текстовая строка длиной 500 символа</para>
        /// <para xml:lang="en">Maximum length: 500.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Значение показателя")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(500)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Value")]
        public string Value { get; set; }
    }

    /// <summary>
    /// <para>Описание объекта (части объекта)</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Описание объекта (части объекта)")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tObjectPart", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TObjectPart
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TAddress> _address;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<Ttei> _tei;

        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="TObjectPart" /> class.</para>
        /// </summary>
        public TObjectPart()
        {
            this._address = new System.Collections.ObjectModel.Collection<TAddress>();
            this._tei = new System.Collections.ObjectModel.Collection<Ttei>();
        }

        /// <summary>
        /// <para>Почтовый (строительный) адрес (местоположение) объекта капитального строительства</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Почтовый (строительный) адрес (местоположение) объекта капитального строительства")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Address")]
        public System.Collections.ObjectModel.Collection<TAddress> Address
        {
            get
            {
                return _address;
            }
            private set
            {
                _address = value;
            }
        }

        /// <summary>
        /// <para>Код классификатора объектов капитального строительства по их назначению и функционально-технологическим особенностям</para>
        /// <para>Код классификатора функционального назначения объектов капитального строительства</para>
        /// <para xml:lang="en">Pattern: [0-9]{1,2}\.[0-9]{1,2}\.[0-9]{1,3}\.[0-9]{1,3}.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Код классификатора объектов капитального строительства по их назначению и функцио" +
            "нально-технологическим особенностям"))]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("[0-9]{1,2}\\.[0-9]{1,2}\\.[0-9]{1,3}\\.[0-9]{1,3}")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("FunctionsClass")]
        public string FunctionsClass { get; set; }

        /// <summary>
        /// <para>Наименование объекта капитального строительства</para>
        /// <para>Текстовая строка длиной 1000 символа</para>
        /// <para xml:lang="en">Maximum length: 1000.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Наименование объекта капитального строительства")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(1000)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// <para>Технико-экономические показатели объекта капитального строительства</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Технико-экономические показатели объекта капитального строительства")]
        [System.Xml.Serialization.XmlElementAttribute("TEI")]
        public System.Collections.ObjectModel.Collection<Ttei> Tei
        {
            get
            {
                return _tei;
            }
            private set
            {
                _tei = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the Tei collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TeiSpecified
        {
            get
            {
                return (this.Tei.Count != 0);
            }
        }
    }

    /// <summary>
    /// <para>Технический заказчик</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Технический заказчик")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tTechnicalCustomer", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TTechnicalCustomer
    {

        /// <summary>
        /// <para>Иностранное юридическое лицо (представительство, филиал)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Иностранное юридическое лицо (представительство, филиал)")]
        [System.Xml.Serialization.XmlElementAttribute("ForeignOrganization")]
        public TForeignOrganization ForeignOrganization { get; set; }

        /// <summary>
        /// <para>Юридическое лицо</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Юридическое лицо")]
        [System.Xml.Serialization.XmlElementAttribute("Organization")]
        public TOrganization Organization { get; set; }
    }

    /// <summary>
    /// <para>Источник финансирования</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Источник финансирования")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tFinance", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TFinance
    {

        /// <summary>
        /// <para>Уровень бюджета (в случае бюджетного финансирования)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Уровень бюджета (в случае бюджетного финансирования)")]
        [System.Xml.Serialization.XmlElementAttribute("BudgetType")]
        public TBudgetType BudgetType { get; set; }

        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the BudgetType property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BudgetTypeSpecified { get; set; }

        /// <summary>
        /// <para>Сведения о юридическом лице – источнике финансирование</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сведения о юридическом лице – источнике финансирование")]
        [System.Xml.Serialization.XmlElementAttribute("FinanceOwner")]
        public TTechnicalCustomer FinanceOwner { get; set; }

        /// <summary>
        /// <para>Размер финансирования (в % от общей суммы)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Размер финансирования (в % от общей суммы)")]
        [System.Xml.Serialization.XmlElementAttribute("FinanceSize")]
        public decimal FinanceSize { get; set; }

        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the FinanceSize property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FinanceSizeSpecified { get; set; }

        /// <summary>
        /// <para>Вид источника финансирования</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вид источника финансирования")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("FinanceType")]
        public TFinanceType FinanceType { get; set; }
    }

    /// <summary>
    /// <para>Вид источника финансирования</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Вид источника финансирования")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tFinanceType", Namespace = "")]
    public enum TFinanceType
    {

        /// <summary>
        /// <para>Бюджетные средства</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Бюджетные средства")]
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <summary>
        /// <para>Средства юридических лиц, перечисленных в части 2 статьи 8.3 Градостроительного кодекса Российской Федерации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Средства юридических лиц, перечисленных в части 2 статьи 8.3 Градостроительного к" +
            "одекса Российской Федерации"))]
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <summary>
        /// <para>Средства, не входящие в перечень, указанный в части 2 статьи 8.3 Градостроительного кодекса Российской Федерации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Средства, не входящие в перечень, указанный в части 2 статьи 8.3 Градостроительно" +
            "го кодекса Российской Федерации"))]
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,
    }

    /// <summary>
    /// <para>Вид бюджета</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Вид бюджета")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tBudgetType", Namespace = "")]
    public enum TBudgetType
    {

        /// <summary>
        /// <para>Федеральный бюджет</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Федеральный бюджет")]
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <summary>
        /// <para>Бюджет субъекта Российской Федерации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Бюджет субъекта Российской Федерации")]
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <summary>
        /// <para>Местный бюджет</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Местный бюджет")]
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,

        /// <summary>
        /// <para>Бюджет государственного внебюджетного фонда Российской Федерации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Бюджет государственного внебюджетного фонда Российской Федерации")]
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        Item4,

        /// <summary>
        /// <para>Бюджет территориального государственного внебюджетного фонда</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Бюджет территориального государственного внебюджетного фонда")]
        [System.Xml.Serialization.XmlEnumAttribute("5")]
        Item5,
    }

    /// <summary>
    /// <para>Климатические условия территории</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Климатические условия территории")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tClimateConditions", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TClimateConditions
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TClimateDistrict> _climateDistrict;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TGeologicalConditions> _geologicalConditions;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TSeismicActivity> _seismicActivity;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TSnowDistrict> _snowDistrict;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TWindDistrict> _windDistrict;

        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="TClimateConditions" /> class.</para>
        /// </summary>
        public TClimateConditions()
        {
            this._climateDistrict = new System.Collections.ObjectModel.Collection<TClimateDistrict>();
            this._geologicalConditions = new System.Collections.ObjectModel.Collection<TGeologicalConditions>();
            this._windDistrict = new System.Collections.ObjectModel.Collection<TWindDistrict>();
            this._snowDistrict = new System.Collections.ObjectModel.Collection<TSnowDistrict>();
            this._seismicActivity = new System.Collections.ObjectModel.Collection<TSeismicActivity>();
        }

        /// <summary>
        /// <para>Климатический район, подрайон</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Климатический район, подрайон")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ClimateDistrict")]
        public System.Collections.ObjectModel.Collection<TClimateDistrict> ClimateDistrict
        {
            get
            {
                return _climateDistrict;
            }
            private set
            {
                _climateDistrict = value;
            }
        }

        /// <summary>
        /// <para>Категория сложности инженерно-геологических(геокриологических) условий</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Категория сложности инженерно-геологических(геокриологических) условий")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("GeologicalConditions")]
        public System.Collections.ObjectModel.Collection<TGeologicalConditions> GeologicalConditions
        {
            get
            {
                return _geologicalConditions;
            }
            private set
            {
                _geologicalConditions = value;
            }
        }

        /// <summary>
        /// <para>Интенсивность сейсмических воздействий</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Интенсивность сейсмических воздействий")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("SeismicActivity")]
        public System.Collections.ObjectModel.Collection<TSeismicActivity> SeismicActivity
        {
            get
            {
                return _seismicActivity;
            }
            private set
            {
                _seismicActivity = value;
            }
        }

        /// <summary>
        /// <para>Рачетное значение интенсивности сейсмических воздействий</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Рачетное значение интенсивности сейсмических воздействий")]
        [System.Xml.Serialization.XmlElementAttribute("SeismicActivityCalculatedValue")]
        public TSeismicActivityCalculatedValue SeismicActivityCalculatedValue { get; set; }

        /// <summary>
        /// <para>Снеговой район</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Снеговой район")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("SnowDistrict")]
        public System.Collections.ObjectModel.Collection<TSnowDistrict> SnowDistrict
        {
            get
            {
                return _snowDistrict;
            }
            private set
            {
                _snowDistrict = value;
            }
        }

        /// <summary>
        /// <para>Ветровой район</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ветровой район")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("WindDistrict")]
        public System.Collections.ObjectModel.Collection<TWindDistrict> WindDistrict
        {
            get
            {
                return _windDistrict;
            }
            private set
            {
                _windDistrict = value;
            }
        }
    }

    /// <summary>
    /// <para>Климатический район (подрайон)</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Климатический район (подрайон)")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tClimateDistrict", Namespace = "")]
    public enum TClimateDistrict
    {

        /// <summary>
        /// <para>Сведения не представлены</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сведения не представлены")]
        [System.Xml.Serialization.XmlEnumAttribute("Сведения не представлены")]
        СведенияНеПредставлены,

        /// <summary>
        /// <para>климатический район I</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("климатический район I")]
        I,

        /// <summary>
        /// <para>климатический подрайон IА</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("климатический подрайон IА")]
        [System.Xml.Serialization.XmlEnumAttribute("IА")]
        Iа,

        /// <summary>
        /// <para>климатический подрайон IБ</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("климатический подрайон IБ")]
        [System.Xml.Serialization.XmlEnumAttribute("IБ")]
        Iб,

        /// <summary>
        /// <para>климатический подрайон IВ</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("климатический подрайон IВ")]
        [System.Xml.Serialization.XmlEnumAttribute("IВ")]
        Iв,

        /// <summary>
        /// <para>климатический подрайон IГ</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("климатический подрайон IГ")]
        [System.Xml.Serialization.XmlEnumAttribute("IГ")]
        Iг,

        /// <summary>
        /// <para>климатический подрайон IД</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("климатический подрайон IД")]
        [System.Xml.Serialization.XmlEnumAttribute("IД")]
        Iд,

        /// <summary>
        /// <para>климатический район II</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("климатический район II")]
        [System.Xml.Serialization.XmlEnumAttribute("II")]
        Ii,

        /// <summary>
        /// <para>климатический подрайон IIА</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("климатический подрайон IIА")]
        [System.Xml.Serialization.XmlEnumAttribute("IIА")]
        Iiа,

        /// <summary>
        /// <para>климатический подрайон IIБ</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("климатический подрайон IIБ")]
        [System.Xml.Serialization.XmlEnumAttribute("IIБ")]
        Iiб,

        /// <summary>
        /// <para>климатический подрайон IIВ</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("климатический подрайон IIВ")]
        [System.Xml.Serialization.XmlEnumAttribute("IIВ")]
        Iiв,

        /// <summary>
        /// <para>климатический подрайон IIГ</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("климатический подрайон IIГ")]
        [System.Xml.Serialization.XmlEnumAttribute("IIГ")]
        Iiг,

        /// <summary>
        /// <para>климатический район III</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("климатический район III")]
        [System.Xml.Serialization.XmlEnumAttribute("III")]
        Iii,

        /// <summary>
        /// <para>климатический подрайон IIIА</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("климатический подрайон IIIА")]
        [System.Xml.Serialization.XmlEnumAttribute("IIIА")]
        Iiiа,

        /// <summary>
        /// <para>климатический подрайон IIIБ</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("климатический подрайон IIIБ")]
        [System.Xml.Serialization.XmlEnumAttribute("IIIБ")]
        Iiiб,

        /// <summary>
        /// <para>климатический подрайон IIIВ</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("климатический подрайон IIIВ")]
        [System.Xml.Serialization.XmlEnumAttribute("IIIВ")]
        Iiiв,

        /// <summary>
        /// <para>климатический район IV</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("климатический район IV")]
        [System.Xml.Serialization.XmlEnumAttribute("IV")]
        Iv,

        /// <summary>
        /// <para>климатический подрайон IVА</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("климатический подрайон IVА")]
        [System.Xml.Serialization.XmlEnumAttribute("IVА")]
        Ivа,

        /// <summary>
        /// <para>климатический подрайон IVБ</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("климатический подрайон IVБ")]
        [System.Xml.Serialization.XmlEnumAttribute("IVБ")]
        Ivб,

        /// <summary>
        /// <para>климатический подрайон IVВ</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("климатический подрайон IVВ")]
        [System.Xml.Serialization.XmlEnumAttribute("IVВ")]
        Ivв,

        /// <summary>
        /// <para>климатический подрайон IVГ</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("климатический подрайон IVГ")]
        [System.Xml.Serialization.XmlEnumAttribute("IVГ")]
        Ivг,
    }

    /// <summary>
    /// <para>Инженерно-геологические условия</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Инженерно-геологические условия")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tGeologicalConditions", Namespace = "")]
    public enum TGeologicalConditions
    {

        /// <summary>
        /// <para>Сведения не представлены</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сведения не представлены")]
        [System.Xml.Serialization.XmlEnumAttribute("Сведения не представлены")]
        СведенияНеПредставлены,

        /// <summary>
        /// <para>I (простые)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("I (простые)")]
        I,

        /// <summary>
        /// <para>II (средней сложности)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("II (средней сложности)")]
        [System.Xml.Serialization.XmlEnumAttribute("II")]
        Ii,

        /// <summary>
        /// <para>III (сложные)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("III (сложные)")]
        [System.Xml.Serialization.XmlEnumAttribute("III")]
        Iii,
    }

    /// <summary>
    /// <para>Ветровой район</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Ветровой район")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tWindDistrict", Namespace = "")]
    public enum TWindDistrict
    {

        /// <summary>
        /// <para>Сведения не представлены</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сведения не представлены")]
        [System.Xml.Serialization.XmlEnumAttribute("Сведения не представлены")]
        СведенияНеПредставлены,

        /// <summary>
        /// <para>Ветровой район Iа</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ветровой район Iа")]
        Iа,

        /// <summary>
        /// <para>Ветровой район I</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ветровой район I")]
        I,

        /// <summary>
        /// <para>Ветровой район II</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ветровой район II")]
        [System.Xml.Serialization.XmlEnumAttribute("II")]
        Ii,

        /// <summary>
        /// <para>Ветровой район III</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ветровой район III")]
        [System.Xml.Serialization.XmlEnumAttribute("III")]
        Iii,

        /// <summary>
        /// <para>Ветровой район IV</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ветровой район IV")]
        [System.Xml.Serialization.XmlEnumAttribute("IV")]
        Iv,

        /// <summary>
        /// <para>Ветровой район V</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ветровой район V")]
        V,

        /// <summary>
        /// <para>Ветровой район VI</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ветровой район VI")]
        [System.Xml.Serialization.XmlEnumAttribute("VI")]
        Vi,

        /// <summary>
        /// <para>Ветровой район VII</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ветровой район VII")]
        [System.Xml.Serialization.XmlEnumAttribute("VII")]
        Vii,
    }

    /// <summary>
    /// <para>Снеговой район</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Снеговой район")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tSnowDistrict", Namespace = "")]
    public enum TSnowDistrict
    {

        /// <summary>
        /// <para>Сведения не представлены</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сведения не представлены")]
        [System.Xml.Serialization.XmlEnumAttribute("Сведения не представлены")]
        СведенияНеПредставлены,

        /// <summary>
        /// <para>Снеговой район I</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Снеговой район I")]
        I,

        /// <summary>
        /// <para>Снеговой район II</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Снеговой район II")]
        [System.Xml.Serialization.XmlEnumAttribute("II")]
        Ii,

        /// <summary>
        /// <para>Снеговой район III</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Снеговой район III")]
        [System.Xml.Serialization.XmlEnumAttribute("III")]
        Iii,

        /// <summary>
        /// <para>Снеговой район IV</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Снеговой район IV")]
        [System.Xml.Serialization.XmlEnumAttribute("IV")]
        Iv,

        /// <summary>
        /// <para>Снеговой район V</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Снеговой район V")]
        V,

        /// <summary>
        /// <para>Снеговой район VI</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Снеговой район VI")]
        [System.Xml.Serialization.XmlEnumAttribute("VI")]
        Vi,

        /// <summary>
        /// <para>Снеговой район VII</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Снеговой район VII")]
        [System.Xml.Serialization.XmlEnumAttribute("VII")]
        Vii,

        /// <summary>
        /// <para>Снеговой район VIII</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Снеговой район VIII")]
        [System.Xml.Serialization.XmlEnumAttribute("VIII")]
        Viii,
    }

    /// <summary>
    /// <para>Интенсивность сейсмических воздействий</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Интенсивность сейсмических воздействий")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tSeismicActivity", Namespace = "")]
    public enum TSeismicActivity
    {

        /// <summary>
        /// <para>Сведения не представлены</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сведения не представлены")]
        [System.Xml.Serialization.XmlEnumAttribute("Сведения не представлены")]
        СведенияНеПредставлены,

        /// <summary>
        /// <para>5 и менее баллов</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5 и менее баллов")]
        [System.Xml.Serialization.XmlEnumAttribute("5")]
        Item5,

        /// <summary>
        /// <para>6</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("6")]
        [System.Xml.Serialization.XmlEnumAttribute("6")]
        Item6,

        /// <summary>
        /// <para>7</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("7")]
        [System.Xml.Serialization.XmlEnumAttribute("7")]
        Item7,

        /// <summary>
        /// <para>8</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("8")]
        [System.Xml.Serialization.XmlEnumAttribute("8")]
        Item8,

        /// <summary>
        /// <para>9</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("9")]
        [System.Xml.Serialization.XmlEnumAttribute("9")]
        Item9,

        /// <summary>
        /// <para>Более 9 баллов</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Более 9 баллов")]
        [System.Xml.Serialization.XmlEnumAttribute("10")]
        Item10,
    }

    /// <summary>
    /// <para>Выводы по результатам рассмотрения документации</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Выводы по результатам рассмотрения документации")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tSeismicActivityCalculatedValue", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TSeismicActivityCalculatedValue
    {

        /// <summary>
        /// <para>Максимальное расчетное значение интенсивности сейсмического воздействия</para>
        /// <para>Непустая текстовая строка</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Максимальное расчетное значение интенсивности сейсмического воздействия")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("MaxValue")]
        public string MaxValue { get; set; }

        /// <summary>
        /// <para>Расчетное (минимальное - в случае диапазона) значение интенсивности сейсмического воздействия</para>
        /// <para>Непустая текстовая строка</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Расчетное (минимальное - в случае диапазона) значение интенсивности сейсмического" +
            " воздействия"))]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("MinValue")]
        public string MinValue { get; set; }
    }

    /// <summary>
    /// <para>Сведения о сметной стоимости</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Сведения о сметной стоимости")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tEstimatedCost", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TEstimatedCost : ICompleteEstimatedCostGroup, IComplexEstimatedCostGroup
    {

        /// <summary>
        /// <para>Валюта в которой, производится расчет сметной стоимости</para>
        /// <para>Непустая текстовая строка</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Валюта в которой, производится расчет сметной стоимости")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("Currency")]
        public string Currency { get; set; }

        /// <summary>
        /// <para>Сметная стоимость на дату представления документации для проведения экспертизы</para>
        /// <para>Описание сумм при указании сметной стоимости</para>
        /// <para xml:lang="en">Pattern: (Не требуется)|(Отсутствует)|(-{0,1}\d*\.{0,1}\d*).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сметная стоимость на дату представления документации для проведения экспертизы")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(Не требуется)|(Отсутствует)|(-{0,1}\\d*\\.{0,1}\\d*)")]
        [System.Xml.Serialization.XmlElementAttribute("EstimatedCompleteCostBefore")]
        public string EstimatedCompleteCostBefore { get; set; }

        /// <summary>
        /// <para>Сметная стоимость на дату утверждения заключения экспертизы</para>
        /// <para>Описание сумм при указании сметной стоимости</para>
        /// <para xml:lang="en">Pattern: (Не требуется)|(Отсутствует)|(-{0,1}\d*\.{0,1}\d*).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сметная стоимость на дату утверждения заключения экспертизы")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(Не требуется)|(Отсутствует)|(-{0,1}\\d*\\.{0,1}\\d*)")]
        [System.Xml.Serialization.XmlElementAttribute("EstimatedCompleteCostPost")]
        public string EstimatedCompleteCostPost { get; set; }

        /// <summary>
        /// <para>Сведения о сметной стоимости на дату представления документации для проведения экспертизы (составное значение в случае проверки определения достоверности сметной стоимости)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сведения о сметной стоимости на дату представления документации для проведения эк" +
            "спертизы (составное значение в случае проверки определения достоверности сметной" +
            " стоимости)"))]
        [System.Xml.Serialization.XmlElementAttribute("EstimatedComplexCostBefore")]
        public TComplexEstimatedCost EstimatedComplexCostBefore { get; set; }

        /// <summary>
        /// <para>Сведения о сметной стоимости по результатам проведения проверки достоверности определения сметной стоимости (составное значение в случае проверки определения достоверности сметной стоимости)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сведения о сметной стоимости по результатам проведения проверки достоверности опр" +
            "еделения сметной стоимости (составное значение в случае проверки определения дос" +
            "товерности сметной стоимости)"))]
        [System.Xml.Serialization.XmlElementAttribute("EstimatedComplexCostPost")]
        public TComplexEstimatedCost EstimatedComplexCostPost { get; set; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    public partial interface ICompleteEstimatedCostGroup
    {

        /// <summary>
        /// <para>Сметная стоимость на дату представления документации для проведения экспертизы</para>
        /// <para>Описание сумм при указании сметной стоимости</para>
        /// <para xml:lang="en">Pattern: (Не требуется)|(Отсутствует)|(-{0,1}\d*\.{0,1}\d*).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сметная стоимость на дату представления документации для проведения экспертизы")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(Не требуется)|(Отсутствует)|(-{0,1}\\d*\\.{0,1}\\d*)")]
        string EstimatedCompleteCostBefore
        {
            get;
            set;
        }

        /// <summary>
        /// <para>Сметная стоимость на дату утверждения заключения экспертизы</para>
        /// <para>Описание сумм при указании сметной стоимости</para>
        /// <para xml:lang="en">Pattern: (Не требуется)|(Отсутствует)|(-{0,1}\d*\.{0,1}\d*).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сметная стоимость на дату утверждения заключения экспертизы")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(Не требуется)|(Отсутствует)|(-{0,1}\\d*\\.{0,1}\\d*)")]
        string EstimatedCompleteCostPost
        {
            get;
            set;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    public partial interface IComplexEstimatedCostGroup
    {

        /// <summary>
        /// <para>Сведения о сметной стоимости на дату представления документации для проведения экспертизы (составное значение в случае проверки определения достоверности сметной стоимости)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сведения о сметной стоимости на дату представления документации для проведения эк" +
            "спертизы (составное значение в случае проверки определения достоверности сметной" +
            " стоимости)"))]
        TComplexEstimatedCost EstimatedComplexCostBefore
        {
            get;
            set;
        }

        /// <summary>
        /// <para>Сведения о сметной стоимости по результатам проведения проверки достоверности определения сметной стоимости (составное значение в случае проверки определения достоверности сметной стоимости)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сведения о сметной стоимости по результатам проведения проверки достоверности опр" +
            "еделения сметной стоимости (составное значение в случае проверки определения дос" +
            "товерности сметной стоимости)"))]
        TComplexEstimatedCost EstimatedComplexCostPost
        {
            get;
            set;
        }
    }

    /// <summary>
    /// <para>Сметная стоимость (расшифровка по составляющим)</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Сметная стоимость (расшифровка по составляющим)")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tComplexEstimatedCost", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TComplexEstimatedCost
    {

        /// <summary>
        /// <para>Возвратные суммы в текущем уровне цен  (в тысячах рублей)</para>
        /// <para>Описание сумм при указании сметной стоимости</para>
        /// <para xml:lang="en">Pattern: (Не требуется)|(Отсутствует)|(-{0,1}\d*\.{0,1}\d*).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Возвратные суммы в текущем уровне цен (в тысячах рублей)")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(Не требуется)|(Отсутствует)|(-{0,1}\\d*\\.{0,1}\\d*)")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("BackSumCost")]
        public string BackSumCost { get; set; }

        /// <summary>
        /// <para>Возвратные суммы в базисном уровне цен (в тысячах рублей)</para>
        /// <para>Описание сумм при указании сметной стоимости</para>
        /// <para xml:lang="en">Pattern: (Не требуется)|(Отсутствует)|(-{0,1}\d*\.{0,1}\d*).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Возвратные суммы в базисном уровне цен (в тысячах рублей)")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(Не требуется)|(Отсутствует)|(-{0,1}\\d*\\.{0,1}\\d*)")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("BackSumCostBasic")]
        public string BackSumCostBasic { get; set; }

        /// <summary>
        /// <para>Сметная стоимость в текущем уровне цен (в тысячах рублей)</para>
        /// <para>Описание сумм при указании сметной стоимости</para>
        /// <para xml:lang="en">Pattern: (Не требуется)|(Отсутствует)|(-{0,1}\d*\.{0,1}\d*).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сметная стоимость в текущем уровне цен (в тысячах рублей)")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(Не требуется)|(Отсутствует)|(-{0,1}\\d*\\.{0,1}\\d*)")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Cost")]
        public string Cost { get; set; }

        /// <summary>
        /// <para>Сметная стоимость в базисном уровне цен (в тысячах рублей)</para>
        /// <para>Описание сумм при указании сметной стоимости</para>
        /// <para xml:lang="en">Pattern: (Не требуется)|(Отсутствует)|(-{0,1}\d*\.{0,1}\d*).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сметная стоимость в базисном уровне цен (в тысячах рублей)")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(Не требуется)|(Отсутствует)|(-{0,1}\\d*\\.{0,1}\\d*)")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("CostBasic")]
        public string CostBasic { get; set; }

        /// <summary>
        /// <para>Комментарий к сметной стоимости в базисном уровне цен</para>
        /// <para>Непустая текстовая строка</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Комментарий к сметной стоимости в базисном уровне цен")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("CostBasicComment")]
        public string CostBasicComment { get; set; }

        /// <summary>
        /// <para>Комментарий к сметной стоимости в текущем уровне цен</para>
        /// <para>Непустая текстовая строка</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Комментарий к сметной стоимости в текущем уровне цен")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("CostComment")]
        public string CostComment { get; set; }

        /// <summary>
        /// <para>Сметная стоимость оборудования в текущем уровне цен (в тысячах рублей)</para>
        /// <para>Описание сумм при указании сметной стоимости</para>
        /// <para xml:lang="en">Pattern: (Не требуется)|(Отсутствует)|(-{0,1}\d*\.{0,1}\d*).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сметная стоимость оборудования в текущем уровне цен (в тысячах рублей)")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(Не требуется)|(Отсутствует)|(-{0,1}\\d*\\.{0,1}\\d*)")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("HardwareCost")]
        public string HardwareCost { get; set; }

        /// <summary>
        /// <para>Сметная стоимость оборудования в базисном уровне цен (в тысячах рублей)</para>
        /// <para>Описание сумм при указании сметной стоимости</para>
        /// <para xml:lang="en">Pattern: (Не требуется)|(Отсутствует)|(-{0,1}\d*\.{0,1}\d*).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сметная стоимость оборудования в базисном уровне цен (в тысячах рублей)")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(Не требуется)|(Отсутствует)|(-{0,1}\\d*\\.{0,1}\\d*)")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("HardwareCostBasic")]
        public string HardwareCostBasic { get; set; }

        /// <summary>
        /// <para>Сумма налога на добавленную стоимость (в тысячах рублей)</para>
        /// <para>Описание сумм при указании сметной стоимости</para>
        /// <para xml:lang="en">Pattern: (Не требуется)|(Отсутствует)|(-{0,1}\d*\.{0,1}\d*).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сумма налога на добавленную стоимость (в тысячах рублей)")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(Не требуется)|(Отсутствует)|(-{0,1}\\d*\\.{0,1}\\d*)")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("NDSCost")]
        public string NdsCost { get; set; }

        /// <summary>
        /// <para>Сметная стоимость прочих затрат в текущем уровне цен (в тысячах рублей)</para>
        /// <para>Описание сумм при указании сметной стоимости</para>
        /// <para xml:lang="en">Pattern: (Не требуется)|(Отсутствует)|(-{0,1}\d*\.{0,1}\d*).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сметная стоимость прочих затрат в текущем уровне цен (в тысячах рублей)")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(Не требуется)|(Отсутствует)|(-{0,1}\\d*\\.{0,1}\\d*)")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("OtherCost")]
        public string OtherCost { get; set; }

        /// <summary>
        /// <para>Сметная стоимость прочих затрат в базисном уровне цен (в тысячах рублей)</para>
        /// <para>Описание сумм при указании сметной стоимости</para>
        /// <para xml:lang="en">Pattern: (Не требуется)|(Отсутствует)|(-{0,1}\d*\.{0,1}\d*).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сметная стоимость прочих затрат в базисном уровне цен (в тысячах рублей)")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(Не требуется)|(Отсутствует)|(-{0,1}\\d*\\.{0,1}\\d*)")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("OtherCostBasic")]
        public string OtherCostBasic { get; set; }

        /// <summary>
        /// <para>Сметная стоимость проектно-изыскательских работ в текущем уровне цен (в тысячах рублей)</para>
        /// <para>Описание сумм при указании сметной стоимости</para>
        /// <para xml:lang="en">Pattern: (Не требуется)|(Отсутствует)|(-{0,1}\d*\.{0,1}\d*).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сметная стоимость проектно-изыскательских работ в текущем уровне цен (в тысячах р" +
            "ублей)"))]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(Не требуется)|(Отсутствует)|(-{0,1}\\d*\\.{0,1}\\d*)")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ProjectWorksCost")]
        public string ProjectWorksCost { get; set; }

        /// <summary>
        /// <para>Сметная стоимость проектно-изыскательских работ в базисном уровне цен (в тысячах рублей)</para>
        /// <para>Описание сумм при указании сметной стоимости</para>
        /// <para xml:lang="en">Pattern: (Не требуется)|(Отсутствует)|(-{0,1}\d*\.{0,1}\d*).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сметная стоимость проектно-изыскательских работ в базисном уровне цен (в тысячах " +
            "рублей)"))]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(Не требуется)|(Отсутствует)|(-{0,1}\\d*\\.{0,1}\\d*)")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ProjectWorksCostBasic")]
        public string ProjectWorksCostBasic { get; set; }

        /// <summary>
        /// <para>Сметная стоимость строительно-монтажных работ в текущем уровне цен (в тысячах рублей)</para>
        /// <para>Описание сумм при указании сметной стоимости</para>
        /// <para xml:lang="en">Pattern: (Не требуется)|(Отсутствует)|(-{0,1}\d*\.{0,1}\d*).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сметная стоимость строительно-монтажных работ в текущем уровне цен (в тысячах руб" +
            "лей)"))]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(Не требуется)|(Отсутствует)|(-{0,1}\\d*\\.{0,1}\\d*)")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("WorksCost")]
        public string WorksCost { get; set; }

        /// <summary>
        /// <para>Сметная стоимость строительно-монтажных работ в базисном уровне цен (в тысячах рублей)</para>
        /// <para>Описание сумм при указании сметной стоимости</para>
        /// <para xml:lang="en">Pattern: (Не требуется)|(Отсутствует)|(-{0,1}\d*\.{0,1}\d*).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сметная стоимость строительно-монтажных работ в базисном уровне цен (в тысячах ру" +
            "блей)"))]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(Не требуется)|(Отсутствует)|(-{0,1}\\d*\\.{0,1}\\d*)")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("WorksCostBasic")]
        public string WorksCostBasic { get; set; }
    }

    /// <summary>
    /// <para>Сведения о необходимости проведения экологической экспертизы в отношении объекта</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Сведения о необходимости проведения экологической экспертизы в отношении объекта")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tEcologyExpertise", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TEcologyExpertise
    {

        /// <summary>
        /// <para>Обоснование необходимости (отсутствия необходимости) проведения экологической экспертизы</para>
        /// <para>Непустая текстовая строка</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Обоснование необходимости (отсутствия необходимости) проведения экологической экс" +
            "пертизы"))]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("Comment")]
        public string Comment { get; set; }

        /// <summary>
        /// <para>Отметка о необходимости проведения экологической экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Отметка о необходимости проведения экологической экспертизы")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("NeedExpertise")]
        public TYesNo NeedExpertise { get; set; }
    }

    /// <summary>
    /// <para>Сведения о лицах, подготовивших проектную документацию</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Сведения о лицах, подготовивших проектную документацию")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tDesigner", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TDesigner
    {

        /// <summary>
        /// <para>Иностранная организация (представительство, филиал)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Иностранная организация (представительство, филиал)")]
        [System.Xml.Serialization.XmlElementAttribute("ForeignOrganization")]
        public TForeignOrganization ForeignOrganization { get; set; }

        /// <summary>
        /// <para>Отметка о роли генерального проектировщика</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Отметка о роли генерального проектировщика")]
        [System.Xml.Serialization.XmlAttributeAttribute("General")]
        public TYesNo General { get; set; }

        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the General property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool GeneralSpecified { get; set; }

        /// <summary>
        /// <para>Проектная организация - Индивидуальный предприниматель</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Проектная организация - Индивидуальный предприниматель")]
        [System.Xml.Serialization.XmlElementAttribute("IP")]
        public Tip Ip { get; set; }

        /// <summary>
        /// <para>Проектная организация  - Организация</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Проектная организация - Организация")]
        [System.Xml.Serialization.XmlElementAttribute("Organization")]
        public TOrganization Organization { get; set; }
    }

    /// <summary>
    /// <para>Использование экономически эффективной проектной документации</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Использование экономически эффективной проектной документации")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tEEPDUse", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TeepdUse
    {

        /// <summary>
        /// <para>Дата утверждение заключения экспертизы в отношении использованной экономически-эффективной проектной организации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Дата утверждение заключения экспертизы в отношении использованной экономически-эф" +
            "фективной проектной организации"))]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("EEPDDate", DataType = "date")]
        public System.DateTime EepdDate { get; set; }

        /// <summary>
        /// <para>Сведения о разделах представленной на экспертизу проектной документации, которые не подвергались изменению и полностью соответствуют экономически эффективной проектной документации повторного использования</para>
        /// <para>Непустая текстовая строка</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сведения о разделах представленной на экспертизу проектной документации, которые " +
            "не подвергались изменению и полностью соответствуют экономически эффективной про" +
            "ектной документации повторного использования"))]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("EEPDNote")]
        public string EepdNote { get; set; }

        /// <summary>
        /// <para>Номер заключения экспертизы в отношении использованной экономически-эффективной проектной организации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Номер заключения экспертизы в отношении использованной экономически-эффективной п" +
            "роектной организации"))]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("EEPDNumber")]
        public TConclusionNumber EepdNumber { get; set; }
    }

    /// <summary>
    /// <para>Местоположение района изысканий</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Местоположение района изысканий")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tEngineeringSurveyAddress", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TEngineeringSurveyAddress
    {

        /// <summary>
        /// <para>Описание района изысканий (Наименование муниципального района, в случае проведения изысканий на территории Российской Федерации)</para>
        /// <para>Текстовая строка длиной 1000 символа</para>
        /// <para xml:lang="en">Maximum length: 1000.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Описание района изысканий (Наименование муниципального района, в случае проведени" +
            "я изысканий на территории Российской Федерации)"))]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(1000)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("EngineeringSurveyDistrict")]
        public string EngineeringSurveyDistrict { get; set; }

        /// <summary>
        /// <para>Код субъекта Российской Федерации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Код субъекта Российской Федерации")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("EngineeringSurveyRegion")]
        public TRegionsRf EngineeringSurveyRegion { get; set; }
    }

    /// <summary>
    /// <para>Перечень лиц, аттестованных на право подготовки заключений экспертизы, подписавших заключение экспертизы</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute(("Перечень лиц, аттестованных на право подготовки заключений экспертизы, подписавши" +
        "х заключение экспертизы"))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tExperts", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TExperts
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TExpert> _expert;

        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="TExperts" /> class.</para>
        /// </summary>
        public TExperts()
        {
            this._expert = new System.Collections.ObjectModel.Collection<TExpert>();
        }

        /// <summary>
        /// <para>Эксперт</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Эксперт")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Expert")]
        public System.Collections.ObjectModel.Collection<TExpert> Expert
        {
            get
            {
                return _expert;
            }
            private set
            {
                _expert = value;
            }
        }
    }

    /// <summary>
    /// <para>Эксперт</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Эксперт")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tExpert", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TExpert
    {

        /// <summary>
        /// <para>Номер квалификационного аттестата</para>
        /// <para>Номер квалификационного аттестата эксперта</para>
        /// <para xml:lang="en">Pattern: (МС-Э-[0-9]{1,3}-[0-9]{1,2}-[0-9]{4,5})|(ГС-Э-[0-9]{1,2}-[0-9]{1,2}-[0-9]{4,5})|(Т[0-9]{3}-[0-9]{5}-00/[0-9]{8}).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Номер квалификационного аттестата")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute(("(МС-Э-[0-9]{1,3}-[0-9]{1,2}-[0-9]{4,5})|(ГС-Э-[0-9]{1,2}-[0-9]{1,2}-[0-9]{4,5})|(" +
            "Т[0-9]{3}-[0-9]{5}-00/[0-9]{8})"))]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ExpertCertificate")]
        public string ExpertCertificate { get; set; }

        /// <summary>
        /// <para>Дата выдачи квалификационного аттестата</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Дата выдачи квалификационного аттестата")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ExpertCertificateBeginDate", DataType = "date")]
        public System.DateTime ExpertCertificateBeginDate { get; set; }

        /// <summary>
        /// <para>Дата окончания действия квалификационного аттестата</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Дата окончания действия квалификационного аттестата")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ExpertCertificateEndDate", DataType = "date")]
        public System.DateTime ExpertCertificateEndDate { get; set; }

        /// <summary>
        /// <para>Направление деятельности эксперта</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Направление деятельности эксперта")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ExpertType")]
        public TExpertType ExpertType { get; set; }

        /// <summary>
        /// <para>Фамилия</para>
        /// <para>Текстовая строка длиной 100 символа</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Фамилия")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("FamilyName")]
        public string FamilyName { get; set; }

        /// <summary>
        /// <para>Имя</para>
        /// <para>Текстовая строка длиной 100 символа</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Имя")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// <para>Отчество</para>
        /// <para>Текстовая строка длиной 50 символа</para>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Отчество")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.Xml.Serialization.XmlElementAttribute("SecondName")]
        public string SecondName { get; set; }
    }

    /// <summary>
    /// <para>Направления деятельности экспертов</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Направления деятельности экспертов")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tExpertType", Namespace = "")]
    public enum TExpertType
    {

        /// <summary>
        /// <para>1.1. Инженерно-геодезические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("1.1. Инженерно-геодезические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("1.1.")]
        Item11,

        /// <summary>
        /// <para>1.2. Инженерно-геологические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("1.2. Инженерно-геологические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("1.2.")]
        Item12,

        /// <summary>
        /// <para>1.3. Инженерно-гидрометеорологические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("1.3. Инженерно-гидрометеорологические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("1.3.")]
        Item13,

        /// <summary>
        /// <para>1.4. Инженерно-экологические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("1.4. Инженерно-экологические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("1.4.")]
        Item14,

        /// <summary>
        /// <para>1.5. Инженерно-геотехнические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("1.5. Инженерно-геотехнические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("1.5.")]
        Item15,

        /// <summary>
        /// <para>2.1. Объемно-планировочные, архитектурные и конструктивные решения, планировочная организация земельного участка, организация строительства</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("2.1. Объемно-планировочные, архитектурные и конструктивные решения, планировочная" +
            " организация земельного участка, организация строительства"))]
        [System.Xml.Serialization.XmlEnumAttribute("2.1.")]
        Item21,

        /// <summary>
        /// <para>2.1.1. Схемы планировочной организации земельных участков</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("2.1.1. Схемы планировочной организации земельных участков")]
        [System.Xml.Serialization.XmlEnumAttribute("2.1.1.")]
        Item211,

        /// <summary>
        /// <para>2.1.2. Объемно-планировочные и архитектурные решения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("2.1.2. Объемно-планировочные и архитектурные решения")]
        [System.Xml.Serialization.XmlEnumAttribute("2.1.2.")]
        Item212,

        /// <summary>
        /// <para>2.1.3. Конструктивные решения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("2.1.3. Конструктивные решения")]
        [System.Xml.Serialization.XmlEnumAttribute("2.1.3.")]
        Item213,

        /// <summary>
        /// <para>2.1.4. Организация строительства</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("2.1.4. Организация строительства")]
        [System.Xml.Serialization.XmlEnumAttribute("2.1.4.")]
        Item214,

        /// <summary>
        /// <para>2.2. Теплогазоснабжение, водоснабжение, водоотведение, канализация, вентиляция и кондиционирование</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("2.2. Теплогазоснабжение, водоснабжение, водоотведение, канализация, вентиляция и " +
            "кондиционирование"))]
        [System.Xml.Serialization.XmlEnumAttribute("2.2.")]
        Item22,

        /// <summary>
        /// <para>2.2.1. Водоснабжение, водоотведение и канализация</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("2.2.1. Водоснабжение, водоотведение и канализация")]
        [System.Xml.Serialization.XmlEnumAttribute("2.2.1.")]
        Item221,

        /// <summary>
        /// <para>2.2.2. Теплоснабжение, вентиляция и кондиционирование</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("2.2.2. Теплоснабжение, вентиляция и кондиционирование")]
        [System.Xml.Serialization.XmlEnumAttribute("2.2.2.")]
        Item222,

        /// <summary>
        /// <para>2.2.3. Системы газоснабжения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("2.2.3. Системы газоснабжения")]
        [System.Xml.Serialization.XmlEnumAttribute("2.2.3.")]
        Item223,

        /// <summary>
        /// <para>2.3. Электроснабжение, связь, сигнализация,  системы автоматизации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("2.3. Электроснабжение, связь, сигнализация, системы автоматизации")]
        [System.Xml.Serialization.XmlEnumAttribute("2.3.")]
        Item23,

        /// <summary>
        /// <para>2.3.1. Электроснабжение и электропотребление</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("2.3.1. Электроснабжение и электропотребление")]
        [System.Xml.Serialization.XmlEnumAttribute("2.3.1.")]
        Item231,

        /// <summary>
        /// <para>2.3.2. Системы автоматизации, связи и сигнализации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("2.3.2. Системы автоматизации, связи и сигнализации")]
        [System.Xml.Serialization.XmlEnumAttribute("2.3.2.")]
        Item232,

        /// <summary>
        /// <para>2.4. Охрана окружающей среды, санитарно-эпидемиологическая безопасность</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("2.4. Охрана окружающей среды, санитарно-эпидемиологическая безопасность")]
        [System.Xml.Serialization.XmlEnumAttribute("2.4.")]
        Item24,

        /// <summary>
        /// <para>2.4.1. Охрана окружающей среды</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("2.4.1. Охрана окружающей среды")]
        [System.Xml.Serialization.XmlEnumAttribute("2.4.1.")]
        Item241,

        /// <summary>
        /// <para>2.4.2. Санитарно-эпидемиологическая безопасность</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("2.4.2. Санитарно-эпидемиологическая безопасность")]
        [System.Xml.Serialization.XmlEnumAttribute("2.4.2.")]
        Item242,

        /// <summary>
        /// <para>2.5. Пожарная безопасность</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("2.5. Пожарная безопасность")]
        [System.Xml.Serialization.XmlEnumAttribute("2.5.")]
        Item25,

        /// <summary>
        /// <para>3.1. Организация экспертизы проектной документации и (или) результатов инженерных изысканий</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("3.1. Организация экспертизы проектной документации и (или) результатов инженерных" +
            " изысканий"))]
        [System.Xml.Serialization.XmlEnumAttribute("3.1.")]
        Item31,

        /// <summary>
        /// <para>4.1. Мосты и трубы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("4.1. Мосты и трубы")]
        [System.Xml.Serialization.XmlEnumAttribute("4.1.")]
        Item41,

        /// <summary>
        /// <para>4.2. Автомобильные дороги</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("4.2. Автомобильные дороги")]
        [System.Xml.Serialization.XmlEnumAttribute("4.2.")]
        Item42,

        /// <summary>
        /// <para>4.3. Объекты топливно-энергетического комплекса</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("4.3. Объекты топливно-энергетического комплекса")]
        [System.Xml.Serialization.XmlEnumAttribute("4.3.")]
        Item43,

        /// <summary>
        /// <para>4.4. Объекты информатизации и связи</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("4.4. Объекты информатизации и связи")]
        [System.Xml.Serialization.XmlEnumAttribute("4.4.")]
        Item44,

        /// <summary>
        /// <para>4.5. Инженерно-технические мероприятия ГО и ЧС</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("4.5. Инженерно-технические мероприятия ГО и ЧС")]
        [System.Xml.Serialization.XmlEnumAttribute("4.5.")]
        Item45,

        /// <summary>
        /// <para>5.1.1. Инженерно-геодезические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.1.1. Инженерно-геодезические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("5.1.1.")]
        Item511,

        /// <summary>
        /// <para>5.1.2. Инженерно-геологические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.1.2. Инженерно-геологические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("5.1.2.")]
        Item512,

        /// <summary>
        /// <para>5.1.3. Инженерно-гидрометеорологические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.1.3. Инженерно-гидрометеорологические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("5.1.3.")]
        Item513,

        /// <summary>
        /// <para>5.1.4. Инженерно-экологические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.1.4. Инженерно-экологические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("5.1.4.")]
        Item514,

        /// <summary>
        /// <para>5.1.5. Инженерно-геотехнические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.1.5. Инженерно-геотехнические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("5.1.5.")]
        Item515,

        /// <summary>
        /// <para>5.1.6. Обследования состояния грунтов оснований зданий и сооружений</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.1.6. Обследования состояния грунтов оснований зданий и сооружений")]
        [System.Xml.Serialization.XmlEnumAttribute("5.1.6.")]
        Item516,

        /// <summary>
        /// <para>5.1.7. Обследование состояния строительных конструкций зданий и сооружений</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.1.7. Обследование состояния строительных конструкций зданий и сооружений")]
        [System.Xml.Serialization.XmlEnumAttribute("5.1.7.")]
        Item517,

        /// <summary>
        /// <para>5.2.1. Схемы планировочной организации земельных участков</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.1. Схемы планировочной организации земельных участков")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.1.")]
        Item521,

        /// <summary>
        /// <para>5.2.2. Объемно-планировочные решения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.2. Объемно-планировочные решения")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.2.")]
        Item522,

        /// <summary>
        /// <para>5.2.3. Конструктивные решения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.3. Конструктивные решения")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.3.")]
        Item523,

        /// <summary>
        /// <para>5.2.4. Сети инженерно-технического обеспечения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.4. Сети инженерно-технического обеспечения")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.4.")]
        Item524,

        /// <summary>
        /// <para>5.2.4.1. Электроснабжение</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.4.1. Электроснабжение")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.4.1.")]
        Item5241,

        /// <summary>
        /// <para>5.2.4.2. Водоснабжение и водоотведение</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.4.2. Водоснабжение и водоотведение")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.4.2.")]
        Item5242,

        /// <summary>
        /// <para>5.2.4.3. Отопление, вентиляция и кондиционирование воздуха, тепловые сети</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.4.3. Отопление, вентиляция и кондиционирование воздуха, тепловые сети")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.4.3.")]
        Item5243,

        /// <summary>
        /// <para>5.2.4.4. Системы связи и сигнализации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.4.4. Системы связи и сигнализации")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.4.4.")]
        Item5244,

        /// <summary>
        /// <para>5.2.4.5. Системы газоснабжения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.4.5. Системы газоснабжения")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.4.5.")]
        Item5245,

        /// <summary>
        /// <para>5.2.4.6. Системы автоматизации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.4.6. Системы автоматизации")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.4.6.")]
        Item5246,

        /// <summary>
        /// <para>5.2.4.7. Тепловые сети</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.4.7. Тепловые сети")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.4.7.")]
        Item5247,

        /// <summary>
        /// <para>5.2.5. Охрана окружающей среды</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.5. Охрана окружающей среды")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.5.")]
        Item525,

        /// <summary>
        /// <para>5.2.6. Санитарно-эпидемиологическая безопасность</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.6. Санитарно-эпидемиологическая безопасность")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.6.")]
        Item526,

        /// <summary>
        /// <para>5.2.7. Пожарная безопасность</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.7. Пожарная безопасность")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.7.")]
        Item527,

        /// <summary>
        /// <para>5.2.8. Инженерно-технические мероприятия ГО и ЧС</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.8. Инженерно-технические мероприятия ГО и ЧС")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.8.")]
        Item528,

        /// <summary>
        /// <para>5.2.9. Промышленная безопасность опасных производственных объектов</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.9. Промышленная безопасность опасных производственных объектов")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.9.")]
        Item529,

        /// <summary>
        /// <para>5.2.10. Ядерная и радиационная безопасность</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.10. Ядерная и радиационная безопасность")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.10.")]
        Item5210,

        /// <summary>
        /// <para>5.2.11. Организация строительства</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.11. Организация строительства")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.11.")]
        Item5211,

        /// <summary>
        /// <para>5.2.12. Объекты транспортного комплекса</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.12. Объекты транспортного комплекса")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.12.")]
        Item5212,

        /// <summary>
        /// <para>5.2.12.1. Воздушный транспорт</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.12.1. Воздушный транспорт")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.12.1.")]
        Item52121,

        /// <summary>
        /// <para>5.2.12.2. Железнодорожный транспорт</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.12.2. Железнодорожный транспорт")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.12.2.")]
        Item52122,

        /// <summary>
        /// <para>5.2.12.3. Мосты и трубы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.12.3. Мосты и трубы")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.12.3.")]
        Item52123,

        /// <summary>
        /// <para>5.2.12.4. Тоннели и метрополитены</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.12.4. Тоннели и метрополитены")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.12.4.")]
        Item52124,

        /// <summary>
        /// <para>5.2.12.5. Автомобильные дороги</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.12.5. Автомобильные дороги")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.12.5.")]
        Item52125,

        /// <summary>
        /// <para>5.2.12.6. Морской транспорт</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.12.6. Морской транспорт")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.12.6.")]
        Item52126,

        /// <summary>
        /// <para>5.2.13. Объекты топливно-энергетического комплекса</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.13. Объекты топливно-энергетического комплекса")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.13.")]
        Item5213,

        /// <summary>
        /// <para>5.2.13.1. Объекты химических, нефтехимических и нефтегазоперерабатывающих, взрыво- и пожароопасных производств</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("5.2.13.1. Объекты химических, нефтехимических и нефтегазоперерабатывающих, взрыво" +
            "- и пожароопасных производств"))]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.13.1.")]
        Item52131,

        /// <summary>
        /// <para>5.2.13.2. Линии электропередачи и иные объекты электросетевого хозяйства</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.13.2. Линии электропередачи и иные объекты электросетевого хозяйства")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.13.2.")]
        Item52132,

        /// <summary>
        /// <para>5.2.13.3. Объекты обустройства нефтяных и газовых месторождений</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.13.3. Объекты обустройства нефтяных и газовых месторождений")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.13.3.")]
        Item52133,

        /// <summary>
        /// <para>5.2.13.4. Строительство скважин</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.13.4. Строительство скважин")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.13.4.")]
        Item52134,

        /// <summary>
        /// <para>5.2.13.5. Строительство магистральных и промысловых трубопроводов</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.13.5. Строительство магистральных и промысловых трубопроводов")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.13.5.")]
        Item52135,

        /// <summary>
        /// <para>5.2.13.6. Объекты использования атомной энергии</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.13.6. Объекты использования атомной энергии")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.13.6.")]
        Item52136,

        /// <summary>
        /// <para>5.2.13.7. Тепловые электростанции</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.13.7. Тепловые электростанции")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.13.7.")]
        Item52137,

        /// <summary>
        /// <para>5.2.14. Объекты производственного назначения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.14. Объекты производственного назначения")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.14.")]
        Item5214,

        /// <summary>
        /// <para>5.2.14.1. Объекты космической инфраструктуры</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.14.1. Объекты космической инфраструктуры")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.14.1.")]
        Item52141,

        /// <summary>
        /// <para>5.2.14.2. Объекты оборонной промышленности</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.14.2. Объекты оборонной промышленности")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.14.2.")]
        Item52142,

        /// <summary>
        /// <para>5.2.14.3. Объекты металлургической промышленности</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.14.3. Объекты металлургической промышленности")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.14.3.")]
        Item52143,

        /// <summary>
        /// <para>5.2.14.4. Объекты горнодобывающей и горно-перерабатывающей промышленности</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.14.4. Объекты горнодобывающей и горно-перерабатывающей промышленности")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.14.4.")]
        Item52144,

        /// <summary>
        /// <para>5.2.15. Объекты информатизации и связи</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.15. Объекты информатизации и связи")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.15.")]
        Item5215,

        /// <summary>
        /// <para>5.2.16. Гидротехнические сооружения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5.2.16. Гидротехнические сооружения")]
        [System.Xml.Serialization.XmlEnumAttribute("5.2.16.")]
        Item5216,

        /// <summary>
        /// <para>5.3.1. Организация государственной экспертизы проектной документации и (или) результатов инженерных изысканий</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("5.3.1. Организация государственной экспертизы проектной документации и (или) резу" +
            "льтатов инженерных изысканий"))]
        [System.Xml.Serialization.XmlEnumAttribute("5.3.1.")]
        Item531,

        /// <summary>
        /// <para>1. Инженерно-геодезические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("1. Инженерно-геодезические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("1.")]
        Item1,

        /// <summary>
        /// <para>2. Инженерно-геологические изыскания и инженерно-геотехнические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("2. Инженерно-геологические изыскания и инженерно-геотехнические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("2.")]
        Item2,

        /// <summary>
        /// <para>3. Инженерно-гидрометеорологические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("3. Инженерно-гидрометеорологические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("3.")]
        Item3,

        /// <summary>
        /// <para>4. Инженерно-экологические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("4. Инженерно-экологические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("4.")]
        Item4,

        /// <summary>
        /// <para>5. Схемы планировочной организации земельных участков</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5. Схемы планировочной организации земельных участков")]
        [System.Xml.Serialization.XmlEnumAttribute("5.")]
        Item5,

        /// <summary>
        /// <para>6. Объемно-планировочные и архитектурные решения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("6. Объемно-планировочные и архитектурные решения")]
        [System.Xml.Serialization.XmlEnumAttribute("6.")]
        Item6,

        /// <summary>
        /// <para>7. Конструктивные решения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("7. Конструктивные решения")]
        [System.Xml.Serialization.XmlEnumAttribute("7.")]
        Item7,

        /// <summary>
        /// <para>8. Охрана окружающей среды</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("8. Охрана окружающей среды")]
        [System.Xml.Serialization.XmlEnumAttribute("8.")]
        Item8,

        /// <summary>
        /// <para>9. Санитарно-эпидемиологическая безопасность</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("9. Санитарно-эпидемиологическая безопасность")]
        [System.Xml.Serialization.XmlEnumAttribute("9.")]
        Item9,

        /// <summary>
        /// <para>10. Пожарная безопасность</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("10. Пожарная безопасность")]
        [System.Xml.Serialization.XmlEnumAttribute("10.")]
        Item10,

        /// <summary>
        /// <para>11. Инженерно-технические мероприятия ГО и ЧС</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("11. Инженерно-технические мероприятия ГО и ЧС")]
        [System.Xml.Serialization.XmlEnumAttribute("11.")]
        Item111,

        /// <summary>
        /// <para>12. Организация строительства</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("12. Организация строительства")]
        [System.Xml.Serialization.XmlEnumAttribute("12.")]
        Item121,

        /// <summary>
        /// <para>13. Системы водоснабжения и водоотведения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("13. Системы водоснабжения и водоотведения")]
        [System.Xml.Serialization.XmlEnumAttribute("13.")]
        Item131,

        /// <summary>
        /// <para>14. Системы отопления, вентиляции, кондиционирования воздуха и холодоснабжения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("14. Системы отопления, вентиляции, кондиционирования воздуха и холодоснабжения")]
        [System.Xml.Serialization.XmlEnumAttribute("14.")]
        Item141,

        /// <summary>
        /// <para>15. Системы газоснабжения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("15. Системы газоснабжения")]
        [System.Xml.Serialization.XmlEnumAttribute("15.")]
        Item151,

        /// <summary>
        /// <para>16. Системы электроснабжения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("16. Системы электроснабжения")]
        [System.Xml.Serialization.XmlEnumAttribute("16.")]
        Item16,

        /// <summary>
        /// <para>17. Системы связи и сигнализации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("17. Системы связи и сигнализации")]
        [System.Xml.Serialization.XmlEnumAttribute("17.")]
        Item17,

        /// <summary>
        /// <para>17.1. Ценообразование и сметное нормирование</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("17.1. Ценообразование и сметное нормирование")]
        [System.Xml.Serialization.XmlEnumAttribute("17.1.")]
        Item171,

        /// <summary>
        /// <para>18. Мосты и трубы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("18. Мосты и трубы")]
        [System.Xml.Serialization.XmlEnumAttribute("18.")]
        Item18,

        /// <summary>
        /// <para>19. Автомобильные дороги</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("19. Автомобильные дороги")]
        [System.Xml.Serialization.XmlEnumAttribute("19.")]
        Item19,

        /// <summary>
        /// <para>20. Объекты топливно-энергетического комплекса</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("20. Объекты топливно-энергетического комплекса")]
        [System.Xml.Serialization.XmlEnumAttribute("20.")]
        Item20,

        /// <summary>
        /// <para>21. Объекты информатизации и связи</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("21. Объекты информатизации и связи")]
        [System.Xml.Serialization.XmlEnumAttribute("21.")]
        Item215,

        /// <summary>
        /// <para>22. Инженерно-геодезические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("22. Инженерно-геодезические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("22.")]
        Item224,

        /// <summary>
        /// <para>23. Инженерно-геологические изыскания и инженерно-геотехнические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("23. Инженерно-геологические изыскания и инженерно-геотехнические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("23.")]
        Item233,

        /// <summary>
        /// <para>24. Инженерно-гидрометеорологические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("24. Инженерно-гидрометеорологические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("24.")]
        Item243,

        /// <summary>
        /// <para>25. Инженерно-экологические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("25. Инженерно-экологические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("25.")]
        Item251,

        /// <summary>
        /// <para>26. Схемы планировочной организации земельных участков</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("26. Схемы планировочной организации земельных участков")]
        [System.Xml.Serialization.XmlEnumAttribute("26.")]
        Item26,

        /// <summary>
        /// <para>27. Объемно-планировочные решения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("27. Объемно-планировочные решения")]
        [System.Xml.Serialization.XmlEnumAttribute("27.")]
        Item27,

        /// <summary>
        /// <para>28. Конструктивные решения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("28. Конструктивные решения")]
        [System.Xml.Serialization.XmlEnumAttribute("28.")]
        Item28,

        /// <summary>
        /// <para>29. Охрана окружающей среды</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("29. Охрана окружающей среды")]
        [System.Xml.Serialization.XmlEnumAttribute("29.")]
        Item29,

        /// <summary>
        /// <para>30. Санитарно-эпидемиологическая безопасность</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("30. Санитарно-эпидемиологическая безопасность")]
        [System.Xml.Serialization.XmlEnumAttribute("30.")]
        Item30,

        /// <summary>
        /// <para>31. Пожарная безопасность</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("31. Пожарная безопасность")]
        [System.Xml.Serialization.XmlEnumAttribute("31.")]
        Item311,

        /// <summary>
        /// <para>32. Инженерно-технические мероприятия ГО и ЧС</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("32. Инженерно-технические мероприятия ГО и ЧС")]
        [System.Xml.Serialization.XmlEnumAttribute("32.")]
        Item32,

        /// <summary>
        /// <para>33. Промышленная безопасность опасных производственных объектов</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("33. Промышленная безопасность опасных производственных объектов")]
        [System.Xml.Serialization.XmlEnumAttribute("33.")]
        Item33,

        /// <summary>
        /// <para>34. Ядерная и радиационная безопасность</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("34. Ядерная и радиационная безопасность")]
        [System.Xml.Serialization.XmlEnumAttribute("34.")]
        Item34,

        /// <summary>
        /// <para>35. Организация строительства</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("35. Организация строительства")]
        [System.Xml.Serialization.XmlEnumAttribute("35.")]
        Item35,

        /// <summary>
        /// <para>35.1. Ценообразование и сметное нормирование</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("35.1. Ценообразование и сметное нормирование")]
        [System.Xml.Serialization.XmlEnumAttribute("35.1.")]
        Item351,

        /// <summary>
        /// <para>36. Системы электроснабжения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("36. Системы электроснабжения")]
        [System.Xml.Serialization.XmlEnumAttribute("36.")]
        Item36,

        /// <summary>
        /// <para>37. Системы водоснабжения и водоотведения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("37. Системы водоснабжения и водоотведения")]
        [System.Xml.Serialization.XmlEnumAttribute("37.")]
        Item37,

        /// <summary>
        /// <para>38. Системы отопления, вентиляции, кондиционирования воздуха и холодоснабжения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("38. Системы отопления, вентиляции, кондиционирования воздуха и холодоснабжения")]
        [System.Xml.Serialization.XmlEnumAttribute("38.")]
        Item38,

        /// <summary>
        /// <para>39. Системы связи и сигнализации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("39. Системы связи и сигнализации")]
        [System.Xml.Serialization.XmlEnumAttribute("39.")]
        Item39,

        /// <summary>
        /// <para>40. Системы газоснабжения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("40. Системы газоснабжения")]
        [System.Xml.Serialization.XmlEnumAttribute("40.")]
        Item40,

        /// <summary>
        /// <para>41. Системы автоматизации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("41. Системы автоматизации")]
        [System.Xml.Serialization.XmlEnumAttribute("41.")]
        Item411,

        /// <summary>
        /// <para>42. Системы теплоснабжения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("42. Системы теплоснабжения")]
        [System.Xml.Serialization.XmlEnumAttribute("42.")]
        Item421,

        /// <summary>
        /// <para>43. Объекты авиационной инфраструктуры</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("43. Объекты авиационной инфраструктуры")]
        [System.Xml.Serialization.XmlEnumAttribute("43.")]
        Item431,

        /// <summary>
        /// <para>44. Объекты инфраструктуры железнодорожного транспорта</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("44. Объекты инфраструктуры железнодорожного транспорта")]
        [System.Xml.Serialization.XmlEnumAttribute("44.")]
        Item441,

        /// <summary>
        /// <para>45. Мосты и трубы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("45. Мосты и трубы")]
        [System.Xml.Serialization.XmlEnumAttribute("45.")]
        Item451,

        /// <summary>
        /// <para>46. Тоннели и метрополитены</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("46. Тоннели и метрополитены")]
        [System.Xml.Serialization.XmlEnumAttribute("46.")]
        Item46,

        /// <summary>
        /// <para>47. Автомобильные дороги</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("47. Автомобильные дороги")]
        [System.Xml.Serialization.XmlEnumAttribute("47.")]
        Item47,

        /// <summary>
        /// <para>48. Объекты морского и речного транспорта</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("48. Объекты морского и речного транспорта")]
        [System.Xml.Serialization.XmlEnumAttribute("48.")]
        Item48,

        /// <summary>
        /// <para>49. Объекты химических, нефтехимических и нефтегазоперерабатывающих, взрыво- и пожароопасных производств</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("49. Объекты химических, нефтехимических и нефтегазоперерабатывающих, взрыво- и по" +
            "жароопасных производств"))]
        [System.Xml.Serialization.XmlEnumAttribute("49.")]
        Item49,

        /// <summary>
        /// <para>50. Линии электропередачи и иные объекты электросетевого хозяйства</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("50. Линии электропередачи и иные объекты электросетевого хозяйства")]
        [System.Xml.Serialization.XmlEnumAttribute("50.")]
        Item50,

        /// <summary>
        /// <para>51. Объекты обустройства нефтяных и газовых месторождений</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("51. Объекты обустройства нефтяных и газовых месторождений")]
        [System.Xml.Serialization.XmlEnumAttribute("51.")]
        Item51,

        /// <summary>
        /// <para>52. Скважины</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("52. Скважины")]
        [System.Xml.Serialization.XmlEnumAttribute("52.")]
        Item52,

        /// <summary>
        /// <para>53. Магистральные и промысловые трубопроводы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("53. Магистральные и промысловые трубопроводы")]
        [System.Xml.Serialization.XmlEnumAttribute("53.")]
        Item53,

        /// <summary>
        /// <para>54. Объекты использования атомной энергии</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("54. Объекты использования атомной энергии")]
        [System.Xml.Serialization.XmlEnumAttribute("54.")]
        Item54,

        /// <summary>
        /// <para>55. Тепловые электростанции</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("55. Тепловые электростанции")]
        [System.Xml.Serialization.XmlEnumAttribute("55.")]
        Item55,

        /// <summary>
        /// <para>56. Объекты оборонной промышленности и иные объекты производственного назначения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("56. Объекты оборонной промышленности и иные объекты производственного назначения")]
        [System.Xml.Serialization.XmlEnumAttribute("56.")]
        Item56,

        /// <summary>
        /// <para>57. Объекты металлургической промышленности</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("57. Объекты металлургической промышленности")]
        [System.Xml.Serialization.XmlEnumAttribute("57.")]
        Item57,

        /// <summary>
        /// <para>58. Объекты горнодобывающей и горноперерабатывающей промышленности</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("58. Объекты горнодобывающей и горноперерабатывающей промышленности")]
        [System.Xml.Serialization.XmlEnumAttribute("58.")]
        Item58,

        /// <summary>
        /// <para>59. Объекты информатизации и связи</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("59. Объекты информатизации и связи")]
        [System.Xml.Serialization.XmlEnumAttribute("59.")]
        Item59,

        /// <summary>
        /// <para>60. Гидротехнические сооружения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("60. Гидротехнические сооружения")]
        [System.Xml.Serialization.XmlEnumAttribute("60.")]
        Item60,

        /// <summary>
        /// <para>61. Объекты обезвреживания и захоронения отходов I-V классов опасности</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("61. Объекты обезвреживания и захоронения отходов I-V классов опасности")]
        [System.Xml.Serialization.XmlEnumAttribute("61.")]
        Item61,

        /// <summary>
        /// <para>62. Охрана объектов культурного наследия</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("62. Охрана объектов культурного наследия")]
        [System.Xml.Serialization.XmlEnumAttribute("62.")]
        Item62,

        /// <summary>
        /// <para>63. Объекты социально-культурного назначения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("63. Объекты социально-культурного назначения")]
        [System.Xml.Serialization.XmlEnumAttribute("63.")]
        Item63,

        /// <summary>
        /// <para>1. Инженерно-геодезические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("1. Инженерно-геодезические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("955.1.")]
        Item9551,

        /// <summary>
        /// <para>2. Инженерно-геологические изыскания и инженерно-геотехнические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("2. Инженерно-геологические изыскания и инженерно-геотехнические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("955.2.")]
        Item9552,

        /// <summary>
        /// <para>3. Инженерно-гидрометеорологические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("3. Инженерно-гидрометеорологические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("955.3.")]
        Item9553,

        /// <summary>
        /// <para>4. Инженерно-экологические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("4. Инженерно-экологические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("955.4.")]
        Item9554,

        /// <summary>
        /// <para>5. Схемы планировочной организации земельных участков</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("5. Схемы планировочной организации земельных участков")]
        [System.Xml.Serialization.XmlEnumAttribute("955.5.")]
        Item9555,

        /// <summary>
        /// <para>6. Объемно-планировочные и архитектурные решения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("6. Объемно-планировочные и архитектурные решения")]
        [System.Xml.Serialization.XmlEnumAttribute("955.6.")]
        Item9556,

        /// <summary>
        /// <para>7. Конструктивные решения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("7. Конструктивные решения")]
        [System.Xml.Serialization.XmlEnumAttribute("955.7.")]
        Item9557,

        /// <summary>
        /// <para>8. Охрана окружающей среды</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("8. Охрана окружающей среды")]
        [System.Xml.Serialization.XmlEnumAttribute("955.8.")]
        Item9558,

        /// <summary>
        /// <para>9. Санитарно-эпидемиологическая безопасность</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("9. Санитарно-эпидемиологическая безопасность")]
        [System.Xml.Serialization.XmlEnumAttribute("955.9.")]
        Item9559,

        /// <summary>
        /// <para>10. Пожарная безопасность</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("10. Пожарная безопасность")]
        [System.Xml.Serialization.XmlEnumAttribute("955.10.")]
        Item95510,

        /// <summary>
        /// <para>11. Инженерно-технические мероприятия по гражданской обороне и чрезвычайным ситуациям</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("11. Инженерно-технические мероприятия по гражданской обороне и чрезвычайным ситуа" +
            "циям"))]
        [System.Xml.Serialization.XmlEnumAttribute("955.11.")]
        Item95511,

        /// <summary>
        /// <para>12. Промышленная безопасность опасных производственных объектов</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("12. Промышленная безопасность опасных производственных объектов")]
        [System.Xml.Serialization.XmlEnumAttribute("955.12.")]
        Item95512,

        /// <summary>
        /// <para>13. Ядерная и радиационная безопасность</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("13. Ядерная и радиационная безопасность")]
        [System.Xml.Serialization.XmlEnumAttribute("955.13.")]
        Item95513,

        /// <summary>
        /// <para>14. Организация строительства</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("14. Организация строительства")]
        [System.Xml.Serialization.XmlEnumAttribute("955.14.")]
        Item95514,

        /// <summary>
        /// <para>15. Ценообразование и сметное нормирование</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("15. Ценообразование и сметное нормирование")]
        [System.Xml.Serialization.XmlEnumAttribute("955.15.")]
        Item95515,

        /// <summary>
        /// <para>16. Системы электроснабжения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("16. Системы электроснабжения")]
        [System.Xml.Serialization.XmlEnumAttribute("955.16.")]
        Item95516,

        /// <summary>
        /// <para>17. Системы водоснабжения и водоотведения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("17. Системы водоснабжения и водоотведения")]
        [System.Xml.Serialization.XmlEnumAttribute("955.17.")]
        Item95517,

        /// <summary>
        /// <para>18. Системы отопления, вентиляции, кондиционирования воздуха и холодоснабжения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("18. Системы отопления, вентиляции, кондиционирования воздуха и холодоснабжения")]
        [System.Xml.Serialization.XmlEnumAttribute("955.18.")]
        Item95518,

        /// <summary>
        /// <para>19. Системы связи и сигнализации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("19. Системы связи и сигнализации")]
        [System.Xml.Serialization.XmlEnumAttribute("955.19.")]
        Item95519,

        /// <summary>
        /// <para>20. Системы газоснабжения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("20. Системы газоснабжения")]
        [System.Xml.Serialization.XmlEnumAttribute("955.20.")]
        Item95520,

        /// <summary>
        /// <para>21. Системы автоматизации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("21. Системы автоматизации")]
        [System.Xml.Serialization.XmlEnumAttribute("955.21.")]
        Item95521,

        /// <summary>
        /// <para>22. Системы теплоснабжения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("22. Системы теплоснабжения")]
        [System.Xml.Serialization.XmlEnumAttribute("955.22.")]
        Item95522,

        /// <summary>
        /// <para>23. Объекты авиационной инфраструктуры</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("23. Объекты авиационной инфраструктуры")]
        [System.Xml.Serialization.XmlEnumAttribute("955.23.")]
        Item95523,

        /// <summary>
        /// <para>24. Объекты инфраструктуры железнодорожного транспорта</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("24. Объекты инфраструктуры железнодорожного транспорта")]
        [System.Xml.Serialization.XmlEnumAttribute("955.24.")]
        Item95524,

        /// <summary>
        /// <para>25. Мосты и трубы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("25. Мосты и трубы")]
        [System.Xml.Serialization.XmlEnumAttribute("955.25.")]
        Item95525,

        /// <summary>
        /// <para>26. Тоннели и метрополитены</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("26. Тоннели и метрополитены")]
        [System.Xml.Serialization.XmlEnumAttribute("955.26.")]
        Item95526,

        /// <summary>
        /// <para>27. Автомобильные дороги</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("27. Автомобильные дороги")]
        [System.Xml.Serialization.XmlEnumAttribute("955.27.")]
        Item95527,

        /// <summary>
        /// <para>28. Объекты морского и речного транспорта</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("28. Объекты морского и речного транспорта")]
        [System.Xml.Serialization.XmlEnumAttribute("955.28.")]
        Item95528,

        /// <summary>
        /// <para>29. Объекты химических, нефтехимических и нефтегазоперерабатывающих, взрыво- и пожароопасных производств</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("29. Объекты химических, нефтехимических и нефтегазоперерабатывающих, взрыво- и по" +
            "жароопасных производств"))]
        [System.Xml.Serialization.XmlEnumAttribute("955.29.")]
        Item95529,

        /// <summary>
        /// <para>30. Линии электропередачи и иные объекты электросетевого хозяйства</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("30. Линии электропередачи и иные объекты электросетевого хозяйства")]
        [System.Xml.Serialization.XmlEnumAttribute("955.30.")]
        Item95530,

        /// <summary>
        /// <para>31. Объекты обустройства нефтяных и газовых месторождений</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("31. Объекты обустройства нефтяных и газовых месторождений")]
        [System.Xml.Serialization.XmlEnumAttribute("955.31.")]
        Item95531,

        /// <summary>
        /// <para>32. Скважины</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("32. Скважины")]
        [System.Xml.Serialization.XmlEnumAttribute("955.32.")]
        Item95532,

        /// <summary>
        /// <para>33. Магистральные и промысловые трубопроводы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("33. Магистральные и промысловые трубопроводы")]
        [System.Xml.Serialization.XmlEnumAttribute("955.33.")]
        Item95533,

        /// <summary>
        /// <para>34. Объекты использования атомной энергии</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("34. Объекты использования атомной энергии")]
        [System.Xml.Serialization.XmlEnumAttribute("955.34.")]
        Item95534,

        /// <summary>
        /// <para>35. Тепловые электростанции</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("35. Тепловые электростанции")]
        [System.Xml.Serialization.XmlEnumAttribute("955.35.")]
        Item95535,

        /// <summary>
        /// <para>36. Объекты оборонной промышленности и иные объекты производственного назначения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("36. Объекты оборонной промышленности и иные объекты производственного назначения")]
        [System.Xml.Serialization.XmlEnumAttribute("955.36.")]
        Item95536,

        /// <summary>
        /// <para>37. Объекты металлургической промышленности</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("37. Объекты металлургической промышленности")]
        [System.Xml.Serialization.XmlEnumAttribute("955.37.")]
        Item95537,

        /// <summary>
        /// <para>38. Объекты горнодобывающей и горноперерабатывающей промышленности</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("38. Объекты горнодобывающей и горноперерабатывающей промышленности")]
        [System.Xml.Serialization.XmlEnumAttribute("955.38.")]
        Item95538,

        /// <summary>
        /// <para>39. Объекты информатизации и связи</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("39. Объекты информатизации и связи")]
        [System.Xml.Serialization.XmlEnumAttribute("955.39.")]
        Item95539,

        /// <summary>
        /// <para>40. Гидротехнические сооружения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("40. Гидротехнические сооружения")]
        [System.Xml.Serialization.XmlEnumAttribute("955.40.")]
        Item95540,

        /// <summary>
        /// <para>41. Объекты обезвреживания и захоронения отходов I-V классов опасности</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("41. Объекты обезвреживания и захоронения отходов I-V классов опасности")]
        [System.Xml.Serialization.XmlEnumAttribute("955.41.")]
        Item95541,

        /// <summary>
        /// <para>42. Охрана объектов культурного наследия</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("42. Охрана объектов культурного наследия")]
        [System.Xml.Serialization.XmlEnumAttribute("955.42.")]
        Item95542,

        /// <summary>
        /// <para>43. Объекты социально-культурного назначения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("43. Объекты социально-культурного назначения")]
        [System.Xml.Serialization.XmlEnumAttribute("955.43.")]
        Item95543,
    }

    /// <summary>
    /// <para>Сведения в части экспертизы результатов инженерных изысканий, подготовленные экспертом</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute(("Сведения в части экспертизы результатов инженерных изысканий, подготовленные эксп" +
        "ертом"))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tExpertEngineeringSurveys", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TExpertEngineeringSurveys
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TMismatch> _mismatches;

        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="TExpertEngineeringSurveys" /> class.</para>
        /// </summary>
        public TExpertEngineeringSurveys()
        {
            this._mismatches = new System.Collections.ObjectModel.Collection<TMismatch>();
        }

        /// <summary>
        /// <para>Вид инженерных изысканий</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вид инженерных изысканий")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlAttributeAttribute("EngineeringSurveyType")]
        public TEngineeringSurveyType EngineeringSurveyType { get; set; }

        /// <summary>
        /// <para>Сведения о несоответствии результатов инженерных изысканий требованиям технических регламентов</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сведения о несоответствии результатов инженерных изысканий требованиям технически" +
            "х регламентов"))]
        [System.Xml.Serialization.XmlArrayAttribute("Mismatches")]
        [System.Xml.Serialization.XmlArrayItemAttribute("NormsMismatch")]
        public System.Collections.ObjectModel.Collection<TMismatch> Mismatches
        {
            get
            {
                return _mismatches;
            }
            private set
            {
                _mismatches = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the Mismatches collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MismatchesSpecified
        {
            get
            {
                return (this.Mismatches.Count != 0);
            }
        }
    }

    /// <summary>
    /// <para>Сведения о несоответствии результатов инженерных изыcканий требованиям технических регламентов</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute(("Сведения о несоответствии результатов инженерных изыcканий требованиям технически" +
        "х регламентов"))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tEngineeringSurveyMismatches", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TEngineeringSurveyMismatches
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TMismatch> _normsMismatch;

        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="TEngineeringSurveyMismatches" /> class.</para>
        /// </summary>
        public TEngineeringSurveyMismatches()
        {
            this._normsMismatch = new System.Collections.ObjectModel.Collection<TMismatch>();
        }

        /// <summary>
        /// <para>Сведения о несоответствии требованиям технических регламентов</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сведения о несоответствии требованиям технических регламентов")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("NormsMismatch")]
        public System.Collections.ObjectModel.Collection<TMismatch> NormsMismatch
        {
            get
            {
                return _normsMismatch;
            }
            private set
            {
                _normsMismatch = value;
            }
        }
    }

    /// <summary>
    /// <para>Сведения о несоответствии</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Сведения о несоответствии")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tMismatch", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TMismatch
    {

        /// <summary>
        /// <para>Ссылка на конкретное требование, несоответствие которому было выявлено в ходе экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Ссылка на конкретное требование, несоответствие которому было выявлено в ходе экс" +
            "пертизы"))]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Link")]
        public string Link { get; set; }

        /// <summary>
        /// <para>Ссылка на материалы, в отношении которых сделан вывод о несоответствии</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ссылка на материалы, в отношении которых сделан вывод о несоответствии")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Part")]
        public string Part { get; set; }

        /// <summary>
        /// <para>Вывод о несоответствии</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вывод о несоответствии")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Summary")]
        public string Summary { get; set; }
    }

    /// <summary>
    /// <para>Виды инженерных изысканий</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Виды инженерных изысканий")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tEngineeringSurveyType", Namespace = "")]
    public enum TEngineeringSurveyType
    {

        /// <summary>
        /// <para>Инженерно-геодезические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Инженерно-геодезические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <summary>
        /// <para>Инженерно-геологические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Инженерно-геологические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <summary>
        /// <para>Инженерно-гидрометеорологические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Инженерно-гидрометеорологические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,

        /// <summary>
        /// <para>Инженерно-экологические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Инженерно-экологические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        Item4,

        /// <summary>
        /// <para>Инженерно-геотехнические изыскания</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Инженерно-геотехнические изыскания")]
        [System.Xml.Serialization.XmlEnumAttribute("5")]
        Item5,

        /// <summary>
        /// <para>Геотехнические исследования</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Геотехнические исследования")]
        [System.Xml.Serialization.XmlEnumAttribute("6")]
        Item6,

        /// <summary>
        /// <para>Обследования состояния грунтов оснований зданий и сооружений, их строительных конструкций</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Обследования состояния грунтов оснований зданий и сооружений, их строительных кон" +
            "струкций"))]
        [System.Xml.Serialization.XmlEnumAttribute("7")]
        Item7,

        /// <summary>
        /// <para>Поиск и разведка подземных вод для целей водоснабжения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Поиск и разведка подземных вод для целей водоснабжения")]
        [System.Xml.Serialization.XmlEnumAttribute("8")]
        Item8,

        /// <summary>
        /// <para>Локальный мониторинг компонентов окружающей среды</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Локальный мониторинг компонентов окружающей среды")]
        [System.Xml.Serialization.XmlEnumAttribute("9")]
        Item9,

        /// <summary>
        /// <para>Разведка грунтовых строительных материалов</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Разведка грунтовых строительных материалов")]
        [System.Xml.Serialization.XmlEnumAttribute("10")]
        Item10,

        /// <summary>
        /// <para>Локальные обследования загрязнения грунтов и грунтовых вод</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Локальные обследования загрязнения грунтов и грунтовых вод")]
        [System.Xml.Serialization.XmlEnumAttribute("11")]
        Item11,
    }

    /// <summary>
    /// <para>Сведения в части эспертизы проектной документации, подготовленные экспертом</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Сведения в части эспертизы проектной документации, подготовленные экспертом")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tExpertProjectDocuments", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TExpertProjectDocuments
    {

        /// <summary>
        /// <para>Сведения о решениях, которые которые в случае их реализации могли привести к риску возникновения аварийных ситуаций, гибели людей, причинения значительного материального ущерба и которые в ходе проведения государственной экспертизы были приведены в соответствие с установленными требованиями</para>
        /// <para>Непустая текстовая строка</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(@"Сведения о решениях, которые которые в случае их реализации могли привести к риску возникновения аварийных ситуаций, гибели людей, причинения значительного материального ущерба и которые в ходе проведения государственной экспертизы были приведены в соответствие с установленными требованиями")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("DangerSolutions")]
        public string DangerSolutions { get; set; }

        /// <summary>
        /// <para>Направление деятельности в области экспертизы проектной документации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Направление деятельности в области экспертизы проектной документации")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlAttributeAttribute("ExpertType")]
        public TExpertType ExpertType { get; set; }

        /// <summary>
        /// <para>Сведения о несоответствии проектной документации установленным требованиям</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сведения о несоответствии проектной документации установленным требованиям")]
        [System.Xml.Serialization.XmlElementAttribute("Mismatches")]
        public TProjectDocumentsMismatches Mismatches { get; set; }
    }

    /// <summary>
    /// <para>Сведения о несоответствии проектной документации установленным требованиям</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Сведения о несоответствии проектной документации установленным требованиям")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tProjectDocumentsMismatches", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TProjectDocumentsMismatches
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TMismatch> _engineeringSurveyMismatch;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TMismatch> _normsMismatch;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TMismatch> _projectTaskMismatch;

        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="TProjectDocumentsMismatches" /> class.</para>
        /// </summary>
        public TProjectDocumentsMismatches()
        {
            this._engineeringSurveyMismatch = new System.Collections.ObjectModel.Collection<TMismatch>();
            this._projectTaskMismatch = new System.Collections.ObjectModel.Collection<TMismatch>();
            this._normsMismatch = new System.Collections.ObjectModel.Collection<TMismatch>();
        }

        /// <summary>
        /// <para>Описание решения, реализация которого может привести к риску возникновения аварийных ситуаций на объекте капитального строительства, гибели людей, причинения значительного материального ущерба</para>
        /// <para>Непустая текстовая строка</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Описание решения, реализация которого может привести к риску возникновения аварий" +
            "ных ситуаций на объекте капитального строительства, гибели людей, причинения зна" +
            "чительного материального ущерба"))]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("DangerMismatch")]
        public string DangerMismatch { get; set; }

        /// <summary>
        /// <para>Сведения о несоответствии проектной документации результату инженерных изысканий</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сведения о несоответствии проектной документации результату инженерных изысканий")]
        [System.Xml.Serialization.XmlElementAttribute("EngineeringSurveyMismatch")]
        public System.Collections.ObjectModel.Collection<TMismatch> EngineeringSurveyMismatch
        {
            get
            {
                return _engineeringSurveyMismatch;
            }
            private set
            {
                _engineeringSurveyMismatch = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the EngineeringSurveyMismatch collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EngineeringSurveyMismatchSpecified
        {
            get
            {
                return (this.EngineeringSurveyMismatch.Count != 0);
            }
        }

        /// <summary>
        /// <para>Сведения о несоответствии проектной документации требованиям технических регламентов</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сведения о несоответствии проектной документации требованиям технических регламен" +
            "тов"))]
        [System.Xml.Serialization.XmlElementAttribute("NormsMismatch")]
        public System.Collections.ObjectModel.Collection<TMismatch> NormsMismatch
        {
            get
            {
                return _normsMismatch;
            }
            private set
            {
                _normsMismatch = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the NormsMismatch collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NormsMismatchSpecified
        {
            get
            {
                return (this.NormsMismatch.Count != 0);
            }
        }

        /// <summary>
        /// <para>Сведения о несоответствии проектной документации заданию на проектирование</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сведения о несоответствии проектной документации заданию на проектирование")]
        [System.Xml.Serialization.XmlElementAttribute("ProjectTaskMismatch")]
        public System.Collections.ObjectModel.Collection<TMismatch> ProjectTaskMismatch
        {
            get
            {
                return _projectTaskMismatch;
            }
            private set
            {
                _projectTaskMismatch = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the ProjectTaskMismatch collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ProjectTaskMismatchSpecified
        {
            get
            {
                return (this.ProjectTaskMismatch.Count != 0);
            }
        }
    }

    /// <summary>
    /// <para>Сведения в части проверки достоверности определения сметной стоимости, подготовленные экспертом</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute(("Сведения в части проверки достоверности определения сметной стоимости, подготовле" +
        "нные экспертом"))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tExpertEstimate", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TExpertEstimate
    {

        /// <summary>
        /// <para>Информация об использовании сметных нормативов</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Информация об использовании сметных нормативов")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("EstimateNorms")]
        public string EstimateNorms { get; set; }

        /// <summary>
        /// <para>Сведения о несоответствии сметной части проектной документации установленным требованиям</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сведения о несоответствии сметной части проектной документации установленным треб" +
            "ованиям"))]
        [System.Xml.Serialization.XmlElementAttribute("Mismatches")]
        public TEstimateMismatches Mismatches { get; set; }
    }

    /// <summary>
    /// <para>Сведения о несоответствии сметной документации, выявленных в ходе проверки достоверности определения сметной стоимости</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute(("Сведения о несоответствии сметной документации, выявленных в ходе проверки достов" +
        "ерности определения сметной стоимости"))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tEstimateMismatches", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TEstimateMismatches
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TMismatch> _basicMismatch;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TMismatch> _commonMismatch;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TMismatch> _fullCalculationMismatch;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TMismatch> _localCalculationMismatch;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TMismatchExtended> _projectDocumentsMismatch;

        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="TEstimateMismatches" /> class.</para>
        /// </summary>
        public TEstimateMismatches()
        {
            this._commonMismatch = new System.Collections.ObjectModel.Collection<TMismatch>();
            this._fullCalculationMismatch = new System.Collections.ObjectModel.Collection<TMismatch>();
            this._localCalculationMismatch = new System.Collections.ObjectModel.Collection<TMismatch>();
            this._projectDocumentsMismatch = new System.Collections.ObjectModel.Collection<TMismatchExtended>();
            this._basicMismatch = new System.Collections.ObjectModel.Collection<TMismatch>();
        }

        /// <summary>
        /// <para>Замечание по порядку пересчета сметной стоимости из базисного уровня цен в текущий уровень цен</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Замечание по порядку пересчета сметной стоимости из базисного уровня цен в текущи" +
            "й уровень цен"))]
        [System.Xml.Serialization.XmlElementAttribute("BasicMismatch")]
        public System.Collections.ObjectModel.Collection<TMismatch> BasicMismatch
        {
            get
            {
                return _basicMismatch;
            }
            private set
            {
                _basicMismatch = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the BasicMismatch collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BasicMismatchSpecified
        {
            get
            {
                return (this.BasicMismatch.Count != 0);
            }
        }

        /// <summary>
        /// <para>Общее замечание</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Общее замечание")]
        [System.Xml.Serialization.XmlElementAttribute("CommonMismatch")]
        public System.Collections.ObjectModel.Collection<TMismatch> CommonMismatch
        {
            get
            {
                return _commonMismatch;
            }
            private set
            {
                _commonMismatch = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the CommonMismatch collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CommonMismatchSpecified
        {
            get
            {
                return (this.CommonMismatch.Count != 0);
            }
        }

        /// <summary>
        /// <para>Замечание по сводному сметному расчету</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Замечание по сводному сметному расчету")]
        [System.Xml.Serialization.XmlElementAttribute("FullCalculationMismatch")]
        public System.Collections.ObjectModel.Collection<TMismatch> FullCalculationMismatch
        {
            get
            {
                return _fullCalculationMismatch;
            }
            private set
            {
                _fullCalculationMismatch = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the FullCalculationMismatch collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FullCalculationMismatchSpecified
        {
            get
            {
                return (this.FullCalculationMismatch.Count != 0);
            }
        }

        /// <summary>
        /// <para>Замечание по объектным или локальным сметным расчетам</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Замечание по объектным или локальным сметным расчетам")]
        [System.Xml.Serialization.XmlElementAttribute("LocalCalculationMismatch")]
        public System.Collections.ObjectModel.Collection<TMismatch> LocalCalculationMismatch
        {
            get
            {
                return _localCalculationMismatch;
            }
            private set
            {
                _localCalculationMismatch = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the LocalCalculationMismatch collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LocalCalculationMismatchSpecified
        {
            get
            {
                return (this.LocalCalculationMismatch.Count != 0);
            }
        }

        /// <summary>
        /// <para>Замечание в части соответствия расчетов, содержащихся в сметной документации, физическим объемам работ, конструктивным, организационно-технологическим и другим решениям, предусмотренным проектной документацией или физическим объемам работ, включенным в ведомость объемов работ, акт, утвержденный застройщиком или техническим заказчиком</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(@"Замечание в части соответствия расчетов, содержащихся в сметной документации, физическим объемам работ, конструктивным, организационно-технологическим и другим решениям, предусмотренным проектной документацией или физическим объемам работ, включенным в ведомость объемов работ, акт, утвержденный застройщиком или техническим заказчиком")]
        [System.Xml.Serialization.XmlElementAttribute("ProjectDocumentsMismatch")]
        public System.Collections.ObjectModel.Collection<TMismatchExtended> ProjectDocumentsMismatch
        {
            get
            {
                return _projectDocumentsMismatch;
            }
            private set
            {
                _projectDocumentsMismatch = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the ProjectDocumentsMismatch collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ProjectDocumentsMismatchSpecified
        {
            get
            {
                return (this.ProjectDocumentsMismatch.Count != 0);
            }
        }
    }

    /// <summary>
    /// <para>Сведения о несоответствии</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Сведения о несоответствии")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tMismatchExtended", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TMismatchExtended
    {

        /// <summary>
        /// <para>Направление деятельности в части которого сформулированы cведения о несоответствии</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Направление деятельности в части которого сформулированы cведения о несоответстви" +
            "и"))]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlAttributeAttribute("ExpertType")]
        public TExpertType ExpertType { get; set; }

        /// <summary>
        /// <para>Ссылка на конкретное требование, несоответствие которому было выявлено в ходе экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Ссылка на конкретное требование, несоответствие которому было выявлено в ходе экс" +
            "пертизы"))]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Link")]
        public string Link { get; set; }

        /// <summary>
        /// <para>Ссылка на материалы, в отношении которых сделан вывод о несоответствии</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ссылка на материалы, в отношении которых сделан вывод о несоответствии")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Part")]
        public string Part { get; set; }

        /// <summary>
        /// <para>Вывод о несоответствии</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вывод о несоответствии")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Summary")]
        public string Summary { get; set; }
    }

    /// <summary>
    /// <para>Выводы по результатам рассмотрения документации</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Выводы по результатам рассмотрения документации")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tSummary", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TSummary
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TEngineeringSurveyType> _engineeringSurveyType;

        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="TSummary" /> class.</para>
        /// </summary>
        public TSummary()
        {
            this._engineeringSurveyType = new System.Collections.ObjectModel.Collection<TEngineeringSurveyType>();
        }

        /// <summary>
        /// <para>Вывод о соответствии результатов инженерных изысканий требованиям технических регламентов</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Вывод о соответствии результатов инженерных изысканий требованиям технических рег" +
            "ламентов"))]
        [System.Xml.Serialization.XmlElementAttribute("EngineeringSurveySummary")]
        public TEngineeringSurveysSummary EngineeringSurveySummary { get; set; }

        /// <summary>
        /// <para>Сведения о дате, по состоянию на которую действовали требования, примененные в соответствии с частью 5.2 статьи 49 Градостроительного кодекса Российской Федерации (в части экспертизы результатов инжененрных изысканий)</para>
        /// <para>Непустая текстовая строка</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сведения о дате, по состоянию на которую действовали требования, примененные в со" +
            "ответствии с частью 5.2 статьи 49 Градостроительного кодекса Российской Федераци" +
            "и (в части экспертизы результатов инжененрных изысканий)"))]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("EngineeringSurveySummaryDate")]
        public string EngineeringSurveySummaryDate { get; set; }

        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the EngineeringSurveySummary property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EngineeringSurveySummarySpecified { get; set; }

        /// <summary>
        /// <para>Вид инженерных изысканий, на соответствие которым проводилась экспертиза проектной документации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Вид инженерных изысканий, на соответствие которым проводилась экспертиза проектно" +
            "й документации"))]
        [System.Xml.Serialization.XmlElementAttribute("EngineeringSurveyType")]
        public System.Collections.ObjectModel.Collection<TEngineeringSurveyType> EngineeringSurveyType
        {
            get
            {
                return _engineeringSurveyType;
            }
            private set
            {
                _engineeringSurveyType = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the EngineeringSurveyType collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EngineeringSurveyTypeSpecified
        {
            get
            {
                return (this.EngineeringSurveyType.Count != 0);
            }
        }

        /// <summary>
        /// <para>Вывод о соответствии расчетов, содержащихся в сметной документации, утвержденным сметным нормативам, сведения о которых включены в федеральный реестр сметных нормативов, физическим объемам работ, конструктивным, организационно-технологическим и другим решениям, предусмотренным проектной документацией или Выводы о соответствии (несоответствии) расчетов, содержащихся в сметной документации, физическим объемам работ, включенным в ведомость объемов работ, акт, утвержденный застройщиком или техническим заказчиком и содержащий перечень дефектов оснований, строительных конструкций, систем инженерно-технического обеспечения и сетей инженерно-технического обеспечения с указанием качественных и количественных характеристик таких дефектов, при проведении проверки достоверности определения сметной стоимости капитального ремонта</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(@"Вывод о соответствии расчетов, содержащихся в сметной документации, утвержденным сметным нормативам, сведения о которых включены в федеральный реестр сметных нормативов, физическим объемам работ, конструктивным, организационно-технологическим и другим решениям, предусмотренным проектной документацией или Выводы о соответствии (несоответствии) расчетов, содержащихся в сметной документации, физическим объемам работ, включенным в ведомость объемов работ, акт, утвержденный застройщиком или техническим заказчиком и содержащий перечень дефектов оснований, строительных конструкций, систем инженерно-технического обеспечения и сетей инженерно-технического обеспечения с указанием качественных и количественных характеристик таких дефектов, при проведении проверки достоверности определения сметной стоимости капитального ремонта")]
        [System.Xml.Serialization.XmlElementAttribute("EstimateNormsAndWorksSummary")]
        public TEstimateNormsAndWorksSummary EstimateNormsAndWorksSummary { get; set; }

        /// <summary>
        /// <para>Вывод о соответствии расчетов, содержащихся в сметной документации, утвержденным сметным нормативам, сведения о которых включены в федеральный реестр сметных нормативов, физическим объемам работ, конструктивным, организационно-технологическим и другим решениям, предусмотренным проектной документацией или Выводы о соответствии (несоответствии) расчетов, содержащихся в сметной документации, физическим объемам работ, включенным в ведомость объемов работ, акт, утвержденный застройщиком или техническим заказчиком и содержащий перечень дефектов оснований, строительных конструкций, систем инженерно-технического обеспечения и сетей инженерно-технического обеспечения с указанием качественных и количественных характеристик таких дефектов, при проведении проверки достоверности определения сметной стоимости капитального ремонта (при оценке в соответствии с постановлением Правительства Российской Федерации от 09.08.2021 №1315</para>
        /// <para>Непустая текстовая строка</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(@"Вывод о соответствии расчетов, содержащихся в сметной документации, утвержденным сметным нормативам, сведения о которых включены в федеральный реестр сметных нормативов, физическим объемам работ, конструктивным, организационно-технологическим и другим решениям, предусмотренным проектной документацией или Выводы о соответствии (несоответствии) расчетов, содержащихся в сметной документации, физическим объемам работ, включенным в ведомость объемов работ, акт, утвержденный застройщиком или техническим заказчиком и содержащий перечень дефектов оснований, строительных конструкций, систем инженерно-технического обеспечения и сетей инженерно-технического обеспечения с указанием качественных и количественных характеристик таких дефектов, при проведении проверки достоверности определения сметной стоимости капитального ремонта (при оценке в соответствии с постановлением Правительства Российской Федерации от 09.08.2021 №1315")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("EstimateNormsAndWorksSummary1315")]
        public string EstimateNormsAndWorksSummary1315 { get; set; }

        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the EstimateNormsAndWorksSummary property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EstimateNormsAndWorksSummarySpecified { get; set; }

        /// <summary>
        /// <para>Вывод о достоверности или недостоверности определения сметной стоимости строительства, реконструкции, капитального ремонта, сноса объекта капитального строительства, работ по сохранению объектов культурного наследия (памятников истории и культуры) народов Российской Федерации</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(@"Вывод о достоверности или недостоверности определения сметной стоимости строительства, реконструкции, капитального ремонта, сноса объекта капитального строительства, работ по сохранению объектов культурного наследия (памятников истории и культуры) народов Российской Федерации")]
        [System.Xml.Serialization.XmlElementAttribute("EstimateSummary")]
        public TEstimateValidationSummary EstimateSummary { get; set; }

        /// <summary>
        /// <para>Вывод о достоверности или недостоверности определения сметной стоимости строительства, реконструкции, капитального ремонта, сноса объекта капитального строительства, работ по сохранению объектов культурного наследия (памятников истории и культуры) народов Российской Федерации (при оценке в соответствии с постановлением Правительства Российской Федерации от 09.08.2021 №1315)</para>
        /// <para>Непустая текстовая строка</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(@"Вывод о достоверности или недостоверности определения сметной стоимости строительства, реконструкции, капитального ремонта, сноса объекта капитального строительства, работ по сохранению объектов культурного наследия (памятников истории и культуры) народов Российской Федерации (при оценке в соответствии с постановлением Правительства Российской Федерации от 09.08.2021 №1315)")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("EstimateSummary1315")]
        public string EstimateSummary1315 { get; set; }

        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the EstimateSummary property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EstimateSummarySpecified { get; set; }

        /// <summary>
        /// <para>Общие выводы (итоговые выводы по результатам проведенной экспертизы)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Общие выводы (итоговые выводы по результатам проведенной экспертизы)")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ExaminationSummary")]
        public TExaminationSummary ExaminationSummary { get; set; }

        /// <summary>
        /// <para>Вывод о соответствии технической части проектной документации результатам инженерных изысканий, заданию застройщика или технического заказчика на проектирование и требованиям технических регламентов</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Вывод о соответствии технической части проектной документации результатам инженер" +
            "ных изысканий, заданию застройщика или технического заказчика на проектирование " +
            "и требованиям технических регламентов"))]
        [System.Xml.Serialization.XmlElementAttribute("ProjectDocumentsSummary")]
        public TProjectDocumentationSummary ProjectDocumentsSummary { get; set; }

        /// <summary>
        /// <para>Сведения о дате, по состоянию на которую действовали требования, примененные в соответствии с частью 5.2 статьи 49 Градостроительного кодекса Российской Федерации (в части экспертизы проектной документации)</para>
        /// <para>Непустая текстовая строка</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сведения о дате, по состоянию на которую действовали требования, примененные в со" +
            "ответствии с частью 5.2 статьи 49 Градостроительного кодекса Российской Федераци" +
            "и (в части экспертизы проектной документации)"))]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("ProjectDocumentsSummaryDate")]
        public string ProjectDocumentsSummaryDate { get; set; }

        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the ProjectDocumentsSummary property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ProjectDocumentsSummarySpecified { get; set; }
    }

    /// <summary>
    /// <para>Вывод о соответствии результатов инженерных изысканий требованиям технических регламентов.</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute(("Вывод о соответствии результатов инженерных изысканий требованиям технических рег" +
        "ламентов."))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tEngineeringSurveysSummary", Namespace = "")]
    public enum TEngineeringSurveysSummary
    {

        /// <summary>
        /// <para>Вывод о соответствии результатов инженерных изысканий требованиям технических регламентов (первичная или повторная экспертиза).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Вывод о соответствии результатов инженерных изысканий требованиям технических рег" +
            "ламентов (первичная или повторная экспертиза)."))]
        [System.Xml.Serialization.XmlEnumAttribute(("Результаты инженерных изысканий соответствуют требованиям технических регламентов" +
            "."))]
        РезультатыИнженерныхИзысканийСоответствуютТребованиямТехническихРегламентов,

        /// <summary>
        /// <para>Вывод о соответствии результатов инженерных изысканий требованиям технических регламентов (по результатам экспертного сопровождения).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Вывод о соответствии результатов инженерных изысканий требованиям технических рег" +
            "ламентов (по результатам экспертного сопровождения)."))]
        [System.Xml.Serialization.XmlEnumAttribute(("Изменения, внесенные в результаты инженерных изысканий, соответствуют требованиям" +
            " технических регламентов."))]
        ИзмененияВнесенныеВРезультатыИнженерныхИзысканийСоответствуютТребованиямТехническихРегламентов,
    }

    /// <summary>
    /// <para>Вывод о соответствии проектной документации требованиям технических регламентов и иным установленным требованиям, а также результатам инженерных изысканий, выполненных для подготовки проектной документации, и заданию на проектирование.</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute(("Вывод о соответствии проектной документации требованиям технических регламентов и" +
        " иным установленным требованиям, а также результатам инженерных изысканий, выпол" +
        "ненных для подготовки проектной документации, и заданию на проектирование."))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tProjectDocumentationSummary", Namespace = "")]
    public enum TProjectDocumentationSummary
    {

        /// <summary>
        /// <para>Вывод о соответствии технической части проектной документации результатам инженерных изысканий, заданию застройщика или технического заказчика на проектирование и требованиям технических регламентов (первичная экспертиза).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Вывод о соответствии технической части проектной документации результатам инженер" +
            "ных изысканий, заданию застройщика или технического заказчика на проектирование " +
            "и требованиям технических регламентов (первичная экспертиза)."))]
        [System.Xml.Serialization.XmlEnumAttribute(("Проектная документация соответствует требованиям технических регламентов и иным у" +
            "становленным требованиям, а также результатам инженерных изысканий, выполненных " +
            "для подготовки проектной документации, и заданию на проектирование."))]
        ПроектнаяДокументацияСоответствуетТребованиямТехническихРегламентовИИнымУстановленнымТребованиямАТакжеРезультатамИнженерныхИзысканийВыполненныхДляПодготовкиПроектнойДокументацииИЗаданиюНаПроектирование,

        /// <summary>
        /// <para>Вывод о соответствии технической части проектной документации результатам инженерных изысканий, заданию застройщика или технического заказчика на проектирование и требованиям технических регламентов (повторная экспертиза).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Вывод о соответствии технической части проектной документации результатам инженер" +
            "ных изысканий, заданию застройщика или технического заказчика на проектирование " +
            "и требованиям технических регламентов (повторная экспертиза)."))]
        [System.Xml.Serialization.XmlEnumAttribute(@"Проектная документация соответствует требованиям технических регламентов и иным установленным требованиям, а также результатам инженерных изысканий, выполненных для подготовки проектной документации, и заданию на проектирование, а также совместима с частью проектной документации, в которую  изменения не вносились.")]
        ПроектнаяДокументацияСоответствуетТребованиямТехническихРегламентовИИнымУстановленнымТребованиямАТакжеРезультатамИнженерныхИзысканийВыполненныхДляПодготовкиПроектнойДокументацииИЗаданиюНаПроектированиеАТакжеСовместимаСЧастьюПроектнойДокументацииВКоторуюИзмененияНеВносились,

        /// <summary>
        /// <para>Вывод о соответствии технической части проектной документации результатам инженерных изысканий, заданию застройщика или технического заказчика на проектирование и требованиям технических регламентов (по результатам экспертного сопровождения).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Вывод о соответствии технической части проектной документации результатам инженер" +
            "ных изысканий, заданию застройщика или технического заказчика на проектирование " +
            "и требованиям технических регламентов (по результатам экспертного сопровождения)" +
            "."))]
        [System.Xml.Serialization.XmlEnumAttribute(@"Изменения, внесенные в проектную документацию, соответствуют требованиям технических регламентов и иным установленным требованиям, результатам инженерных изысканий, выполненных для подготовки проектной документации, и заданию на проектирование, а также совместимы с частью проектной документации, в которую указанные изменения не вносились.")]
        ИзмененияВнесенныеВПроектнуюДокументациюСоответствуютТребованиямТехническихРегламентовИИнымУстановленнымТребованиямРезультатамИнженерныхИзысканийВыполненныхДляПодготовкиПроектнойДокументацииИЗаданиюНаПроектированиеАТакжеСовместимыСЧастьюПроектнойДокументацииВКоторуюУказанныеИзмененияНеВносились,
    }

    /// <summary>
    /// <para>Вывод о соответствии расчетов, содержащихся в сметной документации, утвержденным сметным нормативам, сведения о которых включены в федеральный реестр сметных нормативов, физическим объемам работ, конструктивным, организационно-технологическим и другим решениям, предусмотренным проектной документацией</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute(@"Вывод о соответствии расчетов, содержащихся в сметной документации, утвержденным сметным нормативам, сведения о которых включены в федеральный реестр сметных нормативов, физическим объемам работ, конструктивным, организационно-технологическим и другим решениям, предусмотренным проектной документацией")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tEstimateNormsAndWorksSummary", Namespace = "")]
    public enum TEstimateNormsAndWorksSummary
    {

        /// <summary>
        /// <para>Вывод о соответствии расчетов, содержащиеся в сметной документации, соответствуют утвержденным сметным нормативам, сведения о которых включены в федеральный реестр сметных нормативов, физическим объемам работ, конструктивным, организационно-технологическим и другим решениям, предусмотренным проектной документацией.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(@"Вывод о соответствии расчетов, содержащиеся в сметной документации, соответствуют утвержденным сметным нормативам, сведения о которых включены в федеральный реестр сметных нормативов, физическим объемам работ, конструктивным, организационно-технологическим и другим решениям, предусмотренным проектной документацией.")]
        [System.Xml.Serialization.XmlEnumAttribute(@"Расчеты, содержащиеся в сметной документации, соответствуют утвержденным сметным нормативам, сведения о которых включены в федеральный реестр сметных нормативов, физическим объемам работ, конструктивным, организационно-технологическим и другим решениям, предусмотренным проектной документацией.")]
        РасчетыСодержащиесяВСметнойДокументацииСоответствуютУтвержденнымСметнымНормативамСведенияОКоторыхВключеныВФедеральныйРеестрСметныхНормативовФизическимОбъемамРаботКонструктивнымОрганизационноТехнологическимИДругимРешениямПредусмотреннымПроектнойДокументацией,

        /// <summary>
        /// <para>Положительный вывод о соответствии расчетов, содержащихся в сметной документации, физическим объемам работ, включенным в ведомость объемов работ, акт, утвержденный застройщиком или техническим заказчиком и содержащий перечень дефектов оснований, строительных конструкций, систем инженерно-технического обеспечения и сетей инженерно-технического обеспечения с указанием качественных и количественных характеристик таких дефектов, при проведении проверки достоверности определения сметной стоимости капитального ремонта(в случае наличия акта, утвержденного застройщиком или техническим заказчиком и содержащий перечень дефектов оснований, строительных конструкций, систем инженерно-технического обеспечения и сетей инженерно-технического обеспечения с указанием качественных и количественных характеристик таких дефектов, при проведении проверки достоверности определения сметной стоимости капитального ремонта).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(@"Положительный вывод о соответствии расчетов, содержащихся в сметной документации, физическим объемам работ, включенным в ведомость объемов работ, акт, утвержденный застройщиком или техническим заказчиком и содержащий перечень дефектов оснований, строительных конструкций, систем инженерно-технического обеспечения и сетей инженерно-технического обеспечения с указанием качественных и количественных характеристик таких дефектов, при проведении проверки достоверности определения сметной стоимости капитального ремонта(в случае наличия акта, утвержденного застройщиком или техническим заказчиком и содержащий перечень дефектов оснований, строительных конструкций, систем инженерно-технического обеспечения и сетей инженерно-технического обеспечения с указанием качественных и количественных характеристик таких дефектов, при проведении проверки достоверности определения сметной стоимости капитального ремонта).")]
        [System.Xml.Serialization.XmlEnumAttribute(@"Расчеты, содержащиеся в сметной документации, соответствуют физическим объемам работ, включенным в ведомость объемов работ, акт, утвержденный застройщиком или техническим заказчиком и содержащий перечень дефектов оснований, строительных конструкций, систем инженерно-технического обеспечения и сетей инженерно-технического обеспечения.")]
        РасчетыСодержащиесяВСметнойДокументацииСоответствуютФизическимОбъемамРаботВключеннымВВедомостьОбъемовРаботАктУтвержденныйЗастройщикомИлиТехническимЗаказчикомИСодержащийПереченьДефектовОснованийСтроительныхКонструкцийСистемИнженерноТехническогоОбеспеченияИСетейИнженерноТехническогоОбеспечения,

        /// <summary>
        /// <para>Положительный вывод о соответствии расчетов, содержащихся в сметной документации, физическим объемам работ, включенным в ведомость объемов работ, акт, утвержденный застройщиком или техническим заказчиком и содержащий перечень дефектов оснований, строительных конструкций, систем инженерно-технического обеспечения и сетей инженерно-технического обеспечения с указанием качественных и количественных характеристик таких дефектов, при проведении проверки достоверности определения сметной стоимости капитального ремонта (в случае отсутствия акта, утвержденного застройщиком или техническим заказчиком и содержащий перечень дефектов оснований, строительных конструкций, систем инженерно-технического обеспечения и сетей инженерно-технического обеспечения с указанием качественных и количественных характеристик таких дефектов, при проведении проверки достоверности определения сметной стоимости капитального ремонта).</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(@"Положительный вывод о соответствии расчетов, содержащихся в сметной документации, физическим объемам работ, включенным в ведомость объемов работ, акт, утвержденный застройщиком или техническим заказчиком и содержащий перечень дефектов оснований, строительных конструкций, систем инженерно-технического обеспечения и сетей инженерно-технического обеспечения с указанием качественных и количественных характеристик таких дефектов, при проведении проверки достоверности определения сметной стоимости капитального ремонта (в случае отсутствия акта, утвержденного застройщиком или техническим заказчиком и содержащий перечень дефектов оснований, строительных конструкций, систем инженерно-технического обеспечения и сетей инженерно-технического обеспечения с указанием качественных и количественных характеристик таких дефектов, при проведении проверки достоверности определения сметной стоимости капитального ремонта).")]
        [System.Xml.Serialization.XmlEnumAttribute(("Расчеты, содержащиеся в сметной документации, соответствуют физическим объемам ра" +
            "бот, включенным в ведомость объемов работ."))]
        РасчетыСодержащиесяВСметнойДокументацииСоответствуютФизическимОбъемамРаботВключеннымВВедомостьОбъемовРабот,
    }

    /// <summary>
    /// <para>Вывод об определении достоверности сметной стоимости. (часть общего вывода)</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Вывод об определении достоверности сметной стоимости. (часть общего вывода)")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tEstimateValidationSummary", Namespace = "")]
    public enum TEstimateValidationSummary
    {

        /// <summary>
        /// <para>Положительный вывод об определении достоверности сметной стоимости.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Положительный вывод об определении достоверности сметной стоимости.")]
        [System.Xml.Serialization.XmlEnumAttribute("Сметная стоимость определена достоверно.")]
        СметнаяСтоимостьОпределенаДостоверно,

        /// <summary>
        /// <para>Отрицательный вывод об определении достоверности сметной стоимости.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Отрицательный вывод об определении достоверности сметной стоимости.")]
        [System.Xml.Serialization.XmlEnumAttribute("Сметная стоимость определена недостоверно.")]
        СметнаяСтоимостьОпределенаНедостоверно,
    }

    /// <summary>
    /// <para>Общие выводы (итоговые выводы по результатам проведенной экспертизы)</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Общие выводы (итоговые выводы по результатам проведенной экспертизы)")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tExaminationSummary", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TExaminationSummary
    {

        /// <summary>
        /// <para>Вывод об оценке результатов инженерных изысканий.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вывод об оценке результатов инженерных изысканий.")]
        [System.Xml.Serialization.XmlElementAttribute("EngineeringSurveysResultsSummary")]
        public TEngineeringSurveysResultsSummary EngineeringSurveysResultsSummary { get; set; }

        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the EngineeringSurveysResultsSummary property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EngineeringSurveysResultsSummarySpecified { get; set; }

        /// <summary>
        /// <para>Вывод об оценке достоверности определения сметной стоимости.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вывод об оценке достоверности определения сметной стоимости.")]
        [System.Xml.Serialization.XmlElementAttribute("EstimateSummary")]
        public TEstimateValidationSummary EstimateSummary { get; set; }

        /// <summary>
        /// <para>Вывод об оценке достоверности определения сметной стоимости. (при оценке в соответствии с постановлением Правительства Российской Федерации от 09.08.2021 №1315)</para>
        /// <para>Непустая текстовая строка</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Вывод об оценке достоверности определения сметной стоимости. (при оценке в соотве" +
            "тствии с постановлением Правительства Российской Федерации от 09.08.2021 №1315)"))]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("EstimateSummary1315")]
        public string EstimateSummary1315 { get; set; }

        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the EstimateSummary property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EstimateSummarySpecified { get; set; }

        /// <summary>
        /// <para>Вывод об оценке проектной документации.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вывод об оценке проектной документации.")]
        [System.Xml.Serialization.XmlElementAttribute("ProjectDocumentsSummary")]
        public TProjectDocumentsSummary ProjectDocumentsSummary { get; set; }
    }

    /// <summary>
    /// <para>Вывод о соответствии (несоответсвии) результатов инженерных изысканий техническим регламентам. (часть общего вывода)</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute(("Вывод о соответствии (несоответсвии) результатов инженерных изысканий техническим" +
        " регламентам. (часть общего вывода)"))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tEngineeringSurveysResultsSummary", Namespace = "")]
    public enum TEngineeringSurveysResultsSummary
    {

        /// <summary>
        /// <para>Вывод о соответствии инженерных изысканий требованиям технических регламентов.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вывод о соответствии инженерных изысканий требованиям технических регламентов.")]
        [System.Xml.Serialization.XmlEnumAttribute(("Результаты инженерных изысканий, выполненных для подготовки проектной документаци" +
            "и, соответствуют требованиям технических регламентов."))]
        РезультатыИнженерныхИзысканийВыполненныхДляПодготовкиПроектнойДокументацииСоответствуютТребованиямТехническихРегламентов,

        /// <summary>
        /// <para>Вывод о несоответствии инженерных изысканий требованиям технических регламентов.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вывод о несоответствии инженерных изысканий требованиям технических регламентов.")]
        [System.Xml.Serialization.XmlEnumAttribute(("Результаты инженерных изысканий, выполненных для подготовки проектной документаци" +
            "и, не соответствуют требованиям технических регламентов."))]
        РезультатыИнженерныхИзысканийВыполненныхДляПодготовкиПроектнойДокументацииНеСоответствуютТребованиямТехническихРегламентов,
    }

    /// <summary>
    /// <para>Общие выводы в отношении проектной документации(итоговые выводы по результатам проведенной экспертизы)</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute(("Общие выводы в отношении проектной документации(итоговые выводы по результатам пр" +
        "оведенной экспертизы)"))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tProjectDocumentsSummary", Namespace = "")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TProjectDocumentsSummary
    {

        /// <summary>
        /// <para>Вывод об оценке проектной документации на соответствие заданию на проектирование.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вывод об оценке проектной документации на соответствие заданию на проектирование.")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ProjectDocumentationDesignAssignmentSummary")]
        public TProjectDocumentationDesignAssignmentSummary ProjectDocumentationDesignAssignmentSummary { get; set; }

        /// <summary>
        /// <para>Вывод об оценке проектной документации на соответсвие инженерным изысканиям.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вывод об оценке проектной документации на соответсвие инженерным изысканиям.")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ProjectDocumentationEngineeringSurveysResultsSummary")]
        public TProjectDocumentationEngineeringSurveysResultsSummary ProjectDocumentationEngineeringSurveysResultsSummary { get; set; }

        /// <summary>
        /// <para>Вывод об оценке проектной документации на соответствие требованиям технических регламентов и иных требований.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Вывод об оценке проектной документации на соответствие требованиям технических ре" +
            "гламентов и иных требований."))]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ProjectDocumentationTechnicalRequirementsSummary")]
        public TProjectDocumentationTechnicalRequirementsSummary ProjectDocumentationTechnicalRequirementsSummary { get; set; }
    }

    /// <summary>
    /// <para>Вывод о соответствии (несоответсвии) проектной документации результатам инженерных изысканий. (часть общего вывода)</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute(("Вывод о соответствии (несоответсвии) проектной документации результатам инженерны" +
        "х изысканий. (часть общего вывода)"))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tProjectDocumentationEngineeringSurveysResultsSummary", Namespace = "")]
    public enum TProjectDocumentationEngineeringSurveysResultsSummary
    {

        /// <summary>
        /// <para>Вывод о соответствии проектной документации результатам инженерных изысканий.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вывод о соответствии проектной документации результатам инженерных изысканий.")]
        [System.Xml.Serialization.XmlEnumAttribute("Проектная документация соответствует результатам инженерных изысканий.")]
        ПроектнаяДокументацияСоответствуетРезультатамИнженерныхИзысканий,

        /// <summary>
        /// <para>Вывод о несоответствии проектной документации результатам инженерных изысканий.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вывод о несоответствии проектной документации результатам инженерных изысканий.")]
        [System.Xml.Serialization.XmlEnumAttribute("Проектная документация не соответствует результатам инженерных изысканий.")]
        ПроектнаяДокументацияНеСоответствуетРезультатамИнженерныхИзысканий,
    }

    /// <summary>
    /// <para>Вывод о соответствии (несоответствии) проектной документации заданию на проектирование. (часть общего вывода)</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute(("Вывод о соответствии (несоответствии) проектной документации заданию на проектиро" +
        "вание. (часть общего вывода)"))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tProjectDocumentationDesignAssignmentSummary", Namespace = "")]
    public enum TProjectDocumentationDesignAssignmentSummary
    {

        /// <summary>
        /// <para>Вывод о соответствии проектной документации заданию на проектирование.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вывод о соответствии проектной документации заданию на проектирование.")]
        [System.Xml.Serialization.XmlEnumAttribute("Проектная документация соответствует заданию на проектирование.")]
        ПроектнаяДокументацияСоответствуетЗаданиюНаПроектирование,

        /// <summary>
        /// <para>Вывод о несоответствии проектной документации заданию на проектирование.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Вывод о несоответствии проектной документации заданию на проектирование.")]
        [System.Xml.Serialization.XmlEnumAttribute("Проектная документация не соответствует заданию на проектирование.")]
        ПроектнаяДокументацияНеСоответствуетЗаданиюНаПроектирование,
    }

    /// <summary>
    /// <para>Вывод о соответствии (несоответствии) технической части проектной документации требованиям технических регламентов и иным установленным требованиям. (часть общего вывода)</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute(("Вывод о соответствии (несоответствии) технической части проектной документации тр" +
        "ебованиям технических регламентов и иным установленным требованиям. (часть общег" +
        "о вывода)"))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("tProjectDocumentationTechnicalRequirementsSummary", Namespace = "")]
    public enum TProjectDocumentationTechnicalRequirementsSummary
    {

        /// <summary>
        /// <para>Вывод о соответствии проектной документации требованиям технических регламентов и иным установленным требованиям.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Вывод о соответствии проектной документации требованиям технических регламентов и" +
            " иным установленным требованиям."))]
        [System.Xml.Serialization.XmlEnumAttribute(("Проектная документация соответствует требованиям технических регламентов и иным у" +
            "становленным требованиям."))]
        ПроектнаяДокументацияСоответствуетТребованиямТехническихРегламентовИИнымУстановленнымТребованиям,

        /// <summary>
        /// <para>Вывод о несоответствии проектной документации требованиям технических регламентов и иным установленным требованиям.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Вывод о несоответствии проектной документации требованиям технических регламентов" +
            " и иным установленным требованиям."))]
        [System.Xml.Serialization.XmlEnumAttribute(("Проектная документация не соответствует требованиям технических регламентов и ины" +
            "м установленным требованиям."))]
        ПроектнаяДокументацияНеСоответствуетТребованиямТехническихРегламентовИИнымУстановленнымТребованиям,
    }

    /// <summary>
    /// <para>Заключение экспертизы</para>
    /// </summary>
    [System.ComponentModel.DescriptionAttribute("Заключение экспертизы")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "3.0.1185.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("Conclusion", Namespace = "", AnonymousType = true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Conclusion", Namespace = "")]
    public partial class Conclusion
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<string> _cadastralNumber;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TDesigner> _designer;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TDocument> _documents;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TeepdUse> _eepdUse;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TEngineeringSurveyAddress> _engineeringSurveyAddress;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TDeclarant> _engineeringSurveyDeveloper;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TTechnicalCustomer> _engineeringSurveyTechnicalCustomer;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TExpertEngineeringSurveys> _expertEngineeringSurveys;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TExpertProjectDocuments> _expertProjectDocuments;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TExpert> _experts;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TFinance> _finance;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TPreviousConclusion> _previousConclusions;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TPreviousSimpleConclusion> _previousSimpleConclusions;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TDeclarant> _projectDocumentsDeveloper;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TTechnicalCustomer> _projectDocumentsTechnicalCustomer;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _schemaVersion = "01.03";

        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="Conclusion" /> class.</para>
        /// </summary>
        public Conclusion()
        {
            this._documents = new System.Collections.ObjectModel.Collection<TDocument>();
            this._previousConclusions = new System.Collections.ObjectModel.Collection<TPreviousConclusion>();
            this._previousSimpleConclusions = new System.Collections.ObjectModel.Collection<TPreviousSimpleConclusion>();
            this._cadastralNumber = new System.Collections.ObjectModel.Collection<string>();
            this._projectDocumentsDeveloper = new System.Collections.ObjectModel.Collection<TDeclarant>();
            this._projectDocumentsTechnicalCustomer = new System.Collections.ObjectModel.Collection<TTechnicalCustomer>();
            this._finance = new System.Collections.ObjectModel.Collection<TFinance>();
            this._designer = new System.Collections.ObjectModel.Collection<TDesigner>();
            this._eepdUse = new System.Collections.ObjectModel.Collection<TeepdUse>();
            this._engineeringSurveyAddress = new System.Collections.ObjectModel.Collection<TEngineeringSurveyAddress>();
            this._engineeringSurveyDeveloper = new System.Collections.ObjectModel.Collection<TDeclarant>();
            this._engineeringSurveyTechnicalCustomer = new System.Collections.ObjectModel.Collection<TTechnicalCustomer>();
            this._expertEngineeringSurveys = new System.Collections.ObjectModel.Collection<TExpertEngineeringSurveys>();
            this._expertProjectDocuments = new System.Collections.ObjectModel.Collection<TExpertProjectDocuments>();
            this._experts = new System.Collections.ObjectModel.Collection<TExpert>();
        }

        /// <summary>
        /// <para>Сведения о лице, утвердившем заключение</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сведения о лице, утвердившем заключение")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Approver")]
        public TWorkPerson Approver { get; set; }

        /// <summary>
        /// <para>Кадастровый номер земельного участка, на котором размещается объект капитального строительства</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Кадастровый номер земельного участка, на котором размещается объект капитального " +
            "строительства"))]
        [System.Xml.Serialization.XmlElementAttribute("CadastralNumber")]
        public System.Collections.ObjectModel.Collection<string> CadastralNumber
        {
            get
            {
                return _cadastralNumber;
            }
            private set
            {
                _cadastralNumber = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the CadastralNumber collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CadastralNumberSpecified
        {
            get
            {
                return (this.CadastralNumber.Count != 0);
            }
        }

        /// <summary>
        /// <para>Сведения о природных и техногенных условиях территории, на которой планируется осуществлять строительство, реконструкцию, капитальный ремонт объекта капитального строительства</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сведения о природных и техногенных условиях территории, на которой планируется ос" +
            "уществлять строительство, реконструкцию, капитальный ремонт объекта капитального" +
            " строительства"))]
        [System.Xml.Serialization.XmlElementAttribute("ClimateConditions")]
        public TClimateConditions ClimateConditions { get; set; }

        /// <summary>
        /// <para>Дополнительные сведения о природных и техногенных условиях территории, на которой планируется осуществлять строительство, реконструкцию, капитальный ремонт объекта капитального строительства</para>
        /// <para>Непустая текстовая строка</para>
        /// <para xml:lang="en">Pattern: (\s*[^\s]\s*)+.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Дополнительные сведения о природных и техногенных условиях территории, на которой" +
            " планируется осуществлять строительство, реконструкцию, капитальный ремонт объек" +
            "та капитального строительства"))]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(\\s*[^\\s]\\s*)+")]
        [System.Xml.Serialization.XmlElementAttribute("ClimateConditionsNote")]
        public string ClimateConditionsNote { get; set; }

        /// <summary>
        /// <para>Уникальный идентификатор формирования заключения экспертизы для идентификации</para>
        /// <para>Уникальный идентификатор формирования заключения</para>
        /// <para xml:lang="en">Pattern: [0-9A-Fa-f]{8}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{12}.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Уникальный идентификатор формирования заключения экспертизы для идентификации")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("[0-9A-Fa-f]{8}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{12}")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlAttributeAttribute("ConclusionGUID")]
        public string ConclusionGuid { get; set; }

        /// <summary>
        /// <para>Сведения о заявителе</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сведения о заявителе")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Declarant")]
        public TDeclarant Declarant { get; set; }

        /// <summary>
        /// <para>Сведения о лице, принимавшем участие в подготовке проектной документации (изменений в проектную документацию)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сведения о лице, принимавшем участие в подготовке проектной документации (изменен" +
            "ий в проектную документацию)"))]
        [System.Xml.Serialization.XmlElementAttribute("Designer")]
        public System.Collections.ObjectModel.Collection<TDesigner> Designer
        {
            get
            {
                return _designer;
            }
            private set
            {
                _designer = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the Designer collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DesignerSpecified
        {
            get
            {
                return (this.Designer.Count != 0);
            }
        }

        /// <summary>
        /// <para>Документы, рассмотренные в рамках экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Документы, рассмотренные в рамках экспертизы")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlArrayAttribute("Documents")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Document")]
        public System.Collections.ObjectModel.Collection<TDocument> Documents
        {
            get
            {
                return _documents;
            }
            private set
            {
                _documents = value;
            }
        }

        /// <summary>
        /// <para>Сведения о необходимости проведения экологической экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сведения о необходимости проведения экологической экспертизы")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("EcologyExpertise")]
        public TEcologyExpertise EcologyExpertise { get; set; }

        /// <summary>
        /// <para>Сведения об использовании типовой (ранее - экономически эффективной) проектной документации повторного использования</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сведения об использовании типовой (ранее - экономически эффективной) проектной до" +
            "кументации повторного использования"))]
        [System.Xml.Serialization.XmlElementAttribute("EEPDUse")]
        public System.Collections.ObjectModel.Collection<TeepdUse> EepdUse
        {
            get
            {
                return _eepdUse;
            }
            private set
            {
                _eepdUse = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the EepdUse collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EepdUseSpecified
        {
            get
            {
                return (this.EepdUse.Count != 0);
            }
        }

        /// <summary>
        /// <para>Местоположение района (площадки, трассы) проведения инженерных изысканий</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Местоположение района (площадки, трассы) проведения инженерных изысканий")]
        [System.Xml.Serialization.XmlElementAttribute("EngineeringSurveyAddress")]
        public System.Collections.ObjectModel.Collection<TEngineeringSurveyAddress> EngineeringSurveyAddress
        {
            get
            {
                return _engineeringSurveyAddress;
            }
            private set
            {
                _engineeringSurveyAddress = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the EngineeringSurveyAddress collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EngineeringSurveyAddressSpecified
        {
            get
            {
                return (this.EngineeringSurveyAddress.Count != 0);
            }
        }

        /// <summary>
        /// <para>Застройщик, обеспечивший проведение инженерных изысканий</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Застройщик, обеспечивший проведение инженерных изысканий")]
        [System.Xml.Serialization.XmlElementAttribute("EngineeringSurveyDeveloper")]
        public System.Collections.ObjectModel.Collection<TDeclarant> EngineeringSurveyDeveloper
        {
            get
            {
                return _engineeringSurveyDeveloper;
            }
            private set
            {
                _engineeringSurveyDeveloper = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the EngineeringSurveyDeveloper collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EngineeringSurveyDeveloperSpecified
        {
            get
            {
                return (this.EngineeringSurveyDeveloper.Count != 0);
            }
        }

        /// <summary>
        /// <para>Технический заказчик, обеспечивший проведение инженерных изысканий</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Технический заказчик, обеспечивший проведение инженерных изысканий")]
        [System.Xml.Serialization.XmlElementAttribute("EngineeringSurveyTechnicalCustomer")]
        public System.Collections.ObjectModel.Collection<TTechnicalCustomer> EngineeringSurveyTechnicalCustomer
        {
            get
            {
                return _engineeringSurveyTechnicalCustomer;
            }
            private set
            {
                _engineeringSurveyTechnicalCustomer = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the EngineeringSurveyTechnicalCustomer collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EngineeringSurveyTechnicalCustomerSpecified
        {
            get
            {
                return (this.EngineeringSurveyTechnicalCustomer.Count != 0);
            }
        }

        /// <summary>
        /// <para>Сведения о сметной стоимости</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сведения о сметной стоимости")]
        [System.Xml.Serialization.XmlElementAttribute("EstimatedCost")]
        public TEstimatedCost EstimatedCost { get; set; }

        /// <summary>
        /// <para>Сведения об объекте экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сведения об объекте экспертизы")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ExaminationObject")]
        public TExaminationObject ExaminationObject { get; set; }

        /// <summary>
        /// <para>Сведения в части экспертизы результатов инженерных изысканий, подготовленные экспертом</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сведения в части экспертизы результатов инженерных изысканий, подготовленные эксп" +
            "ертом"))]
        [System.Xml.Serialization.XmlElementAttribute("ExpertEngineeringSurveys")]
        public System.Collections.ObjectModel.Collection<TExpertEngineeringSurveys> ExpertEngineeringSurveys
        {
            get
            {
                return _expertEngineeringSurveys;
            }
            private set
            {
                _expertEngineeringSurveys = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the ExpertEngineeringSurveys collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExpertEngineeringSurveysSpecified
        {
            get
            {
                return (this.ExpertEngineeringSurveys.Count != 0);
            }
        }

        /// <summary>
        /// <para>Сведения в части проверки достоверности определения сметной стоимости, подготовленные экспертом</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сведения в части проверки достоверности определения сметной стоимости, подготовле" +
            "нные экспертом"))]
        [System.Xml.Serialization.XmlElementAttribute("ExpertEstimate")]
        public TExpertEstimate ExpertEstimate { get; set; }

        /// <summary>
        /// <para>Сведения об организации по проведению экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сведения об организации по проведению экспертизы")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("ExpertOrganization")]
        public TOrganization ExpertOrganization { get; set; }

        /// <summary>
        /// <para>Сведения в части эспертизы проектной документации, подготовленные экспертом</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сведения в части эспертизы проектной документации, подготовленные экспертом")]
        [System.Xml.Serialization.XmlElementAttribute("ExpertProjectDocuments")]
        public System.Collections.ObjectModel.Collection<TExpertProjectDocuments> ExpertProjectDocuments
        {
            get
            {
                return _expertProjectDocuments;
            }
            private set
            {
                _expertProjectDocuments = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the ExpertProjectDocuments collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExpertProjectDocumentsSpecified
        {
            get
            {
                return (this.ExpertProjectDocuments.Count != 0);
            }
        }

        /// <summary>
        /// <para>Перечень лиц, аттестованных на право подготовки заключений экспертизы, подписавших заключение экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Перечень лиц, аттестованных на право подготовки заключений экспертизы, подписавши" +
            "х заключение экспертизы"))]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlArrayAttribute("Experts")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Expert")]
        public System.Collections.ObjectModel.Collection<TExpert> Experts
        {
            get
            {
                return _experts;
            }
            private set
            {
                _experts = value;
            }
        }

        /// <summary>
        /// <para>Сведения об источнике финансирования</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сведения об источнике финансирования")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Finance")]
        public System.Collections.ObjectModel.Collection<TFinance> Finance
        {
            get
            {
                return _finance;
            }
            private set
            {
                _finance = value;
            }
        }

        /// <summary>
        /// <para>Дополнительные сведения об источнике финансирования</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Дополнительные сведения об источнике финансирования")]
        [System.Xml.Serialization.XmlElementAttribute("FinanceComment")]
        public string FinanceComment { get; set; }

        /// <summary>
        /// <para>Сведения об объекте капитального строительства</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Сведения об объекте капитального строительства")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Object")]
        public TObject Object { get; set; }

        /// <summary>
        /// <para>Сведения о ранее выданных заключениях экспертизы в отношении объекта капитального строительства, проектная документация и (или) результаты инженерных изысканий по которому представлены для проведения (повторной) экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Сведения о ранее выданных заключениях экспертизы в отношении объекта капитального" +
            " строительства, проектная документация и (или) результаты инженерных изысканий п" +
            "о которому представлены для проведения (повторной) экспертизы"))]
        [System.Xml.Serialization.XmlArrayAttribute("PreviousConclusions")]
        [System.Xml.Serialization.XmlArrayItemAttribute("PreviousConclusion")]
        public System.Collections.ObjectModel.Collection<TPreviousConclusion> PreviousConclusions
        {
            get
            {
                return _previousConclusions;
            }
            private set
            {
                _previousConclusions = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the PreviousConclusions collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PreviousConclusionsSpecified
        {
            get
            {
                return (this.PreviousConclusions.Count != 0);
            }
        }

        /// <summary>
        /// <para>Сведения о ранее выданных заключениях по результатам оценки соответствия в рамках экспертного сопровождения в отношении объекта капитального строительства, проектная документация по которому представлена для проведения оценки соответствия в рамках экспертного сопровождения</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(@"Сведения о ранее выданных заключениях по результатам оценки соответствия в рамках экспертного сопровождения в отношении объекта капитального строительства, проектная документация по которому представлена для проведения оценки соответствия в рамках экспертного сопровождения")]
        [System.Xml.Serialization.XmlArrayAttribute("PreviousSimpleConclusions")]
        [System.Xml.Serialization.XmlArrayItemAttribute("PreviousSimpleConclusion")]
        public System.Collections.ObjectModel.Collection<TPreviousSimpleConclusion> PreviousSimpleConclusions
        {
            get
            {
                return _previousSimpleConclusions;
            }
            private set
            {
                _previousSimpleConclusions = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the PreviousSimpleConclusions collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PreviousSimpleConclusionsSpecified
        {
            get
            {
                return (this.PreviousSimpleConclusions.Count != 0);
            }
        }

        /// <summary>
        /// <para>Застройщик, обеспечивший подготовку проектной документации (внесение изменений в проектную документацию)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Застройщик, обеспечивший подготовку проектной документации (внесение изменений в " +
            "проектную документацию)"))]
        [System.Xml.Serialization.XmlElementAttribute("ProjectDocumentsDeveloper")]
        public System.Collections.ObjectModel.Collection<TDeclarant> ProjectDocumentsDeveloper
        {
            get
            {
                return _projectDocumentsDeveloper;
            }
            private set
            {
                _projectDocumentsDeveloper = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the ProjectDocumentsDeveloper collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ProjectDocumentsDeveloperSpecified
        {
            get
            {
                return (this.ProjectDocumentsDeveloper.Count != 0);
            }
        }

        /// <summary>
        /// <para>Технический заказчик, обеспечивший подготовку проектной документации (внесение изменений в проектную документацию)</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute(("Технический заказчик, обеспечивший подготовку проектной документации (внесение из" +
            "менений в проектную документацию)"))]
        [System.Xml.Serialization.XmlElementAttribute("ProjectDocumentsTechnicalCustomer")]
        public System.Collections.ObjectModel.Collection<TTechnicalCustomer> ProjectDocumentsTechnicalCustomer
        {
            get
            {
                return _projectDocumentsTechnicalCustomer;
            }
            private set
            {
                _projectDocumentsTechnicalCustomer = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the ProjectDocumentsTechnicalCustomer collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ProjectDocumentsTechnicalCustomerSpecified
        {
            get
            {
                return (this.ProjectDocumentsTechnicalCustomer.Count != 0);
            }
        }

        /// <summary>
        /// <para>Ссылка на xml-схему на официальном сайте, где опубликована схема</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Ссылка на xml-схему на официальном сайте, где опубликована схема")]
        [System.Xml.Serialization.XmlAttributeAttribute("SchemaLink")]
        public string SchemaLink { get; set; }

        /// <summary>
        /// <para>Номер версии схемы, используемой при создании заключения</para>
        /// <para>Номер версии, использованной при формировании заключения в формате xx.xx</para>
        /// <para xml:lang="en">Pattern: ([0-9]){2}\.([0-9]){2}.</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Номер версии схемы, используемой при создании заключения")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("([0-9]){2}\\.([0-9]){2}")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlAttributeAttribute("SchemaVersion")]
        public string SchemaVersion
        {
            get
            {
                return _schemaVersion;
            }
            set
            {
                _schemaVersion = value;
            }
        }

        /// <summary>
        /// <para>Выводы по результатам проведения экспертизы</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("Выводы по результатам проведения экспертизы")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        [System.Xml.Serialization.XmlElementAttribute("Summary")]
        public TSummary Summary { get; set; }
    }
}

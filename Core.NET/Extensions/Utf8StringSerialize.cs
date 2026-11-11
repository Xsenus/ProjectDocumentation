using Core.NET.AutoModels.ConclusionObj;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Core.NET.Extensions
{
    public sealed class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }

    public static class Utf8StringSerialize
    {
        /// <summary>
        /// Строгая сериализация в UTF-8.
        /// </summary>
        public static string SerializeObject<T>(this T obj)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (var textWriter = new Utf8StringWriter())
                {
                    serializer.Serialize(textWriter, obj);
                    return textWriter.ToString();
                }
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Десериализация со «смягчением»: при ошибке enum-значений
        /// XML нормализуется и попытка повторяется.
        /// В <paramref name="warnings"/> (если передан) попадают описания удалённых при нормализации элементов.
        /// </summary>
        public static T DeSerializeObjectByString<T>(string xml, bool lenient = true, ICollection<string> warnings = null)
        {
            try
            {
                return DeserializeStrict<T>(xml);
            }
            catch
            {
                if (!lenient) return default;
                var cleaned = NormalizeProblematicEnums(xml, warnings);
                try
                {
                    return DeserializeStrict<T>(cleaned);
                }
                catch
                {
                    return default;
                }
            }
        }

        /// <summary>
        /// Десериализация из файла, с тем же поведением.
        /// В <paramref name="warnings"/> (если передан) попадают описания удалённых при нормализации элементов.
        /// </summary>
        public static T DeSerializeObject<T>(string path, bool lenient = true, ICollection<string> warnings = null)
        {
            try
            {
                using (var fs = File.OpenRead(path))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(fs);
                }
            }
            catch
            {
                if (!lenient) return default;
                try
                {
                    // Кодировку определяет XML-декларация файла (например, windows-1251),
                    // а не жёстко заданный UTF-8.
                    var xml = XDocument.Load(path, LoadOptions.PreserveWhitespace).ToString(SaveOptions.DisableFormatting);
                    var cleaned = NormalizeProblematicEnums(xml, warnings);
                    using (var sr = new StringReader(cleaned)) 
                    {
                        var serializer = new XmlSerializer(typeof(T));
                        return (T)serializer.Deserialize(sr);
                    }
                }
                catch
                {
                    return default;
                }
            }
        }

        // === ВНУТРЕННЕЕ ===

        private static T DeserializeStrict<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Нормализует значения известных enum-элементов:
        /// 1) заменяет «сокращённые»/неформальные тексты на валидные XmlEnum-имена;
        /// 2) удаляет элементы с неподдерживаемыми значениями (чтобы сериализатор не падал).
        /// </summary>
        private static string NormalizeProblematicEnums(string xml, ICollection<string> warnings = null)
        {
            var doc = XDocument.Parse(xml, LoadOptions.PreserveWhitespace);
            var ns = doc.Root?.Name.Namespace ?? XNamespace.None;

            // 1) Карта "элемент → тип enum" (дополняйте при необходимости).
            var enumByElement = new Dictionary<XName, Type>
            {
                [ns + "EngineeringSurveySummary"] = typeof(TEngineeringSurveysSummary),
                [ns + "EngineeringSurveysResultsSummary"] = typeof(TEngineeringSurveysResultsSummary),
                [ns + "ProjectDocumentationEngineeringSurveysResultsSummary"] = typeof(TProjectDocumentationEngineeringSurveysResultsSummary),
                [ns + "ProjectDocumentationDesignAssignmentSummary"] = typeof(TProjectDocumentationDesignAssignmentSummary),
                [ns + "ProjectDocumentationTechnicalRequirementsSummary"] = typeof(TProjectDocumentationTechnicalRequirementsSummary),
                [ns + "EstimateSummary"] = typeof(TEstimateValidationSummary),
                [ns + "EstimateNormsAndWorksSummary"] = typeof(TEstimateNormsAndWorksSummary),
                [ns + "ProjectDocumentsSummary"] = typeof(TProjectDocumentationSummary),
                // при необходимости добавьте DocType и др.
            };

            // 2) Замены «сленга» на валидные XmlEnum-имена (регистронезависимо).
            // Пример для вашего кейса: "Соответствуют." → длинное XmlEnum-имя.
            var synonyms = new Dictionary<(XName element, string input), string>(new ElemValueComparer())
            {
                //// EngineeringSurveySummary
                //{ (ns + "EngineeringSurveySummary", "Соответствуют"),
                //  "РезультатыИнженерныхИзысканийСоответствуютТребованиямТехническихРегламентов" },
                //{ (ns + "EngineeringSurveySummary", "Соответствуют."),
                //  "РезультатыИнженерныхИзысканийСоответствуютТребованиямТехническихРегламентов" },
                //{ (ns + "EngineeringSurveySummary", "Не соответствуют"),
                //  "РезультатыИнженерныхИзысканийНеСоответствуютТребованиямТехническихРегламентов" },
                //{ (ns + "EngineeringSurveySummary", "Не соответствуют."),
                //  "РезультатыИнженерныхИзысканийНеСоответствуютТребованиямТехническихРегламентов" },
                // Аналогично можно добавить мэппинг для остальных элементов при необходимости.
            };

            // 3) Собираем допустимые наборы значений для каждого enum.
            var allowedCache = new Dictionary<Type, HashSet<string>>();
            HashSet<string> GetAllowed(Type tEnum)
            {
                if (!allowedCache.TryGetValue(tEnum, out var set))
                {
                    set = new HashSet<string>(GetXmlEnumNames(tEnum), StringComparer.Ordinal);
                    allowedCache[tEnum] = set;
                }
                return set;
            }

            // 4) Проходим по каждому известному элементу.
            foreach (var kv in enumByElement)
            {
                var element = kv.Key;
                var enumType = kv.Value;
                var allowed = GetAllowed(enumType);

                foreach (var node in doc.Descendants(element).ToList())
                {
                    var original = (node.Value ?? string.Empty).Trim();

                    // 4.1 Применяем словарь синонимов (если есть).
                    if (synonyms.TryGetValue((element, original), out var mapped))
                    {
                        node.Value = mapped;
                        continue;
                    }

                    // 4.2 Если значение пустое — удаляем элемент.
                    if (string.IsNullOrWhiteSpace(original))
                    {
                        warnings?.Add($"{element.LocalName}: пустое значение — элемент удалён.");
                        node.Remove();
                        continue;
                    }

                    // 4.3 Если значение не из XmlEnum-списка — удаляем элемент.
                    if (!allowed.Contains(original))
                    {
                        warnings?.Add($"{element.LocalName}: значение «{original}» отсутствует в схеме — элемент удалён.");
                        node.Remove();
                    }
                }
            }

            return doc.ToString(SaveOptions.DisableFormatting);
        }

        private static IEnumerable<string> GetXmlEnumNames(Type enumType)
        {
            // Возвращает список имён из [XmlEnum(Name = "...")] или имя константы.
            foreach (var f in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var xml = f.GetCustomAttribute<XmlEnumAttribute>();
                yield return xml?.Name ?? f.Name;
            }
        }

        private sealed class ElemValueComparer : IEqualityComparer<(XName element, string input)>
        {
            public bool Equals((XName element, string input) x, (XName element, string input) y) =>
                XName.Equals(x.element, y.element) &&
                string.Equals(x.input?.Trim().TrimEnd('.'), y.input?.Trim().TrimEnd('.'), StringComparison.OrdinalIgnoreCase);

            public int GetHashCode((XName element, string input) obj)
            {
                var e = obj.element?.GetHashCode() ?? 0;
                var v = (obj.input ?? string.Empty).Trim().TrimEnd('.').ToUpperInvariant().GetHashCode();
                return (e * 397) ^ v;
            }
        }
    }
}

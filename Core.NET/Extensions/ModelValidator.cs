using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Core.NET.Extensions
{
    /// <summary>
    /// Проверка документа по ограничениям XSD, перенесённым генератором в атрибуты модели
    /// (Required, MaxLength, MinLength, RegularExpression). Это не полная проверка по XSD
    /// (порядок элементов, choice, xs:pattern без атрибута в модели не проверяются),
    /// но ловит незаполненные обязательные сведения и неверный формат до выгрузки.
    /// </summary>
    public static class ModelValidator
    {
        private const string ModelNamespacePrefix = "Core.NET.AutoModels";

        /// <summary>Замечания в виде «Раздел → Поле: что не так».</summary>
        public static List<string> Validate(object document)
        {
            var errors = new List<string>();
            if (document != null)
            {
                ValidateObject(document, GetDescription(document.GetType()) ?? document.GetType().Name, errors, new HashSet<object>(ReferenceEqualityComparer.Instance));
            }

            return errors;
        }

        private static void ValidateObject(object obj, string path, List<string> errors, HashSet<object> visited)
        {
            if (!visited.Add(obj))
            {
                return;
            }

            foreach (var property in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.GetIndexParameters().Length > 0 || !property.CanRead || property.IsDefined(typeof(XmlIgnoreAttribute), true))
                {
                    continue;
                }

                var value = property.GetValue(obj, null);
                var propertyPath = $"{path} → {GetDescription(property) ?? property.Name}";

                foreach (var attribute in property.GetCustomAttributes<ValidationAttribute>(true))
                {
                    if (!attribute.IsValid(value))
                    {
                        errors.Add($"{propertyPath}: {Describe(attribute, value)}");
                    }
                }

                ValidateChildren(value, propertyPath, errors, visited);
            }
        }

        private static void ValidateChildren(object value, string path, List<string> errors, HashSet<object> visited)
        {
            if (value is null || value is string || value.GetType().IsValueType)
            {
                return;
            }

            if (value is IEnumerable collection)
            {
                var index = 0;
                foreach (var item in collection)
                {
                    index++;
                    ValidateChildren(item, $"{path} [{index}]", errors, visited);
                }

                return;
            }

            if (value.GetType().Namespace?.StartsWith(ModelNamespacePrefix, StringComparison.Ordinal) == true)
            {
                ValidateObject(value, path, errors, visited);
            }
        }

        private static string Describe(ValidationAttribute attribute, object value)
        {
            switch (attribute)
            {
                case RequiredAttribute _:
                    return "не заполнено";
                case MaxLengthAttribute maxLength:
                    return $"длина {GetLength(value)} больше допустимой {maxLength.Length}";
                case MinLengthAttribute minLength:
                    return $"длина {GetLength(value)} меньше допустимой {minLength.Length}";
                case RegularExpressionAttribute regex:
                    return $"значение «{Shorten(value?.ToString())}» не соответствует формату";
                default:
                    return attribute.FormatErrorMessage(string.Empty).Trim();
            }
        }

        private static int GetLength(object value)
        {
            switch (value)
            {
                case string text:
                    return text.Length;
                case ICollection collection:
                    return collection.Count;
                default:
                    return 0;
            }
        }

        private static string Shorten(string text)
        {
            if (text is null)
            {
                return string.Empty;
            }

            return text.Length > 40 ? text.Substring(0, 40) + "…" : text;
        }

        private static string GetDescription(MemberInfo member)
        {
            var description = member.GetCustomAttribute<DescriptionAttribute>(true)?.Description;
            return string.IsNullOrWhiteSpace(description) ? null : description;
        }

        private sealed class ReferenceEqualityComparer : IEqualityComparer<object>
        {
            public static readonly ReferenceEqualityComparer Instance = new ReferenceEqualityComparer();

            public new bool Equals(object x, object y) => ReferenceEquals(x, y);

            public int GetHashCode(object obj) => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
        }
    }
}

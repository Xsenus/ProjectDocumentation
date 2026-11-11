using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace Core.NET.Extensions
{
    public static class EnumExtension
    {
        public static int? GetEnumXmlAttributeValueInt(this Enum genericEnum)
        {
            if (int.TryParse(genericEnum.GetEnumXmlAttributeValue()?.ToString(), out int result))
            {
                return result;
            }

            return default;
        }

        public static object GetEnumXmlAttributeValue(this Enum genericEnum)
        {
            if (genericEnum is null)
            {
                return default;
            }

            var genericEnumType = genericEnum.GetType();
            var memberInfo = genericEnumType.GetMember(genericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var attribute = memberInfo[0].GetCustomAttributes(typeof(XmlEnumAttribute), false);
                if ((attribute != null && attribute.Count() > 0))
                {
                    return ((XmlEnumAttribute)attribute.ElementAt(0)).Name;
                }
            }
            return genericEnum.ToString();
        }

        public static string GetEnumDescription(this Enum genericEnum)
        {
            if (genericEnum is null)
            {
                return default;
            }

            var genericEnumType = genericEnum.GetType();
            var memberInfo = genericEnumType.GetMember(genericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var attribute = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if ((attribute != null && attribute.Count() > 0))
                {
                    return ((DescriptionAttribute)attribute.ElementAt(0)).Description;
                }
            }
            return genericEnum.ToString();
        }

        /// <summary>
        /// Ищет значение перечисления <typeparamref name="T"/> по имени из <see cref="XmlEnumAttribute"/>
        /// (или по имени константы, если атрибута нет).
        /// </summary>
        /// <returns><c>true</c>, если значение найдено; иначе <c>false</c> и <paramref name="result"/> = <c>default</c>.</returns>
        public static bool TryGetEnumByXmlAttributeValue<T>(string value, out T result)
        {
            result = default;

            if (string.IsNullOrWhiteSpace(value) || !typeof(T).IsEnum)
            {
                return false;
            }

            foreach (var item in Enum.GetValues(typeof(T)))
            {
                var itemValue = ((Enum)item).GetEnumXmlAttributeValue()?.ToString();
                if (string.Equals(itemValue, value, StringComparison.Ordinal))
                {
                    result = (T)item;
                    return true;
                }
            }

            return false;
        }

        public static T GetDefaultEnum<T>(string text) where T : Enum
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                foreach (T item in Enum.GetValues(typeof(T)))
                {
                    if (item.GetEnumDescription().Equals(text))
                    {
                        return item;
                    }
                }
            }

            return default;
        }
    }
}

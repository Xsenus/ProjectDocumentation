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

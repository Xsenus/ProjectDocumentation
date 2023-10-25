using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.NET.Extensions
{
    public static class ObjectExtension
    {
        private static Dictionary<string, string> GetObjectDescription(this object obj)
        {
            var result = new Dictionary<string, string>();
            var objItem = obj.GetType().GetProperties()?.FirstOrDefault(f => !string.IsNullOrWhiteSpace(f.Name) && f.Name.ToLower().Contains("item"));

            if (objItem is null)
            {
                var properties = obj.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var name = property.Name;
                    var value = property.GetValue(obj, null);

                    if (!string.IsNullOrWhiteSpace(name) && !result.ContainsKey(name))
                    {
                        result.Add(name, value?.ToString());
                    }
                }
            }

            return result;
        }

        public static string GetCounterPartyDescription(this object obj)
        {
            var result = default(string);

            var dictionary = obj.GetObjectDescription();

            foreach (var property in dictionary)
            {
                if (dictionary.ContainsKey("OrgFullName"))
                {
                    result += GetValue(property, "OrgFullName", "Наименование");
                    result += GetValue(property, "OrgOGRN", "ОГРН");
                    result += GetValue(property, "OrgINN", "ИНН");
                    result += GetValue(property, "OrgKPP", "КПП");
                    result += GetValue(property, "Address", "Место нахождения и адрес");
                }
                else if (dictionary.ContainsKey("OGRNIP"))
                {
                    result += GetValue(property, "FamilyName", "Фамилия");
                    result += GetValue(property, "FirstName", "Имя");
                    result += GetValue(property, "SecondName", "Отчество");
                    result += GetValue(property, "OGRNIP", "ОГРНИП");
                    result += GetValue(property, "PostAddress", "Адрес");
                }
                
                result += GetValue(property, "Email", "E-mail");
            }

            return result;
        }

        private static string GetValue(KeyValuePair<string, string> property, string propertyName, string description, bool isNewLine = true)
        {
            if (property.Key.Equals(propertyName) && !string.IsNullOrWhiteSpace(property.Value))
            {
                var result = $"{description}: {property.Value}";
                if (isNewLine)
                {
                    result += Environment.NewLine;
                }
                return result;
            }

            return default;
        }
    }
}

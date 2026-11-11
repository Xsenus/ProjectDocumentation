using System;
using System.Collections.Generic;
using System.Text;

namespace Core.NET.Extensions
{
    public static class StringExtension
    {
        public static string SplitLenght(this string str, int n)
        {
            return string.Join(Environment.NewLine, str.Split(n));
        }

        private static IEnumerable<string> Split(this string str, int n)
        {
            if (string.IsNullOrWhiteSpace(str) || n < 1)
            {
                throw new ArgumentException();
            }

            for (int i = 0; i < str.Length; i += n)
            {
                yield return str.Substring(i, Math.Min(n, str.Length - i));
            }
        }

        /// <summary>
        /// Код субъекта РФ (две первые цифры) из кода КЛАДР, например "5400000000000" → "54", "0100000000000" → "01".
        /// </summary>
        /// <returns>Двузначный код или <c>null</c>, если код КЛАДР пустой или короче двух символов.</returns>
        public static string GetKladrRegionCode(this string kladrId)
        {
            var value = kladrId?.Trim();
            if (string.IsNullOrEmpty(value) || value.Length < 2)
            {
                return default;
            }

            return value.Substring(0, 2);
        }

        /// <summary>
        /// Вставляет инструкцию обработки (например, &lt;?xml-stylesheet ...?&gt;) в пролог документа —
        /// сразу после XML-декларации или в начало, если декларации нет.
        /// После корневого элемента браузеры такую инструкцию игнорируют.
        /// </summary>
        public static string InsertIntoXmlProlog(this string xml, string processingInstruction)
        {
            if (string.IsNullOrEmpty(xml) || string.IsNullOrEmpty(processingInstruction))
            {
                return xml;
            }

            if (xml.StartsWith("<?xml ", StringComparison.Ordinal))
            {
                var declarationEnd = xml.IndexOf("?>", StringComparison.Ordinal);
                if (declarationEnd >= 0)
                {
                    return xml.Insert(declarationEnd + 2, Environment.NewLine + processingInstruction);
                }
            }

            return processingInstruction + Environment.NewLine + xml;
        }

        public static byte[] StringToByte(this string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return default;
            }
            else
            {
                return Encoding.UTF8.GetBytes(msg);
            }
        }
    }
}
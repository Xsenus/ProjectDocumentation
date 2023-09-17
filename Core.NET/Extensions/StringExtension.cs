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
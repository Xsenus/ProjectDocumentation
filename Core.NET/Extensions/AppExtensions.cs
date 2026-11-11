// Ignore Spelling: App

using System;
using System.Collections.Generic;
using System.IO;

namespace Core.NET.Extensions
{
    public static class AppExtensions
    {
        public static IEnumerable<byte> ReadStream(Stream input)
        {
            // CopyTo не требует Stream.Length, поэтому работает и с потоками без поддержки поиска.
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public static void ConsoleWriteLine(string message)
        {
            ConsoleWriteLineWithColor(message, ConsoleColor.Green);
        }

        public static void ConsoleWriteLineEx(string message)
        {
            ConsoleWriteLineWithColor(message, ConsoleColor.Red);
        }

        public static void ConsoleWriteLineWithColor(string message, ConsoleColor consoleColor)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                Console.ForegroundColor = consoleColor;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Побайтовый XOR массивов. Длина результата — длина первого массива; более короткие массивы
        /// участвуют своими байтами. Входные массивы не изменяются.
        /// </summary>
        public static byte[] GetXORSum(this IEnumerable<byte[]> list)
        {
            byte[] result = null;
            foreach (var bytes in list)
            {
                if (bytes is null)
                {
                    continue;
                }

                if (result is null)
                {
                    // Копия: раньше первый массив списка изменялся на месте.
                    result = (byte[])bytes.Clone();
                    continue;
                }

                var length = Math.Min(result.Length, bytes.Length);
                for (int i = 0; i < length; i++)
                {
                    result[i] ^= bytes[i];
                }
            }

            return result;
        }
    }
}

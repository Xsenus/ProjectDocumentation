using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.NET.Extensions
{
    public static class AppExtensions
    {
        public static IEnumerable<byte> ReadStream(Stream input)
        {
            var buffer = new byte[input.Length];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
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

        public static byte[] GetXORSum(this IEnumerable<byte[]> list)
        {
            byte[] result = null;
            foreach (var bytes in list)
            {
                try
                {
                    if (result == null)
                    {
                        result = bytes;
                    }
                    else
                    {
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            result[i] ^= bytes[i];
                        }
                    }
                }
                catch (Exception) { }
            }
            return result;
        }
    }
}

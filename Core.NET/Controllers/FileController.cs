using System;

namespace Core.NET.Controllers
{
    public static class FileController
    {
        public static bool CheckFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (System.IO.File.Exists(path) is false)
            {
                return false;
            }

            return true;
        }
    }
}

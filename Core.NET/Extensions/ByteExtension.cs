using System.Text;

namespace Core.NET.Extensions
{
    public static class ByteExtension
    {
        public static string ByteToString(this byte[] msg)
        {
            if (msg == null)
            {
                return default;
            }
            else
            {
                return Encoding.UTF8.GetString(msg);
            }
        }
    }
}

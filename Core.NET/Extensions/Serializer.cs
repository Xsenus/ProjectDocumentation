using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Core.NET.Extensions
{
    public sealed class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }

    public static class Serializer
    {
        public static string SerializeObject<T>(this T obj)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (StringWriter textWriter = new Utf8StringWriter())
            {
                serializer.Serialize(textWriter, obj);
                return textWriter.ToString();
            }
        }

        public static T DeserializeObjectByString<T>(string str)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(str))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        public static T DeserializeObject<T>(string path)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var fileStream = File.OpenRead(path))
            {
                var obj = (T)serializer.Deserialize(fileStream);
                return obj;
            }

            //using (var reader = new StreamReader(path))
            //{
            //    var serializer = new XmlSerializer(typeof(T));
            //    var obj = (T)serializer.Deserialize(reader);
            //    return obj;
            //}
        }
    }
}

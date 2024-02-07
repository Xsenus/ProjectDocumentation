// Ignore Spelling: Utf

using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Core.NET.Extensions
{
    public sealed class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }

    public static class Utf8StringSerialize
    {
        public static string SerializeObject<T>(this T obj)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));

                using (StringWriter textWriter = new Utf8StringWriter())
                {
                    serializer.Serialize(textWriter, obj);
                    return textWriter.ToString();
                }
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static T DeSerializeObjectByString<T>(string str)
        {            
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (TextReader reader = new StringReader(str))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static T DeSerializeObject<T>(string path)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (var fileStream = File.OpenRead(path))
                {
                    var obj = (T)serializer.Deserialize(fileStream);
                    return obj;
                }
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}

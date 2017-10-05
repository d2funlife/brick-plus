#region Using statements
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text; 
#endregion

namespace Tetris.Helpers
{
    public static class JsonSerializeHelper
    {
        public static T Deserialize<T>(string json)
        {
            var bytes = Encoding.Unicode.GetBytes(json);
            using (var stream = new MemoryStream(bytes))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(stream);
            }
        }

        public static string Serialize(object instance)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(instance.GetType());
                serializer.WriteObject(stream, instance);
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                { return reader.ReadToEnd(); }
            }
        }
    }
}
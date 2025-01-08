using Newtonsoft.Json;
using System.IO;

namespace ModLibrary.ModUtilities
{
    /// <summary>
    /// Tools to efficiently serialize/deserialize json objects
    /// </summary>
    public static class JsonTools
    {
        public static string Serialize(object obj)
        {
            string result;

            result = JsonConvert.SerializeObject(obj, DataRepository.Instance.GetSettings());

            return result;
        }

        public static T Deserialize<T>(string @string)
        {
            T result;

            using (StringReader stringReader = new StringReader(@string))
            using (JsonReader jsonReader = new JsonTextReader(stringReader))
            {
                result = JsonSerializer.Create(DataRepository.Instance.GetSettings()).Deserialize<T>(jsonReader);
            }

            return result;
        }

        public static T DeserializeFile<T>(string path)
        {
            T result;

            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                result = DeserializeStream<T>(fileStream);

            return result;
        }

        public static void SerializeFile(string path, object obj)
        {
            using (FileStream fileStream = new FileStream(path, File.Exists(path) ? FileMode.Truncate : FileMode.Create, FileAccess.Write))
                SerializeStream(fileStream, obj);
        }

        public static T DeserializeStream<T>(Stream stream)
        {
            T result;

            using (StreamReader streamReader = new StreamReader(stream))
            using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
            {
                result = JsonSerializer.Create(DataRepository.Instance.GetSettings()).Deserialize<T>(jsonTextReader);
            }

            return result;
        }

        public static void SerializeStream(Stream stream, object obj)
        {
            using (StreamWriter streamWriter = new StreamWriter(stream))
            using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                JsonSerializer.Create(DataRepository.Instance.GetSettings()).Serialize(jsonTextWriter, obj);
            }
        }
    }
}

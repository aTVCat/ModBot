using Newtonsoft.Json;
using System.IO;

namespace ModLibrary
{
    /// <summary>
    /// Tools to efficiently serialize/deserialize json objects
    /// </summary>
    public static class JsonTools
    {
        /// <summary>
        /// Serializes an object using <see cref="DataRepository.GetSettings()"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            string result;

            result = JsonConvert.SerializeObject(obj, DataRepository.Instance.GetSettings());

            return result;
        }

        /// <summary>
        /// Deserializes an object of specified type using <see cref="DataRepository.GetSettings()"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="string"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Deserializes an object using <see cref="DataRepository.GetSettings()"/>
        /// </summary>
        /// <param name="string"></param>
        /// <returns></returns>
        public static object Deserialize(string @string)
        {
            object result;

            using (StringReader stringReader = new StringReader(@string))
            using (JsonReader jsonReader = new JsonTextReader(stringReader))
            {
                result = JsonSerializer.Create(DataRepository.Instance.GetSettings()).Deserialize(jsonReader);
            }

            return result;
        }

        /// <summary>
        /// Serializes and writes an object to file using <see cref="DataRepository.GetSettings()"/>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        public static void SerializeFile(string path, object obj)
        {
            using (FileStream fileStream = new FileStream(path, File.Exists(path) ? FileMode.Truncate : FileMode.Create, FileAccess.Write))
                SerializeStream(fileStream, obj);
        }

        /// <summary>
        /// Reads and deserializes an object of specified type from file using <see cref="DataRepository.GetSettings()"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T DeserializeFile<T>(string path)
        {
            T result;

            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                result = DeserializeStream<T>(fileStream);

            return result;
        }

        /// <summary>
        /// Reads and deserializes an object from file using <see cref="DataRepository.GetSettings()"/>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static object DeserializeFile(string path)
        {
            object result;

            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                result = DeserializeStream(fileStream);

            return result;
        }

        /// <summary>
        /// Serializes and writes to stream an object using <see cref="DataRepository.GetSettings()"/>
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="obj"></param>
        public static void SerializeStream(Stream stream, object obj)
        {
            using (StreamWriter streamWriter = new StreamWriter(stream))
            using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                JsonSerializer.Create(DataRepository.Instance.GetSettings()).Serialize(jsonTextWriter, obj);
            }
        }

        /// <summary>
        /// Reads and deserializes an object of specified type from stream using <see cref="DataRepository.GetSettings()"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Reads and deserializes an object from stream using <see cref="DataRepository.GetSettings()"/>
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static object DeserializeStream(Stream stream)
        {
            object result;

            using (StreamReader streamReader = new StreamReader(stream))
            using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
            {
                result = JsonSerializer.Create(DataRepository.Instance.GetSettings()).Deserialize(jsonTextReader);
            }

            return result;
        }

        /// <summary>
        /// Serializes an object using default settings
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="jsonSerializerSettings">Serializer settings. Leave this parameter null to use default settings</param>
        /// <returns></returns>
        public static string SerializeCustomSettings(object obj, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (jsonSerializerSettings == null)
                jsonSerializerSettings = JsonConvert.DefaultSettings();

            string result;

            result = JsonConvert.SerializeObject(obj, jsonSerializerSettings);

            return result;
        }

        /// <summary>
        /// Deserializes an object of specified type using default settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="string"></param>
        /// <param name="jsonSerializerSettings">Serializer settings. Leave this parameter null to use default settings</param>
        /// <returns></returns>
        public static T DeserializeCustomSettings<T>(string @string, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (jsonSerializerSettings == null)
                jsonSerializerSettings = JsonConvert.DefaultSettings();

            T result;

            using (StringReader stringReader = new StringReader(@string))
            using (JsonReader jsonReader = new JsonTextReader(stringReader))
            {
                result = JsonSerializer.Create(jsonSerializerSettings).Deserialize<T>(jsonReader);
            }

            return result;
        }

        /// <summary>
        /// Deserializes an object using default settings
        /// </summary>
        /// <param name="string"></param>
        /// <param name="jsonSerializerSettings">Serializer settings. Leave this parameter null to use default settings</param>
        /// <returns></returns>
        public static object DeserializeCustomSettings(string @string, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (jsonSerializerSettings == null)
                jsonSerializerSettings = JsonConvert.DefaultSettings();

            object result;

            using (StringReader stringReader = new StringReader(@string))
            using (JsonReader jsonReader = new JsonTextReader(stringReader))
            {
                result = JsonSerializer.Create(jsonSerializerSettings).Deserialize(jsonReader);
            }

            return result;
        }

        /// <summary>
        /// Serializes and writes an object to file using default settings
        /// </summary>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        /// <param name="jsonSerializerSettings">Serializer settings. Leave this parameter null to use default settings</param>
        public static void SerializeFileCustomSettings(string path, object obj, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (jsonSerializerSettings == null)
                jsonSerializerSettings = JsonConvert.DefaultSettings();

            using (FileStream fileStream = new FileStream(path, File.Exists(path) ? FileMode.Truncate : FileMode.Create, FileAccess.Write))
                SerializeStreamCustomSettings(fileStream, obj, jsonSerializerSettings);
        }

        /// <summary>
        /// Reads and deserializes an object of specified type from file using default settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="jsonSerializerSettings">Serializer settings. Leave this parameter null to use default settings</param>
        /// <returns></returns>
        public static T DeserializeFileCustomSettings<T>(string path, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (jsonSerializerSettings == null)
                jsonSerializerSettings = JsonConvert.DefaultSettings();

            T result;

            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                result = DeserializeStreamCustomSettings<T>(fileStream, jsonSerializerSettings);

            return result;
        }

        /// <summary>
        /// Reads and deserializes an object from file using default settings
        /// </summary>
        /// <param name="path"></param>
        /// <param name="jsonSerializerSettings">Serializer settings. Leave this parameter null to use default settings</param>
        /// <returns></returns>
        public static object DeserializeFileCustomSettings(string path, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (jsonSerializerSettings == null)
                jsonSerializerSettings = JsonConvert.DefaultSettings();

            object result;

            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                result = DeserializeStreamCustomSettings(fileStream, jsonSerializerSettings);

            return result;
        }

        /// <summary>
        /// Serializes and writes to stream an object using default settings
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="obj"></param>
        /// <param name="jsonSerializerSettings">Serializer settings. Leave this parameter null to use default settings</param>
        public static void SerializeStreamCustomSettings(Stream stream, object obj, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (jsonSerializerSettings == null)
                jsonSerializerSettings = JsonConvert.DefaultSettings();

            using (StreamWriter streamWriter = new StreamWriter(stream))
            using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                JsonSerializer.Create(jsonSerializerSettings).Serialize(jsonTextWriter, obj);
            }
        }

        /// <summary>
        /// Reads and deserializes an object of specified type from stream using custom settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="jsonSerializerSettings">Serializer settings. Leave this parameter null to use default settings</param>
        /// <returns></returns>
        public static T DeserializeStreamCustomSettings<T>(Stream stream, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (jsonSerializerSettings == null)
                jsonSerializerSettings = JsonConvert.DefaultSettings();

            T result;

            using (StreamReader streamReader = new StreamReader(stream))
            using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
            {
                result = JsonSerializer.Create(jsonSerializerSettings).Deserialize<T>(jsonTextReader);
            }

            return result;
        }

        /// <summary>
        /// Reads and deserializes an object from stream using custom settings
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="jsonSerializerSettings">Serializer settings. Leave this parameter null to use default settings</param>
        /// <returns></returns>
        public static object DeserializeStreamCustomSettings(Stream stream, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (jsonSerializerSettings == null)
                jsonSerializerSettings = JsonConvert.DefaultSettings();

            object result;

            using (StreamReader streamReader = new StreamReader(stream))
            using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
            {
                result = JsonSerializer.Create(jsonSerializerSettings).Deserialize(jsonTextReader);
            }

            return result;
        }
    }
}

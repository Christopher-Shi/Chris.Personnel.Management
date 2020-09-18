using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Chris.Personnel.Management.Common.Extensions
{
    public static class Json
    {
        private class AllContractResolver : DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(x => x.CanWrite)
                    .Select(p => base.CreateProperty(p, memberSerialization))
                    .Union(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(f => !f.Name.Contains("k__BackingField"))
                        .Select(f => base.CreateProperty(f, memberSerialization)))
                    .ToList();
                props.ForEach(p => { p.Writable = true; p.Readable = true; });
                return props;
            }
        }

        /// <summary>
        /// Creates a JSON string from given object
        /// </summary>
        /// <param name="objectToConvert"></param>
        /// <param name="indent"></param>
        /// <returns></returns>
        public static string ToJson(object objectToConvert, bool indent = false)
        {
            if (_serializerSettings == null)
            {
                CreateJsonSettings();
            }

            //
            // Should result be indented or not?
            //
            var formatting = indent ? Formatting.Indented : Formatting.None;

            //
            // Call external tool
            //
            var data = JsonConvert.SerializeObject(objectToConvert, formatting, _serializerSettings);
            return data;
        }

        public static void ToJsonFile(object objectToConvert, string filePath = "")
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location),
                    Guid.NewGuid() + ".json");
            }

            var folder = System.IO.Path.GetDirectoryName(filePath);
            if (!System.IO.Directory.Exists(folder))
            {
                System.IO.Directory.CreateDirectory(folder);
            }

            System.IO.File.WriteAllText(filePath, ToJson(objectToConvert, true));
        }

        private static JsonSerializerSettings _serializerSettings;

        /// <summary>
        /// Creates instance of speciefied type from JSON data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T FromJson<T>(string json)
        {
            if (_serializerSettings == null)
            {
                CreateJsonSettings();
            }

            //
            // Call external tool
            //
            return JsonConvert.DeserializeObject<T>(json, _serializerSettings);
        }

        private static void CreateJsonSettings()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new AllContractResolver()
            };
        }

        public static T FromJson<T>(T dummy, string json)
        {
            return FromJson<T>(json);
        }

        public static dynamic FromJsonDynamic(dynamic dummy, string json)
        {
            return JsonConvert.DeserializeObject(json, dummy);
        }

        public static T FromJsonFile<T>(string filePath)
        {
            var json = System.IO.File.ReadAllText(filePath);
            return FromJson<T>(json);
        }

        /// <summary>
        /// Creates a clone of the given object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToClone"></param>
        /// <returns></returns>
        public static T Clone<T>(T objectToClone)
        {
            return FromJson<T>(ToJson(objectToClone));
        }

        public static T CloneTo<T>(dynamic objectToClone)
        {
            return FromJson<T>(ToJson(objectToClone));
        }

        public static T Merge3<T>(T objectOriginal, T objectLeft, T objectRight, bool makeLeftWin = false)
        {
            var jsonO = ToJson(objectOriginal, true);
            var jsonLeft = ToJson(objectLeft, true);
            var jsonRight = ToJson(objectRight, true);
            var jsonM = Diff3.Merge(jsonO, jsonLeft, jsonRight, makeLeftWin);
            return FromJson<T>(jsonM);
        }
    }
}

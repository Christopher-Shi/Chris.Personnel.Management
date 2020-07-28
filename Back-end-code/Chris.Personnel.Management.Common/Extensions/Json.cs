using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Chris.Personnel.Management.Common.Extensions
{
    public static class JSON
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
        public static string ToJSON(object objectToConvert, bool indent = false)
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

        public static void ToJSONFile(object objectToConvert, string filePath = "")
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

            System.IO.File.WriteAllText(filePath, ToJSON(objectToConvert, true));
        }

        private static JsonSerializerSettings _serializerSettings;

        /// <summary>
        /// Creates instance of speciefied type from JSON data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T FromJSON<T>(string json)
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

        public static T FromJSON<T>(T dummy, string json)
        {
            return FromJSON<T>(json); 
        }

        public static dynamic FromJSONDynamic(dynamic dummy, string json)
        {
            return FromJSON(dummy, json);
        }

        public static T FromJSONFile<T>(string filePath)
        {
            var json = System.IO.File.ReadAllText(filePath);
            return FromJSON<T>(json);
        }

        /// <summary>
        /// Creates a clone of the given object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToClone"></param>
        /// <returns></returns>
        public static T Clone<T>(T objectToClone)
        {
            return FromJSON<T>(ToJSON(objectToClone));
        }

        public static T CloneTo<T>(dynamic objectToClone)
        {
            return FromJSON<T>(ToJSON(objectToClone));
        }

        public static T Merge3<T>(T objectOriginal, T objectLeft, T objectRight, bool makeLeftWin = false)
        {
            var jsonO = ToJSON(objectOriginal, true);
            var jsonLeft = ToJSON(objectLeft, true);
            var jsonRight = ToJSON(objectRight, true);
            var jsonM = Diff3.Merge(jsonO, jsonLeft, jsonRight, makeLeftWin);
            return FromJSON<T>(jsonM);
        }
    }
}

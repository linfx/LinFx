using System;
using System.Text;
using System.Text.Json;

namespace LinFx.Utils
{
    public static partial class JsonUtils
    {
        public static object ToObject(byte[] value, bool camelCase = false, bool indented = false)
        {
            return ToObject<object>(value, camelCase, indented);
        }

        public static T ToObject<T>(byte[] value, bool camelCase = false, bool indented = false)
        {
            var s = Encoding.UTF8.GetString(value);
            return ToObject<T>(s, camelCase, indented);
        }

        public static byte[] ToBytes(object value, bool camelCase = false, bool indented = false)
        {
            var s = ToJson(value, camelCase, indented);
            return Encoding.UTF8.GetBytes(s);
        }

        public static string ToJson(object value, bool camelCase = false, bool indented = false)
        {
            var options = new JsonSerializerOptions();

            if (camelCase)
                options.PropertyNameCaseInsensitive = true;

            if (indented)
                options.WriteIndented = true;

            return JsonSerializer.Serialize(value, options);
        }

        public static string ToJson<TValue>(TValue value, bool camelCase = false, bool indented = false)
        {
            var options = new JsonSerializerOptions();

            if (camelCase)
                options.PropertyNameCaseInsensitive = true;

            if (indented)
                options.WriteIndented = true;

            return JsonSerializer.Serialize(value, options);
        }

        public static TValue ToObject<TValue>(string json, bool camelCase = false, bool indented = false)
        {
            var options = new JsonSerializerOptions();

            if (camelCase)
                options.PropertyNameCaseInsensitive = true;

            if (indented)
                options.WriteIndented = true;

            return JsonSerializer.Deserialize<TValue>(json, options);
        }

        private const char TypeSeperator = '|';

        /// <summary>
        /// Serializes an object with a type information included.
        /// So, it can be deserialized using <see cref="DeserializeWithType"/> method later.
        /// </summary>
        public static string SerializeWithType(object obj)
        {
            return SerializeWithType(obj, obj.GetType());
        }

        /// <summary>
        /// Serializes an object with a type information included.
        /// So, it can be deserialized using <see cref="DeserializeWithType"/> method later.
        /// </summary>
        public static string SerializeWithType(object obj, Type type)
        {
            var serialized = obj.ToJson();

            return string.Format(
                "{0}{1}{2}",
                type.AssemblyQualifiedName,
                TypeSeperator,
                serialized);
        }

        /// <summary>
        /// Deserializes an object serialized with <see cref="SerializeWithType(object)"/> methods.
        /// </summary>
        public static T DeserializeWithType<T>(string serializedObj)
        {
            return (T)DeserializeWithType(serializedObj);
        }

        /// <summary>
        /// Deserializes an object serialized with <see cref="SerializeWithType(object)"/> methods.
        /// </summary>
        public static object DeserializeWithType(string serializedObj)
        {
            var typeSeperatorIndex = serializedObj.IndexOf(TypeSeperator);
            var type = Type.GetType(serializedObj.Substring(0, typeSeperatorIndex));
            var serialized = serializedObj.Substring(typeSeperatorIndex + 1);

           var options = new JsonSerializerOptions();

            return JsonSerializer.Deserialize(serialized, type, options);
        }

        public static object DeserializeObject(string value, Type type)
        {
            return JsonSerializer.Deserialize(value, type);
        }
    }

    public static class JsonUtilsExtensions
    {
        /// <summary>
        /// Converts given object to JSON string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="camelCase"></param>
        /// <param name="indented"></param>
        /// <returns></returns>
        public static string ToJson(this object value, bool camelCase = true, bool indented = false)
        {
            return JsonUtils.ToJson(value, camelCase, indented);
        }

        public static byte[] ToBytes(this object value, bool camelCase = false, bool indented = false)
        {
            return JsonUtils.ToBytes(value, camelCase, indented);
        }

        public static T ToObject<T>(this string value, bool camelCase = false, bool indented = false)
        {
            return JsonUtils.ToObject<T>(value, camelCase, indented);
        }
    }
}

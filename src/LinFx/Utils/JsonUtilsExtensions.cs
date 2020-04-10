using System;

namespace LinFx.Utils
{
    public static class JsonUtilsExtensions
    {
        /// <summary>
        /// Converts given object to JSON string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="camelCase"></param>
        /// <param name="indented"></param>
        /// <returns></returns>
        public static string ToJsonString(this object value, bool camelCase = true, bool indented = false)
        {
            return JsonUtils.ToJsonString(value, camelCase, indented);
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

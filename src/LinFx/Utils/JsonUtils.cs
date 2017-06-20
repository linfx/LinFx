using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;

namespace LinFx.Utils
{
    class JsonUtils
    {
    }

    /// <summary>
    /// 转化小写
    /// </summary>
    public class LowercaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
    /// <summary>  
    /// Newtonsoft.Json序列化扩展特性  
    /// <para>DateTime序列化（输出为时间戳）</para>  
    /// </summary>  
    public class TimestampConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ConvertIntDateTime(int.Parse(reader.Value.ToString()));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(ConvertDateTimeInt((DateTime)value));
        }

        public static DateTime ConvertIntDateTime(int aSeconds)
        {
            return new DateTime(1970, 1, 1).AddSeconds(aSeconds);
        }

        public static int ConvertDateTimeInt(DateTime aDT)
        {
            return (int)(aDT - new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }

    /// <summary>  
    /// Newtonsoft.Json序列化扩展特性  
    /// <para>String Unicode 序列化（输出为Unicode编码字符）</para>  
    /// </summary>  
    public class UnicodeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(ToUnicode(value.ToString()));
        }

        public static string ToUnicode(string str)
        {
            byte[] bts = Encoding.Unicode.GetBytes(str);
            string r = "";
            for (int i = 0; i < bts.Length; i += 2)
            {
                r += "\\u" + bts[i + 1].ToString("X").PadLeft(2, '0') + bts[i].ToString("X").PadLeft(2, '0');
            }
            return r;
        }
    }
}

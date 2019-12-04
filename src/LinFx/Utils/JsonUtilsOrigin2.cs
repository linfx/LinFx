using System.Text.Json;

namespace LinFx.Utils
{
    public static partial class JsonUtils
    {
        public static string Serialize<TValue>(TValue value, bool camelCase = true, bool indented = false, bool ignoreNullValues = false, bool ignoreReadOnlyProperties = true)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = indented,                               //格式化json字符串
                AllowTrailingCommas = true,                             //可以结尾有逗号
                PropertyNameCaseInsensitive = true,                     //忽略大小写
                IgnoreNullValues = ignoreNullValues,                    //可以有空值,转换json去除空值属性
                IgnoreReadOnlyProperties = ignoreReadOnlyProperties     //忽略只读属性
            };

            if (camelCase)
                options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            options.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm"));
            options.Converters.Add(new DateTimeOffsetJsonConverter("yyyy-MM-dd HH:mm"));

            return JsonSerializer.Serialize(value, options);
        }

        public static TValue Deserialize<TValue>(string json)
        {
            return JsonSerializer.Deserialize<TValue>(json);
        }
    }
}

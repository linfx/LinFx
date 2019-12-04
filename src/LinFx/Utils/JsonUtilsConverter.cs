using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LinFx.Utils
{
    /// <summary>
    /// 格式化DateTime
    /// </summary>
    public class DateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
    {
        /// <summary>
        /// 获取日期格式
        /// </summary>
        public string DateTimeFormat { get; }

        /// <summary>
        /// ctor
        /// </summary>      
        /// <param name="dateTimeFormat"></param>
        public DateTimeOffsetJsonConverter(string dateTimeFormat)
        {
            this.DateTimeFormat = dateTimeFormat;
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTimeOffset.Parse(reader.GetString());
        }

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            value = value.ToLocalTime();
            writer.WriteStringValue(value.ToString(this.DateTimeFormat, CultureInfo.InvariantCulture));
        }
    }

    /// <summary>
    /// 格式化DateTime
    /// </summary>
    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        /// <summary>
        /// 获取日期格式
        /// </summary>
        public string DateTimeFormat { get; }

        /// <summary>
        /// ctor
        /// </summary>      
        /// <param name="dateTimeFormat"></param>
        public DateTimeJsonConverter(string dateTimeFormat)
        {
            this.DateTimeFormat = dateTimeFormat;
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString());
        }

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            if (value.Kind == DateTimeKind.Utc)
                value = value.ToLocalTime();

            writer.WriteStringValue(value.ToString(this.DateTimeFormat, CultureInfo.InvariantCulture));
        }
    }
}

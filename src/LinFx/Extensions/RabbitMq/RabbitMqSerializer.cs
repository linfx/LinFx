using System;
using System.Text.Json;

namespace LinFx.Extensions.RabbitMq
{
    public class RabbitMqSerializer : IRabbitMqSerializer
    {
        public object Deserialize(byte[] value, Type type)
        {
            return JsonSerializer.Deserialize(value, type);
        }

        public byte[] Serialize(object value)
        {
            return JsonSerializer.SerializeToUtf8Bytes(value);
        }
    }
}
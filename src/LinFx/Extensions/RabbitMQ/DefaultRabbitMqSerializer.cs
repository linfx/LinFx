using LinFx.Utils;
using System;
using System.Text;

namespace LinFx.Extensions.RabbitMq
{
    public class DefaultRabbitMqSerializer : IRabbitMqSerializer
    {
        public object Deserialize(byte[] value, Type type)
        {
            var message = JsonUtils.ToObject(value);
            return message;
        }

        public byte[] Serialize(object value)
        {
            var message = value.ToJsonString();
            var body = Encoding.UTF8.GetBytes(message);
            return body;
        }
    }
}

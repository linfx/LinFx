using Newtonsoft.Json;
using System;
using System.Text;

namespace LinFx.Extensions.RabbitMQ
{
    public class DefaultRabbitMqSerializer : IRabbitMqSerializer
    {
        public object Deserialize(byte[] value, Type type)
        {
            throw new NotImplementedException();
        }

        public byte[] Serialize(object obj)
        {
            var message = JsonConvert.SerializeObject(obj);
            var body = Encoding.UTF8.GetBytes(message);
            return body;
        }
    }
}

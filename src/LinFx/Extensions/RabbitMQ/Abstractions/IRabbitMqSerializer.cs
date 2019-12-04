using System;

namespace LinFx.Extensions.RabbitMq
{
    public interface IRabbitMqSerializer
    {
        byte[] Serialize(object obj);

        object Deserialize(byte[] value, Type type);
    }
}

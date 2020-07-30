using System;

namespace LinFx.Extensions.RabbitMq
{
    /// <summary>
    /// 序列化
    /// </summary>
    public interface IRabbitMqSerializer
    {
        byte[] Serialize(object obj);

        object Deserialize(byte[] value, Type type);
    }
}

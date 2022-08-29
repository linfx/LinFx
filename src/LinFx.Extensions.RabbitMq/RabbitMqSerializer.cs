using System.Text.Json;

namespace LinFx.Extensions.RabbitMq;

public class RabbitMqSerializer : IRabbitMqSerializer
{
    public object? Deserialize(byte[] value, Type type) => JsonSerializer.Deserialize(value, type);

    public byte[] Serialize(object value) => JsonSerializer.SerializeToUtf8Bytes(value);
}

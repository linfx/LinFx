using LinFx.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;

namespace LinFx.Extensions.Kafka;

public class Utf8JsonKafkaSerializer : IKafkaSerializer, ITransientDependency
{
    public byte[] Serialize(object obj) => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));

    public object Deserialize(byte[] value, Type type) => JsonSerializer.Deserialize(value, type);

    public T Deserialize<T>(byte[] value) => JsonSerializer.Deserialize<T>(value);
}

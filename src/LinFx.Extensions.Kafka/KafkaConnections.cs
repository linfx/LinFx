using Confluent.Kafka;
using JetBrains.Annotations;

namespace LinFx.Extensions.Kafka;

/// <summary>
/// 连接池
/// </summary>
[Serializable]
public class KafkaConnections : Dictionary<string, ClientConfig>
{
    public const string DefaultConnectionName = "Default";

    public KafkaConnections()
    {
        Default = new ClientConfig();
    }

    [NotNull]
    public ClientConfig Default
    {
        get => this[DefaultConnectionName];
        set => this[DefaultConnectionName] = Check.NotNull(value, nameof(value));
    }

    public ClientConfig GetOrDefault(string connectionName)
    {
        if (TryGetValue(connectionName, out var connectionFactory))
            return connectionFactory;

        return Default;
    }
}

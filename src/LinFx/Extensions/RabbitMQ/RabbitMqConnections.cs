using RabbitMQ.Client;
using System.Collections.Generic;

namespace LinFx.Extensions.RabbitMQ
{
    public class RabbitMqConnections : Dictionary<string, ConnectionFactory>
    {
        public const string DefaultConnectionName = "Default";

        [NotNull]
        public ConnectionFactory Default
        {
            get => this[DefaultConnectionName];
            set => this[DefaultConnectionName] = Check.NotNull(value, nameof(value));
        }

        public RabbitMqConnections()
        {
            Default = new ConnectionFactory();
        }

        public ConnectionFactory GetOrDefault(string connectionName)
        {
            if (TryGetValue(connectionName, out var connectionFactory))
            {
                return connectionFactory;
            }

            return Default;
        }
    }
}

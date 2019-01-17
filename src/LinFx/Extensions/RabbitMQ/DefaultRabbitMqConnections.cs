using RabbitMQ.Client;
using System.Collections.Generic;

namespace LinFx.Extensions.RabbitMQ
{
    public class DefaultRabbitMqConnections : Dictionary<string, ConnectionFactory>
    {
        public const string DefaultConnectionName = "Default";

        [NotNull]
        public ConnectionFactory Default
        {
            get => this[DefaultConnectionName];
            set => this[DefaultConnectionName] = Check.NotNull(value, nameof(value));
        }

        public DefaultRabbitMqConnections()
        {
            Default = new ConnectionFactory
            {
                HostName = "14.21.34.85",
                UserName = "admin",
                Password = "admin.123456"
            };
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

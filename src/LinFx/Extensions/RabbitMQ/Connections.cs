using RabbitMQ.Client;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.RabbitMq
{
    internal class Connections : Dictionary<string, ConnectionFactory>
    {
        public const string DefaultConnectionName = "Default";

        [NotNull]
        public ConnectionFactory Default
        {
            get => this[DefaultConnectionName];
            set => this[DefaultConnectionName] = Check.NotNull(value, nameof(value));
        }

        public Connections()
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

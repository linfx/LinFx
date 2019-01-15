using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace LinFx.Extensions.RabbitMQ
{
    public class ConnectionPool : IConnectionPool
    {
        private bool _isDisposed;

        protected RabbitMqOptions Options { get; }

        protected ConcurrentDictionary<string, IConnection> Connections { get; }

        public ConnectionPool(IOptions<RabbitMqOptions> options)
        {
            Options = options.Value;
            Connections = new ConcurrentDictionary<string, IConnection>();
        }

        public virtual IConnection Get(string connectionName = null)
        {
            connectionName = connectionName
                             ?? RabbitMqConnections.DefaultConnectionName;

            return Connections.GetOrAdd(
                connectionName,
                () => Options
                    .ConnectionFactories
                    .GetOrDefault(connectionName)
                    .CreateConnection()
            );
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            foreach (var connection in Connections.Values)
            {
                try
                {
                    connection.Dispose();
                }
                catch
                {

                }
            }

            Connections.Clear();
        }
    }
}
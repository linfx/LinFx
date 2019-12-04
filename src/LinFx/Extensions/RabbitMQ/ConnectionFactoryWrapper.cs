using RabbitMQ.Client;

namespace LinFx.Extensions.RabbitMq
{
    public sealed class ConnectionFactoryWrapper
    {
        private readonly Connections _connections;

        public ConnectionConfiguration Configuration { get; }

        public ConnectionFactoryWrapper(ConnectionConfiguration configuration)
        {
            Configuration = configuration;
            _connections = new Connections();
        }

        public IConnection CreateConnection(string connectionName)
        {
            var factory = _connections.GetOrDefault(connectionName);
            factory.HostName = Configuration.Host;
            factory.UserName = Configuration.UserName;
            factory.Password = Configuration.Password;
            return factory.CreateConnection();
        }
    }
}
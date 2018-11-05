using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Threading;

namespace LinFx.Extensions.RabbitMQ
{
    public class RabbitMQContext
    {
        private static IConnection _connection;

        readonly RabbitMQOptions _options;
        readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);

        public RabbitMQContext(IOptions<RabbitMQOptions> options)
        {
            _options = options.Value;
        }

        public void Connect()
        {
            _connectionLock.Wait();
            try
            {
                var factory = new ConnectionFactory
                {
                    UserName = _options.UserName,
                    Password = _options.Password,
                    HostName = _options.HostName,
                };
                _connection = factory.CreateConnection();
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        public IModel CreateModel()
        {
            Connect();

            return _connection.CreateModel();
        }
    }
}

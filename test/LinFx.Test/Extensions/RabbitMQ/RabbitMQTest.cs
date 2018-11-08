using LinFx.Extensions.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LinFx.Test.Extensions
{
    public class RabbitMQTest
    {
        ServiceCollection _services;
        IRabbitMQPersistentConnection _persistentConnection;

        public RabbitMQTest()
        {
            _services = new ServiceCollection();
            _services.AddLinFx()
                .AddRabbitMQ(options =>
                {
                    options.Host = "127.0.0.1";
                    options.UserName = "admin";
                    options.Password = "admin.123456";
                });

            var container = _services.BuildServiceProvider();
            _persistentConnection = container.GetService<IRabbitMQPersistentConnection>();
        }


        [Fact]
        public void Should_Be_Connected()
        {
            _persistentConnection.TryConnect();

            using (var channel = _persistentConnection.CreateModel())
            {
                Assert.True(_persistentConnection.IsConnected);
            }
        }
    }
}

using LinFx.Extensions.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LinFx.Test.Extensions.RabbitMq
{
    public class RabbitMqTests
    {
        private readonly ServiceProvider _services;

        public RabbitMqTests()
        {
            var services = new ServiceCollection();
            services
                .AddLinFx()
                .AddRabbitMq(x =>
                {
                    x.Connections.Default.HostName = "127.0.0.1";
                    x.Connections.Default.UserName = "admin";
                    x.Connections.Default.Password = "admin.123456";
                });

            _services = services.BuildServiceProvider();
        }

        [Fact]
        public void ConnectionPool_Test()
        {
            var pool = _services.GetRequiredService<IConnectionPool>();
            using var channel = pool.Get().CreateModel();

            Assert.True(channel.IsOpen);
        }

        [Fact]
        public void ChannelPool_Test()
        {
            var pool = _services.GetRequiredService<IChannelPool>();
            using var channelAccessor = pool.Acquire();
            var channel = channelAccessor.Channel;

            Assert.True(channel.IsOpen);
        }
    }
}

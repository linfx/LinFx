using LinFx.Extensions.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Text;
using Xunit;

namespace LinFx.Test.RabbitMq
{
    public class RabbitMqPersistentConnectionTests
    {
        private readonly IRabbitMqPersistentConnection _persistentConnection;

        public RabbitMqPersistentConnectionTests()
        {
            var services = new ServiceCollection();
            services
                .AddLinFx()
                .AddRabbitMqPersistentConnection(options =>
                {
                    options.HostName = "localhost:5672";
                    options.UserName = "admin";
                    options.Password = "admin.123456";
                });

            var sp = services.BuildServiceProvider();
            _persistentConnection = sp.GetRequiredService<IRabbitMqPersistentConnection>();
        }

        [Fact]
        public void Should_Call_Handler_Correct_SourceAsync()
        {
            var channel = _persistentConnection.CreateModel();
            var exchangeName = "hello_exchange1";
            var queueName = "hello1";

            //定义一个Direct类型交换机
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true, false, null);

            //定义一个队列
            channel.QueueDeclare(queueName, true, false, false, null);

            //将队列绑定到交换机
            channel.QueueBind(queueName, exchangeName, "", null);

            for (int i = 0; i < 1000; i++)
            {
                string message = "Hello World" + i;
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchangeName, queueName, null, body); //开始传递
            }
        }
    }
}

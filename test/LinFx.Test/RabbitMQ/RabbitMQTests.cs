using LinFx.Extensions.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Text;
using Xunit;

namespace LinFx.Test.RabbitMQ
{
    public class RabbitMQTests
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;

        public RabbitMQTests()
        {
            var services = new ServiceCollection();
            services.AddLinFx()
                .AddHttpContextPrincipalAccessor()
                .AddRabbitMQPersistentConnection(options =>
                {
                    options.Host = "127.0.0.1";
                    options.UserName = "admin";
                    options.Password = "admin.123456";
                });

            var sp = services.BuildServiceProvider();
            _persistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
        }


        [Fact]
        public void Should_Call_Handler__Correct_SourceAsync()
        {
            using var channel = _persistentConnection.CreateModel();
            var exchangeName = "hello_exchange1";
            var queueName = "hello1";

            //定义一个Direct类型交换机
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true, false, null);

            //定义一个队列
            channel.QueueDeclare(queueName, true, false, false, null);

            //将队列绑定到交换机
            channel.QueueBind(queueName, exchangeName, "", null);

            string message = "Hello World"; //传递的消息内容
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchangeName, queueName, null, body); //开始传递
        }
    }
}

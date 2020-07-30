namespace LinFx.Extensions.RabbitMq
{
    public interface IConsumerFactory
    {
        /// <summary>
        /// Creates a new <see cref="IRabbitMqConsumer"/>.
        /// Avoid to create too many consumers since they are
        /// not disposed until end of the application.
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        IRabbitMqConsumer Create(ExchangeDeclareConfiguration exchange, QueueDeclareConfiguration queue, string connectionName = null);
    }
}

namespace LinFx.Extensions.RabbitMQ
{
    public interface IConsumerFactory
    {
        /// <summary>
        /// Creates a new <see cref="IConsumer"/>.
        /// Avoid to create too many consumers since they are
        /// not disposed until end of the application.
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        IConsumer Create(
            ExchangeDeclareConfiguration exchange,
            QueueDeclareConfiguration queue,
            string connectionName = null
        );
    }
}

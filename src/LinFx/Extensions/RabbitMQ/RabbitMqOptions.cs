namespace LinFx.Extensions.RabbitMQ
{
    public class RabbitMqOptions
    {
        public RabbitMqConnections ConnectionFactories { get; }

        public RabbitMqOptions()
        {
            ConnectionFactories = new RabbitMqConnections();
        }
    }
}

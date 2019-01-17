namespace LinFx.Extensions.RabbitMQ
{
    public class RabbitMqOptions
    {
        public DefaultRabbitMqConnections ConnectionFactories { get; }

        public RabbitMqOptions()
        {
            ConnectionFactories = new DefaultRabbitMqConnections();
        }
    }
}

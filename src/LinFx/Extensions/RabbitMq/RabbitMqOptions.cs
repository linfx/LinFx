namespace LinFx.Extensions.RabbitMq
{
    public class RabbitMqOptions
    {
        public RabbitMqConnections Connections { get; }

        public RabbitMqOptions()
        {
            Connections = new RabbitMqConnections();
        }
    }
}

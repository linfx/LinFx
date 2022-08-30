namespace LinFx.Extensions.RabbitMQ;

public class RabbitMqOptions
{
    public RabbitMqConnections Connections { get; }

    public RabbitMqOptions()
    {
        Connections = new RabbitMqConnections();
    }
}

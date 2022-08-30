using LinFx.Extensions.RabbitMQ;

namespace LinFx.Extensions.EventBus.RabbitMQ;

public class RabbitMqEventBusOptions
{
    public const string DefaultExchangeType = RabbitMqConsts.ExchangeTypes.Direct;

    public string ConnectionName { get; set; }

    public string ClientName { get; set; }

    public string ExchangeName { get; set; }

    public string ExchangeType { get; set; }

    public ushort? PrefetchCount { get; set; }

    public string GetExchangeTypeOrDefault()
    {
        return string.IsNullOrEmpty(ExchangeType)
            ? DefaultExchangeType
            : ExchangeType;
    }
}

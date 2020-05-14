using LinFx.Extensions.RabbitMq;
using System.Collections.Specialized;

namespace ShopFx.Extensions.MassTransit.Options
{
    public static class MassTransitOptionsExtensions
    {
        public static RabbitMqOptions GetRabbitMqOptions(this MassTransitOptions config)
        {
            var collection = new NameValueCollection();
            var items = config.RabbitMqConnection.Split(';');
            foreach (var item in items)
            {
                var kv = item.Split('=');
                collection.Add(kv[0], kv[1]);
            }

            return new RabbitMqOptions
            {
                Host = collection["host"],
                UserName = collection["username"],
                Password = collection["password"]
            };
        }
    }
}

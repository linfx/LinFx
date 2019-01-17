using System;

namespace LinFx.Extensions.RabbitMQ
{
    [Obsolete]
    public class EventBusRabbitMqOptions
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
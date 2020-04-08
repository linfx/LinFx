using RabbitMQ.Client;
using System;

namespace LinFx.Extensions.RabbitMq
{
    /// <summary>
    /// 连接配置
    /// </summary>
    public class ConnectionConfiguration
    {
        private const int DefaultPort = 5672;
        private const int DefaultAmqpsPort = 5671;

        public string Host { get; set; }
        public ushort Port { get; set; }
        public string VirtualHost { get; set; } = "/";
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public ushort PrefetchCount { get; set; }
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(10);
        public SslOption Ssl { get; }

        /// <summary>
        /// The environment variable named 'RABBITMQ_URL' isn't set. Set it to e.g. 'amqp://localhost'
        /// </summary>
        public Uri Uri { get; set; }

        public ConnectionConfiguration()
        {
            Port = DefaultPort;

            // prefetchCount determines how many messages will be allowed in the local in-memory queue
            // setting to zero makes this infinite, but risks an out-of-memory exception.
            // set to 50 based on this blog post:
            // http://www.rabbitmq.com/blog/2012/04/25/rabbitmq-performance-measurements-part-2/
            PrefetchCount = 50;

            Ssl = new SslOption();
        }
    }
}

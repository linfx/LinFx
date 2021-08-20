namespace LinFx.Extensions.RabbitMq
{
    public class RabbitMqOptions
    {
        public string ConnectionName { get; set; }

        /// <summary>
        /// amqp://user:pass@localhost:5672
        /// </summary>
        public string HostName { get; set; } = "localhost";

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}

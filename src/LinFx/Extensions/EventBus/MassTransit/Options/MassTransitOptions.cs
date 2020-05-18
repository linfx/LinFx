namespace ShopFx.Extensions.MassTransit.Options
{
    public class MassTransitOptions
    {
        /// <summary>
        /// 数据提供类型
        /// </summary>
        public ProviderType Provider { get; set; } = ProviderType.Memory;

        /// <summary>
        /// Redis连接字符串
        /// </summary>
        public string RedisConnection { get; set; }

        /// <summary>
        /// RabbitMq连接字符串
        /// </summary>
        public string RabbitMqConnection { get; set; }
    }
}

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitMQServiceCollectionExtensions
    {
        public static ILinFxBuilder AddEventBus(this ILinFxBuilder builder)
        {
            return builder;
        }
    }
}
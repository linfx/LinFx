namespace Microsoft.Extensions.DependencyInjection
{
    public static class MetricsServiceCollectionExtensions
    {
        public static ILinFxBuilder AddMetrics(this ILinFxBuilder builder)
        {
            return builder;
        }
    }
}
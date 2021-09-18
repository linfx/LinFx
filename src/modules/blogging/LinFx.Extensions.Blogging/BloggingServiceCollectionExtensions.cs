namespace Microsoft.Extensions.DependencyInjection
{
    public static class BloggingServiceCollectionExtensions
    {
        public static LinFxBuilder AddBlogging(this LinFxBuilder builder)
        {
            builder
                .AddAssembly(typeof(BloggingServiceCollectionExtensions).Assembly);

            return builder;
        }
    }
}

using Microsoft.AspNetCore.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpContextPrincipalAccessorServiceCollectionExtensions
    {
        public static ILinFxBuilder AddHttpContextPrincipalAccessor(this ILinFxBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<IHttpContextPrincipalAccessor, HttpContextPrincipalAccessor>();

            return builder;
        }
    }
}

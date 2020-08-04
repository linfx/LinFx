using LinFx.Security.Claims;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpContextPrincipalAccessorServiceCollectionExtensions
    {
        public static LinFxBuilder AddHttpContextPrincipalAccessor(this LinFxBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<IHttpContextPrincipalAccessor, HttpContextPrincipalAccessor>();
            return builder;
        }
    }
}

using LinFx.Security.Claims;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpContextPrincipalAccessorServiceCollectionExtensions
    {
        public static LinFxBuilder AddHttpContextPrincipalAccessor(this LinFxBuilder fx)
        {
            fx.Services.AddHttpContextAccessor();
            fx.Services.AddTransient<IHttpContextPrincipalAccessor, HttpContextPrincipalAccessor>();
            return fx;
        }
    }
}

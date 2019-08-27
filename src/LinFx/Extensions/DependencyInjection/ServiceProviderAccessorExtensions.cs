using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.DependencyInjection
{
    public static class ServiceProviderAccessorExtensions
    {
        [CanBeNull]
        public static HttpContext GetHttpContext(this IServiceProviderAccessor serviceProviderAccessor)
        {
            return serviceProviderAccessor.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        }
    }
}

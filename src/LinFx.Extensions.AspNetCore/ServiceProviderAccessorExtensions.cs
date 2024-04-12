using LinFx.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.AspNetCore;

public static class ServiceProviderAccessorExtensions
{
    public static HttpContext? GetHttpContext(this IServiceProviderAccessor serviceProviderAccessor) => serviceProviderAccessor.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
}

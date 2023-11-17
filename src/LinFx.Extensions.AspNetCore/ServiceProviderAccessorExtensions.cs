using JetBrains.Annotations;
using LinFx.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.AspNetCore;

public static class ServiceProviderAccessorExtensions
{
    [CanBeNull]
    public static HttpContext GetHttpContext(this IServiceProviderAccessor serviceProviderAccessor) => serviceProviderAccessor.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
}

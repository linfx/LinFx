﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.DependencyInjection;

public static class ServiceProviderAccessorExtensions
{
    public static HttpContext GetHttpContext(this IServiceProviderAccessor serviceProviderAccessor)
    {
        return serviceProviderAccessor.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
    }
}

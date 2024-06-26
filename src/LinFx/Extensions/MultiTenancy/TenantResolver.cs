﻿using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LinFx.Extensions.MultiTenancy;

[Service]
public class TenantResolver(IOptions<TenantResolveOptions> options, IServiceProvider serviceProvider) : ITenantResolver
{
    private readonly TenantResolveOptions _options = options.Value;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public TenantResolveResult ResolveTenantIdOrName()
    {
        var result = new TenantResolveResult();

        using (var scope = _serviceProvider.CreateScope())
        {
            var context = new TenantResolveContext(scope.ServiceProvider);

            foreach (var tenantResolver in _options.TenantResolvers)
            {
                tenantResolver.Resolve(context);

                result.AppliedResolvers.Add(tenantResolver.Name);

                if (context.HasResolvedTenantOrHost())
                {
                    result.TenantIdOrName = context.TenantIdOrName;
                    break;
                }
            }
        }

        return result;
    }
}

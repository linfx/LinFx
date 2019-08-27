using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace LinFx.Extensions.MultiTenancy
{
    public class TenantResolver : ITenantResolver
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TenantResolveOptions _options;

        public TenantResolver(IOptions<TenantResolveOptions> options, IServiceProvider serviceProvider)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
        }

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
}
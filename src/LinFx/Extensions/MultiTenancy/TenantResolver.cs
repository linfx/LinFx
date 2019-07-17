using Microsoft.Extensions.DependencyInjection;
using System;

namespace LinFx.Extensions.MultiTenancy
{
    public class TenantResolver : ITenantResolver
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TenantResolveOptions _options;

        public TenantResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TenantResolveResult ResolveTenantIdOrName()
        {
            var result = new TenantResolveResult();

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = new TenantResolveContext(scope.ServiceProvider);
            }

            return result;
        }
    }
}
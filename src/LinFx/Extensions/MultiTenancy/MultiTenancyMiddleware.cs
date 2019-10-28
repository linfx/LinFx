using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LinFx.Extensions.MultiTenancy
{
    public class MultiTenancyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITenantResolver _tenantResolver;
        private readonly ICurrentTenant _currentTenant;
        private readonly ITenantResolveResultAccessor _tenantResolveResultAccessor;

        public MultiTenancyMiddleware(
            RequestDelegate next,
            ITenantResolver tenantResolver,
            ICurrentTenant currentTenant,
            ITenantResolveResultAccessor tenantResolveResultAccessor)
        {
            _next = next;
            _tenantResolver = tenantResolver;
            _currentTenant = currentTenant;
            _tenantResolveResultAccessor = tenantResolveResultAccessor;
        }

        public async Task InvokeAsync(HttpContext httpContext, ITenantStore tenantStore)
        {
            var resolveResult = _tenantResolver.ResolveTenantIdOrName();
            _tenantResolveResultAccessor.Result = resolveResult;

            TenantInfo tenant = null;
            if (resolveResult.TenantIdOrName != null)
            {
                tenant = await tenantStore.FindAsync(resolveResult.TenantIdOrName);
                if (tenant == null)
                {
                    throw new LinFxException("There is no tenant with given tenant id or name: " + resolveResult.TenantIdOrName);
                }
            }

            using (_currentTenant.Change(tenant?.Id, tenant?.Name))
            {
                await _next(httpContext);
            }
        }
    }
}
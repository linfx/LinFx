using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.MultiTenancy;
using Microsoft.AspNetCore.Http;

namespace LinFx.Extensions.AspNetCore.MultiTenancy;

/// <summary>
/// 多租户中间件
/// </summary>
public class MultiTenancyMiddleware(
    ITenantStore tenantStore,
    ITenantResolver tenantResolver,
    ICurrentTenant currentTenant,
    ITenantResolveResultAccessor tenantResolveResultAccessor) : IMiddleware, ITransientDependency
{
    private readonly ITenantStore tenantStore = tenantStore;
    private readonly ITenantResolver _tenantResolver = tenantResolver;
    private readonly ICurrentTenant _currentTenant = currentTenant;
    private readonly ITenantResolveResultAccessor _tenantResolveResultAccessor = tenantResolveResultAccessor;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var resolveResult = _tenantResolver.ResolveTenantIdOrName();
        _tenantResolveResultAccessor.Result = resolveResult;

        TenantInfo? tenant = null;
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
            await next(context);
        }
    }
}

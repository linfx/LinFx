using LinFx.Extensions.MultiTenancy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LinFx.Extensions.AspNetCore.MultiTenancy;

public abstract class HttpTenantResolveContributorBase : ITenantResolveContributor
{
    public abstract string Name { get; }

    public virtual void Resolve(ITenantResolveContext context)
    {
        var httpContext = context.GetHttpContext();
        if (httpContext == null)
            return;

        try
        {
            ResolveFromHttpContext(context, httpContext);
        }
        catch (Exception ex)
        {
            context.ServiceProvider
                .GetRequiredService<ILogger<HttpTenantResolveContributorBase>>()
                .LogWarning(ex.ToString());
        }
    }

    protected virtual void ResolveFromHttpContext(ITenantResolveContext context, HttpContext httpContext)
    {
        var tenantIdOrName = GetTenantIdOrNameFromHttpContextOrNull(context, httpContext);
        if (!string.IsNullOrEmpty(tenantIdOrName))
        {
            context.TenantIdOrName = tenantIdOrName;
        }
    }

    /// <summary>
    /// 获取租户Id
    /// </summary>
    /// <param name="context"></param>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    protected abstract string? GetTenantIdOrNameFromHttpContextOrNull(ITenantResolveContext context, HttpContext httpContext);
}
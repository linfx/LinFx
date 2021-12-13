using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.TenantManagement.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.TenantManagement;

/// <summary>
/// 租户管理模块
/// </summary>
public class TenantManagementModule : Module
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<PermissionOptions>(options =>
        {
            options.DefinitionProviders.Add(typeof(TenantManagementPermissionDefinitionProvider));
        });

        context.Services.AddDbContext<TenantManagementDbContext>(options =>
        {
            options.AddDefaultRepositories();
        });
    }
}

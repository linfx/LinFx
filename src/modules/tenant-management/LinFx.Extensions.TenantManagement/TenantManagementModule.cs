using LinFx.Extensions.Auditing;
using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.Data;
using LinFx.Extensions.EntityFrameworkCore;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.TenantManagement.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.TenantManagement;

/// <summary>
/// 租户管理模块
/// </summary>
[DependsOn(
    typeof(DataModule),
    typeof(MultiTenancyModule),
    typeof(AuditingModule),
    typeof(EntityFrameworkCoreModule)
)]
public class TenantManagementModule : Module
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //context.Services.Configure<PermissionOptions>(options =>
        //{
        //    options.DefinitionProviders.Add(typeof(TenantManagementPermissionDefinitionProvider));
        //});

        context.Services.AddDbContext<TenantManagementDbContext>(options =>
        {
            options.AddDefaultRepositories();
        });
    }
}

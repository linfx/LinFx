using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.TenantManagement;
using LinFx.Extensions.TenantManagement.EntityFrameworkCore;
using System;

namespace Microsoft.Extensions.DependencyInjection;

public static class TenantManagementServiceCollectionExtensions
{
    /// <summary>
    /// 租户管理模块
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static LinFxBuilder AddTenantManagement(this LinFxBuilder builder, Action<MultiTenancyOptions> optionsAction = default)
    {
        builder
            .AddAssembly(typeof(TenantManagementServiceCollectionExtensions).Assembly)
            .AddDbContext<TenantManagementDbContext>(options =>
            {
                options.AddDefaultRepositories();
            });

        builder.Services.Configure<PermissionOptions>(options =>
        {
            options.DefinitionProviders.Add(typeof(TenantManagementPermissionDefinitionProvider));
        });

        return builder;
    }
}

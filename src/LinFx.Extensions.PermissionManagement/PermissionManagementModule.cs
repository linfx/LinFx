﻿using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.Caching;
using LinFx.Extensions.EntityFrameworkCore;
using LinFx.Extensions.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.PermissionManagement;

/// <summary>
/// 权限管理模块(RBAC)
/// </summary>
[DependsOn(
    typeof(CachingModule),
    typeof(EntityFrameworkCoreModule)
)]
public class PermissionManagementModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<PermissionManagementOptions>(options =>
        {
            options.ManagementProviders.Add<UserPermissionManagementProvider>();
            options.ManagementProviders.Add<RolePermissionManagementProvider>();

            //TODO: Can we prevent duplication of permission names without breaking the design and making the system complicated
            options.ProviderPolicies[UserPermissionValueProvider.ProviderName] = "Users.ManagePermissions";
            options.ProviderPolicies[RolePermissionValueProvider.ProviderName] = "Roles.ManagePermissions";
        });
    }
}

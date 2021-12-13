using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.PermissionManagement.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace LinFx.Extensions.PermissionManagement
{
    public class PermissionManagementModule : Module
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            Configure<PermissionManagementOptions>(options =>
            {
                options.ManagementProviders.Add<UserPermissionManagementProvider>();
                options.ManagementProviders.Add<RolePermissionManagementProvider>();

                //TODO: Can we prevent duplication of permission names without breaking the design and making the system complicated
                options.ProviderPolicies[UserPermissionValueProvider.ProviderName] = "Users.ManagePermissions";
                options.ProviderPolicies[RolePermissionValueProvider.ProviderName] = "Roles.ManagePermissions";
            });

            //builder.Services
            //    .AddLocalization(o =>
            //    {
            //        o.ResourcesPath = "Resources";
            //    });

            //builder
            //    .Services
            //    .AddDbContext<PermissionManagementDbContext>(options =>
            //    {
            //        options.AddDefaultRepositories();
            //    });
        }
    }
}

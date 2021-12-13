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
        public LinFxBuilder AddPermissionManagement(LinFxBuilder builder)
        {
            builder.Services
                .AddLocalization(o =>
                {
                    o.ResourcesPath = "Resources";
                });

            builder
                .Services
                .AddDbContext<PermissionManagementDbContext>(options =>
                {
                    options.AddDefaultRepositories();
                });

            builder.Services.Configure<PermissionManagementOptions>(options =>
            {
                options.ManagementProviders.Add<UserPermissionManagementProvider>();
                options.ManagementProviders.Add<RolePermissionManagementProvider>();

                //TODO: Can we prevent duplication of permission names without breaking the design and making the system complicated
                options.ProviderPolicies[UserPermissionValueProvider.ProviderName] = "Users.ManagePermissions";
                options.ProviderPolicies[RolePermissionValueProvider.ProviderName] = "Roles.ManagePermissions";
            });

            AutoAddDefinitionProviders(builder.Services);

            return builder;
        }

        private void AutoAddDefinitionProviders(IServiceCollection services)
        {
            var definitionProviders = new List<Type>();

            services.OnRegistred(context =>
            {
                if (typeof(IPermissionDefinitionProvider).IsAssignableFrom(context.ImplementationType))
                {
                    definitionProviders.Add(context.ImplementationType);
                }
            });

            services.Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.AddIfNotContains(definitionProviders);
            });
        }
    }
}

using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.PermissionManagement;
using LinFx.Extensions.PermissionManagement.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PermissionManagementServiceCollectionExtensions
    {
        public static LinFxBuilder AddPermissionManagement(this LinFxBuilder builder)
        {
            builder.Services
                .AddLocalization(o =>
                {
                    o.ResourcesPath = "Resources";
                });

            builder
                .AddAssembly(typeof(PermissionManagementServiceCollectionExtensions).Assembly)
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

        private static void AutoAddDefinitionProviders(IServiceCollection services)
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

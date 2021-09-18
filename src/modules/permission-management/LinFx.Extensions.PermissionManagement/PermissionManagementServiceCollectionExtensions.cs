using LinFx.Extensions.Authorization.Permissions;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PermissionManagementServiceCollectionExtensions
    {
        public static LinFxBuilder AddPermissionManagement(this LinFxBuilder context)
        {
            context
                .AddAssembly(typeof(PermissionManagementServiceCollectionExtensions).Assembly);

            AutoAddDefinitionProviders(context.Services);

            return context;
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

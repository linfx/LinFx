using LinFx.Security.Authorization;
using LinFx.Security.Authorization.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 授权
    /// </summary>
    public static class AuthorizationServiceCollectionExtensions
    {
        /// <summary>
        /// Adds authorization services to the specified <see cref="LinFxBuilder" />. 
        /// </summary>
        /// <param name="builder">The current <see cref="LinFxBuilder" /> instance. </param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static LinFxBuilder AddAuthorization(this LinFxBuilder builder)
        {
            builder.Services.OnRegistred(AuthorizationInterceptorRegistrar.RegisterIfNeeded);
            AutoAddDefinitionProviders(builder.Services);

            builder.Services
                .AddAuthorizationCore()
                .AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();

            builder.Services.Configure<PermissionOptions>(options =>
            {
                options.ValueProviders.Add<UserPermissionValueProvider>();
                options.ValueProviders.Add<RolePermissionValueProvider>();
                options.ValueProviders.Add<ClientPermissionValueProvider>();
            });

            return builder;
        }

        private static void AutoAddDefinitionProviders(IServiceCollection services)
        {
            var definitionProviders = new List<Type>();

            services.OnRegistred(context =>
            {
                if (typeof(IPermissionDefinitionProvider).IsAssignableFrom(context.ImplementationType))
                    definitionProviders.Add(context.ImplementationType);
            });

            services.Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.AddIfNotContains(definitionProviders);
            });
        }
    }
}

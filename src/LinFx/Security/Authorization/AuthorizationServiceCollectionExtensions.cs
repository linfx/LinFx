using LinFx;
using LinFx.Security.Authorization;
using LinFx.Security.Authorization.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AuthorizationOptions = LinFx.Security.Authorization.AuthorizationOptions;

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
        /// <param name="configure">An action delegate to configure the provided <see cref="AuthorizationOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        [Obsolete]
        public static LinFxBuilder AddAuthorization(
            [NotNull] this LinFxBuilder builder, 
            [NotNull] Action<AuthorizationOptions> configure)
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(configure, nameof(configure));


            var options = new AuthorizationOptions();
            configure?.Invoke(options);

            builder.Services.Configure(configure);
            builder.Services.Configure<PermissionOptions>(o =>
            {
                options.Permissions.DefinitionProviders.ToList().ForEach(item =>
                {
                    o.DefinitionProviders.Add(item);
                });

                options.Permissions.ValueProviders.ToList().ForEach(item =>
                {
                    o.ValueProviders.Add(item);
                });
            });


            //builder.Services.AddAuthorization();
            //builder.Services.AddAuthorizationCore();
            builder.Services.AddSingleton<IPermissionChecker, PermissionChecker>();
            builder.Services.AddSingleton<IPermissionDefinitionContext, PermissionDefinitionContext>();
            builder.Services.AddSingleton<IPermissionDefinitionManager, PermissionDefinitionManager>();
            //fx.Services.TryAdd(ServiceDescriptor.Transient<IAuthorizationPolicyProvider, LinFx.Security.Authorization.DefaultAuthorizationPolicyProvider>());
            builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

            return builder;
        }

        public static IServiceCollection AddLinFxAuthorization(this IServiceCollection services, Action<AuthorizationOptions> configure)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(configure, nameof(configure));


            var options = new AuthorizationOptions();
            configure?.Invoke(options);

            services.Configure(configure);
            services.Configure<PermissionOptions>(o =>
            {
                options.Permissions.DefinitionProviders.ToList().ForEach(item =>
                {
                    o.DefinitionProviders.Add(item);
                });

                options.Permissions.ValueProviders.ToList().ForEach(item =>
                {
                    o.ValueProviders.Add(item);
                });
            });


            //services.AddAuthorization();

            services.AddSingleton<IPermissionChecker, PermissionChecker>();
            services.AddSingleton<IPermissionDefinitionContext, PermissionDefinitionContext>();
            services.AddSingleton<IPermissionDefinitionManager, PermissionDefinitionManager>();

            //fx.Services.TryAdd(ServiceDescriptor.Transient<IAuthorizationPolicyProvider, LinFx.Security.Authorization.DefaultAuthorizationPolicyProvider>());
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

            return services;
        }
    }
}

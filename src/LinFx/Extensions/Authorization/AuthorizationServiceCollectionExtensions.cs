using LinFx.Extensions.Authorization.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using LinFx.Extensions.Authorization;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 授权
    /// </summary>
    public static class AuthorizationServiceCollectionExtensions
    {
        /// <summary>
        /// 授权
        /// Adds authorization services to the specified <see cref="LinFxBuilder" />. 
        /// </summary>
        /// <param name="context">The current <see cref="LinFxBuilder" /> instance. </param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static LinFxBuilder AddAuthorization(this LinFxBuilder context)
        {
            context.Services.OnRegistred(AuthorizationInterceptorRegistrar.RegisterIfNeeded);
            AutoAddDefinitionProviders(context.Services);

            // 替换掉 ASP.NET Core 提供的权限处理器
            context.Services
                .AddAuthorizationCore()
                .AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();

            // 添加内置的一些权限值检查
            context.Services.Configure<PermissionOptions>(options =>
            {
                options.ValueProviders.Add<UserPermissionValueProvider>();
                options.ValueProviders.Add<RolePermissionValueProvider>();
                options.ValueProviders.Add<ClientPermissionValueProvider>();
            });

            return context;
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

using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.Modularity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LinFx.Extensions.Authorization;

/// <summary>
/// 授权模块
/// </summary>
public class AuthorizationModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        // 授权拦截器注册
        services.OnRegistered(AuthorizationInterceptorRegistrar.RegisterIfNeeded);

        // 注册提供者
        AutoAddDefinitionProviders(services);

        // 注册认证授权服务。
        services.AddAuthorizationCore();

        // 替换掉 ASP.NET Core 提供的权限处理器。
        services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
        services
            .AddTransient<AuthorizationInterceptor>()
            .AddTransient<MethodInvocationAuthorizationService>()
            .TryAddTransient<DefaultAuthorizationPolicyProvider>();

        // 添加内置的一些权限值检查。
        services.Configure<PermissionOptions>(options =>
        {
            options.ValueProviders.Add<UserPermissionValueProvider>();
            options.ValueProviders.Add<RolePermissionValueProvider>();
            options.ValueProviders.Add<ClientPermissionValueProvider>();
        });
    }

    private static void AutoAddDefinitionProviders(IServiceCollection services)
    {
        var definitionProviders = new List<Type>();

        services.OnRegistered(context =>
        {
            if (typeof(IPermissionDefinitionProvider).IsAssignableFrom(context.ImplementationType))
            {
                definitionProviders.Add(context.ImplementationType);
            }
        });

        // 将获取到的 Provider 传递给 PermissionOptions 。
        services.Configure<PermissionOptions>(options =>
        {
            options.DefinitionProviders.AddIfNotContains(definitionProviders);
        });
    }
}

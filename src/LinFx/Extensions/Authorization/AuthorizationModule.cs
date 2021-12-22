using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.Modularity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;

namespace LinFx.Extensions.Authorization;

/// <summary>
/// 授权模块
/// </summary>
[DependsOn(
//typeof(AbpAuthorizationAbstractionsModule),
//typeof(AbpSecurityModule),
//typeof(AbpLocalizationModule)
)]
public class AuthorizationModule : Module
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistred(AuthorizationInterceptorRegistrar.RegisterIfNeeded);
        AutoAddDefinitionProviders(context.Services);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 注册认证授权服务。
        context.Services.AddAuthorizationCore();

        // 替换掉 ASP.NET Core 提供的权限处理器。
        context.Services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
        //context.Services.AddSingleton<IAuthorizationHandler, PermissionsRequirementHandler>();

        context.Services.TryAddTransient<DefaultAuthorizationPolicyProvider>();

        // 添加内置的一些权限值检查。
        context.Services.Configure<PermissionOptions>(options =>
        {
            options.ValueProviders.Add<UserPermissionValueProvider>();
            options.ValueProviders.Add<RolePermissionValueProvider>();
            options.ValueProviders.Add<ClientPermissionValueProvider>();
        });

        //Configure<AbpVirtualFileSystemOptions>(options =>
        //{
        //    options.FileSets.AddEmbedded<AbpAuthorizationResource>();
        //});

        //Configure<LocalizationOptions>(options =>
        //{
        //    options.Resources
        //        .Add<AuthorizationResource>("en")
        //        .AddVirtualJson("/Volo/Abp/Authorization/Localization");
        //});

        //Configure<AbpExceptionLocalizationOptions>(options =>
        //{
        //    options.MapCodeNamespace("Volo.Authorization", typeof(AbpAuthorizationResource));
        //});
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

        // 将获取到的 Provider 传递给 PermissionOptions 。
        services.Configure<PermissionOptions>(options =>
        {
            options.DefinitionProviders.AddIfNotContains(definitionProviders);
        });
    }
}

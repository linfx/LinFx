﻿using LinFx.Application;
using LinFx.Extensions.AspNetCore.Auditing;
using LinFx.Extensions.Auditing;
using LinFx.Extensions.Authorization;
using LinFx.Extensions.Modularity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.AspNetCore;

[DependsOn(
    typeof(AuditingModule),
    //typeof(SecurityModule),
    //typeof(AbpVirtualFileSystemModule),
    typeof(UnitOfWorkModule),
    //typeof(AbpHttpModule),
    typeof(AuthorizationModule)
    //typeof(AbpValidationModule),
    //typeof(AbpExceptionHandlingModule)
    )]
public class AspNetCoreModule : Module
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AuditingOptions>(options =>
        {
            options.Contributors.Add(new AspNetCoreAuditLogContributor());
        });

        //Configure<StaticFileOptions>(options =>
        //{
        //    options.ContentTypeProvider = context.Services.GetRequiredService<AbpFileExtensionContentTypeProvider>();
        //});

        AddAspNetServices(context.Services);
        context.Services.AddObjectAccessor<IApplicationBuilder>();
        //context.Services.AddAbpDynamicOptions<RequestLocalizationOptions, AbpRequestLocalizationOptionsManager>();
    }

    private static void AddAspNetServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var environment = context.GetEnvironmentOrNull();
        if (environment != null)
        {
            //environment.WebRootFileProvider =
            //    new CompositeFileProvider(
            //        context.GetEnvironment().WebRootFileProvider,
            //        context.ServiceProvider.GetRequiredService<IWebContentFileProvider>()
            //    );
        }
    }
}
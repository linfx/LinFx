using LinFx.Application;
using LinFx.Extensions.AspNetCore.Auditing;
using LinFx.Extensions.Auditing;
using LinFx.Extensions.Authorization;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.Uow;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.AspNetCore;

/// <summary>
/// AspNetCore 模块
/// </summary>
[DependsOn(
    typeof(AuditingModule),
    //typeof(SecurityModule),
    //typeof(VirtualFileSystemModule),
    typeof(UnitOfWorkModule),
    //typeof(HttpModule),
    typeof(AuthorizationModule)
    //typeof(ValidationModule),
    //typeof(ExceptionHandlingModule)
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

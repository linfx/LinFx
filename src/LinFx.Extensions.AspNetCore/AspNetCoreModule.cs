using LinFx.Extensions.Modularity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.AspNetCore;

/// <summary>
/// AspNetCore 模块
/// </summary>
public class AspNetCoreModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        //services.Configure<AuditingOptions>(options =>
        //{
        //    options.Contributors.Add(new AspNetCoreAuditLogContributor());
        //});

        //Configure<StaticFileOptions>(options =>
        //{
        //    options.ContentTypeProvider = context.Services.GetRequiredService<AbpFileExtensionContentTypeProvider>();
        //});

        services
            .AddHttpContextAccessor()
            .AddObjectAccessor<IApplicationBuilder>();

        //context.Services.AddAbpDynamicOptions<RequestLocalizationOptions, AbpRequestLocalizationOptionsManager>();

        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
        });
    }
}

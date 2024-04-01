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
        services
            .AddHttpContextAccessor()
            .AddObjectAccessor<IApplicationBuilder>();

        services
            .AddLocalization()
            .AddRouting(options => { options.LowercaseUrls = true; });
    }
}

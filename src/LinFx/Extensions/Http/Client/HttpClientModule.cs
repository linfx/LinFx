using LinFx.Extensions.DynamicProxy;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Http.Client;

[DependsOn(
    //typeof(HttpModule),
    typeof(CastleCoreModule),
    typeof(ThreadingModule),
    typeof(MultiTenancyModule)
    //typeof(ValidationModule),
    //typeof(ExceptionHandlingModule)
)]
public class HttpClientModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        var configuration = services.GetConfiguration();
        //Configure<RemoteServiceOptions>(configuration);

        //context.Services.AddTransient(typeof(DynamicHttpProxyInterceptorClientProxy<>));
    }
}

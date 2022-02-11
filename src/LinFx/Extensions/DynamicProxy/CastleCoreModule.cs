using LinFx.Extensions.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.DynamicProxy;

public class CastleCoreModule : Module
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient(typeof(AsyncDeterminationInterceptor<>));
    }
}

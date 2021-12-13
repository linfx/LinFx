using LinFx.Extensions.Modularity;
using LinFx.Extensions.Uow;

namespace Microsoft.Extensions.DependencyInjection
{
    public class UnitOfWorkModule : Module
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.OnRegistred(UnitOfWorkInterceptorRegistrar.RegisterIfNeeded);
        }
    }
}

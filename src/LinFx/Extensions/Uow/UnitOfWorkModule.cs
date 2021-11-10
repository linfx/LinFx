using LinFx.Extensions.Modules;
using LinFx.Extensions.Uow;

namespace Microsoft.Extensions.DependencyInjection
{
    public class UnitOfWorkModule : Module
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.OnRegistred(UnitOfWorkInterceptorRegistrar.RegisterIfNeeded);
        }
    }
}

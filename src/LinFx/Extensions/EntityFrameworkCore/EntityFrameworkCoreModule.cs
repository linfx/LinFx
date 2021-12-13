using LinFx.Extensions.EntityFrameworkCore.Uow;
using LinFx.Extensions.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LinFx.Extensions.EntityFrameworkCore;

public class EntityFrameworkCoreModule : Module
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddTransient(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));
    }
}

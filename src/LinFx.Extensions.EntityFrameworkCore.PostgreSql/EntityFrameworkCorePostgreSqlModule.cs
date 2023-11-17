using LinFx.Extensions.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.EntityFrameworkCore.PostgreSql;

[DependsOn(
    typeof(EntityFrameworkCoreModule)
)]
public class EntityFrameworkCorePostgreSqlModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}

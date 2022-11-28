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

        //Configure<AbpSequentialGuidGeneratorOptions>(options =>
        //{
        //    if (options.DefaultSequentialGuidType == null)
        //    {
        //        options.DefaultSequentialGuidType = SequentialGuidType.SequentialAsString;
        //    }
        //});

        //context.Services.AddTransient(typeof(IPostgreSqlDbContextEventOutbox<>), typeof(PostgreSqlDbContextEventOutbox<>));
        //context.Services.AddTransient(typeof(IPostgreSqlDbContextEventInbox<>), typeof(PostgreSqlDbContextEventInbox<>));
    }
}

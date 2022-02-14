using LinFx.Extensions.Modularity;

namespace LinFx.Extensions.EntityFrameworkCore.PostgreSql;

[DependsOn(
    typeof(EntityFrameworkCoreModule)
)]
public class EntityFrameworkCorePostgreSqlModule : Module
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
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

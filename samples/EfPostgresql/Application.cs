using LinFx.Extensions.Autofac;
using LinFx.Extensions.EntityFrameworkCore.PostgreSql;
using LinFx.Extensions.EventBus.Kafka;
using LinFx.Extensions.Modularity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EfPostgresql;

[DependsOn(
    typeof(AutofacModule),
    typeof(EventBusKafkaModule),
    typeof(EntityFrameworkCorePostgreSqlModule)
)]
class Application : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        var configuration = BuildConfiguration();
        services.ReplaceConfiguration(configuration);

        services
            .AddHostedService<Class1>()
            .AddHostedService<Services>();

        services.AddDbContext<MyDbContext>(options =>
        {
            /* Remove "includeAllEntities: true" to create
            * default repositories only for aggregate roots */
            //options.AddDefaultRepositories(includeAllEntities: true);
            options.UseNpgsql("server=10.0.1.222;port=15433;database=db5;username=postgres;password=123456;", o => o.UseNetTopologySuite());
            //options.UseNetTopologySuite();
        });

        services.Configure<LoggerFilterOptions>(config =>
        {
            //config.MinLevel = LogLevel.Warning;
        });
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
    }
}

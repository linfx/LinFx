using LinFx.Extensions.MongoDB;
using LinFx.Extensions.MongoDB.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class MongoDbServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDbContext<TMongoDbContext>(this IServiceCollection services, Action<IMongoDbContextRegistrationOptionsBuilder> optionsBuilder = default) //Created overload instead of default parameter
        where TMongoDbContext : MongoDbContext
    {
        var options = new MongoDbContextRegistrationOptions(typeof(TMongoDbContext), services);

        var replacedDbContextTypes = typeof(TMongoDbContext).GetCustomAttributes<ReplaceDbContextAttribute>(true)
            .SelectMany(x => x.ReplacedDbContextTypes).ToList();

        foreach (var dbContextType in replacedDbContextTypes)
        {
            options.ReplaceDbContext(dbContextType);
        }

        optionsBuilder?.Invoke(options);

        foreach (var entry in options.ReplacedDbContextTypes)
        {
            var originalDbContextType = entry.Key;
            var targetDbContextType = entry.Value ?? typeof(TMongoDbContext);

            services.Replace(
                ServiceDescriptor.Transient(
                    originalDbContextType,
                    sp => sp.GetRequiredService(targetDbContextType)
                )
            );

            services.Configure<MongoDbContextOptions>(opts =>
            {
                opts.DbContextReplacements[originalDbContextType] = targetDbContextType;
            });
        }

        new MongoDbRepositoryRegistrar(options).AddRepositories();

        return services;
    }
}

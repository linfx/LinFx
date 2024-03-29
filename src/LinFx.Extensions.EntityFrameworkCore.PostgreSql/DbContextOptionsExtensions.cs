using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace LinFx.Extensions.EntityFrameworkCore;

public static class DbContextOptionsExtensions
{
    public static void UseNpgsql([NotNull] this EfDbContextOptions options, [CanBeNull] Action<NpgsqlDbContextOptionsBuilder> optionsAction)
    {
        options.Configure(context =>
        {
            context.UseNpgsql(optionsAction);
        });
    }

    public static void UseNpgsql<TDbContext>([NotNull] this EfDbContextOptions options, [CanBeNull] Action<NpgsqlDbContextOptionsBuilder> optionsAction) where TDbContext : DbContext
    {
        options.Configure<TDbContext>(context =>
        {
            context.UseNpgsql(optionsAction);
        });
    }
}

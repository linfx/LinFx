using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using System;

namespace LinFx.Extensions.EntityFrameworkCore;

public static class DbContextOptionsExtensions
{
    public static void UseNpgsql(
        [NotNull] this EfDbContextOptions options,
        [CanBeNull] Action<NpgsqlDbContextOptionsBuilder> optionsAction = null)
    {
        options.Configure(context =>
        {
            context.UseNpgsql(optionsAction);
        });
    }

    public static void UseNpgsql<TDbContext>(
        [NotNull] this EfDbContextOptions options,
        [CanBeNull] Action<NpgsqlDbContextOptionsBuilder> optionsAction = null)
        where TDbContext : DbContext
    {
        options.Configure<TDbContext>(context =>
        {
            context.UseNpgsql(optionsAction);
        });
    }
}

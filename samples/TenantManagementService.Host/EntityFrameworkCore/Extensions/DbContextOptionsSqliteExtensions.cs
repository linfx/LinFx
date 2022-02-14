using System;
using JetBrains.Annotations;
using LinFx.Extensions.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace TenantManagementService.EntityFrameworkCore.Extensions;

public static class DbContextOptionsSqliteExtensions
{
    public static void UseSqlite(
        [NotNull] this EfDbContextOptions options,
        [CanBeNull] Action<SqliteDbContextOptionsBuilder> sqliteOptionsAction = null)
    {
        options.Configure(context =>
        {
            context.UseSqlite(sqliteOptionsAction);
        });
    }

    public static void UseSqlite<TDbContext>(
        [NotNull] this EfDbContextOptions options,
        [CanBeNull] Action<SqliteDbContextOptionsBuilder> sqliteOptionsAction = null)
        where TDbContext : DbContext
    {
        options.Configure<TDbContext>(context =>
        {
            context.UseSqlite(sqliteOptionsAction);
        });
    }
}

using JetBrains.Annotations;
using LinFx.Extensions.EntityFrameworkCore.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LinFx.Extensions.EntityFrameworkCore;

public static class DbContextConfigurationContextSqliteExtensions
{
    public static DbContextOptionsBuilder UseSqlite(
        [NotNull] this DbContextConfigurationContext context,
        [CanBeNull] Action<SqliteDbContextOptionsBuilder>? sqliteOptionsAction = default)
    {
        if (context.ExistingConnection != null)
        {
            return context.DbContextOptions.UseSqlite(context.ExistingConnection, optionsBuilder =>
            {
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                sqliteOptionsAction?.Invoke(optionsBuilder);
            });
        }
        else
        {
            return context.DbContextOptions.UseSqlite(context.ConnectionString, optionsBuilder =>
            {
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                sqliteOptionsAction?.Invoke(optionsBuilder);
            });
        }
    }
}

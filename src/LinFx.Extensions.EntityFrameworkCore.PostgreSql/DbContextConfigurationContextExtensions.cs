using JetBrains.Annotations;
using LinFx.Extensions.EntityFrameworkCore.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace LinFx.Extensions.EntityFrameworkCore;

public static class DbContextConfigurationContextExtensions
{
    public static DbContextOptionsBuilder UseNpgsql([NotNull] this DbContextConfigurationContext context, [CanBeNull] Action<NpgsqlDbContextOptionsBuilder>? optionsAction = null)
    {
        if (context.ExistingConnection != null)
        {
            return context.DbContextOptions.UseNpgsql(context.ExistingConnection, optionsBuilder =>
            {
                // 拆分查询
                //optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                optionsAction?.Invoke(optionsBuilder);
            });
        }
        else
        {
            return context.DbContextOptions.UseNpgsql(context.ConnectionString, optionsBuilder =>
            {
                // 拆分查询
                //optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                optionsAction?.Invoke(optionsBuilder);
            });
        }
    }
}

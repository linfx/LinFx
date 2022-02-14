using JetBrains.Annotations;
using LinFx.Extensions.EntityFrameworkCore.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using System;

namespace LinFx.Extensions.EntityFrameworkCore;

public static class DbContextConfigurationContextExtensions
{
    public static DbContextOptionsBuilder UseNpgsql(
        [NotNull] this DbContextConfigurationContext context,
        [CanBeNull] Action<NpgsqlDbContextOptionsBuilder> optionsAction = null)
    {
        if (context.ExistingConnection != null)
        {
            return context.DbContextOptions.UseNpgsql(context.ExistingConnection, optionsBuilder =>
            {
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                optionsAction?.Invoke(optionsBuilder);
            });
        }
        else
        {
            return context.DbContextOptions.UseNpgsql(context.ConnectionString, optionsBuilder =>
            {
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                optionsAction?.Invoke(optionsBuilder);
            });
        }
    }
}

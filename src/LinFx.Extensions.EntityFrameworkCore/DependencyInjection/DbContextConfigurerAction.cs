using System.Diagnostics.CodeAnalysis;
using LinFx.Utils;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.EntityFrameworkCore.DependencyInjection;

public class DbContextConfigurerAction : IDbContextConfigurer
{
    [NotNull]
    public Action<DbContextConfigurationContext> Action { get; }

    public DbContextConfigurerAction([NotNull] Action<DbContextConfigurationContext> action)
    {
        Check.NotNull(action, nameof(action));

        Action = action;
    }

    public void Configure(DbContextConfigurationContext context) => Action.Invoke(context);
}

public class DbContextConfigurerAction<TDbContext> : DbContextConfigurerAction
    where TDbContext : DbContext
{
    public DbContextConfigurerAction([NotNull] Action<DbContextConfigurationContext> action)
        : base(action)
    { }
}

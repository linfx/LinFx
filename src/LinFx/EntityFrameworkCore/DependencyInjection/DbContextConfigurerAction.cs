using System;
using JetBrains.Annotations;

namespace LinFx.EntityFrameworkCore.DependencyInjection
{
    public class DbContextConfigurerAction : IDbContextConfigurer
    {
        [NotNull]
        public Action<DbContextConfigurationContext> Action { get; }

        public DbContextConfigurerAction([NotNull] Action<DbContextConfigurationContext> action)
        {
            Check.NotNull(action, nameof(action));

            Action = action;
        }

        public void Configure(DbContextConfigurationContext context)
        {
            Action.Invoke(context);
        }
    }

    public class DbContextConfigurerAction<TDbContext> : DbContextConfigurerAction
        where TDbContext : EfCodeDbContext
    {
        public DbContextConfigurerAction([NotNull] Action<DbContextConfigurationContext> action)
            : base(action)
        {
        }
    }
}
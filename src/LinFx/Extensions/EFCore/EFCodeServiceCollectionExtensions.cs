using LinFx;
using Microsoft.EntityFrameworkCore;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EFCodeServiceCollectionExtensions
    {
        public static ILinFxBuilder AddDbContext<TContext>([NotNull] this ILinFxBuilder builder, 
            [CanBeNull] Action<DbContextOptionsBuilder> optionsAction = null, 
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped, 
            ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TContext : DbContext
        {
            builder.Services.AddDbContext<TContext>(optionsAction, contextLifetime, optionsLifetime);

            return builder;
        }
    }
}

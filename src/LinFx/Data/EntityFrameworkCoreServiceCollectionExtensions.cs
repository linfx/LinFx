using LinFx;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityFrameworkCoreServiceCollectionExtensions
    {
        public static LinFxBuilder AddDbContext<TContext>([NotNull] this LinFxBuilder builder, 
            [CanBeNull] Action<DbContextOptionsBuilder> optionsAction = null, 
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped, 
            ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TContext : DbContext
        {
            builder.Services.AddDbContext<TContext>(optionsAction, contextLifetime, optionsLifetime);
            return builder;
        }
    }
}

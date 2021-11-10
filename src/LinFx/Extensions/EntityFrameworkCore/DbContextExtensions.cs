using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinFx.Extensions.EntityFrameworkCore
{
    public static class DbContextExtensions
    {
        public static void Update<TEntity, TEntityNew>(this EfCoreDbContext context, TEntity entity, Expression<Func<TEntity, TEntityNew>> expression) where TEntity : class
        {
            context.Update(entity);
            var entry = context.Entry(entity);
            entry.State = EntityState.Unchanged;
            foreach (var propertyInfo in expression.GetPropertyAccessList())
            {
                if (!string.IsNullOrEmpty(propertyInfo.Name))
                    entry.Property(propertyInfo.Name).IsModified = true;
            }
        }

        public static void RegisterCustomMappings(this EfCoreDbContext context, ModelBuilder modelBuilder, IEnumerable<Type> typeToRegisters)
        {
            var customModelBuilderTypes = typeToRegisters.Where(x => typeof(ICustomModelBuilder).IsAssignableFrom(x));
            foreach (var builderType in customModelBuilderTypes)
            {
                if (builderType != null && builderType != typeof(ICustomModelBuilder))
                {
                    var builder = (ICustomModelBuilder)Activator.CreateInstance(builderType);
                    builder.Build(modelBuilder);
                }
            }
        }

        public static bool HasRelationalTransactionManager(this DbContext dbContext)
        {
            return dbContext.Database.GetService<IDbContextTransactionManager>() is IRelationalTransactionManager;
        }
    }
}

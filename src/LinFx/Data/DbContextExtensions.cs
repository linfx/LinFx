using LinFx.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinFx.Data
{
    public static class DbContextExtensions
    {
        public static void Modity<TEntity, TEntityNew>(this DbContext context, TEntity entity, Expression<Func<TEntity, TEntityNew>> expression) where TEntity : class
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

        internal static void RegisterCustomMappings(this DbContext context, ModelBuilder modelBuilder, IEnumerable<Type> typeToRegisters)
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
    }
}

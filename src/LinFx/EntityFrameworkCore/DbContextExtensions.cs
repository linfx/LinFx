using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Linq.Expressions;
using DbContext = LinFx.Extensions.EntityFrameworkCore.DbContext;

namespace LinFx.EntityFrameworkCore
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
    }
}

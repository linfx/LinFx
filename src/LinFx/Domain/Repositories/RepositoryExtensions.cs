using LinFx.Domain.Entities;
using System.Linq.Expressions;

namespace LinFx.Domain.Repositories
{
    public static class RepositoryExtensions
    {
        public static IQueryable<TEntity> Where<TEntity>(this IReadOnlyRepository<TEntity> source, Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntity
        {
            var queryable = source.GetQueryableAsync().Result;
            return queryable.Where(predicate);
        }

        public static IQueryable<TResult> Select<TEntity, TResult>(this IReadOnlyRepository<TEntity> source, Expression<Func<TEntity, TResult>> selector) where TEntity : class, IEntity
        {
            var queryable = source.GetQueryableAsync().Result;
            return queryable.Select(selector);
        }

        //public static IIncludableQueryable<TEntity, TProperty> Include<TEntity, TProperty>([NotNull] this IReadOnlyRepository<TEntity> source, [NotNull] Expression<Func<TEntity, TProperty>> navigationPropertyPath)
        //    where TEntity : class, IEntity
        //{
        //    var queryable = source.GetQueryableAsync().Result;
        //    return queryable.Include(navigationPropertyPath);
        //}
    }
}

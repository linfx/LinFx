using LinFx.Data.Linq;
using LinFx.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Data.Linq
{
    public static class RepositoryExtensions
    {
        public static IIncludableQueryable<TEntity, TProperty> Include<TEntity, TProperty>(this IRepository<TEntity> repository, Expression<Func<TEntity, TProperty>> navigationPropertyPath) where TEntity : class, IEntity
        {
            return repository.Query().Include(navigationPropertyPath);
        }

        public static IQueryable<TEntity> Where<TEntity, TKey>(this IRepository<TEntity, TKey> repository, Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntity
        {
            return repository.Query().Where(predicate);
        }

        public static IQueryable<TResult> Select<TEntity, TResult>(this IRepository<TEntity> repository, Expression<Func<TEntity, TResult>> selector) where TEntity : class, IEntity
        {
            return repository.Query().Select(selector);
        }

        [Obsolete]
        public static Task<TEntity> FirstOrDefaultAsync<TEntity, TKey>(this IRepository<TEntity, TKey> repository, TKey id, CancellationToken cancellationToken = default) where TEntity : class, IEntity<TKey>
        {
            return repository.FirstOrDefaultAsync(c => c.Id.Equals(id), cancellationToken);
        }

        public static Task<TEntity> FirstOrDefaultAsync<TEntity, TKey>(this IRepository<TEntity, TKey> repository, CancellationToken cancellationToken = default) where TEntity : class, IEntity
        {
            return repository.Query().FirstOrDefaultAsync(cancellationToken);
        }

        public static Task<TEntity> FirstOrDefaultAsync<TEntity, TKey>(this IRepository<TEntity, TKey> repository, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) where TEntity : class, IEntity
        {
            return repository.Query().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public static Task<bool> AnyAsync<TEntity, TKey>(this IRepository<TEntity, TKey> repository, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return repository.Query().AnyAsync(predicate, cancellationToken);
        }

        public static Task<int> CountAsync<TEntity, TKey>(this IRepository<TEntity, TKey> repository, CancellationToken cancellationToken = default)
        {
            return repository.Query().CountAsync(cancellationToken);
        }

        public static Task<List<TEntity>> ToListAsync<TEntity, TKey>(this IRepository<TEntity, TKey> repository, CancellationToken cancellationToken = default)
        {
            return repository.Query().ToListAsync(cancellationToken);
        }
    }
}

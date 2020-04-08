using LinFx.Data.Abstractions;
using LinFx.Data.Linq;
using LinFx.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Data.Linq
{
    public static class RepositoryExtensions
    {
        public static Task<TEntity> FirstOrDefaultAsync<TEntity, TKey>([NotNull] this IRepository<TEntity, TKey> repository, TKey id, CancellationToken cancellationToken = default) where TEntity : class, IEntity<TKey>
        {
            return repository.FirstOrDefaultAsync(c => c.Id.Equals(id), cancellationToken);
        }

        public static IQueryable<TEntity> Where<TEntity, TKey>([NotNull] this IRepository<TEntity, TKey> repository, Func<TEntity, bool> predicate) where TEntity : class, IEntity
        {
            return repository.Query().Where(predicate).AsQueryable();
        }

        public static Task<TEntity> FirstOrDefaultAsync<TEntity, TKey>([NotNull] this IRepository<TEntity, TKey> repository, CancellationToken cancellationToken = default) where TEntity : class, IEntity
        {
            return repository.Query().FirstOrDefaultAsync(cancellationToken);
        }

        public static Task<TEntity> FirstOrDefaultAsync<TEntity, TKey>([NotNull] this IRepository<TEntity, TKey> repository, [NotNull] Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) where TEntity : class, IEntity
        {
            return repository.Query().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public static Task<bool> AnyAsync<TEntity, TKey>([NotNull] this IRepository<TEntity, TKey> repository, [NotNull] Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return repository.Query().AnyAsync(predicate, cancellationToken);
        }

        public static Task<List<TEntity>> ToListAsync<TEntity, TKey>([NotNull] this IRepository<TEntity, TKey> repository, CancellationToken cancellationToken = default)
        {
            return repository.Query().ToListAsync(cancellationToken);
        }
    }
}

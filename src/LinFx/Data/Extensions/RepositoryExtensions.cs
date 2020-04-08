using LinFx.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Data.Extensions
{
    public static class RepositoryExtensions
    {
        public static IQueryable<TEntity> Where<TEntity>([NotNull] this IRepository<TEntity> repository, Func<TEntity, bool> predicate) where TEntity : class, IEntity
        {
            return repository.Query().Where(predicate).AsQueryable();
        }

        public static Task<TEntity> FirstOrDefaultAsync<TEntity>([NotNull] this IRepository<TEntity> repository, CancellationToken cancellationToken = default) where TEntity : class, IEntity
        {
            return repository.Query().FirstOrDefaultAsync(cancellationToken);
        }

        public static Task<TEntity> FirstOrDefaultAsync<TEntity>([NotNull] this IRepository<TEntity> repository, [NotNull] Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) where TEntity : class, IEntity
        {
            return repository.Query().FirstOrDefaultAsync(predicate, cancellationToken);
        }
    }
}

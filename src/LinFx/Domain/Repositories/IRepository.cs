using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinFx.Domain.Repositories
{
    /// <summary>
    /// Just to mark a class as repository.
    /// </summary>
    public interface IRepository
    {
    }

    public interface IRepository<TEntity> : IRepository
    {
    }

    public interface IRepository<TEntity, TKey> : IRepository<TEntity>
    {
        void Add(TEntity entity);

        void AddRange(IList<TEntity> entities);

        void Remove(TEntity entity);

        IDbContextTransaction BeginTransaction();

        Task<int> SaveChangesAsync();

        IQueryable<TEntity> Query();
    }
}

using LinFx.Domain.Entities;
using LinFx.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LinFx.EntityFrameworkCore.Repositories
{
    public interface IEfCoreRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        Task<DbContext> GetDbContextAsync();

        Task<DbSet<TEntity>> GetDbSetAsync();
    }

    public interface IEfCoreRepository<TEntity, TKey> : IEfCoreRepository<TEntity>, IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
    }
}

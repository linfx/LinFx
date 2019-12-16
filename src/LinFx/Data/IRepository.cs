using LinFx.Domain.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LinFx.Data
{
    /// <summary>
    /// Just to mark a class as repository.
    /// </summary>
    public interface IRepository
    {
    }

    public interface IRepository<TEntity> : IRepository<TEntity, long> where TEntity : class, IEntity
    {
    }

    public interface IRepository<TEntity, TKey> : IRepository
    {
        void Add(TEntity entity);

        void AddRange(IList<TEntity> entities);

        void Remove(TEntity entity);

        IDbContextTransaction BeginTransaction();

        Task<int> SaveChangesAsync();

        Task<TEntity> FirstOrDefaultAsync(TKey id);

        IQueryable<TEntity> Query();

        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate);
    }
}

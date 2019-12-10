using LinFx.Domain.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LinFx.Data
{
    public interface IRepository<TEntity> : IRepository<TEntity, long> where TEntity : class, IEntity<long>
    {
    }

    public interface IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        IQueryable<TEntity> Query();

        void Add(TEntity entity);

        void AddRange(IList<TEntity> entities);

        void Remove(TEntity entity);

        IDbContextTransaction BeginTransaction();

        Task<int> SaveChangesAsync();

        Task<TEntity> FirstOrDefaultAsync(TKey id);

        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate);
    }
}

using LinFx.Data.Abstractions;
using LinFx.Extensions.Dapper.Mapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LinFx.Extensions.Dapper
{
    public interface IDatabase : IDisposable
    {
        IDbConnection Connection { get; }
        int? CommandTimeout { get; set; }
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        void Commit();
        void Rollback();
        void RunInTransaction(Action action);
        T RunInTransaction<T>(Func<T> func);
        bool HasActiveTransaction { get; }
        IDbTransaction Transaction { get; }

        Task InsertAsync<TEntity>(TEntity[] entities, int? commandTimeout = null) where TEntity : class;
        Task<dynamic> InsertAsync<TEntity>(TEntity entity, int? commandTimeout = null) where TEntity : class;
        Task<bool> UpdateAsync<T>(T entity, int? commandTimeout = null) where T : class;
        bool Delete<T>(T entity, int? commandTimeout = null) where T : class;
        bool Delete<TEntity>(Expression<Func<TEntity, bool>> predicate, int? commandTimeout = null) where TEntity : class;

        int Count<TEntity>(Expression<Func<TEntity, bool>> predicate = null) where TEntity : class;
        TEntity Get<TEntity>(dynamic id, int? commandTimeout = null) where TEntity : class;
        TEntity Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        //IEnumerable<TEntity> Select<TEntity>(Expression<Func<TEntity, bool>> predicate = null, Paging paging = null, params Expression<Func<TEntity, object>>[] sort) where TEntity : class;
        IEnumerable<TEntity> Select<TEntity>(Expression<Func<TEntity, bool>> predicate = null, Paging paging = null, params Sorting[] sorting) where TEntity : class;
        IEnumerable<T> GetSet<T>(object predicate, IList<ISort> sort, uint firstResult, uint maxResults, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class;
        IEnumerable<T> GetSet<T>(object predicate, IList<ISort> sort, uint firstResult, uint maxResults, int? commandTimeout, bool buffered) where T : class;
        IMultipleResultReader GetMultiple(MultiplePredicate predicate, int? commandTimeout = null);

        bool Any<T>(Expression<Func<T, bool>> predicate) where T : class;

        void ClearCache();
        Guid GetNextGuid();
        IClassMapper GetMap<T>() where T : class;
    }
}

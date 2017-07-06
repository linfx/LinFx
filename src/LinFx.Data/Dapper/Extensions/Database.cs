using System;
using System.Collections.Generic;
using System.Data;
using LinFx.Data.Dapper.Extensions.Mapper;
using LinFx.Data.Dapper.Extensions.Sql;
using System.Linq.Expressions;
using LinFx.Data.Dapper.Filters.Query;

namespace LinFx.Data.Dapper.Extensions
{
    public interface IDatabase : IDisposable
    {
        bool HasActiveTransaction { get; }
        IDbConnection Connection { get; }
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        void Commit();
        void Rollback();
        void RunInTransaction(Action action);
        T RunInTransaction<T>(Func<T> func);

        void Insert<TEntity>(IList<TEntity> entities, int? commandTimeout = null) where TEntity : class;
        dynamic Insert<TEntity>(TEntity entity, int? commandTimeout = null) where TEntity : class;

        bool Update<T>(T entity, int? commandTimeout = null) where T : class;
        bool Delete<T>(T entity, int? commandTimeout = null) where T : class;

        bool Delete<TEntity>(Expression<Func<TEntity, bool>> predicate, int? commandTimeout = null) where TEntity : class;

		int Count<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
		TEntity Get<TEntity>(dynamic id, int? commandTimeout = null) where TEntity : class;
		IEnumerable<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> predicate = null, int page = 0, int limit = 0, bool ascending = true, params Expression<Func<TEntity, object>>[] sort) where TEntity : class;
		IEnumerable<T> GetSet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class;
        IEnumerable<T> GetSet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, int? commandTimeout, bool buffered) where T : class;
        IMultipleResultReader GetMultiple(GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout = null);
        IMultipleResultReader GetMultiple(GetMultiplePredicate predicate, int? commandTimeout = null);

        void ClearCache();
        Guid GetNextGuid();
        IClassMapper GetMap<T>() where T : class;
        int Execute(string sql, object param = null);
    }

    public class Database : IDatabase
    {
        private readonly IDapperImplementor _dapper;
		private IDbTransaction _transaction;
		private static DapperQueryFilterExecuter _dapperQueryFilterExecuter = new DapperQueryFilterExecuter();

        public IDbConnection Connection { get; private set; }

        public Database(IDbConnection connection, ISqlGenerator sqlGenerator)
        {
            _dapper = new DapperImplementor(sqlGenerator);
            Connection = connection;

            if (Connection.State != ConnectionState.Open)
                Connection.Open();
        }

        public bool HasActiveTransaction
        {
            get { return _transaction != null; }
        }

        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
            {
                if (_transaction != null)
                    _transaction.Rollback();
                Connection.Close();
            }
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _transaction = Connection.BeginTransaction(isolationLevel);
        }

        public void Commit()
        {
            _transaction.Commit();
            _transaction = null;
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _transaction = null;
        }

        public void RunInTransaction(Action action)
        {
            BeginTransaction();
            try
            {
                action();
                Commit();
            }
            catch (Exception ex)
            {
                if (HasActiveTransaction)
                    Rollback();

                throw ex;
            }
        }

        public T RunInTransaction<T>(Func<T> func)
        {
            BeginTransaction();
            try
            {
                T result = func();
                Commit();
                return result;
            }
            catch (Exception ex)
            {
                if (HasActiveTransaction)
                    Rollback();

                throw ex;
            }
        }

		public TEntity Get<TEntity>(dynamic id, int? commandTimeout) where TEntity : class
		{
			return _dapper.Get<TEntity>(Connection, id, _transaction, commandTimeout);
		}

        public void Insert<T>(IList<T> entities, int? commandTimeout) where T : class
        {
            _dapper.Insert(Connection, entities, _transaction, commandTimeout);
        }

        public dynamic Insert<T>(T entity, int? commandTimeout) where T : class
        {
            return _dapper.Insert(Connection, entity, _transaction, commandTimeout);
        }

        public bool Update<T>(T entity, int? commandTimeout) where T : class
        {
            return _dapper.Update(Connection, entity, _transaction, commandTimeout);
        }

        public bool Delete<T>(T entity, int? commandTimeout) where T : class
        {
            return _dapper.Delete(Connection, entity, _transaction, commandTimeout);
        }

		public bool Delete<TEntity>(Expression<Func<TEntity, bool>> predicate, int? commandTimeout) where TEntity : class
		{
			var filteredPredicate = _dapperQueryFilterExecuter.ExecuteFilter(predicate);
			return _dapper.Delete<TEntity>(Connection, filteredPredicate, _transaction, commandTimeout);
		}

		public IEnumerable<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> predicate, int page, int limit, bool ascending = true, params Expression<Func<TEntity, object>>[] sort) where TEntity : class
		{
			var filteredPredicate = _dapperQueryFilterExecuter.ExecuteFilter(predicate);
			return _dapper.GetList<TEntity>(Connection, filteredPredicate, sort.ToSortable(ascending), page, limit, _transaction);
		}

		public IEnumerable<T> GetSet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class
        {
            return _dapper.GetSet<T>(Connection, predicate, sort, firstResult, maxResults, transaction, commandTimeout, buffered);
        }

        public IEnumerable<T> GetSet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, int? commandTimeout, bool buffered) where T : class
        {
            return _dapper.GetSet<T>(Connection, predicate, sort, firstResult, maxResults, _transaction, commandTimeout, buffered);
        }

        public int Count<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
			var filteredPredicate = _dapperQueryFilterExecuter.ExecuteFilter(predicate);
			return _dapper.Count<TEntity>(Connection, filteredPredicate, _transaction);
        }

        public IMultipleResultReader GetMultiple(GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout)
        {
            return _dapper.GetMultiple(Connection, predicate, transaction, commandTimeout);
        }

        public IMultipleResultReader GetMultiple(GetMultiplePredicate predicate, int? commandTimeout)
        {
            return _dapper.GetMultiple(Connection, predicate, _transaction, commandTimeout);
        }

        public void ClearCache()
        {
            _dapper.SqlGenerator.Configuration.ClearCache();
        }

        public Guid GetNextGuid()
        {
            return _dapper.SqlGenerator.Configuration.GetNextGuid();
        }

        public IClassMapper GetMap<T>() where T : class
        {
            return _dapper.SqlGenerator.Configuration.GetMap<T>();
        }

        public int Execute(string sql, object param)
        {
            return _dapper.Execute(Connection, sql, param, _transaction);
        }
    }
}
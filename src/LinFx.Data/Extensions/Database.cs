using System;
using System.Collections.Generic;
using System.Data;
using LinFx.Data.Extensions.Mapper;
using System.Linq.Expressions;
using LinFx.Data.Filters.Query;
using System.Linq;
using LinFx.Data.Extensions.Dapper;
using LinFx.Data.Extensions.Sql;

namespace LinFx.Data.Extensions
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

		void Insert<TEntity>(List<TEntity> entities, int? commandTimeout = null) where TEntity : class;
		dynamic Insert<TEntity>(TEntity entity, int? commandTimeout = null) where TEntity : class;
		bool Update<T>(T entity, int? commandTimeout = null) where T : class;
        bool Delete<T>(T entity, int? commandTimeout = null) where T : class;
        bool Delete<TEntity>(Expression<Func<TEntity, bool>> predicate, int? commandTimeout = null) where TEntity : class;

		int Count<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
		TEntity Get<TEntity>(dynamic id, int? commandTimeout = null) where TEntity : class;
		TEntity Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
		IEnumerable<TEntity> Select<TEntity>(Expression<Func<TEntity, bool>> predicate = null, Paging paging = null, params Expression<Func<TEntity, object>>[] sort) where TEntity : class;
		IEnumerable<T> GetSet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class;
        IEnumerable<T> GetSet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, int? commandTimeout, bool buffered) where T : class;
        IMultipleResultReader GetMultiple(GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout = null);
        IMultipleResultReader GetMultiple(GetMultiplePredicate predicate, int? commandTimeout = null);

        void ClearCache();
        Guid GetNextGuid();
        IClassMapper GetMap<T>() where T : class;
    }

    public class Database : IDatabase
    {
        private readonly IDapperImplementor _dapper;
		private IDbTransaction _transaction;
		private static QueryFilterExecuter _dapperFilterExecuter = new QueryFilterExecuter();

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

		public TEntity Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
		{
			return Select(predicate, null).FirstOrDefault();
		}

		public void Insert<T>(List<T> entities, int? commandTimeout) where T : class
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
			var filteredPredicate = _dapperFilterExecuter.ExecuteFilter(predicate);
			return _dapper.Delete<TEntity>(Connection, filteredPredicate, _transaction, commandTimeout);
		}

		public IEnumerable<TEntity> Select<TEntity>(Expression<Func<TEntity, bool>> predicate, Paging paging, params Expression<Func<TEntity, object>>[] sort) where TEntity : class
		{
			var filteredPredicate = _dapperFilterExecuter.ExecuteFilter(predicate);
			return _dapper.Select<TEntity>(Connection, filteredPredicate, paging, sort.ToSorting());
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
			var filteredPredicate = _dapperFilterExecuter.ExecuteFilter(predicate);
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
	}

	public static class DatabaseExtensions
	{
		public static int Execute(this IDatabase db, string sql, object param)
		{
			return db.Connection.Execute(sql, param);
		}

		public static T ExecuteScalar<T>(this IDatabase db, string sql, object param = null, IDbTransaction transaction = null)
		{
			return db.Connection.ExecuteScalar<T>(sql, param);
		}


		public static IEnumerable<T> Query<T>(this IDatabase db, string sql, object param = null)
		{
			return db.Connection.Query<T>(sql, param);
		}

		public static T QueryFirstOrDefault<T>(this IDatabase db, string sql, object param = null)
		{
			return db.Connection.QueryFirstOrDefault<T>(sql, param);
		}
	}
}
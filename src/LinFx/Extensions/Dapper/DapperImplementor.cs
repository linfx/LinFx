using System;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using Dapper;
using LinFx.Extensions.Dapper.Mapper;
using System.Threading.Tasks;
using LinFx.Extensions.Dapper.Dialect;
using System.Collections.Generic;
using LinFx.Data.Abstractions;

namespace LinFx.Extensions.Dapper
{
    public interface IDapperImplementor
	{
		ISqlGenerator SqlGenerator { get; }
		IDbTransaction Transaction { get; set; }
		Task InsertAsync<T>(IDbConnection connection, T[] entities, IDbTransaction transaction, int? commandTimeout) where T : class;
		Task<dynamic> InsertAsync<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout) where T : class;
		Task<bool> UpdateAsync<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout) where T : class;
		bool Delete<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout) where T : class;
		bool Delete<T>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout) where T : class;
		int Count<T>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout = default(int?)) where T : class;
		T Get<T>(IDbConnection connection, dynamic id, IDbTransaction transaction, int? commandTimeout) where T : class;
		IEnumerable<T> GetList<T>(IDbConnection connection, object predicate, Paging paging, params Sorting[] sorting) where T : class;
		IEnumerable<T> GetSet<T>(IDbConnection connection, object predicate, IList<ISort> sort, uint firstResult, uint maxResults, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class;
		IMultipleResultReader GetMultiple(IDbConnection connection, MultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout);
	}

	public class DapperImplementor : IDapperImplementor
    {
        public DapperImplementor(ISqlGenerator sqlGenerator)
        {
            SqlGenerator = sqlGenerator;
        }

        public ISqlGenerator SqlGenerator { get; }

		public IDbTransaction Transaction { get; set; }

		public Task InsertAsync<T>(IDbConnection connection, T[] entities, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            var classMap = SqlGenerator.Configuration.GetMap<T>();
            var properties = classMap.Properties.Where(p => p.KeyType != KeyType.NotAKey);

            foreach (var e in entities)
            {
                foreach (var column in properties)
                {
                    if (column.KeyType == KeyType.Guid)
                    {
                        Guid comb = SqlGenerator.Configuration.GetNextGuid();
                        column.PropertyInfo.SetValue(e, comb, null);
                    }
                }
            }

            string sql = SqlGenerator.Insert(classMap);
            return connection.ExecuteAsync(sql, entities, transaction, commandTimeout, CommandType.Text);
        }

        public async Task<dynamic> InsertAsync<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            var classMap = SqlGenerator.Configuration.GetMap<T>();
            List<IPropertyMap> nonIdentityKeyProperties = classMap.Properties.Where(p => p.KeyType == KeyType.Guid || p.KeyType == KeyType.Assigned).ToList();
            var identityColumn = classMap.Properties.SingleOrDefault(p => p.KeyType == KeyType.Identity);
            foreach (var column in nonIdentityKeyProperties)
            {
                if (column.KeyType == KeyType.Guid)
                {
                    Guid comb = SqlGenerator.Configuration.GetNextGuid();
                    column.PropertyInfo.SetValue(entity, comb, null);
                }
            }

            IDictionary<string, object> keyValues = new ExpandoObject();
            string sql = SqlGenerator.Insert(classMap);
            if (identityColumn != null)
            {
                IEnumerable<long> result;
                if (SqlGenerator.SupportsMultipleStatements())
                {
                    sql += SqlGenerator.Configuration.Dialect.BatchSeperator + SqlGenerator.IdentitySql(classMap);
                    result = connection.Query<long>(sql, entity, transaction, false, commandTimeout, CommandType.Text);
                }
                else
                {
                    await connection.ExecuteAsync(sql, entity, transaction, commandTimeout, CommandType.Text);
                    sql = SqlGenerator.IdentitySql(classMap);
                    result = connection.Query<long>(sql, entity, transaction, false, commandTimeout, CommandType.Text);
                }

                long identityValue = result.First();
                int identityInt = Convert.ToInt32(identityValue);
                keyValues.Add(identityColumn.Name, identityInt);
                identityColumn.PropertyInfo.SetValue(entity, identityInt, null);
            }
            else
            {
                await connection.ExecuteAsync(sql, entity, transaction, commandTimeout, CommandType.Text);
            }

            foreach (var column in nonIdentityKeyProperties)
            {
                keyValues.Add(column.Name, column.PropertyInfo.GetValue(entity, null));
            }

            if (keyValues.Count == 1)
            {
                return keyValues.First().Value;
            }

            return keyValues;
        }

        public async Task<bool> UpdateAsync<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<T>();
            IPredicate predicate = GetKeyPredicate(classMap, entity);
            var parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.Update(classMap, predicate, parameters);
            var dynamicParameters = new DynamicParameters();

            var columns = classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly || p.KeyType == KeyType.Identity));
            foreach (var property in Reflection.GetObjectValues(entity).Where(property => columns.Any(c => c.Name == property.Key)))
            {
                dynamicParameters.Add(property.Key, property.Value);
            }

            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            if (connection.State != ConnectionState.Open)
                connection.Open();
            return await connection.ExecuteAsync(sql, dynamicParameters, transaction, commandTimeout, CommandType.Text) > 0;
        }

        public bool Delete<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<T>();
            IPredicate predicate = GetKeyPredicate(classMap, entity);
            return Delete<T>(connection, classMap, predicate, transaction, commandTimeout);
        }

        public bool Delete<T>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<T>();
            IPredicate wherePredicate = GetPredicate(classMap, predicate);
            return Delete<T>(connection, classMap, wherePredicate, transaction, commandTimeout);
        }

        public T Get<T>(IDbConnection connection, dynamic id, IDbTransaction transaction, int? commandTimeout) where T : class
        {
			IClassMapper classMap = SqlGenerator.Configuration.GetMap<T>();
			IPredicate predicate = GetIdPredicate(classMap, id);
			return Select<T>(connection, classMap, transaction, commandTimeout, true, predicate, null).SingleOrDefault();
        }

		public IEnumerable<T> GetList<T>(IDbConnection connection, object predicate, Paging paging, params Sorting[] sorting) where T : class
		{
			IClassMapper classMap = SqlGenerator.Configuration.GetMap<T>();
			IPredicate wherePredicate = GetPredicate(classMap, predicate);
			return Select<T>(connection, classMap, Transaction, 0, true, wherePredicate, paging, sorting);
		}

		protected IEnumerable<T> Select<T>(IDbConnection connection, IClassMapper classMap, IDbTransaction transaction, int? commandTimeout, bool buffered, IPredicate predicate, Paging paging, params Sorting[] sorting) where T : class
		{
			var parameters = new Dictionary<string, object>();
			string sql = SqlGenerator.Select(classMap, parameters, predicate, paging, sorting);
			var dynamicParameters = new DynamicParameters();
			foreach(var parameter in parameters)
			{
				dynamicParameters.Add(parameter.Key, parameter.Value);
			}
            if (connection.State != ConnectionState.Open)
                connection.Open();
			return connection.Query<T>(sql, dynamicParameters, transaction, buffered, commandTimeout, CommandType.Text);
		}

		public IEnumerable<T> GetSet<T>(IDbConnection connection, object predicate, IList<ISort> sort, uint firstResult, uint maxResults, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<T>();
            IPredicate wherePredicate = GetPredicate(classMap, predicate);
            return GetSet<T>(connection, classMap, wherePredicate, sort, firstResult, maxResults, transaction, commandTimeout, buffered);
        }

        public int Count<T>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<T>();
            IPredicate wherePredicate = GetPredicate(classMap, predicate);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.Count(classMap, wherePredicate, parameters);
            DynamicParameters dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }
            return (int)connection.Query(sql, dynamicParameters, transaction, false, commandTimeout, CommandType.Text).Single().Total;
        }

        public IMultipleResultReader GetMultiple(IDbConnection connection, MultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout)
        {
            if (SqlGenerator.SupportsMultipleStatements())
            {
                return GetMultipleByBatch(connection, predicate, transaction, commandTimeout);
            }
            return GetMultipleBySequence(connection, predicate, transaction, commandTimeout);
        }

        protected IEnumerable<T> GetSet<T>(IDbConnection connection, IClassMapper classMap, IPredicate predicate, IList<ISort> sort, uint firstResult, uint maxResults, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.SelectSet(classMap, predicate, sort, firstResult, maxResults, parameters);
            DynamicParameters dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection.Query<T>(sql, dynamicParameters, transaction, buffered, commandTimeout, CommandType.Text);
        }

        protected bool Delete<T>(IDbConnection connection, IClassMapper classMap, IPredicate predicate, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.Delete(classMap, predicate, parameters);
            DynamicParameters dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection.Execute(sql, dynamicParameters, transaction, commandTimeout, CommandType.Text) > 0;
        }

        protected IPredicate GetPredicate(IClassMapper classMap, object predicate)
        {
            IPredicate wherePredicate = predicate as IPredicate;
            if (wherePredicate == null && predicate != null)
            {
                wherePredicate = GetEntityPredicate(classMap, predicate);
            }
            return wherePredicate;
        }

        protected IPredicate GetIdPredicate(IClassMapper classMap, object id)
        {
            bool isSimpleType = Reflection.IsSimpleType(id.GetType());
            var keys = classMap.Properties.Where(p => p.KeyType != KeyType.NotAKey);
            IDictionary<string, object> paramValues = null;
            IList<IPredicate> predicates = new List<IPredicate>();
            if (!isSimpleType)
            {
                paramValues = Reflection.GetObjectValues(id);
            }

            foreach (var key in keys)
            {
                object value = id;
                if (!isSimpleType)
                {
                    value = paramValues[key.Name];
                }

                Type predicateType = typeof(FieldPredicate<>).MakeGenericType(classMap.EntityType);

                IFieldPredicate fieldPredicate = Activator.CreateInstance(predicateType) as IFieldPredicate;
                fieldPredicate.Not = false;
                fieldPredicate.Operator = Operator.Eq;
                fieldPredicate.PropertyName = key.Name;
                fieldPredicate.Value = value;
                predicates.Add(fieldPredicate);
            }

            return predicates.Count == 1
                       ? predicates[0]
                       : new PredicateGroup
                             {
                                 Operator = GroupOperator.And,
                                 Predicates = predicates
                             };
        }

        protected IPredicate GetKeyPredicate<T>(IClassMapper classMap, T entity) where T : class
        {
            var whereFields = classMap.Properties.Where(p => p.KeyType != KeyType.NotAKey);
            if (!whereFields.Any())
            {
                throw new ArgumentException("At least one Key column must be defined.");
            }

            IList<IPredicate> predicates = (from field in whereFields
                                            select new FieldPredicate<T>
                                                       {
                                                           Not = false,
                                                           Operator = Operator.Eq,
                                                           PropertyName = field.Name,
                                                           Value = field.PropertyInfo.GetValue(entity, null)
                                                       }).Cast<IPredicate>().ToList();

            return predicates.Count == 1
                       ? predicates[0]
                       : new PredicateGroup
                             {
                                 Operator = GroupOperator.And,
                                 Predicates = predicates
                             };
        }

        protected IPredicate GetEntityPredicate(IClassMapper classMap, object entity)
        {
            Type predicateType = typeof(FieldPredicate<>).MakeGenericType(classMap.EntityType);
            IList<IPredicate> predicates = new List<IPredicate>();
            foreach (var kvp in Reflection.GetObjectValues(entity))
            {
                IFieldPredicate fieldPredicate = Activator.CreateInstance(predicateType) as IFieldPredicate;
                fieldPredicate.Not = false;
                fieldPredicate.Operator = Operator.Eq;
                fieldPredicate.PropertyName = kvp.Key;
                fieldPredicate.Value = kvp.Value;
                predicates.Add(fieldPredicate);
            }

            return predicates.Count == 1
                       ? predicates[0]
                       : new PredicateGroup
                       {
                           Operator = GroupOperator.And,
                           Predicates = predicates
                       };
        }

        protected GridReaderResultReader GetMultipleByBatch(IDbConnection connection, MultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            StringBuilder sql = new StringBuilder();
            foreach (var item in predicate.Items)
            {
                IClassMapper classMap = SqlGenerator.Configuration.GetMap(item.Type);
                IPredicate itemPredicate = item.Value as IPredicate;
                if (itemPredicate == null && item.Value != null)
                    itemPredicate = GetPredicate(classMap, item.Value);

                sql.AppendLine(SqlGenerator.Select(classMap, parameters, itemPredicate) + SqlGenerator.Configuration.Dialect.BatchSeperator);
            }

            DynamicParameters dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            SqlMapper.GridReader grid = connection.QueryMultiple(sql.ToString(), dynamicParameters, transaction, commandTimeout, CommandType.Text);
            return new GridReaderResultReader(grid);
        }

        protected SequenceReaderResultReader GetMultipleBySequence(IDbConnection connection, MultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout)
        {
            IList<SqlMapper.GridReader> items = new List<SqlMapper.GridReader>();
            foreach (var item in predicate.Items)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                IClassMapper classMap = SqlGenerator.Configuration.GetMap(item.Type);
                IPredicate itemPredicate = item.Value as IPredicate;
                if (itemPredicate == null && item.Value != null)
                {
                    itemPredicate = GetPredicate(classMap, item.Value);
                }

                string sql = SqlGenerator.Select(classMap, parameters, itemPredicate);
                DynamicParameters dynamicParameters = new DynamicParameters();
                foreach (var parameter in parameters)
                {
                    dynamicParameters.Add(parameter.Key, parameter.Value);
                }

                SqlMapper.GridReader queryResult = connection.QueryMultiple(sql, dynamicParameters, transaction, commandTimeout, CommandType.Text);
                items.Add(queryResult);
            }

            return new SequenceReaderResultReader(items);
        }
	}

	public static class DapperImplementorExtensions
	{
		public static int Execute(this IDapperImplementor impl, IDbConnection connection, string sql, object param, IDbTransaction transaction, int? commandTimeout)
		{
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection.Execute(sql, param, transaction, commandTimeout, CommandType.Text);
		}

		public static T ExecuteScalar<T>(this IDapperImplementor impl, IDbConnection connection, string sql, object param, IDbTransaction transaction, int? commandTimeout)
		{
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection.ExecuteScalar<T>(sql, param, transaction, commandTimeout, CommandType.Text);
		}

		public static IEnumerable<T> Query<T>(this IDapperImplementor impl, IDbConnection connection, string sql, object param, IDbTransaction transaction, int? commandTimeout)
		{
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection.Query<T>(sql, param, transaction, true, commandTimeout, CommandType.Text);
		}

		public static T QueryFirstOrDefault<T>(this IDapperImplementor impl, IDbConnection connection, string sql, object param, IDbTransaction transaction, int? commandTimeout)
		{
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection.QueryFirst<T>(sql, param, transaction, commandTimeout);
		}

		public static SqlMapper.GridReader QueryMultiple(this IDapperImplementor impl, IDbConnection connection, string sql, object param, IDbTransaction transaction, int? commandTimeout)
		{
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection.QueryMultiple(sql, param, transaction, commandTimeout);
		}
	}
}
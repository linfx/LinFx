using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using Dapper;
using LinFx.Data.Dapper.Extensions.Mapper;
using LinFx.Data.Dapper.Extensions.Sql;

namespace LinFx.Data.Dapper.Extensions
{
    public interface IDapperImplementor
    {
        ISqlGenerator SqlGenerator { get; }
        void Insert<T>(IDbConnection connection, IEnumerable<T> entities, IDbTransaction transaction, int? commandTimeout) where T : class;
        dynamic Insert<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout) where T : class;
        bool Update<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout) where T : class;
        bool Delete<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout) where T : class;
        bool Delete<T>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout) where T : class;
        T Get<T>(IDbConnection connection, dynamic id, IDbTransaction transaction, int? commandTimeout) where T : class;
        IEnumerable<T> GetList<T>(IDbConnection connection, object predicate, IList<ISort> sort, int page, int limit, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class;
        IEnumerable<T> GetSet<T>(IDbConnection connection, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class;
        int Count<T>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout) where T : class;
        IMultipleResultReader GetMultiple(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout);
        int Execute(IDbConnection connection, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?));
    }

    public class DapperImplementor : IDapperImplementor
    {
        public DapperImplementor(ISqlGenerator sqlGenerator)
        {
            SqlGenerator = sqlGenerator;
        }

        public ISqlGenerator SqlGenerator { get; }

        public void Insert<T>(IDbConnection connection, IEnumerable<T> entities, IDbTransaction transaction, int? commandTimeout) where T : class
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
            connection.Execute(sql, entities, transaction, commandTimeout, CommandType.Text);
        }

        public dynamic Insert<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout) where T : class
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
                    connection.Execute(sql, entity, transaction, commandTimeout, CommandType.Text);
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
                connection.Execute(sql, entity, transaction, commandTimeout, CommandType.Text);
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

        public bool Update<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<T>();
            IPredicate predicate = GetKeyPredicate<T>(classMap, entity);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.Update(classMap, predicate, parameters);
            DynamicParameters dynamicParameters = new DynamicParameters();

            var columns = classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly || p.KeyType == KeyType.Identity));
            foreach (var property in ReflectionUtils.GetObjectValues(entity).Where(property => columns.Any(c => c.Name == property.Key)))
            {
                dynamicParameters.Add(property.Key, property.Value);
            }

            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            return connection.Execute(sql, dynamicParameters, transaction, commandTimeout, CommandType.Text) > 0;
        }

        public bool Delete<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<T>();
            IPredicate predicate = GetKeyPredicate<T>(classMap, entity);
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
            T result = GetList<T>(connection, classMap, predicate, null, 0, 0, transaction, commandTimeout, true).SingleOrDefault();
            return result;
        }

        public IEnumerable<T> GetList<T>(IDbConnection connection, object predicate, IList<ISort> sort, int page, int limit, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<T>();
            IPredicate wherePredicate = GetPredicate(classMap, predicate);
            return GetList<T>(connection, classMap, wherePredicate, sort, page, limit, transaction, commandTimeout, buffered);
        }

        public IEnumerable<T> GetSet<T>(IDbConnection connection, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class
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

        public IMultipleResultReader GetMultiple(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout)
        {
            if (SqlGenerator.SupportsMultipleStatements())
            {
                return GetMultipleByBatch(connection, predicate, transaction, commandTimeout);
            }
            return GetMultipleBySequence(connection, predicate, transaction, commandTimeout);
        }

        protected IEnumerable<T> GetList<T>(IDbConnection connection, IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int page, int limit, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class
        {
            var parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.SelectPaged(classMap, predicate, sort, page, limit, parameters);
            var dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }
            return connection.Query<T>(sql, dynamicParameters, transaction, buffered, commandTimeout, CommandType.Text);
        }

        protected IEnumerable<T> GetSet<T>(IDbConnection connection, IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.SelectSet(classMap, predicate, sort, firstResult, maxResults, parameters);
            DynamicParameters dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }
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
            bool isSimpleType = ReflectionUtils.IsSimpleType(id.GetType());
            var keys = classMap.Properties.Where(p => p.KeyType != KeyType.NotAKey);
            IDictionary<string, object> paramValues = null;
            IList<IPredicate> predicates = new List<IPredicate>();
            if (!isSimpleType)
            {
                paramValues = ReflectionUtils.GetObjectValues(id);
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
            foreach (var kvp in ReflectionUtils.GetObjectValues(entity))
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

        protected GridReaderResultReader GetMultipleByBatch(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            StringBuilder sql = new StringBuilder();
            foreach (var item in predicate.Items)
            {
                IClassMapper classMap = SqlGenerator.Configuration.GetMap(item.Type);
                IPredicate itemPredicate = item.Value as IPredicate;
                if (itemPredicate == null && item.Value != null)
                {
                    itemPredicate = GetPredicate(classMap, item.Value);
                }
                sql.AppendLine(SqlGenerator.Select(classMap, itemPredicate, item.Sort, parameters) + SqlGenerator.Configuration.Dialect.BatchSeperator);
            }

            DynamicParameters dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            SqlMapper.GridReader grid = connection.QueryMultiple(sql.ToString(), dynamicParameters, transaction, commandTimeout, CommandType.Text);
            return new GridReaderResultReader(grid);
        }

        protected SequenceReaderResultReader GetMultipleBySequence(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout)
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

                string sql = SqlGenerator.Select(classMap, itemPredicate, item.Sort, parameters);
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

        public int Execute(IDbConnection connection, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?))
        {
            return connection.Execute(sql, param, transaction, commandTimeout);
        }
    }
}

//#define COREFX
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Collections.Concurrent;
//using System.Reflection.Emit;
//using Dapper;
//using DataException = System.InvalidOperationException;

//namespace LinFx.Data.Dapper.Extensions
//{
//    public static partial class SqlMapperExtensions
//    {
//        // ReSharper disable once MemberCanBePrivate.Global
//        public interface IProxy //must be kept public
//        {
//            bool IsDirty { get; set; }
//        }

//        public interface ITableNameMapper
//        {
//            string GetTableName(Type type);
//        }

//        public delegate string GetDatabaseTypeDelegate(IDbConnection connection);
//        public delegate string TableNameMapperDelegate(Type type);

//        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> KeyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
//        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> ExplicitKeyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
//        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> TypeProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
//        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> ComputedProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
//        private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> GetQueries = new ConcurrentDictionary<RuntimeTypeHandle, string>();
//        private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> TypeTableName = new ConcurrentDictionary<RuntimeTypeHandle, string>();

//        private static readonly ISqlAdapter DefaultAdapter = new SqlServerAdapter();

//        private static readonly Dictionary<string, ISqlAdapter> AdapterDictionary = new Dictionary<string, ISqlAdapter>
//        {
//            {"sqlconnection", new SqlServerAdapter()},
//            {"sqlceconnection", new SqlCeServerAdapter()},
//            {"npgsqlconnection", new PostgresAdapter()},
//            {"sqliteconnection", new SQLiteAdapter()},
//            {"mysqlconnection", new MySqlAdapter()},
//            {"fbconnection", new FbAdapter() },
//        };

//        private static List<PropertyInfo> ComputedPropertiesCache(Type type)
//        {
//            if (ComputedProperties.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo> pi))
//            {
//                return pi.ToList();
//            }

//            var computedProperties = TypePropertiesCache(type).Where(p => p.GetCustomAttributes(true).Any(a => a is ComputedAttribute)).ToList();

//            ComputedProperties[type.TypeHandle] = computedProperties;
//            return computedProperties;
//        }

//        private static List<PropertyInfo> ExplicitKeyPropertiesCache(Type type)
//        {
//            if (ExplicitKeyProperties.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo> pi))
//            {
//                return pi.ToList();
//            }
//            //var explicitKeyProperties = TypePropertiesCache(type).Where(p => p.GetCustomAttributes(true).Any(a => a is ExplicitKeyAttribute)).ToList();

//            var result = new List<PropertyInfo>();

//            var properties = TypePropertiesCache(type);
//            var idKey = properties.FirstOrDefault(p => p.Name.ToLower() == "id");
//            if (idKey != null)
//                result.Add(idKey);

//            result.AddRange(properties.Where(p => p.GetCustomAttributes(true).Any(a => a is ExplicitKeyAttribute)));

//            ExplicitKeyProperties[type.TypeHandle] = result;
//            return result;
//        }

//        private static List<PropertyInfo> KeyPropertiesCache(Type type)
//        {
//            if (KeyProperties.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo> pi))
//            {
//                return pi.ToList();
//            }

//            var allProperties = TypePropertiesCache(type);
//            var keyProperties = allProperties.Where(p =>
//            {
//                return p.GetCustomAttributes(true).Any(a => a is KeyAttribute);
//            }).ToList();

//            //if (keyProperties.Count == 0)
//            //{
//            //    var idProp = allProperties.FirstOrDefault(p => p.Name.ToLower() == "id");
//            //    if (idProp != null && !idProp.GetCustomAttributes(true).Any(a => a is ExplicitKeyAttribute))
//            //    {
//            //        keyProperties.Add(idProp);
//            //    }
//            //}

//            KeyProperties[type.TypeHandle] = keyProperties;
//            return keyProperties;
//        }

//        private static List<PropertyInfo> TypePropertiesCache(Type type)
//        {
//            if (TypeProperties.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo> pis))
//            {
//                return pis.ToList();
//            }

//            var properties = type.GetProperties().Where(IsWriteable).ToArray();
//            TypeProperties[type.TypeHandle] = properties;
//            return properties.ToList();
//        }

//        private static bool IsWriteable(PropertyInfo pi)
//        {
//            var attributes = pi.GetCustomAttributes(typeof(WriteAttribute), false).AsList();
//            if (attributes.Count != 1) return true;

//            var writeAttribute = (WriteAttribute)attributes[0];
//            return writeAttribute.Write;
//        }

//        private static PropertyInfo GetSingleKey<T>(string method)
//        {
//            var type = typeof(T);
//            var keys = KeyPropertiesCache(type);
//            var explicitKeys = ExplicitKeyPropertiesCache(type);
//            var keyCount = keys.Count + explicitKeys.Count;
//            if (keyCount > 1)
//                throw new DataException($"{method}<T> only supports an entity with a single [Key] or [ExplicitKey] property");
//            if (keyCount == 0)
//                throw new DataException($"{method}<T> only supports an entity with a [Key] or an [ExplicitKey] property");

//            return keys.Any() ? keys.First() : explicitKeys.First();
//        }

//        /// <summary>
//        /// Returns a single entity by a single id from table "Ts".  
//        /// Id must be marked with [Key] attribute.
//        /// Entities created from interfaces are tracked/intercepted for changes and used by the Update() extension
//        /// for optimal performance. 
//        /// </summary>
//        /// <typeparam name="T">Interface or type to create and populate</typeparam>
//        /// <param name="connection">Open SqlConnection</param>
//        /// <param name="id">Id of the entity to get, must be marked with [Key] attribute</param>
//        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
//        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
//        /// <returns>Entity of T</returns>
//        public static T Get<T>(this IDbConnection connection, dynamic id, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
//        {
//            var type = typeof(T);

//            if (!GetQueries.TryGetValue(type.TypeHandle, out string sql))
//            {
//                var key = GetSingleKey<T>(nameof(Get));
//                var name = GetTableName(type);

//                sql = $"select * from {name} where {key.Name} = @id";
//                GetQueries[type.TypeHandle] = sql;
//            }

//            var dynParms = new DynamicParameters();
//            dynParms.Add("@id", id);

//            T obj;

//            if (type.IsInterface())
//            {
//                var res = connection.Query(sql, dynParms).FirstOrDefault() as IDictionary<string, object>;

//                if (res == null)
//                    return null;

//                obj = ProxyGenerator.GetInterfaceProxy<T>();

//                foreach (var property in TypePropertiesCache(type))
//                {
//                    var val = res[property.Name];
//                    property.SetValue(obj, Convert.ChangeType(val, property.PropertyType), null);
//                }

//                ((IProxy)obj).IsDirty = false;   //reset change tracking and return
//            }
//            else
//            {
//                obj = connection.Query<T>(sql, dynParms, transaction, commandTimeout: commandTimeout).FirstOrDefault();
//            }
//            return obj;
//        }

//        /// <summary>
//        /// Returns a list of entites from table "Ts".  
//        /// Id of T must be marked with [Key] attribute.
//        /// Entities created from interfaces are tracked/intercepted for changes and used by the Update() extension
//        /// for optimal performance. 
//        /// </summary>
//        /// <typeparam name="T">Interface or type to create and populate</typeparam>
//        /// <param name="connection">Open SqlConnection</param>
//        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
//        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
//        /// <returns>Entity of T</returns>
//        public static IEnumerable<T> GetAll<T>(this IDbConnection connection, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
//        {
//            var type = typeof(T);
//            var cacheType = typeof(List<T>);

//            if (!GetQueries.TryGetValue(cacheType.TypeHandle, out string sql))
//            {
//                GetSingleKey<T>(nameof(GetAll));
//                var name = GetTableName(type);

//                sql = "select * from " + name;
//                GetQueries[cacheType.TypeHandle] = sql;
//            }

//            if (!type.IsInterface()) return connection.Query<T>(sql, null, transaction, commandTimeout: commandTimeout);

//            var result = connection.Query(sql);
//            var list = new List<T>();
//            foreach (IDictionary<string, object> res in result)
//            {
//                var obj = ProxyGenerator.GetInterfaceProxy<T>();
//                foreach (var property in TypePropertiesCache(type))
//                {
//                    var val = res[property.Name];
//                    property.SetValue(obj, Convert.ChangeType(val, property.PropertyType), null);
//                }
//                ((IProxy)obj).IsDirty = false;   //reset change tracking and return
//                list.Add(obj);
//            }
//            return list;
//        }

//        /// <summary>
//        /// Specify a custom table name mapper based on the POCO type name
//        /// </summary>
//        public static TableNameMapperDelegate TableNameMapper;

//        private static string GetTableName(Type type)
//        {
//            if (TypeTableName.TryGetValue(type.TypeHandle, out string name)) return name;

//            if (TableNameMapper != null)
//            {
//                name = TableNameMapper(type);
//            }
//            else
//            {
//                //NOTE: This as dynamic trick should be able to handle both our own Table-attribute as well as the one in EntityFramework 
//                if (type
//#if COREFX
//                    .GetTypeInfo()
//#endif
//                    .GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "TableAttribute") is dynamic tableAttr)
//                    name = tableAttr.Name;
//                else
//                {
//                    name = type.Name;
//                    if (type.IsInterface() && name.StartsWith("I"))
//                        name = name.Substring(1);
//                }
//            }

//            TypeTableName[type.TypeHandle] = name;
//            return name;
//        }


//        /// <summary>
//        /// Inserts an entity into table "Ts" and returns identity id or number if inserted rows if inserting a list.
//        /// </summary>
//        /// <param name="connection">Open SqlConnection</param>
//        /// <param name="entityToInsert">Entity to insert, can be list of entities</param>
//        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
//        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
//        /// <returns>Identity of inserted entity, or number of inserted rows if inserting a list</returns>
//        internal static long Insert<T>(this IDbConnection connection, T entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
//        {
//            var isList = false;

//            var type = typeof(T);

//            if (type.IsArray)
//            {
//                isList = true;
//                type = type.GetElementType();
//            }
//            else if (type.IsGenericType())
//            {
//                isList = true;
//                type = type.GetGenericArguments()[0];
//            }

//            var name = GetTableName(type);
//            var sbColumnList = new StringBuilder(null);
//            var allProperties = TypePropertiesCache(type);
//            var keyProperties = KeyPropertiesCache(type);
//            var computedProperties = ComputedPropertiesCache(type);
//            var allPropertiesExceptKeyAndComputed = allProperties.Except(keyProperties.Union(computedProperties)).ToList();

//            var adapter = GetFormatter(connection);

//            for (var i = 0; i < allPropertiesExceptKeyAndComputed.Count; i++)
//            {
//                var property = allPropertiesExceptKeyAndComputed.ElementAt(i);
//                adapter.AppendColumnName(sbColumnList, property.Name);  //fix for issue #336
//                if (i < allPropertiesExceptKeyAndComputed.Count - 1)
//                    sbColumnList.Append(", ");
//            }

//            var sbParameterList = new StringBuilder(null);
//            for (var i = 0; i < allPropertiesExceptKeyAndComputed.Count; i++)
//            {
//                var property = allPropertiesExceptKeyAndComputed.ElementAt(i);
//                sbParameterList.AppendFormat("@{0}", property.Name);
//                if (i < allPropertiesExceptKeyAndComputed.Count - 1)
//                    sbParameterList.Append(", ");
//            }

//            int returnVal;
//            var wasClosed = connection.State == ConnectionState.Closed;
//            if (wasClosed) connection.Open();

//            if (!isList)    //single entity
//            {
//                returnVal = adapter.Insert(connection, transaction, commandTimeout, name, sbColumnList.ToString(),
//                    sbParameterList.ToString(), keyProperties, entityToInsert);
//            }
//            else
//            {
//                //insert list of entities
//                var cmd = $"insert into {name} ({sbColumnList}) values ({sbParameterList})";
//                returnVal = connection.Execute(cmd, entityToInsert, transaction, commandTimeout);
//            }
//            if (wasClosed) connection.Close();
//            return returnVal;
//        }

//        /// <summary>
//        /// Updates entity in table "Ts", checks if the entity is modified if the entity is tracked by the Get() extension.
//        /// </summary>
//        /// <typeparam name="T">Type to be updated</typeparam>
//        /// <param name="connection">Open SqlConnection</param>
//        /// <param name="entityToUpdate">Entity to be updated</param>
//        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
//        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
//        /// <returns>true if updated, false if not found or not modified (tracked entities)</returns>
//        internal static bool Update<T>(this IDbConnection connection, T entityToUpdate, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
//        {
//            if (entityToUpdate is IProxy proxy)
//            {
//                if (!proxy.IsDirty) return false;
//            }

//            var type = typeof(T);

//            if (type.IsArray)
//            {
//                type = type.GetElementType();
//            }
//            else if (type.IsGenericType())
//            {
//                type = type.GetGenericArguments()[0];
//            }

//            var keyProperties = KeyPropertiesCache(type).ToList();  //added ToList() due to issue #418, must work on a list copy
//            var explicitKeyProperties = ExplicitKeyPropertiesCache(type);
//            if (!keyProperties.Any() && !explicitKeyProperties.Any())
//                throw new ArgumentException("Entity must have at least one [Key] or [ExplicitKey] property");

//            var name = GetTableName(type);

//            var sb = new StringBuilder();
//            sb.AppendFormat("update {0} set ", name);

//            var allProperties = TypePropertiesCache(type);
//            keyProperties.AddRange(explicitKeyProperties);
//            var computedProperties = ComputedPropertiesCache(type);
//            var nonIdProps = allProperties.Except(keyProperties.Union(computedProperties)).ToList();

//            var adapter = GetFormatter(connection);

//            for (var i = 0; i < nonIdProps.Count; i++)
//            {
//                var property = nonIdProps.ElementAt(i);
//                adapter.AppendColumnNameEqualsValue(sb, property.Name);  //fix for issue #336
//                if (i < nonIdProps.Count - 1)
//                    sb.AppendFormat(", ");
//            }
//            sb.Append(" where ");
//            for (var i = 0; i < keyProperties.Count; i++)
//            {
//                var property = keyProperties.ElementAt(i);
//                adapter.AppendColumnNameEqualsValue(sb, property.Name);  //fix for issue #336
//                if (i < keyProperties.Count - 1)
//                    sb.AppendFormat(" and ");
//            }
//            var updated = connection.Execute(sb.ToString(), entityToUpdate, commandTimeout: commandTimeout, transaction: transaction);
//            return updated > 0;
//        }

//        /// <summary>
//        /// Delete entity in table "Ts".
//        /// </summary>
//        /// <typeparam name="T">Type of entity</typeparam>
//        /// <param name="connection">Open SqlConnection</param>
//        /// <param name="entityToDelete">Entity to delete</param>
//        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
//        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
//        /// <returns>true if deleted, false if not found</returns>
//        public static bool Delete<T>(this IDbConnection connection, T entityToDelete, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
//        {
//            if (entityToDelete == null)
//                throw new ArgumentException("Cannot Delete null Object", nameof(entityToDelete));

//            var type = typeof(T);

//            if (type.IsArray)
//            {
//                type = type.GetElementType();
//            }
//            else if (type.IsGenericType())
//            {
//                type = type.GetGenericArguments()[0];
//            }

//            var keyProperties = KeyPropertiesCache(type).ToList();  //added ToList() due to issue #418, must work on a list copy
//            var explicitKeyProperties = ExplicitKeyPropertiesCache(type);
//            if (!keyProperties.Any() && !explicitKeyProperties.Any())
//                throw new ArgumentException("Entity must have at least one [Key] or [ExplicitKey] property");

//            var name = GetTableName(type);
//            keyProperties.AddRange(explicitKeyProperties);

//            var sb = new StringBuilder();
//            sb.AppendFormat("delete from {0} where ", name);

//            var adapter = GetFormatter(connection);

//            for (var i = 0; i < keyProperties.Count; i++)
//            {
//                var property = keyProperties.ElementAt(i);
//                adapter.AppendColumnNameEqualsValue(sb, property.Name);  //fix for issue #336
//                if (i < keyProperties.Count - 1)
//                    sb.AppendFormat(" and ");
//            }
//            var deleted = connection.Execute(sb.ToString(), entityToDelete, transaction, commandTimeout);
//            return deleted > 0;
//        }

//        /// <summary>
//        /// Delete all entities in the table related to the type T.
//        /// </summary>
//        /// <typeparam name="T">Type of entity</typeparam>
//        /// <param name="connection">Open SqlConnection</param>
//        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
//        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
//        /// <returns>true if deleted, false if none found</returns>
//        public static bool DeleteAll<T>(this IDbConnection connection, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
//        {
//            var type = typeof(T);
//            var name = GetTableName(type);
//            var statement = $"delete from {name}";
//            var deleted = connection.Execute(statement, null, transaction, commandTimeout);
//            return deleted > 0;
//        }

//        /// <summary>
//        /// Specifies a custom callback that detects the database type instead of relying on the default strategy (the name of the connection type object).
//        /// Please note that this callback is global and will be used by all the calls that require a database specific adapter.
//        /// </summary>
//        public static GetDatabaseTypeDelegate GetDatabaseType;

//        private static ISqlAdapter GetFormatter(IDbConnection connection)
//        {
//            var name = GetDatabaseType?.Invoke(connection).ToLower()
//                       ?? connection.ToString().ToLower();

//            return !AdapterDictionary.ContainsKey(name)
//                ? DefaultAdapter
//                : AdapterDictionary[name];
//        }

//        static class ProxyGenerator
//        {
//            private static readonly Dictionary<Type, Type> TypeCache = new Dictionary<Type, Type>();

//            private static AssemblyBuilder GetAsmBuilder(string name)
//            {
//#if COREFX
//                return AssemblyBuilder.DefineDynamicAssembly(new AssemblyName { Name = name }, AssemblyBuilderAccess.Run);
//#else
//                return Thread.GetDomain().DefineDynamicAssembly(new AssemblyName { Name = name }, AssemblyBuilderAccess.Run);
//#endif
//            }

//            public static T GetInterfaceProxy<T>()
//            {
//                Type typeOfT = typeof(T);

//                Type k;
//                if (TypeCache.TryGetValue(typeOfT, out k))
//                {
//                    return (T)Activator.CreateInstance(k);
//                }
//                var assemblyBuilder = GetAsmBuilder(typeOfT.Name);

//                var moduleBuilder = assemblyBuilder.DefineDynamicModule("SqlMapperExtensions." + typeOfT.Name); //NOTE: to save, add "asdasd.dll" parameter

//                var interfaceType = typeof(IProxy);
//                var typeBuilder = moduleBuilder.DefineType(typeOfT.Name + "_" + Guid.NewGuid(),
//                    TypeAttributes.Public | TypeAttributes.Class);
//                typeBuilder.AddInterfaceImplementation(typeOfT);
//                typeBuilder.AddInterfaceImplementation(interfaceType);

//                //create our _isDirty field, which implements IProxy
//                var setIsDirtyMethod = CreateIsDirtyProperty(typeBuilder);

//                // Generate a field for each property, which implements the T
//                foreach (var property in typeof(T).GetProperties())
//                {
//                    var isId = property.GetCustomAttributes(true).Any(a => a is KeyAttribute);
//                    CreateProperty<T>(typeBuilder, property.Name, property.PropertyType, setIsDirtyMethod, isId);
//                }

//#if COREFX
//                var generatedType = typeBuilder.CreateTypeInfo().AsType();
//#else
//                var generatedType = typeBuilder.CreateType();
//#endif

//                TypeCache.Add(typeOfT, generatedType);
//                return (T)Activator.CreateInstance(generatedType);
//            }


//            private static MethodInfo CreateIsDirtyProperty(TypeBuilder typeBuilder)
//            {
//                var propType = typeof(bool);
//                var field = typeBuilder.DefineField("_" + "IsDirty", propType, FieldAttributes.Private);
//                var property = typeBuilder.DefineProperty("IsDirty",
//                                               System.Reflection.PropertyAttributes.None,
//                                               propType,
//                                               new[] { propType });

//                const MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.NewSlot | MethodAttributes.SpecialName |
//                                                    MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig;

//                // Define the "get" and "set" accessor methods
//                var currGetPropMthdBldr = typeBuilder.DefineMethod("get_" + "IsDirty",
//                                             getSetAttr,
//                                             propType,
//                                             Type.EmptyTypes);
//                var currGetIl = currGetPropMthdBldr.GetILGenerator();
//                currGetIl.Emit(OpCodes.Ldarg_0);
//                currGetIl.Emit(OpCodes.Ldfld, field);
//                currGetIl.Emit(OpCodes.Ret);
//                var currSetPropMthdBldr = typeBuilder.DefineMethod("set_" + "IsDirty",
//                                             getSetAttr,
//                                             null,
//                                             new[] { propType });
//                var currSetIl = currSetPropMthdBldr.GetILGenerator();
//                currSetIl.Emit(OpCodes.Ldarg_0);
//                currSetIl.Emit(OpCodes.Ldarg_1);
//                currSetIl.Emit(OpCodes.Stfld, field);
//                currSetIl.Emit(OpCodes.Ret);

//                property.SetGetMethod(currGetPropMthdBldr);
//                property.SetSetMethod(currSetPropMthdBldr);
//                var getMethod = typeof(IProxy).GetMethod("get_" + "IsDirty");
//                var setMethod = typeof(IProxy).GetMethod("set_" + "IsDirty");
//                typeBuilder.DefineMethodOverride(currGetPropMthdBldr, getMethod);
//                typeBuilder.DefineMethodOverride(currSetPropMthdBldr, setMethod);

//                return currSetPropMthdBldr;
//            }

//            private static void CreateProperty<T>(TypeBuilder typeBuilder, string propertyName, Type propType, MethodInfo setIsDirtyMethod, bool isIdentity)
//            {
//                //Define the field and the property 
//                var field = typeBuilder.DefineField("_" + propertyName, propType, FieldAttributes.Private);
//                var property = typeBuilder.DefineProperty(propertyName,
//                                               System.Reflection.PropertyAttributes.None,
//                                               propType,
//                                               new[] { propType });

//                const MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.Virtual |
//                                                    MethodAttributes.HideBySig;

//                // Define the "get" and "set" accessor methods
//                var currGetPropMthdBldr = typeBuilder.DefineMethod("get_" + propertyName,
//                                             getSetAttr,
//                                             propType,
//                                             Type.EmptyTypes);

//                var currGetIl = currGetPropMthdBldr.GetILGenerator();
//                currGetIl.Emit(OpCodes.Ldarg_0);
//                currGetIl.Emit(OpCodes.Ldfld, field);
//                currGetIl.Emit(OpCodes.Ret);

//                var currSetPropMthdBldr = typeBuilder.DefineMethod("set_" + propertyName,
//                                             getSetAttr,
//                                             null,
//                                             new[] { propType });

//                //store value in private field and set the isdirty flag
//                var currSetIl = currSetPropMthdBldr.GetILGenerator();
//                currSetIl.Emit(OpCodes.Ldarg_0);
//                currSetIl.Emit(OpCodes.Ldarg_1);
//                currSetIl.Emit(OpCodes.Stfld, field);
//                currSetIl.Emit(OpCodes.Ldarg_0);
//                currSetIl.Emit(OpCodes.Ldc_I4_1);
//                currSetIl.Emit(OpCodes.Call, setIsDirtyMethod);
//                currSetIl.Emit(OpCodes.Ret);

//                //TODO: Should copy all attributes defined by the interface?
//                if (isIdentity)
//                {
//                    var keyAttribute = typeof(KeyAttribute);
//                    var myConstructorInfo = keyAttribute.GetConstructor(new Type[] { });
//                    var attributeBuilder = new CustomAttributeBuilder(myConstructorInfo, new object[] { });
//                    property.SetCustomAttribute(attributeBuilder);
//                }

//                property.SetGetMethod(currGetPropMthdBldr);
//                property.SetSetMethod(currSetPropMthdBldr);
//                var getMethod = typeof(T).GetMethod("get_" + propertyName);
//                var setMethod = typeof(T).GetMethod("set_" + propertyName);
//                typeBuilder.DefineMethodOverride(currGetPropMthdBldr, getMethod);
//                typeBuilder.DefineMethodOverride(currSetPropMthdBldr, setMethod);
//            }
//        }
//    }

//    [AttributeUsage(AttributeTargets.Class)]
//    public class TableAttribute : Attribute
//    {
//        public TableAttribute(string tableName)
//        {
//            Name = tableName;
//        }

//        // ReSharper disable once MemberCanBePrivate.Global
//        // ReSharper disable once UnusedAutoPropertyAccessor.Global
//        public string Name { get; set; }
//    }

//    // do not want to depend on data annotations that is not in client profile
//    [AttributeUsage(AttributeTargets.Property)]
//    public class KeyAttribute : Attribute
//    {
//    }

//    [AttributeUsage(AttributeTargets.Property)]
//    public class ExplicitKeyAttribute : Attribute
//    {
//    }

//    [AttributeUsage(AttributeTargets.Property)]
//    public class WriteAttribute : Attribute
//    {
//        public WriteAttribute(bool write)
//        {
//            Write = write;
//        }
//        public bool Write { get; }
//    }

//    [AttributeUsage(AttributeTargets.Property)]
//    public class ComputedAttribute : Attribute
//    {
//    }

//    public static class TypeExtensions
//    {
//        public static bool IsInterface(this Type type)
//        {
//#if COREFX
//            return type.GetTypeInfo().IsInterface;
//#else
//            return type.IsInterface;
//#endif
//        }

//        public static bool IsGenericType(this Type type)
//        {
//#if COREFX
//            return type.GetTypeInfo().IsGenericType;
//#else
//            return type.IsGenericType;
//#endif
//        }
//    }
//}

//public partial interface ISqlAdapter
//{
//    int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert);

//    //new methods for issue #336
//    void AppendColumnName(StringBuilder sb, string columnName);
//    void AppendColumnNameEqualsValue(StringBuilder sb, string columnName);
//}

//public partial class SqlServerAdapter : ISqlAdapter
//{
//    public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
//    {
//        var cmd = $"insert into {tableName} ({columnList}) values ({parameterList});select SCOPE_IDENTITY() id";
//        var multi = connection.QueryMultiple(cmd, entityToInsert, transaction, commandTimeout);

//        var first = multi.Read().FirstOrDefault();
//        if (first == null || first.id == null) return 0;

//        var id = (int)first.id;
//        var propertyInfos = keyProperties as PropertyInfo[] ?? keyProperties.ToArray();
//        if (!propertyInfos.Any()) return id;

//        var idProperty = propertyInfos.First();
//        idProperty.SetValue(entityToInsert, Convert.ChangeType(id, idProperty.PropertyType), null);

//        return id;
//    }

//    public void AppendColumnName(StringBuilder sb, string columnName)
//    {
//        sb.AppendFormat("[{0}]", columnName);
//    }

//    public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
//    {
//        sb.AppendFormat("[{0}] = @{1}", columnName, columnName);
//    }
//}

//public partial class SqlCeServerAdapter : ISqlAdapter
//{
//    public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
//    {
//        var cmd = $"insert into {tableName} ({columnList}) values ({parameterList})";
//        connection.Execute(cmd, entityToInsert, transaction, commandTimeout);
//        var r = connection.Query("select @@IDENTITY id", transaction: transaction, commandTimeout: commandTimeout).ToList();

//        if (r.First().id == null) return 0;
//        var id = (int)r.First().id;

//        var propertyInfos = keyProperties as PropertyInfo[] ?? keyProperties.ToArray();
//        if (!propertyInfos.Any()) return id;

//        var idProperty = propertyInfos.First();
//        idProperty.SetValue(entityToInsert, Convert.ChangeType(id, idProperty.PropertyType), null);

//        return id;
//    }

//    public void AppendColumnName(StringBuilder sb, string columnName)
//    {
//        sb.AppendFormat("[{0}]", columnName);
//    }

//    public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
//    {
//        sb.AppendFormat("[{0}] = @{1}", columnName, columnName);
//    }
//}

//public partial class MySqlAdapter : ISqlAdapter
//{
//    public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
//    {
//        var cmd = $"insert into {tableName} ({columnList}) values ({parameterList})";
//        connection.Execute(cmd, entityToInsert, transaction, commandTimeout);
//        var r = connection.Query("Select LAST_INSERT_ID() id", transaction: transaction, commandTimeout: commandTimeout);

//        var id = r.First().id;
//        if (id == null) return 0;
//        var propertyInfos = keyProperties as PropertyInfo[] ?? keyProperties.ToArray();
//        if (!propertyInfos.Any()) return Convert.ToInt32(id);

//        var idp = propertyInfos.First();
//        idp.SetValue(entityToInsert, Convert.ChangeType(id, idp.PropertyType), null);

//        return Convert.ToInt32(id);
//    }

//    public void AppendColumnName(StringBuilder sb, string columnName)
//    {
//        sb.AppendFormat("`{0}`", columnName);
//    }

//    public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
//    {
//        sb.AppendFormat("`{0}` = @{1}", columnName, columnName);
//    }
//}

//public partial class PostgresAdapter : ISqlAdapter
//{
//    public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
//    {
//        var sb = new StringBuilder();
//        sb.AppendFormat("insert into {0} ({1}) values ({2})", tableName, columnList, parameterList);

//        // If no primary key then safe to assume a join table with not too much data to return
//        var propertyInfos = keyProperties as PropertyInfo[] ?? keyProperties.ToArray();
//        if (!propertyInfos.Any())
//            sb.Append(" RETURNING *");
//        else
//        {
//            sb.Append(" RETURNING ");
//            var first = true;
//            foreach (var property in propertyInfos)
//            {
//                if (!first)
//                    sb.Append(", ");
//                first = false;
//                sb.Append(property.Name);
//            }
//        }

//        var results = connection.Query(sb.ToString(), entityToInsert, transaction, commandTimeout: commandTimeout).ToList();

//        // Return the key by assinging the corresponding property in the object - by product is that it supports compound primary keys
//        var id = 0;
//        foreach (var p in propertyInfos)
//        {
//            var value = ((IDictionary<string, object>)results.First())[p.Name.ToLower()];
//            p.SetValue(entityToInsert, value, null);
//            if (id == 0)
//                id = Convert.ToInt32(value);
//        }
//        return id;
//    }

//    public void AppendColumnName(StringBuilder sb, string columnName)
//    {
//        sb.AppendFormat("\"{0}\"", columnName);
//    }

//    public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
//    {
//        sb.AppendFormat("\"{0}\" = @{1}", columnName, columnName);
//    }
//}

//public partial class SQLiteAdapter : ISqlAdapter
//{
//    public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
//    {
//        var cmd = $"INSERT INTO {tableName} ({columnList}) VALUES ({parameterList}); SELECT last_insert_rowid() id";
//        var multi = connection.QueryMultiple(cmd, entityToInsert, transaction, commandTimeout);

//        var id = (int)multi.Read().First().id;
//        var propertyInfos = keyProperties as PropertyInfo[] ?? keyProperties.ToArray();
//        if (!propertyInfos.Any()) return id;

//        var idProperty = propertyInfos.First();
//        idProperty.SetValue(entityToInsert, Convert.ChangeType(id, idProperty.PropertyType), null);

//        return id;
//    }

//    public void AppendColumnName(StringBuilder sb, string columnName)
//    {
//        sb.AppendFormat("\"{0}\"", columnName);
//    }

//    public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
//    {
//        sb.AppendFormat("\"{0}\" = @{1}", columnName, columnName);
//    }
//}

//public partial class FbAdapter : ISqlAdapter
//{
//    public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
//    {
//        var cmd = $"insert into {tableName} ({columnList}) values ({parameterList})";
//        connection.Execute(cmd, entityToInsert, transaction, commandTimeout);

//        var propertyInfos = keyProperties as PropertyInfo[] ?? keyProperties.ToArray();
//        var keyName = propertyInfos.First().Name;
//        var r = connection.Query($"SELECT FIRST 1 {keyName} ID FROM {tableName} ORDER BY {keyName} DESC", transaction: transaction, commandTimeout: commandTimeout);

//        var id = r.First().ID;
//        if (id == null) return 0;
//        if (!propertyInfos.Any()) return Convert.ToInt32(id);

//        var idp = propertyInfos.First();
//        idp.SetValue(entityToInsert, Convert.ChangeType(id, idp.PropertyType), null);

//        return Convert.ToInt32(id);
//    }

//    public void AppendColumnName(StringBuilder sb, string columnName)
//    {
//        sb.AppendFormat("{0}", columnName);
//    }

//    public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
//    {
//        sb.AppendFormat("{0} = @{1}", columnName, columnName);
//    }
//}
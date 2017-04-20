using Dapper;
using LinFx.Data.Provider;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace LinFx.Data
{
    public static class DbConnectionExtensions
    {
        public static Task InsertAsync<T>(this IDbConnection conn, T item, IDbTransaction transaction = null) where T : class
        {
            return Task.FromResult(SqlMapperExtensions.Insert(conn, item));
        }

        public static Task UpdateAsync<T>(this IDbConnection conn, T item, IDbTransaction transaction = null) where T : class
        {
            return Task.FromResult(SqlMapperExtensions.Update(conn, item));
        }

        public static Task DeleteAsync<T>(this IDbConnection conn, T item, IDbTransaction transaction = null) where T : class
        {
            return Task.FromResult(SqlMapperExtensions.Delete(conn, item));
        }

        public static Task<IEnumerable<T>> SelectAsync<T>(this IDbConnection conn, string sql, object param = null, IDbTransaction transaction = null)
        {
            return conn.QueryAsync<T>(sql, param, transaction);
        }

        public static Task<T> GetAsync<T>(this IDbConnection conn, dynamic id, IDbTransaction transaction = null) where T : class
        {
            return Task.FromResult(SqlMapperExtensions.Get<T>(conn, id, transaction));
        }
    }
}
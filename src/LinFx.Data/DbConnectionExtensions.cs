using Dapper;
using System.Collections.Generic;
using System.Data;

namespace LinFx.Data
{
    public static class DbConnectionExtensions
    {
        public static IEnumerable<T> Select<T>(this IDbConnection conn, string sql, object param = null, IDbTransaction transaction = null)
        {
            return conn.Query<T>(sql, param, transaction);
        }

        public static void Insert<T>(this IDbConnection conn, T item, IDbTransaction transaction = null) where T : class
        {
            var ret = SqlMapperExtensions.Insert(conn, item);
        }

        public static void Update<T>(this IDbConnection conn, T item, IDbTransaction transaction = null) where T : class
        {
            SqlMapperExtensions.Update(conn, item);
        }
    }
}
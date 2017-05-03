using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using LinFx.Data.Dapper.Extensions;
using System;

namespace LinFx.Data
{
    public static class DbConnectionExtensions
    {
        public static Task InsertAsync<T>(this IDbConnection conn, T item, IDbTransaction transaction = null) where T : class
        {
            return Task.FromResult(conn.Insert(item, transaction));
        }

        public static Task<bool> UpdateAsync<T>(this IDbConnection conn, T item, IDbTransaction transaction = null) where T : class
        {
            return Task.FromResult(conn.Update(item, transaction));
        }

        public static Task DeleteAsync<T>(this IDbConnection conn, T item, IDbTransaction transaction = null) where T : class
        {
            return Task.FromResult(conn.Delete(item, transaction));
        }

        //public static Task<IEnumerable<T>> SelectAsync<T>(this IDbConnection conn, string sql, object param = null, IDbTransaction transaction = null)
        //{
        //    return conn.GetSet<>
        //}

        public static Task<T> GetAsync<T>(this IDbConnection conn, dynamic id, IDbTransaction transaction = null) where T : class
        {
            return Task.FromResult(DapperExtensions.Get<T>(conn, id, transaction));
        }
    }
}
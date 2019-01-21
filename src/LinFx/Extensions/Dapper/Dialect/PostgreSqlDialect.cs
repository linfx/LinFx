using System.Collections.Generic;

namespace LinFx.Extensions.Dapper.Dialect
{
    public class PostgreSqlDialect : SqlDialectBase
    {
        public override string GetIdentitySql(string tableName)
        {
            return "SELECT LASTVAL() AS Id";
        }

        public override string GetPagingSql(string sql, uint page, uint resultsPerPage, IDictionary<string, object> parameters)
        {
            uint offset = (page - 1) * resultsPerPage;
            return GetSetSql(sql, resultsPerPage, offset, parameters);
        }

        public override string GetSetSql(string sql, uint limit, uint offset, IDictionary<string, object> parameters)
        {
            string result = string.Format("{0} LIMIT @limit OFFSET @offset ", sql);
            parameters.Add("@limit", limit);
            parameters.Add("@offset", offset);
            return result;
        }

        public override string GetColumnName(string prefix, string columnName, string alias)
        {
            return base.GetColumnName(null, columnName, alias).ToLower();
        }

        public override string GetTableName(string schemaName, string tableName, string alias)
        {
            return base.GetTableName(schemaName, tableName, alias).ToLower();
        }
    }

}
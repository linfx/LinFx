using Npgsql;
using System.Data;

namespace LinFx.Data.Provider.PostgreSql
{
    public class PostgreSqlProvider : IDbProvider
    {
        public static IDbProvider Instance => new PostgreSqlProvider();

        public IDbConnection CreateConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }

        public override string ToString()
        {
            return nameof(NpgsqlConnection);
        }
    }
}

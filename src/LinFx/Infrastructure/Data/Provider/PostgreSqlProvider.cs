using Npgsql;
using System.Data;

namespace LinFx.Infrastructure.Data.Provider
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
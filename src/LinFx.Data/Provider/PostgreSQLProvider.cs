using Npgsql;
using System.Data;

namespace LinFx.Data.Provider
{
    public class PostgreSQLProvider : IDbProvider
    {
        public static IDbProvider Instance => new PostgreSQLProvider();

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

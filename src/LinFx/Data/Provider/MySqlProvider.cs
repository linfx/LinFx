using System.Data;
using LinFx.Data.Abstractions;
using MySql.Data.MySqlClient;

namespace LinFx.Extensions.Data.Provider
{
    public class MySqlProvider : IDbProvider
    {
        public static IDbProvider Instance => new MySqlProvider();

        public IDbConnection CreateConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        public override string ToString()
        {
            return nameof(MySqlConnection);
        }
    }
}
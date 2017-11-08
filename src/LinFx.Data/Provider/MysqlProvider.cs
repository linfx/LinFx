using System.Data;
using MySql.Data.MySqlClient;

namespace LinFx.Data.Provider
{
    public class MySQLProvider : IDbProvider
    {
        public static IDbProvider Instance => new MySQLProvider();

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

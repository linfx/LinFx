using MySql.Data.MySqlClient;
using System.Data;

namespace LinFx.Data.Provider
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

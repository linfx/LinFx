using System.Data;
using MySql.Data.MySqlClient;

namespace LinFx.Data.Provider
{
    public class MysqlProvider : IDbProvider
    {
        public static IDbProvider Instance => new MysqlProvider();

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

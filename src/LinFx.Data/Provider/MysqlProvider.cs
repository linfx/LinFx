using System.Data;
using MySql.Data.MySqlClient;

namespace LinFx.Data.Provider
{
    public class MysqlSqlProvider : IDbProvider
    {
        public static IDbProvider Instance => new MysqlSqlProvider();

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

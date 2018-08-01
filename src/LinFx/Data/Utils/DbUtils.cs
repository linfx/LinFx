using LinFx.Extensions.DapperExtensions;
using LinFx.Extensions.DapperExtensions.Mapper;
using LinFx.Extensions.DapperExtensions.Sql;
using LinFx.Data.Provider;
using System.Collections.Generic;
using System.Reflection;

namespace LinFx.Data.Utils
{
    public static class DbUtils
    {
        public static IDatabase GetPostgreSqlDatabase(string connectionString)
        {
            var factory = new DbConnectionFactory(connectionString, PostgreSqlProvider.Instance);
            var config = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new PostgreSqlDialect());
            return new Database(factory.Create(), new SqlGeneratorImpl(config));
        }

        public static IDatabase GetMySqlDatabase(string connectionString)
        {
            var factory = new DbConnectionFactory(connectionString, MySqlProvider.Instance);
            var config = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new MySqlDialect());
            return new Database(factory.Create(), new SqlGeneratorImpl(config));
        }
    }
}
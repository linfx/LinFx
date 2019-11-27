using LinFx;
using LinFx.Data;
using LinFx.Extensions.Dapper;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DataServiceCollectionExtensions
    {
        public static LinFxBuilder AddDatabase(this LinFxBuilder builder, Action<DbConnectionOptions> optionsAction)
        {
            Check.NotNull(builder, nameof(builder));

            //var factory = new DbConnectionFactory(connectionString, PostgreSqlProvider.Instance);
            //var config = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new PostgreSqlDialect());
            //var db = new Database(factory.Create(), new SqlGeneratorImpl(config));

            //var factory = new DbConnectionFactory(connectionString, MySqlProvider.Instance);
            //var config = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new MySqlDialect());
            //return new Database(factory.Create(), new SqlGeneratorImpl(config));

            builder.Services.AddTransient<IDatabase, Database>();

            return builder;
        }
    }
}

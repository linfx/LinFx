using LinFx.Data;
using LinFx.Data.Provider;
using System;
using Xunit;
using LinFx.Data.Dapper.Extensions;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using Shouldly;
using System.Linq;
using LinFx.Data.Dapper.Extensions.Sql;
using LinFx.Data.Dapper.Extensions.Mapper;

namespace LinFx.UnitTest.Data.Dapper
{
    public class Test
    {
        const string connectionString = "server=10.10.10.183;database=test;uid=postgres;pwd=123456;pooling=true;";

        public void UsingDbConnectionFactory(Action<IDbConnection> action)
        {
            using (var factory = new DbConnectionFactory(connectionString, PostgreSqlProvider.Instance))
            {
                using (var conn = factory.Open())
                {
                    action(conn);
                }
            }
        }

        [Fact]
        public void Test1()
        {
            //DapperExtensions.SetMappingAssemblies(new List<Assembly> { Assembly.GetEntryAssembly() });
            //UsingDbConnectionFactory(db =>
            //{
            //    var r = db.GetList<User>().ToList();
            //    r.Count().ShouldNotBe(0);
            //});

            var factory = new DbConnectionFactory(connectionString, PostgreSqlProvider.Instance);
            var config = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new PostgreSqlDialect());
            IDatabase db2 = new Database(factory.Create(), new SqlGeneratorImpl(config));

            db2.RunInTransaction(() =>
            {
                var id = db2.Insert(new User
                {
                    Name = "New1"
                });

                //db2.Insert(new UserEx
                //{
                //    Id = id,
                //    NameEx = "New1Ex"
                //});
            });
            //var users = db2.GetList<User>();
        }
    }


}

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

namespace LinFx.UnitTest.Data.Dapper
{
    public class Test
    {
        public void UsingDbConnectionFactory(Action<IDbConnection> action)
        {
            //const string connectionString = "server=db.wayto.com.cn;database=test;uid=root;pwd=Wayto2017!;Charset=utf8;";
            const string connectionString = "server=10.10.10.183;database=test;uid=postgres;pwd=123456;pooling=true;";
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
            DapperExtensions.SetMappingAssemblies(new List<Assembly> { Assembly.GetEntryAssembly() });

            UsingDbConnectionFactory(db =>
            {
                var r = db.GetList<Author>(p => p.Name == "Lin").ToList();


                r.Count().ShouldNotBe(0);
            });
        }
    }


}

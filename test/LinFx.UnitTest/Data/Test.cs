using LinFx.Data;
using LinFx.Data.Provider;
using System;
using Xunit;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using LinFx.Data.Extensions;
using LinFx.Data.Extensions.Mapper;
using LinFx.Data.Extensions.Sql;
using System.Linq;
using Shouldly;
using Dapper;

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
			DataAccessExtensions.SetMappingAssemblies(new List<Assembly> { Assembly.GetEntryAssembly() });
			UsingDbConnectionFactory(db =>
			{
				var r = db.Select<User>().ToList();
				r.Count().ShouldNotBe(0);

				var sql =
					@"select * from post as p 
						left join #user as u on u.id = p.ownerid 
						Order by p.Id";

				var data = db.Query<Post, User, Post>(sql, (p, user) => { p.Owner = user; return p; });
				var post = data.First();
			});

			//var factory = new DbConnectionFactory(connectionString, PostgreSqlProvider.Instance);
			//var config = new DataAccessExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new PostgreSqlDialect());
			//IDatabase db2 = new Database(factory.Create(), new SqlGeneratorImpl(config));


			//db2.RunInTransaction(() =>
			//{
			//    //var id = db2.Insert(new User
			//    //{
			//    //    Name = "New1"
			//    //});

			//    //db2.Insert(new UserEx
			//    //{
			//    //    Id = id,
			//    //    NameEx = "New1Ex"
			//    //});
			//});
			////var users = db2.GetList<User>();
		}
    }


}

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
        const string connectionString = "server=db.wayto.com.cn;database=test;uid=sa;pwd=123456;pooling=true;";

        public void UsingDbConnectionFactory(Action<IDbConnection> action)
        {
            using (var factory = new DbConnectionFactory(connectionString, PostgreSQLProvider.Instance))
            {
                using (var conn = factory.Open())
                {
                    action(conn);
                }
            }
        }

		public void UsingDataBase(Action<IDatabase> action)
		{
			var factory = new DbConnectionFactory(connectionString, PostgreSQLProvider.Instance);
			var config = new DataAccessExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new PostgreSqlDialect());
			using(IDatabase db = new Database(factory.Create(), new SqlGeneratorImpl(config)))
			{
				action(db);
			}
 		}

		//public void InsertTest()
		//{
		//	UsingDataBase(db =>
		//	{
		//		db.Insert(new Post { Title = "titl", Content = "222" });
		//	});
		//}

        [Fact]
        public void Test1()
        {
			var factory = new DbConnectionFactory(connectionString, PostgreSQLProvider.Instance);
			var config = new DataAccessExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new PostgreSqlDialect());
			IDatabase db = new Database(factory.Create(), new SqlGeneratorImpl(config));

			var posts = db.Select<Post>();
			var p = db.Select<Post>(x => x.Id == 1);

			//UsingDbConnectionFactory(db =>
			//{
			//	//var r = db.Select<User>().ToList();
			//	//r.Count().ShouldNotBe(0);

			//	var sql =@"
			//			select * from post as p 
			//			left join #user as u on u.id = p.ownerid 
			//			Order by p.Id";

			//	var data = db.Query<Post, User, Post>(sql, (p, user) => { p.Owner = user; return p; });
			//	var post = data.First();
			//});

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

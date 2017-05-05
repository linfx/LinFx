using LinFx.Authorization.Accounts;
using LinFx.Data;
using LinFx.Data.Dapper.Repositories;
using LinFx.Data.Provider;
using LinFx.Domain.Entities;
using Shouldly;
using System;
using Xunit;

namespace LinFx.UnitTest.Data.Dapper
{
    class Test
    {
        public void UsingDbConnectionFactory(Action<DbConnectionFactory> action)
        {
            const string connectionString = "server=db.wayto.com.cn;database=test;uid=root;pwd=Wayto2017!;Charset=utf8;";
            using (var factory = new DbConnectionFactory(connectionString, MySqlProvider.Instance))
            {
                action(factory);
            }
        }

        public void UsingRepository<TEntity>(Action<DapperRepository<TEntity>> action) where TEntity : class, IEntity<string>
        {
            UsingDbConnectionFactory(factory =>
            {
                action(new DapperRepository<TEntity>(factory));
            });
        }

        [Fact]
        public void Test1()
        {
            UsingRepository<Account>(repository =>
            {
                repository.Count(p => p.UserName == "1").ShouldBe(1);
            });

        }
    }
}

using LinFx.Data;
using LinFx.Data.Dapper.Repositories;
using LinFx.Data.Provider;
using LinFx.Domain.Entities;
using System;

namespace LinFx.SaaS.UnitTest
{
    public class UnitTestBase
    {
        public void UsingDbConnectionFactory(Action<DbConnectionFactory> action)
        {
            const string connectionString = "server=db.wayto.com.cn;database=test;uid=root;pwd=Wayto2017!;Charset=utf8;";
            using (var factory = new DbConnectionFactory(connectionString, MySqlProvider.Instance))
            {
                action(factory);
            }
        }

        public void UsingRepository<TEntity>(Action<IRepository<TEntity>> action) where TEntity : class, IEntity<string>
        {
            UsingDbConnectionFactory(factory =>
            {
                action(new DapperRepository<TEntity>(factory));
            });
        }
    }
}

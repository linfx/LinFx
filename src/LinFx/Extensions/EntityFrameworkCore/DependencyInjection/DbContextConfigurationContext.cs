using JetBrains.Annotations;
using LinFx.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Common;

namespace LinFx.Extensions.EntityFrameworkCore.DependencyInjection
{
    /// <summary>
    /// 配置上下文
    /// </summary>
    public class DbContextConfigurationContext : IServiceProviderAccessor
    {
        public IServiceProvider ServiceProvider { get; }

        public string ConnectionString { get; }

        public string ConnectionStringName { get; }

        public DbConnection ExistingConnection { get; }

        /// <summary>
        /// 数据库上下文配置
        /// </summary>
        public DbContextOptionsBuilder DbContextOptions { get; protected set; }

        public DbContextConfigurationContext(
            [NotNull] string connectionString,
            [NotNull] IServiceProvider serviceProvider,
            [CanBeNull] string connectionStringName,
            [CanBeNull] DbConnection existingConnection)
        {
            ConnectionString = connectionString;
            ServiceProvider = serviceProvider;
            ConnectionStringName = connectionStringName;
            ExistingConnection = existingConnection;

            DbContextOptions = new DbContextOptionsBuilder()
                .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
                .UseApplicationServiceProvider(serviceProvider);
        }
    }

    public class DbContextConfigurationContext<TDbContext> : DbContextConfigurationContext
        where TDbContext : EfCoreDbContext
    {
        public new DbContextOptionsBuilder<TDbContext> DbContextOptions => (DbContextOptionsBuilder<TDbContext>)base.DbContextOptions;

        public DbContextConfigurationContext(
            string connectionString,
            [NotNull] IServiceProvider serviceProvider,
            [CanBeNull] string connectionStringName,
            [CanBeNull] DbConnection existingConnection)
            : base(
                  connectionString,
                  serviceProvider,
                  connectionStringName,
                  existingConnection)
        {
            base.DbContextOptions = new DbContextOptionsBuilder<TDbContext>()
                .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
                .UseApplicationServiceProvider(serviceProvider); ;
        }
    }
}

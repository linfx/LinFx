using JetBrains.Annotations;
using LinFx.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace LinFx.Extensions.EntityFrameworkCore.DependencyInjection;

/// <summary>
/// 数据库配置上下文
/// </summary>
public class DbContextConfigurationContext : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// 连接字符串
    /// </summary>
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

/// <summary>
/// 数据库配置上下文
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
public class DbContextConfigurationContext<TDbContext> : DbContextConfigurationContext
    where TDbContext : DbContext
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

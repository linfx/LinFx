using LinFx.Extensions.Data;
using LinFx.Extensions.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace LinFx.Extensions.EntityFrameworkCore.DependencyInjection;

/// <summary>
/// 数据库上下文配置工厂
/// </summary>
public static class DbContextOptionsFactory
{
    /// <summary>
    /// 创建数据库上下文配置对象
    /// </summary>
    /// <typeparam name="TDbContext">数据库上下文</typeparam>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public static DbContextOptions<TDbContext> Create<TDbContext>(IServiceProvider serviceProvider)
        where TDbContext : DbContext
    {
        // 获取一个 DbContextCreationContext 对象
        var creationContext = GetCreationContext<TDbContext>(serviceProvider);

        // 依据 creationContext 信息构造一个配置上下文
        var context = new DbContextConfigurationContext<TDbContext>(
            creationContext.ConnectionString,
            serviceProvider,
            creationContext.ConnectionStringName,
            creationContext.ExistingConnection
        );

        // 获取 DbOptions 配置
        var options = GetDbContextOptions<TDbContext>(serviceProvider);

        PreConfigure(options, context);
        Configure(options, context);

        return context.DbContextOptions.Options;
    }

    private static void PreConfigure<TDbContext>(
        EfCoreDbContextOptions options,
        DbContextConfigurationContext<TDbContext> context)
        where TDbContext : DbContext
    {
        foreach (var defaultPreConfigureAction in options.DefaultPreConfigureActions)
        {
            defaultPreConfigureAction.Invoke(context);
        }

        var preConfigureActions = options.PreConfigureActions.GetOrDefault(typeof(TDbContext));
        if (!preConfigureActions.IsNullOrEmpty())
        {
            foreach (var preConfigureAction in preConfigureActions)
            {
                ((Action<DbContextConfigurationContext<TDbContext>>)preConfigureAction).Invoke(context);
            }
        }
    }

    private static void Configure<TDbContext>(
        EfCoreDbContextOptions options,
        DbContextConfigurationContext<TDbContext> context)
        where TDbContext : DbContext
    {
        var configureAction = options.ConfigureActions.GetOrDefault(typeof(TDbContext));
        if (configureAction != null)
        {
            ((Action<DbContextConfigurationContext<TDbContext>>)configureAction).Invoke(context);
        }
        else if (options.DefaultConfigureAction != null)
        {
            options.DefaultConfigureAction.Invoke(context);
        }
        else
        {
            throw new Exception($"No configuration found for {typeof(DbContext).AssemblyQualifiedName}! Use services.Configure<AbpDbContextOptions>(...) to configure it.");
        }
    }

    /// <summary>
    /// 获取 DbOptions 配置
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    private static EfCoreDbContextOptions GetDbContextOptions<TDbContext>(IServiceProvider serviceProvider)
        where TDbContext : DbContext
    {
        return serviceProvider.GetRequiredService<IOptions<EfCoreDbContextOptions>>().Value;
    }

    private static DbContextCreationContext GetCreationContext<TDbContext>(IServiceProvider serviceProvider)
        where TDbContext : DbContext
    {
        // 优先从一个 AsyncLocal 当中获取。
        var context = DbContextCreationContext.Current;
        if (context != null)
            return context;

        // 从 TDbContext 的 ConnectionStringName 特性获取连接字符串名称。
        // 使用 IConnectionStringResolver 根据指定的名称获得连接字符串。
        var connectionStringName = ConnectionStringNameAttribute.GetConnStringName<TDbContext>();
        var connectionString = ResolveConnectionString<TDbContext>(serviceProvider, connectionStringName);

        // 构造一个新的 DbContextCreationContext 对象。
        return new DbContextCreationContext(
            connectionStringName,
            connectionString
        );
    }

    private static string ResolveConnectionString<TDbContext>(IServiceProvider serviceProvider, string connectionStringName)
    {
        // Use DefaultConnectionStringResolver.Resolve when we remove IConnectionStringResolver.Resolve
#pragma warning disable 618
        var connectionStringResolver = serviceProvider.GetRequiredService<IConnectionStringResolver>();
        var currentTenant = serviceProvider.GetRequiredService<ICurrentTenant>();

        // Multi-tenancy unaware contexts should always use the host connection string
        if (typeof(TDbContext).IsDefined(typeof(IgnoreMultiTenancyAttribute), false))
        {
            using (currentTenant.Change(null))
            {
                return connectionStringResolver.ResolveAsync(connectionStringName).Result;
            }
        }

        return connectionStringResolver.ResolveAsync(connectionStringName).Result;
//#pragma warning restore 618
    }
}

using LinFx.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace LinFx.Extensions.EntityFrameworkCore.DependencyInjection
{
    /// <summary>
    /// 数据库上下文配置工厂
    /// </summary>
    public static class DbContextOptionsFactory
    {
        public static DbContextOptions<TDbContext> Create<TDbContext>(IServiceProvider serviceProvider)
            where TDbContext : EfCoreDbContext
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
            //var options = GetDbContextOptions<TDbContext>(serviceProvider);

            //PreConfigure(options, context);
            //Configure(options, context);

            return context.DbContextOptions.Options;
        }

        private static void PreConfigure<TDbContext>(
            DbContextOptions options,
            DbContextConfigurationContext<TDbContext> context)
            where TDbContext : EfCoreDbContext
        {
            //foreach (var defaultPreConfigureAction in options.DefaultPreConfigureActions)
            //{
            //    defaultPreConfigureAction.Invoke(context);
            //}

            //var preConfigureActions = options.PreConfigureActions.GetOrDefault(typeof(TDbContext));
            //if (!preConfigureActions.IsNullOrEmpty())
            //{
            //    foreach (var preConfigureAction in preConfigureActions)
            //    {
            //        ((Action<DbContextConfigurationContext<TDbContext>>)preConfigureAction).Invoke(context);
            //    }
            //}
        }

        private static void Configure<TDbContext>(
            DbContextOptions options,
            DbContextConfigurationContext<TDbContext> context)
            where TDbContext : EfCoreDbContext
        {
            //var configureAction = options.ConfigureActions.GetOrDefault(typeof(TDbContext));
            //if (configureAction != null)
            //{
            //    ((Action<DbContextConfigurationContext<TDbContext>>)configureAction).Invoke(context);
            //}
            //else if (options.DefaultConfigureAction != null)
            //{
            //    options.DefaultConfigureAction.Invoke(context);
            //}
            //else
            //{
            //    throw new Exception(
            //        $"No configuration found for {typeof(DbContext).AssemblyQualifiedName}! Use services.Configure<AbpDbContextOptions>(...) to configure it.");
            //}
        }

        private static DbContextOptions GetDbContextOptions<TDbContext>(IServiceProvider serviceProvider)
            where TDbContext : EfCoreDbContext
        {
            return serviceProvider.GetRequiredService<IOptions<DbContextOptions>>().Value;
        }

        private static DbContextCreationContext GetCreationContext<TDbContext>(IServiceProvider serviceProvider)
            where TDbContext : EfCoreDbContext
        {
            var context = DbContextCreationContext.Current;
            if (context != null)
                return context;

            var connectionStringName = ConnectionStringNameAttribute.GetConnStringName<TDbContext>();
            var connectionString = ResolveConnectionString<TDbContext>(serviceProvider, connectionStringName);

            return new DbContextCreationContext(
                connectionStringName,
                connectionString
            );
        }

        private static string ResolveConnectionString<TDbContext>(IServiceProvider serviceProvider, string connectionStringName)
        {
            //            // Use DefaultConnectionStringResolver.Resolve when we remove IConnectionStringResolver.Resolve
            //#pragma warning disable 618
            //            var connectionStringResolver = serviceProvider.GetRequiredService<IConnectionStringResolver>();
            //            var currentTenant = serviceProvider.GetRequiredService<ICurrentTenant>();

            //            // Multi-tenancy unaware contexts should always use the host connection string
            //            if (typeof(TDbContext).IsDefined(typeof(IgnoreMultiTenancyAttribute), false))
            //            {
            //                using (currentTenant.Change(null))
            //                {
            //                    return connectionStringResolver.Resolve(connectionStringName);
            //                }
            //            }

            //            return connectionStringResolver.Resolve(connectionStringName);
            //#pragma warning restore 618

            throw new NotImplementedException();
        }
    }
}

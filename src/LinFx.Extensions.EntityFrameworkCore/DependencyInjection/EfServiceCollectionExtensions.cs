﻿using LinFx.Extensions.EntityFrameworkCore;
using LinFx.Extensions.EntityFrameworkCore.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class EfServiceCollectionExtensions
{
    public static IServiceCollection AddEfDbContext<TDbContext>(this IServiceCollection services, Action<IDbContextRegistrationOptionsBuilder>? optionsBuilder = null)
        where TDbContext : DbContext
    {
        services.AddMemoryCache();

        // 构造一个数据库注册配置对象
        var options = new DbContextRegistrationOptions(typeof(TDbContext), services);

        var replacedDbContextTypes = typeof(TDbContext)
            .GetCustomAttributes<ReplaceDbContextAttribute>(true)
            .SelectMany(x => x.ReplacedDbContextTypes)
            .ToList();

        foreach (var dbContextType in replacedDbContextTypes)
        {
            options.ReplaceDbContext(dbContextType);
        }

        // 回调传入的委托。
        optionsBuilder?.Invoke(options);

        // 注入指定 TDbContext 的 DbOptions<TDbContext> ，将会使用 Create<TDbContext> 方法进行瞬时对象构造。
        services.TryAddTransient(DbContextOptionsFactory.Create<TDbContext>);

        // 替换指定类型的 DbContext 为当前 TDbContext
        foreach (var entry in options.ReplacedDbContextTypes)
        {
            var originalDbContextType = entry.Key;
            var targetDbContextType = entry.Value ?? typeof(TDbContext);

            services.Replace(ServiceDescriptor.Transient(originalDbContextType, sp => sp.GetRequiredService(targetDbContextType)));

            services.Configure<EfDbContextOptions>(opts =>
            {
                opts.DbContextReplacements[originalDbContextType] = targetDbContextType;
            });
        }

        return services;
    }
}

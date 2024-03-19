using JetBrains.Annotations;
using LinFx.Extensions.EntityFrameworkCore.DependencyInjection;
using LinFx.Utils;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.EntityFrameworkCore;

/// <summary>
/// 数据库上下文选项
/// </summary>
public class EfDbContextOptions
{
    internal List<Action<DbContextConfigurationContext>> DefaultPreConfigureActions { get; } = [];

    internal Action<DbContextConfigurationContext>? DefaultConfigureAction { get; set; }

    internal Dictionary<Type, List<object>> PreConfigureActions { get; } = [];

    internal Dictionary<Type, object> ConfigureActions { get; } = [];

    internal Dictionary<Type, Type> DbContextReplacements { get; } = [];

    /// <summary>
    /// 预配置
    /// </summary>
    /// <param name="action"></param>
    public void PreConfigure([NotNull] Action<DbContextConfigurationContext> action) => DefaultPreConfigureActions.Add(action);

    /// <summary>
    /// 配置
    /// </summary>
    /// <param name="action"></param>
    public void Configure([NotNull] Action<DbContextConfigurationContext> action) => DefaultConfigureAction = action;

    /// <summary>
    /// 预配置
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="action"></param>
    public void PreConfigure<TDbContext>([NotNull] Action<DbContextConfigurationContext<TDbContext>> action)
        where TDbContext : DbContext
    {
        Check.NotNull(action, nameof(action));

        var actions = PreConfigureActions.GetOrDefault(typeof(TDbContext));
        if (actions == null)
            PreConfigureActions[typeof(TDbContext)] = actions = [];

        actions.Add(action);
    }

    /// <summary>
    /// 配置
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="action"></param>
    public void Configure<TDbContext>([NotNull] Action<DbContextConfigurationContext<TDbContext>> action) where TDbContext : DbContext => ConfigureActions[typeof(TDbContext)] = action;

    public bool IsConfiguredDefault() => DefaultConfigureAction != null;

    public bool IsConfigured<TDbContext>() => IsConfigured(typeof(TDbContext));

    public bool IsConfigured(Type dbContextType) => ConfigureActions.ContainsKey(dbContextType);

    internal Type GetReplacedTypeOrSelf(Type dbContextType)
    {
        var replacementType = dbContextType;
        while (true)
        {
            if (DbContextReplacements.TryGetValue(replacementType, out var foundType))
            {
                if (foundType == dbContextType)
                    throw new Exception("Circular DbContext replacement found for " + dbContextType.AssemblyQualifiedName);

                replacementType = foundType;
            }
            else
                return replacementType;
        }
    }
}

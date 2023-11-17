using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;

namespace LinFx.Extensions.Authorization.Permissions;

/// <summary>
/// 权限管理器
/// </summary>
public class PermissionDefinitionManager : IPermissionDefinitionManager, ISingletonDependency
{
    /// <summary>
    /// 权限
    /// </summary>
    protected IDictionary<string, PermissionDefinition> PermissionDefinitions => _lazyPermissionDefinitions.Value;
    private readonly Lazy<Dictionary<string, PermissionDefinition>> _lazyPermissionDefinitions;

    /// <summary>
    /// 权限组
    /// </summary>
    protected IDictionary<string, PermissionGroupDefinition> PermissionGroupDefinitions => _lazyPermissionGroupDefinitions.Value;
    private readonly Lazy<Dictionary<string, PermissionGroupDefinition>> _lazyPermissionGroupDefinitions;

    protected PermissionOptions Options { get; }

    private readonly IServiceProvider _serviceProvider;

    public PermissionDefinitionManager(
        IOptions<PermissionOptions> options,
        IServiceProvider serviceProvider)
    {
        Options = options.Value;
        _serviceProvider = serviceProvider;

        _lazyPermissionDefinitions = new Lazy<Dictionary<string, PermissionDefinition>>(CreatePermissionDefinitions, true);
        _lazyPermissionGroupDefinitions = new Lazy<Dictionary<string, PermissionGroupDefinition>>(CreatePermissionGroupDefinitions, true);
    }

    /// <summary>
    /// 创建权限定义
    /// </summary>
    /// <returns></returns>
    protected virtual Dictionary<string, PermissionDefinition> CreatePermissionDefinitions()
    {
        var permissions = new Dictionary<string, PermissionDefinition>();

        foreach (var groupDefinition in PermissionGroupDefinitions.Values)
        {
            foreach (var permission in groupDefinition.Permissions)
            {
                AddPermissionToDictionaryRecursively(permissions, permission);
            }
        }

        return permissions;
    }

    /// <summary>
    /// 创建权限组定义
    /// </summary>
    /// <returns></returns>
    protected virtual Dictionary<string, PermissionGroupDefinition> CreatePermissionGroupDefinitions()
    {
        using var scope = _serviceProvider.CreateScope();

        //  创建一个权限定义上下文。
        var context = new PermissionDefinitionContext();

        Options
            .DefinitionProviders
            .Select(p => scope.ServiceProvider.GetRequiredService(p) as IPermissionDefinitionProvider)
            .ToList()
            .ForEach(item => item.Define(context));

        // 返回权限组名称 - 权限组定义的字典。
        return context.Groups;
    }

    public virtual PermissionDefinition Get(string name)
    {
        var permission = GetOrNull(name);

        if (permission == null)
            throw new LinFxException("Undefined permission: " + name);

        return permission;
    }

    public virtual PermissionDefinition GetOrNull(string name)
    {
        if (name is null)
            throw new ArgumentNullException(nameof(name));

        return PermissionDefinitions.GetOrDefault(name);
    }

    public virtual IReadOnlyList<PermissionDefinition> GetPermissions() => PermissionDefinitions.Values.ToImmutableList();

    public IReadOnlyList<PermissionGroupDefinition> GetGroups() => PermissionGroupDefinitions.Values.ToImmutableList();

    protected virtual void AddPermissionToDictionaryRecursively(Dictionary<string, PermissionDefinition> permissions, PermissionDefinition permission)
    {
        if (permissions.ContainsKey(permission.Name))
            throw new LinFxException("Duplicate permission name: " + permission.Name);

        permissions[permission.Name] = permission;

        foreach (var child in permission.Children)
        {
            AddPermissionToDictionaryRecursively(permissions, child);
        }
    }
}

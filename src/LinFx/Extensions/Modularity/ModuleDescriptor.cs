using LinFx.Utils;
using System.Collections.Immutable;
using System.Reflection;

namespace LinFx.Extensions.Modularity;

/// <summary>
/// 模块描述
/// </summary>
public class ModuleDescriptor : IModuleDescriptor
{
    private readonly List<IModuleDescriptor> _dependencies;

    public Type Type { get; }

    public Assembly Assembly { get; }

    public IModule Instance { get; }

    public bool IsLoadedAsPlugIn { get; }

    public IReadOnlyList<IModuleDescriptor> Dependencies => _dependencies.ToImmutableList();

    public ModuleDescriptor(Type type, IModule instance, bool isLoadedAsPlugIn)
    {
        Check.NotNull(type, nameof(type));
        Check.NotNull(instance, nameof(instance));

        if (!type.GetTypeInfo().IsAssignableFrom(instance.GetType()))
            throw new ArgumentException($"Given module instance ({instance.GetType().AssemblyQualifiedName}) is not an instance of given module type: {type.AssemblyQualifiedName}");

        Type = type;
        Assembly = type.Assembly;
        Instance = instance;
        IsLoadedAsPlugIn = isLoadedAsPlugIn;
        _dependencies = [];
    }

    public void AddDependency(IModuleDescriptor descriptor) => _dependencies.AddIfNotContains(descriptor);

    public override string ToString() => $"[AbpModuleDescriptor {Type.FullName}]";
}

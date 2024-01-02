using JetBrains.Annotations;
using LinFx.Utils;
using System.Collections.Immutable;
using System.Reflection;

namespace LinFx.Extensions.Modularity;

public class ModuleDescriptor : IModuleDescriptor
{
    public Type Type { get; }

    public Assembly Assembly { get; }

    public IModule Instance { get; }

    public bool IsLoadedAsPlugIn { get; }

    public IReadOnlyList<IModuleDescriptor> Dependencies => _dependencies.ToImmutableList();
    private readonly List<IModuleDescriptor> _dependencies;

    public ModuleDescriptor(
        [NotNull] Type type,
        [NotNull] IModule instance,
        bool isLoadedAsPlugIn)
    {
        Check.NotNull(type, nameof(type));
        Check.NotNull(instance, nameof(instance));

        if (!type.GetTypeInfo().IsAssignableFrom(instance.GetType()))
        {
            throw new ArgumentException($"Given module instance ({instance.GetType().AssemblyQualifiedName}) is not an instance of given module type: {type.AssemblyQualifiedName}");
        }

        Type = type;
        Assembly = type.Assembly;
        Instance = instance;
        IsLoadedAsPlugIn = isLoadedAsPlugIn;

        _dependencies = new List<IModuleDescriptor>();
    }

    public void AddDependency(IModuleDescriptor descriptor)
    {
        _dependencies.AddIfNotContains(descriptor);
    }

    public override string ToString()
    {
        return $"[AbpModuleDescriptor {Type.FullName}]";
    }
}

using System.Reflection;

namespace LinFx.Extensions.Modularity;

/// <summary>
/// 模块描述
/// </summary>
public interface IModuleDescriptor
{
    Type Type { get; }

    Assembly Assembly { get; }

    IModule Instance { get; }

    bool IsLoadedAsPlugIn { get; }

    IReadOnlyList<IModuleDescriptor> Dependencies { get; }
}

using JetBrains.Annotations;

namespace LinFx.Extensions.Modularity;

public interface IModuleContainer
{
    [NotNull]
    IReadOnlyList<IModuleDescriptor> Modules { get; }
}

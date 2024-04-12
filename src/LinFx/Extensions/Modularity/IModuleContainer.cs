using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.Modularity;

public interface IModuleContainer
{
    [NotNull]
    IReadOnlyList<IModuleDescriptor> Modules { get; }
}

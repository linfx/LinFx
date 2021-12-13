using JetBrains.Annotations;
using System.Collections.Generic;

namespace LinFx.Extensions.Modularity;

public interface IModuleContainer
{
    [NotNull]
    IReadOnlyList<IModuleDescriptor> Modules { get; }
}

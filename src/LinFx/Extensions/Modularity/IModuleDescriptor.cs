using System;
using System.Collections.Generic;
using System.Reflection;

namespace LinFx.Extensions.Modularity;

public interface IModuleDescriptor
{
    Type Type { get; }

    Assembly Assembly { get; }

    IModule Instance { get; }

    bool IsLoadedAsPlugIn { get; }

    IReadOnlyList<IModuleDescriptor> Dependencies { get; }
}

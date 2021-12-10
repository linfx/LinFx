using JetBrains.Annotations;
using System;

namespace LinFx.Extensions.Modularity.PlugIns;

public interface IPlugInSource
{
    [NotNull]
    Type[] GetModules();
}
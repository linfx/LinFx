using JetBrains.Annotations;
using LinFx.Extensions.Modularity.PlugIns;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LinFx.Extensions.Modularity;

public interface IModuleLoader
{
    [NotNull]
    IModuleDescriptor[] LoadModules(
        [NotNull] IServiceCollection services,
        [NotNull] Type startupModuleType,
        [NotNull] PlugInSourceList plugInSources
    );
}

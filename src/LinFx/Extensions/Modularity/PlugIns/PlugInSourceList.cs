using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace LinFx.Extensions.Modularity.PlugIns;

public class PlugInSourceList : List<IPlugInSource>
{
    [NotNull]
    internal Type[] GetAllModules(ILogger logger) => this.SelectMany(pluginSource => pluginSource.GetModulesWithAllDependencies(logger)).Distinct().ToArray();
}

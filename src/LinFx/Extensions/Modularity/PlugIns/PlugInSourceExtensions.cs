using Microsoft.Extensions.Logging;

namespace LinFx.Extensions.Modularity.PlugIns;

public static class PlugInSourceExtensions
{
    public static Type[] GetModulesWithAllDependencies(this IPlugInSource plugInSource, ILogger logger) => plugInSource
            .GetModules()
            .SelectMany(type => ModuleHelper.FindAllModuleTypes(type, logger))
            .Distinct()
            .ToArray();
}

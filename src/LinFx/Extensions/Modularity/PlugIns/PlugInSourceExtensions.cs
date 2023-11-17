using JetBrains.Annotations;
using LinFx.Utils;
using Microsoft.Extensions.Logging;

namespace LinFx.Extensions.Modularity.PlugIns;

public static class PlugInSourceExtensions
{
    [NotNull]
    public static Type[] GetModulesWithAllDependencies([NotNull] this IPlugInSource plugInSource, ILogger logger)
    {
        Check.NotNull(plugInSource, nameof(plugInSource));

        return plugInSource
            .GetModules()
            .SelectMany(type => ModuleHelper.FindAllModuleTypes(type, logger))
            .Distinct()
            .ToArray();
    }
}

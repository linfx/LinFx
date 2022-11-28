using Microsoft.Extensions.Logging;
using System.Reflection;

namespace LinFx.Extensions.Modularity;

class ModuleHelper
{
    public static List<Type> FindAllModuleTypes(Type startupModuleType, ILogger logger)
    {
        var moduleTypes = new List<Type>();
        logger.Log(LogLevel.Information, "Loaded modules:");
        AddModuleAndDependenciesRecursively(moduleTypes, startupModuleType, logger);
        return moduleTypes;
    }

    public static List<Type> FindDependedModuleTypes(Type moduleType)
    {
        //Module.CheckAbpModuleType(moduleType);

        var dependencies = new List<Type>();

        var dependencyDescriptors = moduleType
            .GetCustomAttributes()
            .OfType<IDependedTypesProvider>();

        foreach (var descriptor in dependencyDescriptors)
        {
            foreach (var dependedModuleType in descriptor.GetDependedTypes())
            {
                dependencies.AddIfNotContains(dependedModuleType);
            }
        }

        return dependencies;
    }

    private static void AddModuleAndDependenciesRecursively(List<Type> moduleTypes, Type moduleType, ILogger logger, int depth = 0)
    {
        //Module.CheckModuleType(moduleType);

        if (moduleTypes.Contains(moduleType))
            return;

        moduleTypes.Add(moduleType);
        logger.Log(LogLevel.Information, $"{new string(' ', depth * 2)}- {moduleType.FullName}");

        foreach (var dependedModuleType in FindDependedModuleTypes(moduleType))
        {
            AddModuleAndDependenciesRecursively(moduleTypes, dependedModuleType, logger, depth + 1);
        }
    }
}

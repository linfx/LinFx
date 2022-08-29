using LinFx.Application;
using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LinFx.Extensions.Modularity;

public class ModuleManager : IModuleManager, ISingletonDependency
{
    private readonly ILogger _logger;
    private readonly IModuleContainer _moduleContainer;
    private readonly IEnumerable<IModuleLifecycleContributor> _lifecycleContributors;

    public ModuleManager(
         ILogger<ModuleManager> logger,
         IOptions<ModuleLifecycleOptions> options,
         IModuleContainer moduleContainer,
         IServiceProvider serviceProvider)
    {
        _logger = logger;
        _moduleContainer = moduleContainer;
        _lifecycleContributors = options.Value
            .Contributors
            .Select(serviceProvider.GetRequiredService)
            .Cast<IModuleLifecycleContributor>()
            .ToArray();
    }

    /// <summary>
    /// 初化始模块
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="LinFxException"></exception>
    public virtual async Task InitializeModulesAsync(ApplicationInitializationContext context)
    {
        foreach (var contributor in _lifecycleContributors)
        {
            foreach (var module in _moduleContainer.Modules)
            {
                try
                {
                    await contributor.InitializeAsync(context, module.Instance);
                }
                catch (Exception ex)
                {
                    throw new LinFxException($"An error occurred during the initialize {contributor.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.", ex);
                }
            }
        }

        _logger.LogInformation("Initialized all modules.");
    }

    public virtual async Task ShutdownModulesAsync(ApplicationShutdownContext context)
    {
        var modules = _moduleContainer.Modules.Reverse().ToList();

        foreach (var contributor in _lifecycleContributors)
        {
            foreach (var module in modules)
            {
                try
                {
                    await contributor.ShutdownAsync(context, module.Instance);
                }
                catch (Exception ex)
                {
                    throw new LinFxException($"An error occurred during the shutdown {contributor.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.", ex);
                }
            }
        }
    }
}

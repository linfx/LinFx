using LinFx.Application;
using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinFx.Extensions.Modularity;

[Service(ServiceLifetime.Singleton)]
public class ModuleManager : IModuleManager
{
    private readonly IModuleContainer _moduleContainer;
    private readonly IEnumerable<IModuleLifecycleContributor> _lifecycleContributors;
    private readonly ILogger _logger;

    public ModuleManager(
         IModuleContainer moduleContainer,
         ILogger<ModuleManager> logger,
         IOptions<ModuleLifecycleOptions> options,
         IServiceProvider serviceProvider)
    {
        _moduleContainer = moduleContainer;
        _logger = logger;

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
    public void InitializeModules(ApplicationInitializationContext context)
    {
        foreach (var contributor in _lifecycleContributors)
        {
            foreach (var module in _moduleContainer.Modules)
            {
                try
                {
                    contributor.Initialize(context, module.Instance);
                }
                catch (Exception ex)
                {
                    throw new LinFxException($"An error occurred during the initialize {contributor.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.", ex);
                }
            }
        }

        _logger.LogInformation("Initialized all modules.");
    }

    public void ShutdownModules(ApplicationShutdownContext context)
    {
        //var modules = _moduleContainer.Modules.Reverse().ToList();

        //foreach (var contributor in _lifecycleContributors)
        //{
        //    foreach (var module in modules)
        //    {
        //        try
        //        {
        //            contributor.Shutdown(context, module.Instance);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new AbpShutdownException($"An error occurred during the shutdown {contributor.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.", ex);
        //        }
        //    }
        //}
    }
}

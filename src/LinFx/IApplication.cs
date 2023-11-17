using LinFx.Extensions.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx;

public interface IApplication : IModuleContainer, IDisposable
{
    /// <summary>
    /// Type of the startup (entrance) module of the application.
    /// </summary>
    Type StartupModuleType { get; }

    /// <summary>
    /// Reference to the root service provider used by the application.
    /// This can not be used before initialize the application.
    /// </summary>
    IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// List of services registered to this application.
    /// Can not add new services to this collection after application initialize.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Used to gracefully shutdown the application and all modules.
    /// </summary>
    Task ShutdownAsync();
}

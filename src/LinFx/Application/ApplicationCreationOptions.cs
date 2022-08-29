using JetBrains.Annotations;
using LinFx.Extensions.Modularity.PlugIns;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Application;

public class ApplicationCreationOptions
{
    [NotNull]
    public IServiceCollection Services { get; }

    [NotNull]
    public PlugInSourceList PlugInSources { get; }

    /// <summary>
    /// The options in this property only take effect when IConfiguration not registered.
    /// </summary>
    [NotNull]
    public ConfigurationBuilderOptions Configuration { get; }

    public ApplicationCreationOptions(IServiceCollection services)
    {
        Services = services;
        PlugInSources = new PlugInSourceList();
        Configuration = new ConfigurationBuilderOptions();
    }
}

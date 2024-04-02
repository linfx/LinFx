using System.Diagnostics.CodeAnalysis;
using LinFx.Extensions.Modularity.PlugIns;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx;

public class ApplicationCreationOptions(IServiceCollection services)
{
    [NotNull]
    public IServiceCollection Services { get; } = services;

    [NotNull]
    public PlugInSourceList PlugInSources { get; } = [];

    /// <summary>
    /// The options in this property only take effect when IConfiguration not registered.
    /// </summary>
    [NotNull]
    public ConfigurationBuilderOptions Configuration { get; } = new ConfigurationBuilderOptions();
}

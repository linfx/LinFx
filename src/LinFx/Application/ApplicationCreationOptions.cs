using JetBrains.Annotations;
using LinFx.Extensions.Modularity.PlugIns;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Application;

public class ApplicationCreationOptions
{
    [NotNull]
    public IServiceCollection Services { get; }

    [NotNull]
    public PlugInSourceList PlugInSources { get; }

    public ApplicationCreationOptions(IServiceCollection services)
    {
        Services = services;
        PlugInSources = new PlugInSourceList();
    }
}

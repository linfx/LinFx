using JetBrains.Annotations;
using LinFx.Extensions.DependencyInjection;

namespace LinFx.Extensions.Modularity;

public interface IModuleLifecycleContributor : ITransientDependency
{
    Task InitializeAsync([NotNull] ApplicationInitializationContext context, [NotNull] IModule module);

    Task ShutdownAsync([NotNull] ApplicationShutdownContext context, [NotNull] IModule module);
}

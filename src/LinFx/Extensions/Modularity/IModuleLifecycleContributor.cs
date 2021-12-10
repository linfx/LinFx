using JetBrains.Annotations;
using LinFx.Application;
using LinFx.Extensions.DependencyInjection;

namespace LinFx.Extensions.Modularity;

public interface IModuleLifecycleContributor : ITransientDependency
{
    void Initialize([NotNull] ApplicationInitializationContext context, [NotNull] IModule module);

    void Shutdown([NotNull] ApplicationShutdownContext context, [NotNull] IModule module);
}

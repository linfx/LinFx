using JetBrains.Annotations;

namespace LinFx.Extensions.Modularity;

public abstract class ModuleLifecycleContributorBase : IModuleLifecycleContributor
{
    public virtual Task InitializeAsync([NotNull] ApplicationInitializationContext context, [NotNull] IModule module)
    {
        return Task.CompletedTask;
    }

    public virtual Task ShutdownAsync([NotNull] ApplicationShutdownContext context, [NotNull] IModule module)
    {
        return Task.CompletedTask;
    }
}
using LinFx.Application;

namespace LinFx.Extensions.Modularity;

public abstract class ModuleLifecycleContributorBase : IModuleLifecycleContributor
{
    public virtual void Initialize(ApplicationInitializationContext context, IModule module)
    {
    }

    public virtual void Shutdown(ApplicationShutdownContext context, IModule module)
    {
    }
}
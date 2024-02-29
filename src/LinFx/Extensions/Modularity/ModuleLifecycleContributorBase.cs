namespace LinFx.Extensions.Modularity;

public abstract class ModuleLifecycleContributorBase : IModuleLifecycleContributor
{
    public virtual Task InitializeAsync(ApplicationInitializationContext context, IModule module)
    {
        return Task.CompletedTask;
    }

    public virtual Task ShutdownAsync(ApplicationShutdownContext context, IModule module)
    {
        return Task.CompletedTask;
    }
}
namespace LinFx.Extensions.Modularity;

public class OnApplicationInitializationModuleLifecycleContributor : ModuleLifecycleContributorBase
{
    public async override Task InitializeAsync(ApplicationInitializationContext context, IModule module)
    {
        if (module is IOnApplicationInitialization onApplicationInitialization)
        {
            await onApplicationInitialization.OnApplicationInitializationAsync(context);
        }
    }
}

public class OnApplicationShutdownModuleLifecycleContributor : ModuleLifecycleContributorBase
{
    public async override Task ShutdownAsync(ApplicationShutdownContext context, IModule module)
    {
        if (module is IOnApplicationShutdown onApplicationShutdown)
        {
            await onApplicationShutdown.OnApplicationShutdownAsync(context);
        }
    }
}

public class OnPreApplicationInitializationModuleLifecycleContributor : ModuleLifecycleContributorBase { }

public class OnPostApplicationInitializationModuleLifecycleContributor : ModuleLifecycleContributorBase { }

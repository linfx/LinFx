using LinFx.Application;

namespace LinFx.Extensions.Modularity;

public class OnApplicationInitializationModuleLifecycleContributor : ModuleLifecycleContributorBase
{
    public override void Initialize(ApplicationInitializationContext context, IModule module)
    {
        (module as IOnApplicationInitialization)?.OnApplicationInitialization(context);
    }
}

public class OnApplicationShutdownModuleLifecycleContributor : ModuleLifecycleContributorBase
{
    public override void Shutdown(ApplicationShutdownContext context, IModule module)
    {
        (module as IOnApplicationShutdown)?.OnApplicationShutdown(context);
    }
}

public class OnPreApplicationInitializationModuleLifecycleContributor : ModuleLifecycleContributorBase
{
    public override void Initialize(ApplicationInitializationContext context, IModule module)
    {
        //(module as IOnPreApplicationInitialization)?.OnPreApplicationInitialization(context);
    }
}

public class OnPostApplicationInitializationModuleLifecycleContributor : ModuleLifecycleContributorBase
{
    public override void Initialize(ApplicationInitializationContext context, IModule module)
    {
        //(module as IOnPostApplicationInitialization)?.OnPostApplicationInitialization(context);
    }
}

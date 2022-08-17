using JetBrains.Annotations;

namespace LinFx.Application;

public interface IOnApplicationInitialization
{
    Task OnApplicationInitializationAsync([NotNull] ApplicationInitializationContext context);
}

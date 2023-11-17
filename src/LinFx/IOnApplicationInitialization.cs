using JetBrains.Annotations;

namespace LinFx;

public interface IOnApplicationInitialization
{
    Task OnApplicationInitializationAsync([NotNull] ApplicationInitializationContext context);
}

using JetBrains.Annotations;

namespace LinFx.Application;

public interface IOnApplicationInitialization
{
    void OnApplicationInitialization([NotNull] ApplicationInitializationContext context);
}

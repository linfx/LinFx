using JetBrains.Annotations;

namespace LinFx.Application;

public interface IOnApplicationShutdown
{
    void OnApplicationShutdown([NotNull] ApplicationShutdownContext context);
}

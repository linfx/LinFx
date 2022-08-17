using JetBrains.Annotations;

namespace LinFx.Application;

public interface IOnApplicationShutdown
{
    Task OnApplicationShutdownAsync([NotNull] ApplicationShutdownContext context);
}

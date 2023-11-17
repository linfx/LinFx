using JetBrains.Annotations;

namespace LinFx;

public interface IOnApplicationShutdown
{
    Task OnApplicationShutdownAsync([NotNull] ApplicationShutdownContext context);
}

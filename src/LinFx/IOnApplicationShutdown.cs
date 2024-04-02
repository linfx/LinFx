using System.Diagnostics.CodeAnalysis;

namespace LinFx;

public interface IOnApplicationShutdown
{
    Task OnApplicationShutdownAsync([NotNull] ApplicationShutdownContext context);
}

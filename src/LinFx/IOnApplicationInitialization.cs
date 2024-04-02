using System.Diagnostics.CodeAnalysis;

namespace LinFx;

public interface IOnApplicationInitialization
{
    Task OnApplicationInitializationAsync([NotNull] ApplicationInitializationContext context);
}

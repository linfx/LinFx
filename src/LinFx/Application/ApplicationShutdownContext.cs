using JetBrains.Annotations;

namespace LinFx.Application;

public class ApplicationShutdownContext
{
    public IServiceProvider ServiceProvider { get; }

    public ApplicationShutdownContext([NotNull] IServiceProvider serviceProvider)
    {
        Check.NotNull(serviceProvider, nameof(serviceProvider));

        ServiceProvider = serviceProvider;
    }
}
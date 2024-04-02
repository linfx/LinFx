using System.Diagnostics.CodeAnalysis;

namespace LinFx;

public interface IApplicationWithExternalServiceProvider : IApplication
{
    /// <summary>
    /// Sets the service provider, but not initializes the modules.
    /// </summary>
    void SetServiceProvider([NotNull] IServiceProvider serviceProvider);

    /// <summary>
    /// Sets the service provider and initializes all the modules.
    /// If <see cref="SetServiceProvider"/> was called before, the same
    /// <see cref="IServiceProvider"/> instance should be passed to this method.
    /// </summary>
    Task InitializeAsync([NotNull] IServiceProvider applicationServices);
}

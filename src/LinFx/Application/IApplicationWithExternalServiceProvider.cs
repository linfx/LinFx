using JetBrains.Annotations;
using System;

namespace LinFx.Application;

public interface IApplicationWithExternalServiceProvider
{
    /// <summary>
    /// Sets the service provider, but not initializes the modules.
    /// </summary>
    void SetServiceProvider([NotNull] IServiceProvider serviceProvider);

    /// <summary>
    /// Sets the service provider and initializes all the modules.
    /// If <see cref="SetServiceProvider"/> was called before, the same
    /// <see cref="serviceProvider"/> instance should be passed to this method.
    /// </summary>
    void Initialize([NotNull] IServiceProvider serviceProvider);
}

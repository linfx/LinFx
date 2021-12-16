﻿using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Application;

internal class ApplicationWithExternalServiceProvider : ApplicationBase, IApplicationWithExternalServiceProvider
{
    public ApplicationWithExternalServiceProvider(
        [NotNull] Type startupModuleType,
        [NotNull] IServiceCollection services,
        [CanBeNull] Action<ApplicationCreationOptions> optionsAction
        ) : base(
            startupModuleType,
            services,
            optionsAction)
    {
        services.AddSingleton<IApplicationWithExternalServiceProvider>(this);
    }

    void IApplicationWithExternalServiceProvider.SetServiceProvider([NotNull] IServiceProvider serviceProvider)
    {
        Check.NotNull(serviceProvider, nameof(serviceProvider));

        if (ServiceProvider != null)
        {
            if (ServiceProvider != serviceProvider)
            {
                throw new LinFxException("Service provider was already set before to another service provider instance.");
            }

            return;
        }

        SetServiceProvider(serviceProvider);
    }

    public void Initialize([NotNull] IServiceProvider serviceProvider)
    {
        Check.NotNull(serviceProvider, nameof(serviceProvider));

        SetServiceProvider(serviceProvider);

        InitializeModules();
    }

    public override void Dispose()
    {
        base.Dispose();

        if (ServiceProvider is IDisposable disposableServiceProvider)
        {
            disposableServiceProvider.Dispose();
        }
    }
}
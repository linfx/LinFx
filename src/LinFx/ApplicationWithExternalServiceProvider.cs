using JetBrains.Annotations;
using LinFx.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx;

/// <summary>
/// 应用程序提供者
/// </summary>
internal class ApplicationWithExternalServiceProvider : ApplicationBase, IApplicationWithExternalServiceProvider
{
    public ApplicationWithExternalServiceProvider(
        [NotNull] Type startupModuleType,
        [NotNull] IServiceCollection services,
        [CanBeNull] Action<ApplicationCreationOptions> optionsAction
        ) : base(startupModuleType, services, optionsAction)
    {
        // 注入自己到 IoC 当中。
        services.AddSingleton<IApplicationWithExternalServiceProvider>(this);
    }

    void IApplicationWithExternalServiceProvider.SetServiceProvider([NotNull] IServiceProvider serviceProvider)
    {
        Check.NotNull(serviceProvider, nameof(serviceProvider));

        if (ServiceProvider != null)
        {
            if (ServiceProvider != serviceProvider)
                throw new LinFxException("Service provider was already set before to another service provider instance.");

            return;
        }

        SetServiceProvider(serviceProvider);
    }

    public async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        Check.NotNull(serviceProvider, nameof(serviceProvider));

        SetServiceProvider(serviceProvider);

        await InitializeModulesAsync();
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

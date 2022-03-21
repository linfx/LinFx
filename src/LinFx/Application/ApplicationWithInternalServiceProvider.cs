using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Application;

internal class ApplicationWithInternalServiceProvider : ApplicationBase, IApplicationWithInternalServiceProvider
{
    public IServiceScope ServiceScope { get; private set; }

    public ApplicationWithInternalServiceProvider(
        [NotNull] Type startupModuleType,
        [CanBeNull] Action<ApplicationCreationOptions> optionsAction)
        : this(startupModuleType, new ServiceCollection(), optionsAction) { }

    private ApplicationWithInternalServiceProvider(
        [NotNull] Type startupModuleType,
        [NotNull] IServiceCollection services,
        [CanBeNull] Action<ApplicationCreationOptions> optionsAction)
        : base(startupModuleType, services, optionsAction)
    {
        // 注入自己到 IoC 当中。
        Services.AddSingleton<IApplicationWithInternalServiceProvider>(this);
    }

    public IServiceProvider CreateServiceProvider()
    {
        if (ServiceProvider != null)
            return ServiceProvider;

        ServiceScope = Services.BuildServiceProviderFromFactory().CreateScope();
        SetServiceProvider(ServiceScope.ServiceProvider);

        return ServiceProvider;
    }

    // 执行框架初始化操作，主要工作是加载模块并执行其初始化方法。
    public void Initialize()
    {
        CreateServiceProvider();
        InitializeModules();
    }

    public override void Dispose()
    {
        base.Dispose();
        ServiceScope.Dispose();
    }
}

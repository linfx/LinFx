using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace LinFx;

/// <summary>
/// 应用程序提供者
/// </summary>
internal class ApplicationWithInternalServiceProvider : ApplicationBase, IApplicationWithInternalServiceProvider
{
    [NotNull]
    public IServiceScope? ServiceScope { get; private set; }

    public ApplicationWithInternalServiceProvider(Type startupModuleType, Action<ApplicationCreationOptions>? optionsAction)
        : this(startupModuleType, new ServiceCollection(), optionsAction) { }

    private ApplicationWithInternalServiceProvider(Type startupModuleType, IServiceCollection services, Action<ApplicationCreationOptions>? optionsAction) 
        : base(startupModuleType, services, optionsAction)
    {
        // 注入自己到 IoC 当中。
        Services.AddSingleton<IApplicationWithInternalServiceProvider>(this);
    }

    public void CreateServiceProvider()
    {
        if (ServiceProvider != null)
            return;

        ServiceScope = Services.BuildServiceProviderFromFactory().CreateScope();
        SetServiceProvider(ServiceScope.ServiceProvider);
    }

    // 执行框架初始化操作，主要工作是加载模块并执行其初始化方法。
    public async Task InitializeAsync()
    {
        CreateServiceProvider();
        await InitializeModulesAsync();
    }

    public override void Dispose()
    {
        base.Dispose();
        ServiceScope?.Dispose();
    }
}

using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace LinFx;

/// <summary>
/// Ӧ�ó����ṩ��
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
        // ע���Լ��� IoC ���С�
        Services.AddSingleton<IApplicationWithInternalServiceProvider>(this);
    }

    public void CreateServiceProvider()
    {
        if (ServiceProvider != null)
            return;

        ServiceScope = Services.BuildServiceProviderFromFactory().CreateScope();
        SetServiceProvider(ServiceScope.ServiceProvider);
    }

    // ִ�п�ܳ�ʼ����������Ҫ�����Ǽ���ģ�鲢ִ�����ʼ��������
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

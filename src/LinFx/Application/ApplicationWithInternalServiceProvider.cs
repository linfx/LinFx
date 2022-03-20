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
        // ע���Լ��� IoC ���С�
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

    // ִ�п�ܳ�ʼ����������Ҫ�����Ǽ���ģ�鲢ִ�����ʼ��������
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

namespace LinFx.Extensions.DependencyInjection;

/// <summary>
/// 服务提供程序上下文
/// </summary>
[Service]
public class ServiceContext(IServiceProvider serviceProvider) : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; } = serviceProvider;
}

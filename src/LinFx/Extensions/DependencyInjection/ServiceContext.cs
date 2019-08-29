using System;

namespace LinFx.Extensions.DependencyInjection
{
    /// <summary>
    /// 服务提供程序上下文
    /// </summary>
    [Service]
    public class ServiceContext : IServiceProviderAccessor
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public ServiceContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}

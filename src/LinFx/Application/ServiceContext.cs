using LinFx.Extensions.DependencyInjection;
using System;

namespace LinFx.Application
{
    public class ServiceContext : IServiceProviderAccessor
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public ServiceContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;

namespace LinFx.Application
{
    /// <summary>
    /// 服务抽象类
    /// </summary>
    public abstract class ApplicationService
    {
        public IServiceProvider ServiceProvider { get; set; }
        protected readonly object ServiceProviderLock = new object();

        protected TService LazyGetRequiredService<TService>(ref TService reference)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = ServiceProvider.GetRequiredService<TService>();
                    }
                }
            }
            return reference;
        }
    }
}

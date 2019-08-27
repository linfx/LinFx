using Microsoft.Extensions.DependencyInjection;
using System;

namespace LinFx
{
    /// <summary>
    /// @Service
    /// </summary>
    public class ServiceAttribute : Attribute
    {
        public virtual ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;

        public virtual bool TryRegister { get; set; }

        public virtual bool ReplaceServices { get; set; }

        public ServiceAttribute()
        {
        }

        public ServiceAttribute(ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
        }
    }
}

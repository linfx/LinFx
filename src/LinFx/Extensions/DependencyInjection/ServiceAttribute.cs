using Microsoft.Extensions.DependencyInjection;
using System;

namespace LinFx.Extensions.DependencyInjection
{
    /// <summary>
    /// Service 特性注入
    /// </summary>
    public class ServiceAttribute : Attribute
    {
        /// <summary>
        /// 生命周期
        /// </summary>
        public virtual ServiceLifetime? Lifetime { get; set; }

        /// <summary>
        /// 设置true则只注册以前未注册的服务
        /// </summary>
        public virtual bool TryRegister { get; set; }

        /// <summary>
        /// 设置true则替换之前已经注册过的服务
        /// </summary>
        public virtual bool ReplaceServices { get; set; }

        public ServiceAttribute() { }

        public ServiceAttribute(ServiceLifetime lifetime) => Lifetime = lifetime;
    }
}

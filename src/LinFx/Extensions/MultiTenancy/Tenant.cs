using System;

namespace LinFx.Extensions.MultiTenancy
{
    /// <summary>
    /// 租户
    /// </summary>
    public class Tenant
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; }
    }
}

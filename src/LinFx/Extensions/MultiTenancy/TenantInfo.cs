using System;

namespace LinFx.Extensions.MultiTenancy
{
    /// <summary>
    /// 租户
    /// </summary>
    public class TenantInfo
    {
        public TenantInfo() { }

        public TenantInfo(string id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; }
    }
}
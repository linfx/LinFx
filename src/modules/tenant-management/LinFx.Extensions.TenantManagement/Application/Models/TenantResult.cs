using System;

namespace LinFx.Extensions.TenantManagement.Application.Models
{
    public class TenantResult
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }
    }
}

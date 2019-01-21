using LinFx.Domain.Abstractions;
using LinFx.Domain.Models.MultiTenancy;
using System;
using System.Collections.Generic;

namespace LinFx.Domain.Models.Auditing
{
    public class AuditLogActionInfo : IMultiTenant, IHasExtraProperties
    {
        public Guid? TenantId { get; set; }

        public string ServiceName { get; set; }

        public string MethodName { get; set; }

        public string Parameters { get; set; }

        public DateTime ExecutionTime { get; set; }

        public int ExecutionDuration { get; set; }

        public Dictionary<string, object> ExtraProperties { get; }

        public AuditLogActionInfo()
        {
            ExtraProperties = new Dictionary<string, object>();
        }
    }
}
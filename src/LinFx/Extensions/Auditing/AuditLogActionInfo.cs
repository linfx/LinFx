using LinFx.Domain.Abstractions;
using LinFx.Extensions.MultiTenancy;
using System;
using System.Collections.Generic;

namespace LinFx.Extensions.Auditing
{
    public class AuditLogActionInfo : IMultiTenant, IHasExtraProperties
    {
        public string TenantId { get; set; }

        public string ServiceName { get; set; }

        public string MethodName { get; set; }

        public string Parameters { get; set; }

        public DateTimeOffset ExecutionTime { get; set; }

        public int ExecutionDuration { get; set; }

        public Dictionary<string, object> ExtraProperties { get; }

        public AuditLogActionInfo()
        {
            ExtraProperties = new Dictionary<string, object>();
        }
    }
}
using LinFx.Domain.Entities;
using LinFx.Extensions.Auditing;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.ObjectExtending;
using System;

namespace LinFx.Extensions.AuditLogging
{
    [DisableAuditing]
    public class AuditLogAction : Entity<string>, IMultiTenant, IHasExtraProperties
    {
        public virtual string TenantId { get; protected set; }

        public virtual string AuditLogId { get; protected set; }

        public virtual string ServiceName { get; protected set; }

        public virtual string MethodName { get; protected set; }

        public virtual string Parameters { get; protected set; }

        public virtual DateTime ExecutionTime { get; protected set; }

        public virtual int ExecutionDuration { get; protected set; }

        public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }

        protected AuditLogAction() { }

        public AuditLogAction(string id, string auditLogId, AuditLogActionInfo actionInfo, string tenantId = null)
        {
            Id = id;
            TenantId = tenantId;
            AuditLogId = auditLogId;
            ExecutionTime = actionInfo.ExecutionTime;
            ExecutionDuration = actionInfo.ExecutionDuration;
            ExtraProperties = new ExtraPropertyDictionary(actionInfo.ExtraProperties);
            ServiceName = actionInfo.ServiceName.TruncateFromBeginning(AuditLogActionConsts.MaxServiceNameLength);
            MethodName = actionInfo.MethodName.TruncateFromBeginning(AuditLogActionConsts.MaxMethodNameLength);
            Parameters = actionInfo.Parameters.Length > AuditLogActionConsts.MaxParametersLength ? "" : actionInfo.Parameters;
        }
    }
}

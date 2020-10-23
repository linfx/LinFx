﻿using LinFx.Domain.Models;
using LinFx.Extensions.Auditing;
using LinFx.Extensions.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinFx.Extensions.AuditLogging.Domain
{
    public class AuditLogAction : Entity<Guid>, IMultiTenant, IHasExtraProperties
    {
        public virtual string TenantId { get; protected set; }

        public virtual Guid AuditLogId { get; protected set; }

        public virtual string ServiceName { get; protected set; }

        public virtual string MethodName { get; protected set; }

        public virtual string Parameters { get; protected set; }

        public virtual DateTimeOffset ExecutionTime { get; protected set; }

        public virtual int ExecutionDuration { get; protected set; }

        public virtual Dictionary<string, object> ExtraProperties { get; protected set; }

        protected AuditLogAction()
        {
            ExtraProperties = new Dictionary<string, object>();
        }

        public AuditLogAction(Guid id, Guid auditLogId, AuditLogActionInfo actionInfo, string tenantId = default)
        {
            Id = id;
            TenantId = tenantId;
            AuditLogId = auditLogId;
            ExecutionTime = actionInfo.ExecutionTime;
            ExecutionDuration = actionInfo.ExecutionDuration;
            ExtraProperties = actionInfo.ExtraProperties.ToDictionary(pair => pair.Key, pair => pair.Value);
            //ServiceName = actionInfo.ServiceName.TruncateFromBeginning(AuditLogActionConsts.MaxServiceNameLength);
            //MethodName = actionInfo.MethodName.TruncateFromBeginning(AuditLogActionConsts.MaxMethodNameLength);
            //Parameters = actionInfo.Parameters.Length > AuditLogActionConsts.MaxParametersLength ? "" : actionInfo.Parameters;
        }
    }
}

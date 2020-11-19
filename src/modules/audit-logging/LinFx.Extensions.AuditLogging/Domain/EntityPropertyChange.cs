using LinFx.Domain.Models;
using LinFx.Extensions.MultiTenancy;
using System;

namespace LinFx.Extensions.AuditLogging.Domain
{
    public class EntityPropertyChange : Entity<Guid>, IMultiTenant
    {
        public virtual string TenantId { get; protected set; }

        public virtual Guid EntityChangeId { get; protected set; }

        public virtual string NewValue { get; protected set; }

        public virtual string OriginalValue { get; protected set; }

        public virtual string PropertyName { get; protected set; }

        public virtual string PropertyTypeFullName { get; protected set; }

        protected EntityPropertyChange() { }

        public EntityPropertyChange(Guid entityChangeId, EntityPropertyChangeInfo entityChangeInfo, string tenantId = default)
        {
            //Id = Guid.NewGuid();
            //TenantId = tenantId;
            //EntityChangeId = entityChangeId;
            //NewValue = entityChangeInfo.NewValue.Truncate(EntityPropertyChangeConsts.MaxNewValueLength);
            //OriginalValue = entityChangeInfo.OriginalValue.Truncate(EntityPropertyChangeConsts.MaxOriginalValueLength);
            //PropertyName = entityChangeInfo.PropertyName.TruncateFromBeginning(EntityPropertyChangeConsts.MaxPropertyNameLength);
            //PropertyTypeFullName = entityChangeInfo.PropertyTypeFullName.TruncateFromBeginning(EntityPropertyChangeConsts.MaxPropertyTypeFullNameLength);
        }
    }
}
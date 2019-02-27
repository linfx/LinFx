using LinFx.Domain.Models;
using LinFx.Extensions.MultiTenancy;
using System;

namespace LinFx.Extensions.PermissionManagement
{
    public class PermissionGrant : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }

        [NotNull]
        public virtual string Name { get; protected set; }

        [NotNull]
        public virtual string ProviderName { get; protected set; }

        [NotNull]
        public virtual string ProviderKey { get; protected set; }

        protected PermissionGrant() { }

        public PermissionGrant(
            Guid id,
            [NotNull] string name,
            [NotNull] string providerName,
            [NotNull] string providerKey,
            Guid? tenantId = null)
        {
            Check.NotNull(name, nameof(name));

            Id = id;
            Name = name;
            ProviderName = providerName;
            ProviderKey = providerKey;
            TenantId = tenantId;
        }
    }
}

using System;

namespace LinFx.Domain.Models.MultiTenancy
{
    public interface IMultiTenant
    {
        Guid? TenantId { get; }
    }
}

using System;

namespace LinFx.Extensions.MultiTenancy
{
    public interface IMultiTenant
    {
        Guid? TenantId { get; }
    }
}

namespace LinFx.Extensions.MultiTenancy;

public class TenantIdWrapper(string tenantId)
{
    /// <summary>
    /// Null indicates the host.
    /// Not null value for a tenant.
    /// </summary>
    public string TenantId { get; } = tenantId;
}

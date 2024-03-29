namespace LinFx.Extensions.MultiTenancy;

public interface ITenantStore
{
    ValueTask<TenantInfo?> FindAsync(string tenantIdOrName);
}

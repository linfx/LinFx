namespace LinFx.Extensions.MultiTenancy
{
    public interface IMultiTenant
    {
        string TenantId { get; }
    }
}

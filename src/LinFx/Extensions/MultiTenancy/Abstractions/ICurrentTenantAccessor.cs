namespace LinFx.Extensions.MultiTenancy
{
    public interface ICurrentTenantAccessor
    {
        TenantInfo Current { get; set; }
    }
}

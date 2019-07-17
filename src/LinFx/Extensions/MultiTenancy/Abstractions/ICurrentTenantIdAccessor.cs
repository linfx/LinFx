namespace LinFx.Extensions.MultiTenancy
{
    public interface ICurrentTenantIdAccessor
    {
        TenantInfo Current { get; set; }
    }
}

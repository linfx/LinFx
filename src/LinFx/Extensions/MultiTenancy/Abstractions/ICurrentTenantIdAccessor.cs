namespace LinFx.Extensions.MultiTenancy
{
    public interface ICurrentTenantIdAccessor
    {
        TenantIdWrapper Current { get; set; }
    }
}

namespace LinFx.Extensions.MultiTenancy
{
    public interface ITenantResolveResultAccessor
    {
        [CanBeNull]
        TenantResolveResult Result { get; set; }
    }
}
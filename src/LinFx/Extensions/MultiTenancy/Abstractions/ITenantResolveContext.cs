namespace LinFx.Extensions.MultiTenancy
{
    public interface ITenantResolveContext
    {
        [CanBeNull]
        string TenantIdOrName { get; set; }

        bool Handled { get; set; }
    }
}
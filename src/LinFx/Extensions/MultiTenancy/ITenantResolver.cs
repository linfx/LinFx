namespace LinFx.Extensions.MultiTenancy
{
    public interface ITenantResolver
    {
        TenantResolveResult ResolveTenantIdOrName();
    }
}
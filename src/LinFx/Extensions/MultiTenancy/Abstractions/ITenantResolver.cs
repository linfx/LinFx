namespace LinFx.Extensions.AspNetCore.MultiTenancy
{
    internal interface ITenantResolver
    {
        object ResolveTenantIdOrName();
    }
}
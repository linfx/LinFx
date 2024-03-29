namespace LinFx.Extensions.MultiTenancy;

public interface ITenantResolveResultAccessor
{
    TenantResolveResult? Result { get; set; }
}
namespace LinFx.Extensions.MultiTenancy;

public class TenantResolveResult
{
    public string? TenantIdOrName { get; set; }

    public List<string> AppliedResolvers { get; } = [];
}
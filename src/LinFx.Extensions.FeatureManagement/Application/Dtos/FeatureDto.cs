using Microsoft.Extensions.Localization;

namespace LinFx.Extensions.FeatureManagement;

/// <summary>
/// ÌØÕ÷
/// </summary>
public class FeatureDto
{
    public required string Name { get; set; }

    public string? DisplayName { get; set; }

    public string? Value { get; set; }

    public FeatureProviderDto? Provider { get; set; }

    public LocalizedString? Description { get; set; }

    //public IStringValueType ValueType { get; set; }

    public int Depth { get; set; }

    public string? ParentName { get; set; }
}

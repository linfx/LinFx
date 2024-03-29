using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.FeatureManagement;

/// <summary>
/// 特征组
/// </summary>
public class FeatureGroupDto
{
    public required string Name { get; set; }

    public string? DisplayName { get; set; }

    [NotNull]
    public List<FeatureDto>? Features { get; set; }

    public string GetNormalizedGroupName() => Name.Replace(".", "_");
}

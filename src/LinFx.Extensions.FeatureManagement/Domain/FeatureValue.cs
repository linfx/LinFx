using System.Diagnostics.CodeAnalysis;
using LinFx.Domain.Entities;

namespace LinFx.Extensions.FeatureManagement;

/// <summary>
/// 特征值
/// </summary>
public class FeatureValue : Entity<string>, IAggregateRoot<string>
{
    [NotNull]
    public virtual string Name { get; protected set; }

    [NotNull]
    public virtual string Value { get; internal set; }

    [NotNull]
    public virtual string ProviderName { get; protected set; }

    [AllowNull]
    public virtual string ProviderKey { get; protected set; }

    protected FeatureValue() { }

    public FeatureValue(
        string id,
        [NotNull] string name,
        [NotNull] string value,
        [NotNull] string providerName,
        [AllowNull] string providerKey)
    {
        Id = id;
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        Value = Check.NotNullOrWhiteSpace(value, nameof(value));
        ProviderName = Check.NotNullOrWhiteSpace(providerName, nameof(providerName));
        ProviderKey = providerKey;
    }
}

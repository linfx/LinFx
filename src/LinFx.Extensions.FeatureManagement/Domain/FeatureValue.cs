using JetBrains.Annotations;
using LinFx.Domain.Entities;
using LinFx;

namespace Volo.Abp.FeatureManagement;

public class FeatureValue : Entity<string>, IAggregateRoot<string>
{
    [NotNull]
    public virtual string Name { get; protected set; }

    [NotNull]
    public virtual string Value { get; internal set; }

    [NotNull]
    public virtual string ProviderName { get; protected set; }

    [CanBeNull]
    public virtual string ProviderKey { get; protected set; }

    protected FeatureValue() { }

    public FeatureValue(
        string id,
        [NotNull] string name,
        [NotNull] string value,
        [NotNull] string providerName,
        [CanBeNull] string providerKey)
    {
        Id = id;
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        Value = Check.NotNullOrWhiteSpace(value, nameof(value));
        ProviderName = Check.NotNullOrWhiteSpace(providerName, nameof(providerName));
        ProviderKey = providerKey;
    }
}

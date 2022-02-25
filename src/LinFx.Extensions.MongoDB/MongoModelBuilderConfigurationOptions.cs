using JetBrains.Annotations;

namespace LinFx.Extensions.MongoDB;

public class MongoModelBuilderConfigurationOptions
{
    [NotNull]
    public string CollectionPrefix
    {
        get => _collectionPrefix;
        set
        {
            //Check.NotNull(value, nameof(value), $"{nameof(CollectionPrefix)} can not be null! Set to empty string if you don't want a collection prefix.");
            _collectionPrefix = value;
        }
    }
    private string _collectionPrefix;

    public MongoModelBuilderConfigurationOptions([NotNull] string collectionPrefix = "")
    {
        Check.NotNull(collectionPrefix, nameof(collectionPrefix));

        CollectionPrefix = collectionPrefix;
    }
}

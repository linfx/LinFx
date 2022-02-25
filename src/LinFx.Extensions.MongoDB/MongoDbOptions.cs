namespace LinFx.Extensions.MongoDB;

public class MongoDbOptions
{
    /// <summary>
    /// Serializer the datetime based on <see cref="AbpClockOptions.Kind"/> in MongoDb.
    /// Default: true.
    /// </summary>
    public bool UseClockHandleDateTime { get; set; }

    public MongoDbOptions()
    {
        UseClockHandleDateTime = true;
    }
}

namespace LinFx.Unity
{
    /// <summary>
    /// Base interface for services that are instantiated per unit of work (i.e. web request).
    /// </summary>
    public interface IDependency
    {
    }
    /// <summary>
    /// All classes implement this interface are automatically registered to dependency injection as singleton object.
    /// </summary>
    public interface ISingletonDependency
    {
    }
    /// <summary>
    /// Base interface for services that are instantiated per usage.
    /// </summary>
    public interface ITransientDependency : IDependency
    {
    }
}

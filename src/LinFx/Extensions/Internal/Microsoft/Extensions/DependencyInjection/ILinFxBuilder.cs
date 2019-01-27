namespace Microsoft.Extensions.DependencyInjection
{
    public interface ILinFxBuilder
    {
        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> where LinFx services are configured.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// LinFx helper class for DI configuration
    /// </summary>
    internal class LinFxBuilder : ILinFxBuilder
    {
        public LinFxBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}

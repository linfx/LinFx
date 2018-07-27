using Microsoft.Extensions.DependencyInjection;

namespace LinFx
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

using Microsoft.Extensions.DependencyInjection;

namespace LinFx
{
    internal class LinFxBuilder : ILinFxBuilder
    {
        public LinFxBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}

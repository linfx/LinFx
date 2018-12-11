using Microsoft.Extensions.DependencyInjection;

namespace LinFx
{
    public interface ILinFxOptionsExtension
    {
        /// <summary>
        /// Registered child service.
        /// </summary>
        /// <param name="services">add service to the <see cref="ILinFxBuilder" /></param>
        void AddServices(ILinFxBuilder services);
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace LinFx
{
    public static class LinFxServiceCollectionExtensions
    {
        /// <summary>
        /// Adds logging services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configure">The <see cref="ILinFxBuilder"/> configuration delegate.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddLinFx(this IServiceCollection services, Action<ILinFxBuilder> configure = null)
        {
            Check.NotNull(services, nameof(services));

            services.AddOptions();

            var t = AppDomain.CurrentDomain.GetAssemblies();

            //var handlers = allTyeps.Where(x => x.IsClass && x.GetInterfaces().Any(o => o.IsGenericType && (o.GetGenericTypeDefinition() == typeof(ICommandHandler<>) || o.GetGenericTypeDefinition() == typeof(IEventHandler<>))));
            //foreach (var hdl in handlers)
            //{
            //    foreach (var itf in hdl.GetInterfaces())
            //    {
            //        builder.Services.Add(new ServiceDescriptor(itf, hdl, ServiceLifetime.Transient));
            //    }
            //}

            configure?.Invoke(new LinFxBuilder(services));

            return services;
        }
    }
}

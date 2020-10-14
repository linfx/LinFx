﻿using IdentityServer4.Configuration;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.Hosting;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for configuring Identity Server.
    /// </summary>
    public static class IdentityServerBuilderConfigurationExtensions
    {
        /// <summary>
        /// Configures defaults for Identity Server for ASP.NET Core scenarios.
        /// </summary>
        /// <typeparam name="TUser">The <typeparamref name="TUser"/> type.</typeparam>
        /// <typeparam name="TContext">The <typeparamref name="TContext"/> type.</typeparam>
        /// <param name="builder">The <see cref="IIdentityServerBuilder"/>.</param>
        /// <returns>The <see cref="IIdentityServerBuilder"/>.</returns>
        public static IIdentityServerBuilder AddApiAuthorization<TUser, TContext>(this IIdentityServerBuilder builder)
            where TUser : class
            where TContext : DbContext, IPersistedGrantDbContext
        {
            builder.AddApiAuthorization<TUser, TContext>(o => { });
            return builder;
        }

        /// <summary>
        /// Configures defaults on Identity Server for ASP.NET Core scenarios.
        /// </summary>
        /// <typeparam name="TUser">The <typeparamref name="TUser"/> type.</typeparam>
        /// <typeparam name="TContext">The <typeparamref name="TContext"/> type.</typeparam>
        /// <param name="builder">The <see cref="IIdentityServerBuilder"/>.</param>
        /// <param name="configure">The <see cref="Action{ApplicationsOptions}"/>
        /// to configure the <see cref="ApiAuthorizationOptions"/>.</param>
        /// <returns>The <see cref="IIdentityServerBuilder"/>.</returns>
        public static IIdentityServerBuilder AddApiAuthorization<TUser, TContext>(this IIdentityServerBuilder builder, Action<ApiAuthorizationOptions> configure)
            where TUser : class
            where TContext : DbContext, IPersistedGrantDbContext
        {
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            builder
                .AddAspNetIdentity<TUser>()
                .AddOperationalStore<TContext>()
                .ConfigureReplacedServices()
                .AddIdentityResources()
                .AddApiResources()
                .AddClients();

            builder.Services.Configure(configure);

            return builder;
        }

        /// <summary>
        /// Adds API resources from the default configuration to the server using the key
        /// IdentityServer:Resources
        /// </summary>
        /// <param name="builder">The <see cref="IIdentityServerBuilder"/>.</param>
        /// <returns>The <see cref="IIdentityServerBuilder"/>.</returns>
        public static IIdentityServerBuilder AddApiResources(this IIdentityServerBuilder builder) => builder.AddApiResources(configuration: null);

        /// <summary>
        /// Adds API resources from the given <paramref name="configuration"/> instance.
        /// </summary>
        /// <param name="builder">The <see cref="IIdentityServerBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance containing the API definitions.</param>
        /// <returns>The <see cref="IIdentityServerBuilder"/>.</returns>
        public static IIdentityServerBuilder AddApiResources(this IIdentityServerBuilder builder, IConfiguration configuration)
        {
            builder.ConfigureReplacedServices();
            builder.AddInMemoryApiResources(Enumerable.Empty<ApiResource>());
            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IConfigureOptions<ApiAuthorizationOptions>, ConfigureApiResources>(sp =>
                {
                    var logger = sp.GetRequiredService<ILogger<ConfigureApiResources>>();
                    var effectiveConfig = configuration ?? sp.GetRequiredService<IConfiguration>().GetSection("IdentityServer:Resources");
                    var localApiDescriptor = sp.GetService<IIdentityServerJwtDescriptor>();
                    return new ConfigureApiResources(effectiveConfig, localApiDescriptor, logger);
                }));

            // We take over the setup for the API resources as Identity Server registers the enumerable as a singleton
            // and that prevents normal composition.
            builder.Services.AddSingleton<IEnumerable<ApiResource>>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<ApiAuthorizationOptions>>();
                return options.Value.ApiResources;
            });

            return builder;
        }

        /// <summary>
        /// Adds identity resources from the default configuration to the server using the key
        /// IdentityServer:Resources
        /// </summary>
        /// <param name="builder">The <see cref="IIdentityServerBuilder"/>.</param>
        /// <returns>The <see cref="IIdentityServerBuilder"/>.</returns>
        public static IIdentityServerBuilder AddIdentityResources(this IIdentityServerBuilder builder) => builder.AddIdentityResources(configuration: null);

        /// <summary>
        /// Adds identity resources from the given <paramref name="configuration"/> instance.
        /// </summary>
        /// <param name="builder">The <see cref="IIdentityServerBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance containing the API definitions.</param>
        /// <returns>The <see cref="IIdentityServerBuilder"/>.</returns>
        public static IIdentityServerBuilder AddIdentityResources(this IIdentityServerBuilder builder, IConfiguration configuration)
        {
            builder.ConfigureReplacedServices();
            builder.AddInMemoryIdentityResources(Enumerable.Empty<IdentityResource>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<ApiAuthorizationOptions>, ConfigureIdentityResources>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ConfigureIdentityResources>>();
                var effectiveConfig = configuration ?? sp.GetRequiredService<IConfiguration>().GetSection("IdentityServer:Identity");
                return new ConfigureIdentityResources(effectiveConfig, logger);
            }));

            // We take over the setup for the identity resources as Identity Server registers the enumerable as a singleton
            // and that prevents normal composition.
            builder.Services.AddSingleton<IEnumerable<IdentityResource>>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<ApiAuthorizationOptions>>();
                return options.Value.IdentityResources;
            });

            return builder;
        }

        /// <summary>
        /// Adds clients from the default configuration to the server using the key
        /// IdentityServer:Clients
        /// </summary>
        /// <param name="builder">The <see cref="IIdentityServerBuilder"/>.</param>
        /// <returns>The <see cref="IIdentityServerBuilder"/>.</returns>
        public static IIdentityServerBuilder AddClients(this IIdentityServerBuilder builder) => builder.AddClients(configuration: null);

        /// <summary>
        /// Adds clients from the given <paramref name="configuration"/> instance.
        /// </summary>
        /// <param name="builder">The <see cref="IIdentityServerBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance containing the client definitions.</param>
        /// <returns>The <see cref="IIdentityServerBuilder"/>.</returns>
        public static IIdentityServerBuilder AddClients(this IIdentityServerBuilder builder, IConfiguration configuration)
        {
            builder.ConfigureReplacedServices();
            builder.AddInMemoryClients(Enumerable.Empty<Client>());

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<ApiAuthorizationOptions>, ConfigureClientScopes>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<ApiAuthorizationOptions>, ConfigureClients>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ConfigureClients>>();
                var effectiveConfig = configuration ?? sp.GetRequiredService<IConfiguration>().GetSection("IdentityServer:Clients");
                return new ConfigureClients(effectiveConfig, logger);
            }));

            // We take over the setup for the clients as Identity Server registers the enumerable as a singleton and that prevents normal composition.
            builder.Services.AddSingleton<IEnumerable<Client>>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<ApiAuthorizationOptions>>();
                return options.Value.Clients;
            });

            return builder;
        }

        internal static IIdentityServerBuilder ConfigureReplacedServices(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<IdentityServerOptions>, AspNetConventionsConfigureOptions>());
            //builder.Services.TryAddSingleton<IAbsoluteUrlFactory, AbsoluteUrlFactory>();
            //builder.Services.AddSingleton<IRedirectUriValidator, RelativeRedirectUriValidator>();
            //builder.Services.AddSingleton<IClientRequestParametersProvider, DefaultClientRequestParametersProvider>();
            ReplaceEndSessionEndpoint(builder);

            return builder;
        }

        private static void ReplaceEndSessionEndpoint(IIdentityServerBuilder builder)
        {
            // We don't have a better way to replace the end session endpoint as far as we know other than looking the descriptor up
            // on the container and replacing the instance. This is due to the fact that we chain on AddIdentityServer which configures the
            // list of endpoints by default.
            //var endSessionEndpointDescriptor = builder.Services
            //                .Single(s => s.ImplementationInstance is Endpoint e &&
            //                        string.Equals(e.Name, "Endsession", StringComparison.OrdinalIgnoreCase) &&
            //                        string.Equals("/connect/endsession", e.Path, StringComparison.OrdinalIgnoreCase));

            //builder.Services.Remove(endSessionEndpointDescriptor);
            //builder.AddEndpoint<AutoRedirectEndSessionEndpoint>("EndSession", "/connect/endsession");
        }
    }
}

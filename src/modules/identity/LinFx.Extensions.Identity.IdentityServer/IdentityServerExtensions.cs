//using IdentityServer4.EntityFramework.Interfaces;
//using LinFx.Extensions.EntityFrameworkCore;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace LinFx.Extensions.Identity.IdentityServer
//{
//    public static class IdentityServerExtensions
//    {
//        /// <summary>
//        /// Add services for authentication, including Identity model, IdentityServer4 and external providers
//        /// </summary>
//        /// <typeparam name="TIdentityDbContext">DbContext for Identity</typeparam>
//        /// <typeparam name="TUserIdentity">User Identity class</typeparam>
//        /// <typeparam name="TUserIdentityRole">User Identity Role class</typeparam>
//        /// <typeparam name="TConfigurationDbContext"></typeparam>
//        /// <typeparam name="TPersistedGrantDbContext"></typeparam>
//        /// <param name="services"></param>
//        /// <param name="hostingEnvironment"></param>
//        /// <param name="configuration"></param>
//        /// <param name="logger"></param>
//        public static void AddAuthenticationServices<TConfigurationDbContext, TPersistedGrantDbContext, TIdentityDbContext, TUserIdentity, TUserIdentityRole>(this IServiceCollection services)
//            where TPersistedGrantDbContext : DbContext, IPersistedGrantDbContext
//            where TConfigurationDbContext : DbContext, IConfigurationDbContext
//            where TIdentityDbContext : DbContext
//            where TUserIdentity : class
//            where TUserIdentityRole : class
//        {
//            var loginConfiguration = GetLoginConfiguration(configuration);
//            var registrationConfiguration = GetRegistrationConfiguration(configuration);

//            services
//                .AddSingleton(registrationConfiguration)
//                .AddSingleton(loginConfiguration)
//                .AddScoped<UserResolver<TUserIdentity>>()
//                .AddIdentity<TUserIdentity, TUserIdentityRole>(options =>
//                {
//                    options.User.RequireUniqueEmail = true;
//                })
//                .AddEntityFrameworkStores<TIdentityDbContext>()
//                .AddDefaultTokenProviders();

//            services.Configure<IISOptions>(iis =>
//            {
//                iis.AuthenticationDisplayName = "Windows";
//                iis.AutomaticAuthentication = false;
//            });

//            var authenticationBuilder = services.AddAuthentication();

//            //AddExternalProviders(authenticationBuilder, configuration);
//            //AddIdentityServer<TConfigurationDbContext, TPersistedGrantDbContext, TUserIdentity>(services, configuration, logger, hostingEnvironment);
//        }

//        /// <summary>
//        /// Add configuration for IdentityServer4
//        /// </summary>
//        /// <typeparam name="TUserIdentity"></typeparam>
//        /// <typeparam name="TConfigurationDbContext"></typeparam>
//        /// <typeparam name="TPersistedGrantDbContext"></typeparam>
//        /// <param name="services"></param>
//        /// <param name="configuration"></param>
//        /// <param name="logger"></param>
//        /// <param name="hostingEnvironment"></param>
//        private static void AddIdentityServer<TConfigurationDbContext, TPersistedGrantDbContext, TUserIdentity>(IServiceCollection services)
//            where TUserIdentity : class
//            where TPersistedGrantDbContext : DbContext, IPersistedGrantDbContext
//            where TConfigurationDbContext : DbContext, IConfigurationDbContext
//        {
//            var builder = services.AddIdentityServer(options =>
//            {
//                options.Events.RaiseErrorEvents = true;
//                options.Events.RaiseInformationEvents = true;
//                options.Events.RaiseFailureEvents = true;
//                options.Events.RaiseSuccessEvents = true;
//            })
//                .AddAspNetIdentity<TUserIdentity>()
//                .AddIdentityServerStoresWithDbContexts<TConfigurationDbContext, TPersistedGrantDbContext>(configuration, hostingEnvironment);

//            builder.AddCustomSigningCredential(configuration, logger);
//            builder.AddCustomValidationKey(configuration, logger);
//        }
//    }
//}

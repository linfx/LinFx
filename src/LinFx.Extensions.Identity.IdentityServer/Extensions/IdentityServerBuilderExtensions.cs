using LinFx.Extensions.IdentityServer.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LinFx.Extensions.Identity.IdentityServer.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace LinFx.Extensions.Identity.IdentityServer.Extensions
{
    public static class IdentityServerBuilderExtensions
    {
        private const string CertificateNotFound = "Certificate not found";
        private const string SigningCertificateThumbprintNotFound = "Signing certificate thumbprint not found";
        private const string SigningCertificatePathIsNotSpecified = "Signing certificate file path is not specified";
        private const string ValidationCertificateThumbprintNotFound = "Validation certificate thumbprint not found";
        private const string ValidationCertificatePathIsNotSpecified = "Validation certificate file path is not specified";

        /// <summary>
        /// Add custom signing certificate from certification store according thumbprint or from file
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddCustomSigningCredential(this IIdentityServerBuilder builder, IConfiguration configuration)
        {
            var certificateConfiguration = configuration.GetSection(nameof(CertificateConfiguration)).Get<CertificateConfiguration>();

            if (certificateConfiguration.UseSigningCertificateThumbprint)
            {
                if (string.IsNullOrWhiteSpace(certificateConfiguration.SigningCertificateThumbprint))
                    throw new Exception(SigningCertificateThumbprintNotFound);

                var certStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                certStore.Open(OpenFlags.ReadOnly);

                var certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, certificateConfiguration.SigningCertificateThumbprint, true);
                if (certCollection.Count == 0)
                    throw new Exception(CertificateNotFound);

                var certificate = certCollection[0];

                builder.AddSigningCredential(certificate);
            }
            else if (certificateConfiguration.UseSigningCertificatePfxFile)
            {
                if (string.IsNullOrWhiteSpace(certificateConfiguration.SigningCertificatePfxFilePath))
                    throw new Exception(SigningCertificatePathIsNotSpecified);

                if (File.Exists(certificateConfiguration.SigningCertificatePfxFilePath))
                {
                    try
                    {
                        builder.AddSigningCredential(new X509Certificate2(certificateConfiguration.SigningCertificatePfxFilePath, certificateConfiguration.SigningCertificatePfxFilePassword));
                    }
                    catch (CryptographicException e)
                    {
                        throw new Exception($"There was an error adding the key file - during the creation of the signing key {e.Message}");
                    }
                }
                else
                    throw new Exception($"Signing key file: {certificateConfiguration.SigningCertificatePfxFilePath} not found");
            }
            else if (certificateConfiguration.UseTemporarySigningKeyForDevelopment)
            {
                builder.AddDeveloperSigningCredential();
            }

            return builder;
        }

        /// <summary>
        /// Add custom validation key for signing key rollover
        /// http://docs.identityserver.io/en/latest/topics/crypto.html#signing-key-rollover
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddCustomValidationKey(this IIdentityServerBuilder builder, IConfiguration configuration)
        {
            var certificateConfiguration = configuration.GetSection(nameof(CertificateConfiguration)).Get<CertificateConfiguration>();

            if (certificateConfiguration.UseValidationCertificateThumbprint)
            {
                if (string.IsNullOrWhiteSpace(certificateConfiguration.ValidationCertificateThumbprint))
                {
                    throw new Exception(ValidationCertificateThumbprintNotFound);
                }

                var certStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                certStore.Open(OpenFlags.ReadOnly);

                var certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, certificateConfiguration.ValidationCertificateThumbprint, false);
                if (certCollection.Count == 0)
                {
                    throw new Exception(CertificateNotFound);
                }

                var certificate = certCollection[0];

                builder.AddValidationKey(certificate);

            }
            else if (certificateConfiguration.UseValidationCertificatePfxFile)
            {
                if (string.IsNullOrWhiteSpace(certificateConfiguration.ValidationCertificatePfxFilePath))
                {
                    throw new Exception(ValidationCertificatePathIsNotSpecified);
                }

                if (File.Exists(certificateConfiguration.ValidationCertificatePfxFilePath))
                {
                    try
                    {
                        builder.AddValidationKey(new X509Certificate2(certificateConfiguration.ValidationCertificatePfxFilePath, certificateConfiguration.ValidationCertificatePfxFilePassword));
                    }
                    catch (CryptographicException e)
                    {
                        throw new Exception($"There was an error adding the key file - during the creation of the validation key {e.Message}");
                    }
                }
                else
                {
                    throw new Exception($"Validation key file: {certificateConfiguration.SigningCertificatePfxFilePath} not found");
                }
            }

            return builder;
        }

        public static IIdentityServerBuilder AddUserClaimsPrincipalFactory<TUser>(this IIdentityServerBuilder builder)
            where TUser : class
        {
            builder.Services.AddTransientDecorator<IUserClaimsPrincipalFactory<TUser>, UserClaimsFactory<TUser>>();
            return builder;
        }

        public static void AddTransientDecorator<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddDecorator<TService>();
            services.AddTransient<TService, TImplementation>();
        }

        internal static void AddDecorator<TService>(this IServiceCollection services)
        {
            var registration = services.LastOrDefault(x => x.ServiceType == typeof(TService));
            if (registration == null)
            {
                throw new InvalidOperationException($"Service type: {typeof(TService).Name} not registered.");
            }
            if (services.Any(x => x.ServiceType == typeof(Decorator<TService>)))
            {
                throw new InvalidOperationException($"Decorator already registered for type: {typeof(TService).Name}.");
            }

            services.Remove(registration);

            if (registration.ImplementationInstance != null)
            {
                var type = registration.ImplementationInstance.GetType();
                var innerType = typeof(Decorator<,>).MakeGenericType(typeof(TService), type);
                services.Add(new ServiceDescriptor(typeof(Decorator<TService>), innerType, ServiceLifetime.Transient));
                services.Add(new ServiceDescriptor(type, registration.ImplementationInstance));
            }
            else if (registration.ImplementationFactory != null)
            {
                services.Add(new ServiceDescriptor(typeof(Decorator<TService>), provider =>
                {
                    return new DisposableDecorator<TService>((TService)registration.ImplementationFactory(provider));
                }, registration.Lifetime));
            }
            else
            {
                var type = registration.ImplementationType;
                var innerType = typeof(Decorator<,>).MakeGenericType(typeof(TService), registration.ImplementationType);
                services.Add(new ServiceDescriptor(typeof(Decorator<TService>), innerType, ServiceLifetime.Transient));
                services.Add(new ServiceDescriptor(type, type, registration.Lifetime));
            }
        }
    }
}

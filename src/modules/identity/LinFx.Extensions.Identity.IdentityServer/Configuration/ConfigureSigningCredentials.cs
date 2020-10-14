﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.AspNetCore.ApiAuthorization.IdentityServer
{
    internal class ConfigureSigningCredentials : IConfigureOptions<ApiAuthorizationOptions>
    {
        // We need to cast the underlying int value of the EphemeralKeySet to X509KeyStorageFlags
        // due to the fact that is not part of .NET Standard. This value is only used with non-windows
        // platforms (all .NET Core) for which the value is defined on the underlying platform.
        private const X509KeyStorageFlags UnsafeEphemeralKeySet = (X509KeyStorageFlags)32;
        private const string DefaultTempKeyRelativePath = "obj/tempkey.json";
        private readonly IConfiguration _configuration;
        private readonly ILogger<ConfigureSigningCredentials> _logger;

        public ConfigureSigningCredentials(
            IConfiguration configuration,
            ILogger<ConfigureSigningCredentials> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void Configure(ApiAuthorizationOptions options)
        {
            var key = LoadKey();
            options.SigningCredential = key;
        }

        public SigningCredentials LoadKey()
        {
            var key = new KeyDefinition();
            _configuration.Bind(key);
            switch (key.Type)
            {
                case KeySources.Development:
                    var developmentKeyPath = Path.Combine(Directory.GetCurrentDirectory(), key.FilePath ?? DefaultTempKeyRelativePath);
                    var createIfMissing = key.Persisted ?? true;
                    _logger.LogInformation($"Loading development key at '{developmentKeyPath}'.");
                    var developmentKey = new RsaSecurityKey(SigningKeysLoader.LoadDevelopment(developmentKeyPath, createIfMissing))
                    {
                        KeyId = "Development"
                    };
                    return new SigningCredentials(developmentKey, "RS256");
                case KeySources.File:
                    var pfxPath = Path.Combine(Directory.GetCurrentDirectory(), key.FilePath);
                    var pfxPassword = key.Password;
                    var storageFlags = GetStorageFlags(key);
                    _logger.LogInformation($"Loading certificate file at '{pfxPath}' with storage flags '{key.StorageFlags}'.");
                    return new SigningCredentials(new X509SecurityKey(SigningKeysLoader.LoadFromFile(pfxPath, key.Password, storageFlags)), "RS256");
                case KeySources.Store:
                    if (!Enum.TryParse<StoreLocation>(key.StoreLocation, out var storeLocation))
                    {
                        throw new InvalidOperationException($"Invalid certificate store location '{key.StoreLocation}'.");
                    }
                    _logger.LogInformation($"Loading certificate with subject '{key.Name}' in '{key.StoreLocation}\\{key.StoreName}'.");
                    return new SigningCredentials(new X509SecurityKey(SigningKeysLoader.LoadFromStoreCert(key.Name, key.StoreName, storeLocation, GetCurrentTime())), "RS256");
                case null:
                    throw new InvalidOperationException($"Key type not specified.");
                default:
                    throw new InvalidOperationException($"Invalid key type '{key.Type ?? "(null)"}'.");
            }
        }

        // for testing purposes only
        internal virtual DateTimeOffset GetCurrentTime() => DateTimeOffset.UtcNow;

        private X509KeyStorageFlags GetStorageFlags(KeyDefinition key)
        {
            var defaultFlags = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ?
                UnsafeEphemeralKeySet : (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? X509KeyStorageFlags.PersistKeySet :
                X509KeyStorageFlags.DefaultKeySet);

            if (key.StorageFlags == null)
            {
                return defaultFlags;
            }

            var flagsList = key.StorageFlags.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (flagsList.Length == 0)
            {
                return defaultFlags;
            }

            var result = ParseCurrentFlag(flagsList[0]);
            foreach (var flag in flagsList.Skip(1))
            {
                result |= ParseCurrentFlag(flag);
            }

            return result;

            X509KeyStorageFlags ParseCurrentFlag(string candidate)
            {
                if (Enum.TryParse<X509KeyStorageFlags>(candidate, out var flag))
                {
                    return flag;
                }
                else
                {
                    throw new InvalidOperationException($"Invalid storage flag '{candidate}'");
                }
            }
        }
    }
}

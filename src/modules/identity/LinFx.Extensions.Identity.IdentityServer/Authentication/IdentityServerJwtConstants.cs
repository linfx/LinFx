﻿namespace Microsoft.AspNetCore.ApiAuthorization.IdentityServer
{
    /// <summary>
    /// Constants for a default API authentication handler.
    /// </summary>
    public class IdentityServerJwtConstants
    {
        /// <summary>
        /// Scheme used for the default API policy authentication scheme.
        /// </summary>
        public const string IdentityServerJwtScheme = "IdentityServerJwt";

        /// <summary>
        /// Scheme used for the underlying default API JwtBearer authentication scheme.
        /// </summary>
        public const string IdentityServerJwtBearerScheme = "IdentityServerJwtBearer";
    }
}

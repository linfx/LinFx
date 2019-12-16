using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.ApiAuthorization.IdentityServer.Configuration
{
    internal class IdentityServerJwtDescriptor : IIdentityServerJwtDescriptor
    {
        //public IdentityServerJwtDescriptor(IWebHostEnvironment environment)
        //{
        //    Environment = environment;
        //}

        //public IWebHostEnvironment Environment { get; }

        public IDictionary<string, ResourceDefinition> GetResourceDefinitions()
        {
            return new Dictionary<string, ResourceDefinition>
            {
                //[Environment.ApplicationName + "API"] = new ResourceDefinition() { Profile = ApplicationProfiles.IdentityServerJwt }
                ["API"] = new ResourceDefinition() { Profile = ApplicationProfiles.IdentityServerJwt }
            };
        }
    }
}

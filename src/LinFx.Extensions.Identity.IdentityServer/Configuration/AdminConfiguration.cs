using LinFx.Extensions.Identity.IdentityServer.Configuration.Intefaces;

namespace LinFx.Extensions.Identity.IdentityServer.Configuration
{
    public class AdminConfiguration : IAdminConfiguration
    {
        public string IdentityAdminBaseUrl { get; set; } = "http://localhost:9000";
    }
}
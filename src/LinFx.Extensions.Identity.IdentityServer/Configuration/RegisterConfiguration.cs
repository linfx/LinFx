using LinFx.Extensions.Identity.IdentityServer.Configuration.Intefaces;

namespace LinFx.Extensions.Identity.IdentityServer.Configuration
{
    public class RegisterConfiguration : IRegisterConfiguration
    {
        public bool Enabled { get; set; } = true;
    }
}

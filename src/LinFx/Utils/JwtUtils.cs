using Microsoft.IdentityModel.Tokens;

namespace LinFx.Utils
{
    class JwtUtils
    {
    }
    
    public class JwtTokenOptions
    {
        public string Audience { get; set; }
        public RsaSecurityKey Key { get; set; }
        public SigningCredentials Credentials { get; set; }
    }
}

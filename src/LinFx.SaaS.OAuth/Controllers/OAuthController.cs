using System;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using LinFx.Utils;
using Microsoft.Extensions.Configuration;

namespace LinFx.SaaS.OAuth.Controllers
{
    [Route("api/[controller]/[action]")]
    public class OAuthController : Controller
    {
        private readonly IConfiguration _configuration;

        public OAuthController(IConfiguration configureation)
        {
            _configuration = configureation;
        }

        public IActionResult Token()
        {
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "Linsongbin"));
            JwtUtils.CreateJwtClaims(identity);

            var options = new JwtTokenOptions
            {
                Issuer = _configuration["jwt:Issuer"],
                Audience = _configuration["jwt:Audience"],
                SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["jwt:SecurityKey"])),
                Expiration = TimeSpan.FromDays(1),
            };
            options.SigningCredentials = new SigningCredentials(options.SecurityKey, SecurityAlgorithms.HmacSha256);
            var accessToken = JwtUtils.CreateAccessToken(options, identity.Claims);

            return Json(new
            {
                access_token = accessToken
            });
        }
    }
}
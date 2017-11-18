using System;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using LinFx.Utils;
using LinFx.SaaS.OAuth.Models;
using Microsoft.AspNetCore.Authorization;

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

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Token([FromBody] AuthenticateModel model)
        {
            if(model.UserName != "admin")
            {
                return BadRequest("密码不正码");
            }
           
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "Linsongbin"));
            JwtUtils.CreateJwtClaims(identity);

            var options = new JwtTokenOptions
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:SecurityKey"])),
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
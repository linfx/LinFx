using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using LinFx.Utils;
using LinFx.SaaS.OAuth.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
            if (model.UserName != "admin")
            {
                return BadRequest("密码不正码");
            }

            //var user = new UserInfo

            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(Security.ClaimTypes.Id, "Id"));
            identity.AddClaim(new Claim(Security.ClaimTypes.Name, "Linsongbin"));

            var options = new JwtTokenOptions
            {
                Issuer = _configuration["JwtBearer:Issuer"],
                Audience = _configuration["JwtBearer:Audience"],
                SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtBearer:SecurityKey"])),
                Expiration = TimeSpan.FromDays(14),
            };
            options.SigningCredentials = new SigningCredentials(options.SecurityKey, SecurityAlgorithms.HmacSha256);

            var accessToken = JwtUtils.CreateAccessToken(identity, options);
            return Json(new
            {
                access_token = accessToken,
                token_type = "Bearer",
                //expires_in = new DateTimeOffset(DateTime.UtcNow, options.Expiration).ToUnixTimeSeconds(),
                profile = new
                {
                    id = "11",
                    name = "name"
                }
            });
        }
    }
}
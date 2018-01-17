using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using LinFx.Utils;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using LinFx.SaaS.OAuth.Models;

namespace LinFx.SaaS.OAuth.Controllers
{
    [Route("api/[controller]")]
    public class OAuthController : Controller
    {
        private readonly IConfiguration _configuration;

        public OAuthController(IConfiguration configureation)
        {
            _configuration = configureation;
        }

        [HttpGet("token")]
        [AllowAnonymous]
        public IActionResult Token(AuthenticateModel model)
        {
            try
            {
                var identity = new ClaimsIdentity();
                identity.AddClaim(new Claim(Security.ClaimTypes.Client_Id, model.Client_Id));
                identity.AddClaim(new Claim(Security.ClaimTypes.Client_Secret, model.Client_Secret));

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
                    expires_in = DateTimeOffset.UtcNow.Add(options.Expiration).ToUnixTimeSeconds(),
                    item = new
                    {
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { errcode = 400, errmsg = ex.Message });
            }
        }
    }
}
using IdentityService.Dtos;
using LinFx;
using LinFx.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Controllers;

/// <summary>
/// 账户api
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    /// <summary>
    /// 用户名/邮箱 登录
    /// </summary> 
    /// <param name="input">登录表单</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("login")]
    //[ProducesResponseType(typeof(Result<LoginResult>), 200)]
    public Result Login(LoginInput input)
    {
        RSA rsa = RSA.Create();

        var tokenHandler = new JwtSecurityTokenHandler();
        //var key = new RsaSecurityKey(rsa);
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("5172510c6f5640a796070c3cdf8a937e"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);


        var claims = new List<Claim>
        {
            new(JwtClaimTypes.Subject , input.UserName),
            new(JwtClaimTypes.Name , "张三"),
            new(JwtClaimTypes.AuthenticationMethod , "2"),
            new(JwtClaimTypes.Audience , "DEVICE"),
            new(JwtClaimTypes.ClientId , "机身码"),
        };

        var token = tokenHandler.WriteToken(new JwtSecurityToken(
            //issuer: issuer,
            //audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        ));


        return Result.Ok(token);
    }
}

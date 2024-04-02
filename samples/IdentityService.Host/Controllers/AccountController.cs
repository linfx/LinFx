using IdentityService.Dtos;
using LinFx;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("5172510c6f5640a796070c3cdf8a937e");

        var claims = new List<Claim>
        {
            //new(ClaimTypes.Id, user.Id),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Result.Ok(tokenString);
    }
}

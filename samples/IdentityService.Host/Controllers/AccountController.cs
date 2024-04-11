using IdentityService.Dtos;
using LinFx;
using LinFx.Extensions.Identity;
using LinFx.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityService.Controllers;

/// <summary>
/// 账户api
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController(JwtTokenOptions options) : ControllerBase
{
    private readonly JwtTokenOptions options = options;

    /// <summary>
    /// 用户名/邮箱 登录
    /// </summary> 
    /// <param name="input">登录表单</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(Result<LoginResult>), 200)]
    public Result Login(LoginInput input)
    {
        var claims = new List<Claim>
        {
            new(JwtClaimTypes.Subject , input.UserName),
            new(JwtClaimTypes.Name , "张三"),
            new(JwtClaimTypes.AuthenticationMethod , "2"),
            new(JwtClaimTypes.Audience , "DEVICE"),
            new(JwtClaimTypes.ClientId , "009"),
        };

        var token = options.CreateToken(claims);
        return Result.Ok(token);
    }
}

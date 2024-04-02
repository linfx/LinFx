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
        rsa.ImportFromPem(PRIVATE_KEY);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new RsaSecurityKey(rsa);
        //var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("5172510c6f5640a796070c3cdf8a937e"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);


        var claims = new List<Claim>
        {
            new(JwtClaimTypes.Subject , input.UserName),
            new(JwtClaimTypes.Name , "张三"),
            new(JwtClaimTypes.AuthenticationMethod , "2"),
            new(JwtClaimTypes.Audience , "DEVICE"),
            new(JwtClaimTypes.ClientId , "009"),
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


    public const string PRIVATE_KEY = @"
-----BEGIN PRIVATE KEY-----
MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCus+q5AVayER1j
YK2QeUP4qEqqJ4nFK7i8QGoxbdDKek3ax0dosA8Ed75tx6u+nunASYNwcm0Ccsit
/6eDgyj6JTMbTnU7JkzDBuOBtaPPezpgB2MKm7HOxQHaz6sb1EGJqlztSRzlQi9+
Gu0O77BdmKfzAJ/lsqp0MPk77ngSCKtSmWWmJsexhXg65rbBUEM+80VpvjrjfV+1
dNwNrHKNf/tn3pfvMYf6W0TcqdHfnu1j8e5g52lYdpP6LbBffCjLtypwLA3MAIJp
n67QH8C3ljAqDk/Y5x7G8gyCT3CE6R8lJ1S7PlwmhUMSwXnviZRQW7sNui0hADcS
FKvoyp2pAgMBAAECggEAAmiRklZd4xuQS6fwDJ9XXAy55smfwRuest2yeRb5+2if
PSZlKiDPPDEIa4wGppybPV7mFH1HWnLJqHhvT0VWIci0gSpePGAFS+UC6u1dokUh
I2TGrUVDOMYHhgQ56CrPcn3qRQ0ufEyiyfrGDOAqDshFfHikO67wYaSxMu3RMvJs
0Bn41MucUNdQg4AmtExDIDnXKD6qmAcgbSTZ6mi21K46HnfZgvfefo0kFaqBRfKr
PEckJnpZoV+idQ+vngAnTGdcokLzhkh6lam7HLH34Ts3+kQesSEN/jXKKDP/5Kbb
z0pA2sCvMGYiu925/XhOIXWESbtMZiT/CM3y9QwsdwKBgQDcu1GGHfX6dnBsMja8
iCku1quxaNlMXWU1PD5ki/mRncq4/RboJochhhXZaZnSvBwBdwXf7jLm0VO4fEQx
5v8qNggVk6sz60FLCGkaodaZrdRNzV6tl84V2BCEmxcceL5AWrJg4arO93Cmvvfg
VGnYAtKOG/15MwNF1IuCFISenwKBgQDKndvMDBpi2W8Mpyu9dfBqi/2EZT9dHTH2
i40nGvNFaT7hCJxXSnhmnk8oEZE30hEbisdiSKDttavCgJAwyCwoOEbxRFMCnOKr
71ycRuntms0EXhM9mN03Cv5T6XFAvMKcclHG8DuXE8mhAH764OJdBmAFWP2xZU4R
vVP2Z0qGtwKBgBZVYadHSscfyro3zm/++uPBVrfpmuruHDppsQptW0APjh5vhSzS
ESQkM/u+gpDe9Zp0V4TTzhSqo2TsNd4yuCimNOUx/sWPDRkxbakDLPp5qxyAJg88
Z96v8OPp6Um5Mc7DHn9M9gJg6OYGNoBdKiRwDKvSSzBDKBadrcvolY6TAoGBAKf7
JEURK7cUSYsV9Z+H7iCHm1Ful4/dKpUasZXgNBwx2126Q9Pi+9soGLxBhEHhOSm6
bv8+85zgZ1xENkcbTfCyZ66TVLbv1sVxFzjBWiQRmTOrwwvodk6yIJT1D136oOYP
qVjjxqPEVf/TZ3MbitBQ/r6wPvQZ5xWx/7BjhlgVAoGAMzOvD5HEigTlaePMehA+
BpRjTqpr6ug9ANTgFREyVRRZxqc3iw3pGebxVyU9Z3iop9lkHvvAhZAArwN9UQ9N
SVmTWuqahKviRNBu8LMa4AX0HgZkAvHzHpYbfjU9iBR3buH7FMp5yf8tS/gWN3qu
n18GLGjdMeos2rWAbrVNcEI=
-----END PRIVATE KEY-----
";


    public const string PUBLIC_KEY = @"
-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArrPquQFWshEdY2CtkHlD
+KhKqieJxSu4vEBqMW3QynpN2sdHaLAPBHe+bcervp7pwEmDcHJtAnLIrf+ng4Mo
+iUzG051OyZMwwbjgbWjz3s6YAdjCpuxzsUB2s+rG9RBiapc7Ukc5UIvfhrtDu+w
XZin8wCf5bKqdDD5O+54EgirUpllpibHsYV4Oua2wVBDPvNFab46431ftXTcDaxy
jX/7Z96X7zGH+ltE3KnR357tY/HuYOdpWHaT+i2wX3woy7cqcCwNzACCaZ+u0B/A
t5YwKg5P2OcexvIMgk9whOkfJSdUuz5cJoVDEsF574mUUFu7DbotIQA3EhSr6Mqd
qQIDAQAB
-----END PUBLIC KEY-----
";

}

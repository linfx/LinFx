using LinFx.Extensions.Modularity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 账户模块
/// </summary>
public class AccountModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        // 添加身份认证服务
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; // 可根据需求修改
            options.SaveToken = true;             // 可根据需求修改

            /* 
            配置 JWT 选项，包括 Issuer、Audience、密钥等。
            这些值应该根据你的实际应用程序设置进行配置。
            */
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "your_issuer",
                ValidAudience = "your_audience",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key"))
            };
        });
    }
}

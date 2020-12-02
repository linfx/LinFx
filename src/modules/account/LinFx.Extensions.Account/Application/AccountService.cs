using LinFx.Extensions.Identity;
using LinFx.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinFx.Extensions.Account
{
    /// <summary>
    /// 账户服务
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public async Task<Result> LoginAsync(LoginInput input)
        {
            User user = null;
            if (RegexUtils.VerifyPhone(input.UserName).Succeeded)
            {
                // 手机号验证
                user = await _userManager.Users.FirstOrDefaultAsync(c => c.PhoneNumber == input.UserName);
                if (user != null && !user.PhoneNumberConfirmed)
                    return Result.Failed("手机未验证，不允许用手机登录");
            }
            else if (RegexUtils.VerifyEmail(input.UserName).Succeeded)
            {
                // 邮箱验证
                user = await _userManager.FindByEmailAsync(input.UserName);
                if (user != null && !user.EmailConfirmed)
                    return Result.Failed("邮箱未验证，不允许用邮箱登录");
            }

            // 用户名登录验证
            if (user == null)
                user = await _userManager.FindByNameAsync(input.UserName);

            if (user == null)
                return Result.Failed("用户不存在");

            //if (!user.IsActive)
            //    return Result.Failed("用户已禁用");

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            // update false -> set lockoutOnFailure: true
            var signInResult = await _signInManager.PasswordSignInAsync(user, input.Password, input.RememberMe, true);
            if (signInResult.IsLockedOut)
            {
                return Result.Failed("用户已锁定，请稍后重试");
            }
            else if (signInResult.IsNotAllowed)
            {
                return Result.Failed("用户邮箱未验证或手机未验证，不允许登录");
            }
            else if (signInResult.RequiresTwoFactor)
            {
                // 当手机或邮箱验证通过时，TwoFactorEnabled才会生效，否则，则RequiresTwoFactor不可能=true。
                // 设置启用双因素身份验证。
                // 用户帐户已启用双因素身份验证，因此您必须提供第二个身份验证因素。
                var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
                //return Result.Fail(userFactors.Select(c => c), "当前账号存在安全风险，请进一步验证");
                var ls = new List<(string, string)>();
                foreach (var item in userFactors)
                {
                    if (item == "Phone")
                        ls.Add((item, StringUtils.PhoneEncryption(user.PhoneNumber)));
                    else if (item == "Email")
                        ls.Add((item, StringUtils.EmailEncryption(user.Email)));
                }
                return Result.Failed(new
                {
                    Providers = ls.Select(c => new { key = c.Item1, value = c.Item2 }),
                    signInResult.RequiresTwoFactor
                }, "当前账号存在安全风险，请进一步验证身份");
            }
            //else if (signInResult.Succeeded)
            //{
            //    var token = await _tokenService.CreateAccessTokenAsync(user);
            //    var loginResult = new LoginResult
            //    {
            //        AccessToken = token,
            //        Avatar = user.AvatarUrl,
            //        Email = user.Email,
            //        Name = user.FullName,
            //        Phone = user.PhoneNumber
            //    };
            //    if (includeRefreshToken)
            //    {
            //        loginResult.RefreshToken = _tokenService.CreateRefreshToken();
            //        user.RefreshTokenHash = _userManager.PasswordHasher.HashPassword(user, loginResult.RefreshToken);
            //        await _userManager.UpdateAsync(user);
            //    }
            //    if (includeRoles)
            //    {
            //        loginResult.Roles = await _userManager.GetRolesAsync(user);
            //    }
            //    return Result.Ok(loginResult);
            //}
            return Result.Failed("用户名或密码错误");
        }

        public Task<Result> RegisterAsync(RegisterInput input)
        {
            throw new NotImplementedException();
        }
    }
}

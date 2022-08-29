using LinFx.Application.Services;
using LinFx.Extensions.Identity;
using LinFx.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.Account.Application;

/// <summary>
/// 账户服务
/// </summary>
public class AccountService : ApplicationService, IAccountService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    /// <summary>
    /// 获取我的账户
    /// </summary>
    /// <returns></returns>
    public ValueTask<Result> Get()
    {
        //var user = await _session.GetCurrentUserAsync();
        //if (user == null)
        //    return Result.Failed("Error");

        //var result = user.MapTo<AccountResult>();
        //result.NotifyCount = 20;
        //result.Email = StringUtils.EmailEncryption(user.Email);
        //result.PhoneNumber = StringUtils.PhoneEncryption(user.PhoneNumber);

        //return Result.Ok(result);

        throw new NotImplementedException();
    }

    /// <summary>
    /// 修改我的账户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public ValueTask<Result> Put(AccountInput input)
    {
        //var user = await _session.GetCurrentUserAsync();
        //if (user == null)
        //    return Result.Failed("Error");

        //if (input.MediaId.HasValue)
        //{
        //    var media = await _mediaRepository.FirstOrDefaultAsync(p => p.Id == input.MediaId.Value);
        //    if (media != null)
        //    {
        //        //user.AvatarId = media.Id;
        //        //user.AvatarUrl = media.Url;
        //    }
        //}

        //input.MapTo(user);
        //await _userManager.UpdateAsync(user);
        //await _signInManager.SignInAsync(user, false);
        //return Result.Ok();

        throw new NotImplementedException();
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async ValueTask<Result> LoginAsync(LoginInput input)
    {
        IdentityUser? user = null;
        if (RegexUtils.VerifyPhone(input.UserName))
        {
            // 手机号验证
            user = await _userManager.Users.FirstOrDefaultAsync(c => c.PhoneNumber == input.UserName);
            if (user != null && !user.PhoneNumberConfirmed)
                return Result.Failed("手机未验证，不允许用手机登录");
        }
        else if (RegexUtils.VerifyEmail(input.UserName))
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
        else if (signInResult.Succeeded)
        {
            //var token = await _tokenService.CreateAccessTokenAsync(user);
            //var loginResult = new LoginResult
            //{
            //    AccessToken = token,
            //    Avatar = user.AvatarUrl,
            //    Email = user.Email,
            //    Name = user.FullName,
            //    Phone = user.PhoneNumber
            //};
            //if (includeRefreshToken)
            //{
            //    loginResult.RefreshToken = _tokenService.CreateRefreshToken();
            //    user.RefreshTokenHash = _userManager.PasswordHasher.HashPassword(user, loginResult.RefreshToken);
            //    await _userManager.UpdateAsync(user);
            //}
            //if (includeRoles)
            //{
            //    loginResult.Roles = await _userManager.GetRolesAsync(user);
            //}
            //return Result.Ok(loginResult);
        }
        return Result.Failed("用户名或密码错误");
    }

    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async ValueTask<Result> RegisterAsync(RegisterInput input)
    {
        var user = new IdentityUser
        {
            UserName = input.UserName,
        };
        await _userManager.CreateAsync(user, input.Password);
        return Result.Ok();
    }

    /// <summary>
    /// 重置密码 - 手机找回
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async ValueTask<Result> ResetPasswordByPhone(ResetPasswordPutInput input)
    {
        var user = await _userManager.FindByNameAsync(input.UserName);
        if (user == null)
            throw new Exception("用户不存在");

        //if (!user.IsActive)
        //    throw new Exception("用户已禁用");

        if (string.IsNullOrWhiteSpace(user.PhoneNumber))
            throw new Exception("用户未绑定手机，无法通过手机找回密码");

        ////5分钟内的验证码
        //var sms = await _smsSendRepository
        //    .Where(c => c.PhoneNumber == user.PhoneNumber && c.IsSucceed && !c.IsUsed && c.TemplateType == SmsTemplateType.Captcha && c.CreatedOn.CompareTo(DateTime.UtcNow.AddMinutes(-5)) > 0)
        //    .OrderByDescending(c => c.CreatedOn)
        //    .FirstOrDefaultAsync();
        //if (sms == null)
        //    return Result.Failed("验证码不存在或已失效，请重新获取验证码");

        //if (sms.Value != input.Code)
        //    return Result.Failed("验证码错误");

        ////设置验证码被使用
        //sms.IsUsed = true;
        //await _smsSendRepository.UpdateAsync(sms);

        //重新生成重置密码的令牌
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, code, input.Password);
        if (result.Succeeded)
        {
            //_tokenService.RemoveUserToken(user.Id);
            return Result.Ok();
        }

        return Result.Failed("重置密码失败，验证码错误或链接已失效，请稍后重试");
    }

    /// <summary>
    /// 移除手机绑定
    /// </summary>
    /// <returns></returns>
    public async ValueTask<Result> RemovePhoneNumber(IUser current)
    {
        //var user = await _session.GetCurrentUserAsync();
        //if (user != null)
        //{
        //    var result = await _userManager.SetPhoneNumberAsync(user, null);
        //    if (result.Succeeded)
        //    {
        //        //await _signInManager.SignInAsync(user, isPersistent: false);
        //        return Result.Ok();
        //    }
        //}
        return Result.Failed("解绑失败");
    }

    /// <summary>
    /// 移除邮箱绑定
    /// </summary>
    /// <returns></returns>
    public async Task<Result> RemoveEmail()
    {
        //var user = await _session.GetCurrentUserAsync();
        //if (user != null)
        //{
        //    var result = await _userManager.SetEmailAsync(user, null);
        //    if (result.Succeeded)
        //    {
        //        //await _signInManager.SignInAsync(user, isPersistent: false);
        //        return Result.Ok();
        //    }
        //}
        return Result.Failed("解绑失败");
    }

    /// <summary>
    /// 注销
    /// </summary>
    /// <returns></returns>
    public async Task<Result> Logout()
    {
        //var user = await _session.GetCurrentUserAsync();
        //await _signInManager.SignOutAsync();

        //if (user != null)
        //    _tokenService.RemoveUserToken(user.Id);

        //_logger.LogInformation(4, "User logged out.");
        return Result.Ok();
    }
}

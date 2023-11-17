using System.Text.RegularExpressions;

namespace LinFx.Utils;

public static class RegexUtils
{
    const string patternEmail = @"\w[-\w.+]*@([A-Za-z0-9][-A-Za-z0-9]+\.)+[A-Za-z]{2,14}";
    const string patternPhone = @"^\d{11}$";

    /// <summary>
    /// 验证是否电子邮件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static Result VerifyEmail(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return Result.Failed("请输入邮箱地址");

        var regex = new Regex(patternEmail);
        if (!regex.IsMatch(input))
            return Result.Failed("邮箱地址格式错误");

        return Result.Ok("邮箱地址格式正确");
    }

    /// <summary>
    /// 验证是否手机
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static Result VerifyPhone(string input)
    {
        if (string.IsNullOrWhiteSpace(input.Trim()))
            return Result.Failed("请输入手机号码");

        var regex = new Regex(patternPhone);
        if (!regex.IsMatch(input.Trim()))
            return Result.Failed("手机号格式错误");

        return Result.Ok("手机号格式正确");
    }
}

using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.Account;

/// <summary>
/// 重置密码表单
/// </summary>
public class ResetPasswordPutInput
{
    /// <summary>
    /// 账号
    /// </summary>
    [Required(ErrorMessage = "用户名参数异常")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 验证码
    /// </summary>
    [Required(ErrorMessage = "请输入验证码")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 新密码
    /// </summary>
    [Required(ErrorMessage = "请输入密码")]
    [StringLength(100, ErrorMessage = "密码长度6-32字符", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 确认密码
    /// </summary>
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "两次输入的密码不匹配")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
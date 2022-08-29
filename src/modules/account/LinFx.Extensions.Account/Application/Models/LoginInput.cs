using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.Account;

/// <summary>
/// 登录表单 
/// </summary>
public class LoginInput
{
    /// <summary>
    /// 账号
    /// </summary>
    [Required]
    public virtual string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 密码
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    public virtual string Password { get; set; } = string.Empty;

    /// <summary>
    /// 记住
    /// </summary>
    public bool RememberMe { get; set; }
}

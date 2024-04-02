using System.ComponentModel.DataAnnotations;

namespace IdentityService.Dtos;

/// <summary>
/// 登录表单
/// </summary>
public class LoginInput
{
    /// <summary>
    /// 账号
    /// </summary>
    [Required]
    public required string UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    /// <summary>
    /// 记住
    /// </summary>
    public bool RememberMe { get; set; }
}

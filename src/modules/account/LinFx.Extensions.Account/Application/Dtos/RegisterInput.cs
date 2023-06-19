using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.Account;
    
/// <summary>
/// 注册
/// </summary>
public class RegisterInput
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
}

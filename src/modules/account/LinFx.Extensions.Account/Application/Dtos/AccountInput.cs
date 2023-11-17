using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.Account;

public class AccountInput
{
    /// <summary>
    /// 昵称/全名
    /// </summary>
    [Required(ErrorMessage = "请输入您的昵称/全名")]
    [StringLength(20, ErrorMessage = "昵称不能超过20个字符")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 头像(媒体ID)
    /// </summary>
    public long? MediaId { get; set; }
}
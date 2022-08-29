namespace LinFx.Extensions.Account;

/// <summary>
/// 双因子登录表单
/// </summary>
public class LoginTwoFactorInput
{
    /// <summary>
    /// The two factor authentication provider to validate the code against.
    /// </summary>
    public string SelectedProvider { get; set; } = string.Empty;

    /// <summary>
    /// The two factor authentication code to validate.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    public bool RememberMe { get; set; }

    public bool RememberBrowser { get; set; }
}

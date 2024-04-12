using System.Text.Json.Serialization;

namespace LinFx.Extensions.Identity;

public class JwtTokenResult
{
    /// <summary>
    /// access_token
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// refresh_token
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// 有效时间
    /// </summary>
    [JsonPropertyName("expires_in")]
    public double Expires { get; set; }

    /// <summary>
    /// token 类型
    /// </summary>
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = "Bearer";
}

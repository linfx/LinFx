namespace LinFx.Extensions.Account
{
    public class AuthenticationOptions
    {
        public JwtOptions Jwt { get; set; } = new JwtOptions();
    }

    public class JwtOptions
    {
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 提示：暂未开启JWT过期策略，此时间用于令牌生成过期时间及令牌续签过期时间
        /// </summary>
        public int AccessTokenDurationInMinutes { get; set; }
    }
}
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LinFx.Extensions.Identity
{
    public class JwtTokenOptions
    {
        private readonly JwtSecurityTokenHandler tokenHandler = new();

        /// <summary>
        /// Issuer字段
        /// </summary>
        public string? Issuer { get; set; }

        /// <summary>
        /// Audience字段
        /// </summary>
        public string? Audience { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public TimeSpan Expires { get; set; } = TimeSpan.FromDays(7d);

        /// <summary>
        /// 创建jwt token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public JwtTokenResult CreateToken(IEnumerable<Claim> claims)
        {
            //var key = new RsaSecurityKey(rsa);
            //RSA rsa = RSA.Create();
            //rsa.ImportFromPem(PRIVATE_KEY);
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("5172510c6f5640a796070c3cdf8a937e"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);

            var token = tokenHandler.WriteToken(new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(Expires),
                signingCredentials: creds
            ));

            return new JwtTokenResult
            {
                AccessToken = token,
                Expires = Expires.TotalMilliseconds,
            };
        }
    }
}

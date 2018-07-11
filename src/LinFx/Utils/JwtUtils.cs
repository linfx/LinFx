//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;

//namespace LinFx.Utils
//{
//    public static class JwtUtils
//    {
//        public static void AddJwtClaims(ClaimsIdentity identity)
//        {
//            var nameIdClaim = identity.Claims.First(c => c.Type == Security.ClaimTypes.Id);
//            identity.AddClaims(new[]
//            {
//                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),         //该JWT所面向的用户
//                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //JWT ID为web token提供唯一标识
//                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64) //在什么时候签发的token
//            });
//        }

//        public static string CreateAccessToken(ClaimsIdentity identity, JwtTokenOptions options, TimeSpan? expiration = null)
//        {
//            var now = DateTime.UtcNow;
//            var jwtSecurityToken = new JwtSecurityToken(
//                issuer: options.Issuer,
//                audience: options.Audience,
//                claims: identity.Claims,
//                notBefore: now,
//                expires: now.Add(expiration ?? options.Expiration),
//                signingCredentials: options.SigningCredentials
//            );
//            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
//        }
//    }
    
//    public class JwtTokenOptions
//    {
//        /// <summary>
//        /// 发行人
//        /// </summary>
//        public string Issuer { get; set; }
//        /// <summary>
//        /// 订阅人
//        /// </summary>
//        public string Audience { get; set; }
//        /// <summary>
//        /// 有效期
//        /// </summary>
//        public TimeSpan Expiration { get; set; } = TimeSpan.FromDays(14);
//        /// <summary>
//        /// 秘钥
//        /// </summary>
//        public SymmetricSecurityKey SecurityKey { get; set; }
//        /// <summary>
//        /// 签名验证
//        /// </summary>
//        public SigningCredentials SigningCredentials { get; set; }
//    }
//}

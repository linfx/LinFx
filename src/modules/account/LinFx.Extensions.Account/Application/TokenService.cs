using LinFx.Extensions.Caching;
using LinFx.Extensions.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LinFx.Extensions.Account.Application
{
    /// <summary>
    /// token 服务
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IStaticCacheManager _cacheManager;
        private readonly UserManager<User> _userManager;
        private readonly AuthenticationConfig _config;

        public TokenService(
            IStaticCacheManager cacheManager,
            UserManager<User> userManager,
            AuthenticationConfig config)
        {
            _cacheManager = cacheManager;
            _userManager = userManager;
            _config = config;
        }

        /// <summary>
        /// 生成 access_token
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public async Task<string> CreateAccessTokenAsync(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Jwt.Key));
            var minutes = _config.Jwt.AccessTokenDurationInMinutes;
            var utcNow = DateTime.UtcNow;
            var expires = utcNow.AddMinutes(minutes);
            var claims = await BuildClaims(user);
            var jwtToken = new JwtSecurityToken(
                issuer: _config.Jwt.Issuer,
                audience: "Anyone",
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            //_cacheManager.Set(ShopFxKeys.UserJwtTokenPrefix + user.Id, new UserTokenCache
            //{
            //    UserId = user.Id,
            //    Token = token,
            //    TokenCreatedOnUtc = utcNow,
            //    TokenUpdatedOnUtc = utcNow,
            //    TokenExpiresOnUtc = expires
            //}, minutes);
            return token;
        }

        /// <summary>
        /// 生成 refresh_token
        /// </summary>
        /// <returns></returns>
        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// 验证 token
        /// </summary>
        /// <param name="identityId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool ValidateToken(string identityId, string token)
        {
            //if (string.IsNullOrWhiteSpace(identityId) || string.IsNullOrWhiteSpace(token))
            //    return false;

            //var utcNow = DateTimeOffset.UtcNow;
            //var userToken = _cacheManager.Get<UserTokenCache>(UserJwtTokenPrefix + identityId);
            //if (userToken == null)
            //{
            //    return false;
            //}
            //else if (string.IsNullOrWhiteSpace(userToken.Token))
            //{
            //    _cacheManager.Remove(ShopFxKeys.UserJwtTokenPrefix + identityId);
            //    return false;
            //}

            //var validate = userToken.Token.Equals(token, StringComparison.OrdinalIgnoreCase);
            //if (!validate)
            //{
            //    _cacheManager.Remove(ShopFxKeys.UserJwtTokenPrefix + identityId);
            //    return false;
            //}

            //var minutes = _config.Jwt.AccessTokenDurationInMinutes;
            //if (userToken.TokenExpiresOnUtc != null && userToken.TokenExpiresOnUtc < utcNow)
            //{
            //    // 过期
            //    validate = false;
            //    _cacheManager.Remove(ShopFxKeys.UserJwtTokenPrefix + identityId);
            //}
            //else if (minutes > 0 && userToken.TokenExpiresOnUtc == null)
            //{
            //    // 当调整配置时，访问时更新配置（无过期时间->有过期时间）
            //    userToken.TokenUpdatedOnUtc = utcNow;
            //    userToken.TokenExpiresOnUtc = utcNow.AddMinutes(minutes);
            //    _cacheManager.Set(ShopFxKeys.UserJwtTokenPrefix + userToken.UserId, userToken, minutes);
            //}
            //else if (userToken.TokenExpiresOnUtc != null && userToken.TokenType != UserTokenType.Disposable && (utcNow - userToken.TokenUpdatedOnUtc).TotalMinutes >= 1)
            //{
            //    // 如果是一次性令牌则不续签
            //    // 每分钟自动续签
            //    // 注意：默认jwt令牌不开启过期策略的
            //    userToken.TokenUpdatedOnUtc = utcNow;
            //    userToken.TokenExpiresOnUtc = utcNow.AddMinutes(minutes);
            //    _cacheManager.Set(ShopFxKeys.UserJwtTokenPrefix + userToken.UserId, userToken, minutes);
            //}
            //return validate;
            throw new NotImplementedException();
        }

        /// <summary>
        /// 移降用户 token
        /// </summary>
        /// <param name="userId"></param>
        public void RemoveUserToken(long userId)
        {
            //_cacheManager.Remove(ShopFxKeys.UserJwtTokenPrefix + userId);
        }

        /// <summary>
        /// 从 token 中获取 Principal
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidIssuer = _config.Jwt.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Jwt.Key)),
                ValidateLifetime = false //in this case, we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !string.Equals(jwtSecurityToken.Header.Alg, SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }

        /// <summary>
        /// 生成用户 claims
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected virtual async Task<IList<Claim>> BuildClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti,  Guid.NewGuid().ToString()),
                // https://stackoverflow.com/questions/51119926/jwt-authentication-usermanager-getuserasync-returns-null
                // default the value of UserIdClaimType is ClaimTypes.NameIdentifier, i.e. "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                new Claim(JwtRegisteredClaimNames.NameId,  user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            };
            var userRoles = await _userManager.GetRolesAsync(user);
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            return claims;
        }
    }
}
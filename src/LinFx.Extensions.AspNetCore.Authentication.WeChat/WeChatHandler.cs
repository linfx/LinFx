using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.Wechat;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Globalization;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Microsoft.AspNetCore.Authentication.WeChat;

internal class WeChatHandler(IOptionsMonitor<WeChatOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISecureDataFormat<AuthenticationProperties> secureDataFormat) : OAuthHandler<WeChatOptions>(options, logger, encoder)
{
    private readonly ISecureDataFormat<AuthenticationProperties> _secureDataFormat = secureDataFormat;

    /// <summary>
    /// Called after options/events have been initialized for the handler to finish initializing itself.
    /// </summary>
    /// <returns>A task</returns>
    protected override async Task InitializeHandlerAsync()
    {
        await base.InitializeHandlerAsync();
        if (Options.UseCachedStateDataFormat)
        {
            Options.StateDataFormat = _secureDataFormat;
        }
    }

    /*
     * Challenge 盘问握手认证协议
     * 这个词有点偏，好多翻译工具都查不出。
     * 这个解释才是有些靠谱 http://abbr.dict.cn/Challenge/CHAP
     */
    /// <summary>
    /// 构建请求CODE的Url地址（这是第一步，准备工作）
    /// </summary>
    /// <param name="properties"></param>
    /// <param name="redirectUri"></param>
    /// <returns></returns>
    protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
    {
        var scope = FormatScope();
        var state = Options.StateDataFormat.Protect(properties);

        if (!string.IsNullOrEmpty(Options.CallbackUrl))
        {
            redirectUri = Options.CallbackUrl;
        }

        var parameters = new Dictionary<string, string?>
        {
            { "appid", Options.ClientId },
            { "scope", scope },
            { "state", state },
            { "response_type", "code" },
            { "redirect_uri", redirectUri },
        };

        //判断当前请求是否由微信内置浏览器发出
        var isMicroMessenger = Options.IsWeChatBrowser(Request);
        var ret = QueryHelpers.AddQueryString(isMicroMessenger ? Options.AuthorizationEndpoint2 : Options.AuthorizationEndpoint, parameters);

        //scope 不能被UrlEncode
        ret += $"&scope={scope}&state={state}";

        return ret;
    }

    ///// <summary>
    ///// 处理微信授权结果（接收微信授权的回调）
    ///// </summary>
    ///// <returns></returns>
    //protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
    //{
    //    /*
    //     * 返回示例：
    //     * scope: snsapi_userinfo
    //     * state: wechat_sdk_demo
    //     * 
    //     * scope 应用授权作用域，获取用户个人信息则填写 snsapi_userinfo （只能填 snsapi_userinfo）
    //     * state 用于保持请求和回调的状态，授权请求后原样带回给第三方。
    //     *       该参数可用于防止 csrf 攻击（跨站请求伪造攻击），建议第三方带上该参数，可设置为简单的随机数加 session 进行校验。
    //     *       在state传递的过程中会将该参数作为url的一部分进行处理，因此建议对该参数进行url encode操作，防止其中含有影响url解析的特殊字符（如'#'、'&'等）导致该参数无法正确回传。
    //    */
    //    //微信只会发送code和state两个参数，不会返回错误消息
    //    //若用户禁止授权，则重定向后不会带上code参数，仅会带上state参数
    //    var code = Request.Query["code"];
    //    var state = Request.Query["state"];

    //    //第一步，处理工作
    //    AuthenticationProperties? properties = Options.StateDataFormat.Unprotect(state);
    //    if (properties == null)
    //        return HandleRequestResult.Fail("The oauth state was missing or invalid.");

    //    // OAuth2 10.12 CSRF
    //    if (!ValidateCorrelationId(properties))
    //        return HandleRequestResult.Fail("Correlation failed.");

    //    if (StringValues.IsNullOrEmpty(code)) //code为null就是
    //        return HandleRequestResult.Fail("Code was not found.");

    //    //第二步，通过 code 获取 access_token
    //    var redirectUrl = !string.IsNullOrEmpty(Options.CallbackUrl) ? Options.CallbackUrl : BuildRedirectUri(Options.CallbackPath);
    //    var codeExchangeContext = new OAuthCodeExchangeContext(properties, code.ToString(), redirectUrl);
    //    var tokens = await ExchangeCodeAsync(codeExchangeContext);

    //    if (tokens.Error != null)
    //        return HandleRequestResult.Fail(tokens.Error);

    //    if (string.IsNullOrEmpty(tokens.AccessToken))
    //        return HandleRequestResult.Fail("Failed to retrieve access token.");

    //    var identity = new ClaimsIdentity(ClaimsIssuer);

    //    if (Options.SaveTokens)
    //    {
    //        var authTokens = new List<AuthenticationToken>
    //        {
    //            new() { Name = "access_token", Value = tokens.AccessToken }
    //        };

    //        if (!string.IsNullOrEmpty(tokens.RefreshToken))
    //            authTokens.Add(new AuthenticationToken { Name = "refresh_token", Value = tokens.RefreshToken });

    //        //微信就没有这个
    //        if (!string.IsNullOrEmpty(tokens.TokenType)) 
    //            authTokens.Add(new AuthenticationToken { Name = "token_type", Value = tokens.TokenType });

    //        if (!string.IsNullOrEmpty(tokens.ExpiresIn))
    //        {
    //            if (int.TryParse(tokens.ExpiresIn, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
    //            {
    //                // https://www.w3.org/TR/xmlschema-2/#dateTime
    //                // https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx
    //                var expiresAt = TimeProvider.GetUtcNow() + TimeSpan.FromSeconds(value);
    //                authTokens.Add(new AuthenticationToken
    //                {
    //                    Name = "expires_at",
    //                    Value = expiresAt.ToString("o", CultureInfo.InvariantCulture)
    //                });
    //            }
    //        }

    //        properties.StoreTokens(authTokens);
    //    }

    //    var ticket = await CreateTicketAsync(identity, properties, tokens);
    //    if (ticket != null)
    //    {
    //        return HandleRequestResult.Success(ticket);
    //    }
    //    else
    //    {
    //        return HandleRequestResult.Fail("Failed to retrieve user information from remote server.");
    //    }
    //}

    /// <summary>
    /// 通过 code 获取 access_token
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(OAuthCodeExchangeContext context)
    {
        /*
         *  获取第一步的 code 后，请求以下链接获取 access_token:
         *  https://api.weixin.qq.com/sns/oauth2/access_token?appid=APPID&secret=SECRET&code=CODE&grant_type=authorization_code
         *  appid:  应用唯一标识，在微信开放平台提交应用审核通过后获得
         *  secret: 应用密钥 AppSecret，在微信开放平台提交应用审核通过后获得
         *  code:   填写第一步获取的 code 参数
         *  grant_type: 填 authorization_code
         *  
         *  正确的返回：
            {
              "access_token": "ACCESS_TOKEN",
              "expires_in": 7200,
              "refresh_token": "REFRESH_TOKEN",
              "openid": "OPENID",
              "scope": "snsapi_userinfo",
              "unionid": "o6_bmasdasdsad6_2sgVt7hMZOPfL"
            }
         *
         * 错误返回样例：
         * {"errcode":40029,"errmsg":"invalid code"}
         * 
        */

        var parameters = new Dictionary<string, string?>
        {
            {  "appid", Options.ClientId },
            {  "secret", Options.ClientSecret },
            {  "code", context.Code },
            {  "grant_type", "authorization_code" }
        };
        var requestUri = QueryHelpers.AddQueryString(Options.TokenEndpoint, parameters);
        var response = await Backchannel.GetAsync(requestUri, Context.RequestAborted);
        if (response.IsSuccessStatusCode)
        {
            return OAuthTokenResponse.Success(JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted)));
        }
        else
        {
            return OAuthTokenResponse.Failed(new Exception("获取微信AccessToken出错。"));
        }
    }

    /// <summary>
    /// 创建身份票据(这是第三步) 
    /// </summary>
    /// <param name="identity"></param>
    /// <param name="properties"></param>
    /// <param name="tokens"></param>
    /// <returns></returns>
    protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
    {
        //微信获取用户信息是需要开通权限的，没有开通权限的只能用openId来标示用户
        // GET https://api.weixin.qq.com/sns/userinfo?access_token=ACCESS_TOKEN&openid=OPENID
        /*
         * 正确的 Json 返回结果
         * 
           {
              "openid": "OPENID",
              "nickname": "NICKNAME",
              "sex": 1,
              "province": "PROVINCE",
              "city": "CITY",
              "country": "COUNTRY",
              "headimgurl": "https://thirdwx.qlogo.cn/mmopen/g3MonUZtNHkdmzicIlibx6iaFqAc56vxLSUfpb6n5WKSYVY0ChQKkiaJSgQ1dZuTOgvLLrhJbERQQ4eMsv84eavHiaiceqxibJxCfHe/0",
              "privilege": ["PRIVILEGE1", "PRIVILEGE2"],
              "unionid": " o6_bmasdasdsad6_2sgVt7hMZOPfL"
            }
         * 
        */

        var openid = tokens.Response?.RootElement.GetString("openid");
        var unionid = tokens.Response?.RootElement.GetString("unionid");

        var user = JsonDocument.Parse("{}");

        if (!string.IsNullOrEmpty(unionid))
        {
            //获取用户信息
            var parameters = new Dictionary<string, string?>
            {
                {  "openid", openid },
                {  "access_token", tokens.AccessToken },
                {  "lang", "zh-CN" } //如果是多语言，这个参数该怎么获取？
            };
            var requestUri = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, parameters);
            var response = await Backchannel.GetAsync(requestUri, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"未能获取到微信用户个人信息(返回状态码:{response.StatusCode})，请检查access_token是正确。");

            user = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));
        }

        var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme, Options, Backchannel, tokens, user.RootElement);
        context.RunClaimActions();
        await Events.CreatingTicket(context);
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    /// <summary>
    /// 根据是否为微信浏览器返回不同Scope
    /// </summary>
    /// <returns></returns>
    protected override string FormatScope()
    {
        if (Options.IsWeChatBrowser(Request))
        {
            return string.Join(",", Options.Scope2);
        }
        else
        {
            return string.Join(",", Options.Scope);
        }
    }
}

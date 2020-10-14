namespace LinFx.Extensions.Identity.IdentityServer.Configuration
{
    public class ExternalProvidersConfiguration
    {
        /// <summary>
        /// USE Github Login
        /// </summary>
        public bool UseGitHubProvider { get; set; }
        public string GitHubClientId { get; set; }
        public string GitHubClientSecret { get; set; }

        /// <summary>
        /// USE MicrosoftAccount Login
        /// </summary>
        public bool UseMicrosoftAccountProvider { get; set; }

        public string MicrosoftAccountClientId { get; set; }

        public string MicrosoftAccountClientSecret { get; set; }


        /// <summary>
        /// USE QQ Login
        /// </summary>
        public bool UseQQProvider { get; set; }

        public string QQAppId { get; set; }

        public string QQAppSecret { get; set; }


        /// <summary>
        /// USE WeChat Login
        /// </summary>

        public bool UseWeChatProvider { get; set; }

        public string WeChatAppId { get; set; }
        public string WeChatSecret { get; set; }

        /// <summary>
        /// USE Weibo Login
        /// </summary>
        public bool UseWeiboProvider { get; set; }
        public string WeiboAppId { get; set; }
        public string WeiboSecret { get; set; }

    }
}

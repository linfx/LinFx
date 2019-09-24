namespace LinFx.Extensions.Email
{
    public class SmtpEmailOptions
    {
        /// <summary>
        /// 邮箱服务器地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; } = 25;

        /// <summary>
        /// 是否启用SSL
        /// </summary>
        public bool UseSSL { get; set; } = true;
    }
}

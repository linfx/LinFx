namespace LinFx.Extensions.Account
{
    /// <summary>
    /// 账户服务
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ValueTask<Result> LoginAsync(LoginInput input);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ValueTask<Result> RegisterAsync(RegisterInput input);
    }
}

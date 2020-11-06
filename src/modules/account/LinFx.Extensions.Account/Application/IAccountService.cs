using System.Threading.Tasks;

namespace LinFx.Extensions.Account
{
    /// <summary>
    /// 账户服务
    /// </summary>
    public interface IAccountService
    {
        Task<Result> LoginAsync(LoginInput input);

        Task<Result> RegisterAsync(RegisterInput input);
    }
}

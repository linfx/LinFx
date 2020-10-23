using LinFx.Extensions.Account.Application.Models;
using System.Threading.Tasks;

namespace LinFx.Extensions.Account.Application
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

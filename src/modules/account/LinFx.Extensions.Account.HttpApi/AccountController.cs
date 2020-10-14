using LinFx.Extensions.Account.Application;
using LinFx.Extensions.Account.Application.Models;
using LinFx.Security.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1;
using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Account.HttpApi
{
    /// <summary>
    /// 账户 Api 接口
    /// </summary>
    [Authorize]
    [ApiController]
    [Area("account")]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        protected ICurrentUser CurrentUser { get; }

        protected IAccountService AccountService { get; }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual Task<Result> Get()
        {
            //if (CurrentUser.IsAuthenticated)
            //    return Result.Failed("Error");

            throw new NotImplementedException();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public virtual Task<Result> RegisterAsync(RegisterInput input)
        {
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public virtual void Login()
        {
        }
    }
}

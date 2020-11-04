using LinFx.Extensions.Account.Application;
using LinFx.Extensions.Account.Application.Models;
using LinFx.Security.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Account.HttpApi
{
    /// <summary>
    /// 账户 Api 接口
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        protected readonly ICurrentUser _currentUser;
        protected readonly IAccountService _accountService;

        public AccountController(
            ICurrentUser currentUser, 
            IAccountService accountService)
        {
            _currentUser = currentUser;
            _accountService = accountService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input">登录表单 </param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public virtual Task<Result> LoginAsync(LoginInput input)
        {
            return _accountService.LoginAsync(input);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input">注册表单</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public virtual Task<Result> RegisterAsync(RegisterInput input)
        {
            return _accountService.RegisterAsync(input);
        }

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
    }
}
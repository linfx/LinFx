using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LinFx.Extensions.Identity.HttpApi
{
    /// <summary>
    /// 用户Api
    /// </summary>
    [ApiController]
    [Route("api/identity/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        ///// <summary>
        ///// 列表
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public virtual Task<PagedResult<IdentityUserResult>> GetListAsync(IdentityUserInput input)
        //{
        //    return _userService.GetListAsync(input);
        //}

        ///// <summary>
        ///// 获取
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet("{id}")]
        //public virtual Task<IdentityUserResult> GetAsync(string id)
        //{
        //    return _userService.GetAsync(id);
        //}

        ///// <summary>
        ///// 创建
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public virtual Task<IdentityUserResult> CreateAsync(IdentityUserInput input)
        //{
        //    return _userService.CreateAsync(input);
        //}

        ///// <summary>
        ///// 更新
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPut("{id}")]
        //public virtual Task<IdentityUserResult> UpdateAsync(string id, IdentityUserUpdateInput input)
        //{
        //    return _userService.UpdateAsync(id, input);
        //}

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual Task DeleteAsync(string id)
        {
            return _userService.DeleteAsync(id);
        }
    }
}

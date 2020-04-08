using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.Identity.Models
{
    public class UserRole : IdentityUserRole<string>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [StringLength(36)]
        public override string UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [StringLength(36)]
        public override string RoleId { get; set; }
    }
}

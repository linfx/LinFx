using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.Identity.Models
{
    public class UserRole : IdentityUserRole<string>
    {
        [StringLength(36)]
        public override string UserId { get; set; }

        [StringLength(36)]
        public override string RoleId { get; set; }
    }
}

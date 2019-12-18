using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.Identity.Models
{
    public class RoleClaim : IdentityRoleClaim<string>
    {
        [StringLength(36)]
        public override string RoleId { get; set; }
    }
}

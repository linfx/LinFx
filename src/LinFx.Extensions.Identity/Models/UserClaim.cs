using LinFx.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.Identity.Models
{
    public class UserClaim : IdentityUserClaim<string>, IEntity
    {
        [StringLength(36)]
        public override string UserId { get; set; }
    }
}

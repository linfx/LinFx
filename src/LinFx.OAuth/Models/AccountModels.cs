using System.ComponentModel.DataAnnotations;

namespace LinFx.SaaS.OAuth.Models
{
    public class AccountModels
    {
    }

    public class AuthenticateModel
    {
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}

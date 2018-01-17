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
        public string Client_Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Client_Secret { get; set; }

        [MaxLength(50)]
        public string UserName { get; set; }

        [MaxLength(50)]
        public string Password { get; set; }
    }
}

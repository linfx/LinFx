using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.Account.Application.Models
{
    public class LoginInput
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

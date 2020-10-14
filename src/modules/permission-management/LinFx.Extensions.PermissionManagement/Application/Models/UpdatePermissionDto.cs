using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.PermissionManagement.Application.Models
{
    public class UpdatePermissionDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsGranted { get; set; }
    }
}

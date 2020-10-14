using System.Collections.Generic;

namespace LinFx.Extensions.PermissionManagement.Application.Models
{
    public class PermissionGrantInfoDto
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string ParentName { get; set; }

        public bool IsGranted { get; set; }

        public List<string> AllowedProviders { get; set; }

        public List<ProviderInfoDto> GrantedProviders { get; set; }
    }
}

using LinFx.Extensions.Identity.Models;
using LinFx.Security.Authorization.Permissions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LinFx.Extensions.Identity.UI.Pages.Identity.Roles
{
    public class PermissionsModel : PageModel
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<PermissionsModel> _logger;
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;

        public PermissionsModel(
            RoleManager<Role> roleManager,
            ILogger<PermissionsModel> logger,
            IPermissionDefinitionManager permissionDefinitionManager)
        {
            _roleManager = roleManager;
            _logger = logger;
            _permissionDefinitionManager = permissionDefinitionManager;
        }

        [BindProperty]
        public List<PermissionGroupModel> Groups { get; set; }

        public async Task OnGetAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var claims = await _roleManager.GetClaimsAsync(role);

            var groups = _permissionDefinitionManager.GetGroups();




            Groups = new List<PermissionGroupModel>();

            foreach (var sys in groups)
            {
                foreach (var mod in sys.Permissions)
                {
                    var group = new PermissionGroupModel
                    {
                        Name = mod.Name,
                        DisplayName = mod.DisplayName
                    };

                    group.Permissions = mod.Children.Select(p => new PermissionGrantModel
                    {
                        Name = p.Name,
                        DisplayName = p.DisplayName,
                    }).ToList();

                    Groups.Add(group);
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var updatePermissions = Groups
                    .SelectMany(g => g.Permissions)
                    //.Select(p => new UpdatePermission
                    //{
                    //    Name = p.Name,
                    //    IsGranted = p.IsGranted
                    //})
                    .ToArray();

                return RedirectToPage("./Index");
            }
            return Page();
        }

        public class PermissionGroupModel
        {
            public string Name { get; set; }

            public string DisplayName { get; set; }

            public List<PermissionGrantModel> Permissions { get; set; }
        }

        public class PermissionGrantModel
        {
            [Required]
            [HiddenInput]
            public string Name { get; set; }

            public string DisplayName { get; set; }

            public bool IsGranted { get; set; }
        }
    }
}

using LinFx.Extensions.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace LinFx.Extensions.Identity.UI.Pages.Identity.Roles
{
    public class EditModel : PageModel
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<EditModel> _logger;

        public EditModel(
            RoleManager<Role> roleManager,
            ILogger<EditModel> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
        }

        public string ReturnUrl { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [HiddenInput]
            public string Id { get; set; }

            [HiddenInput]
            public string ConcurrencyStamp { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Ãû³Æ")]
            public string Name { get; set; }
        }

        public async Task OnGetAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            Input = new InputModel
            {
                Id = role.Id,
                Name = role.Name,
                ConcurrencyStamp = role.ConcurrencyStamp,
            };
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var roleToUpdate = await _roleManager.FindByIdAsync(Input.Id);
                if (await TryUpdateModelAsync(roleToUpdate))
                {
                    await _roleManager.SetRoleNameAsync(roleToUpdate, Input.Name);
                    var result = await _roleManager.UpdateAsync(roleToUpdate);
                    if (result.Succeeded)
                    {
                        //return LocalRedirect(returnUrl);
                        return RedirectToPage("./Index");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return Page();
        }
    }
}
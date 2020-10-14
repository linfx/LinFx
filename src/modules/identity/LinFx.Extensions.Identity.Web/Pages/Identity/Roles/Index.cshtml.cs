using LinFx.Extensions.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinFx.Extensions.Identity.UI.Pages.Identity.Roles
{
    public class IndexModel : PageModel
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            RoleManager<Role> roleManager,
            ILogger<IndexModel> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
        }

        public string ReturnUrl { get; set; }

        [BindProperty]
        public ICollection<Role> Items { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Items = await _roleManager.Roles.ToListAsync();
        }
    }
}

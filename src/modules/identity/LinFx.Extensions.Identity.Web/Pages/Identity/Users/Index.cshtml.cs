using LinFx.Extensions.Identity.Models;
using LinFx.Extensions.Identity.Permissions;
using LinFx.Extensions.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinFx.Extensions.Identity.UI.Pages.Identity.Users
{
    [Authorize(IdentityPermissions.Users.Default)]
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly UserService _userService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            UserManager<User> userManager,
            UserService userService,
            ILogger<IndexModel> logger)
        {
            _userManager = userManager;
            _userService = userService;
            _logger = logger;
        }

        public string ReturnUrl { get; set; }

        [BindProperty]
        public ICollection<User> Items { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            //Items = await _userService.GetListAsync();
        }
    }
}

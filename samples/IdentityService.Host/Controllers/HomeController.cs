using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Host.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}

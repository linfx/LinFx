using Microsoft.AspNetCore.Mvc;

namespace TenantManagementService.Host.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}

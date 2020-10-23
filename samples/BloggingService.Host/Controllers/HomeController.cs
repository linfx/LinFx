using Microsoft.AspNetCore.Mvc;

namespace BloggingService.Host.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}

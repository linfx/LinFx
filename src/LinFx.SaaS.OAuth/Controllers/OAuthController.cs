using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LinFx.SaaS.OAuth.Controllers
{
    [Route("api/[controller]/[action]")]
    public class OAuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
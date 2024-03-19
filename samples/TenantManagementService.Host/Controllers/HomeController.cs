﻿using Microsoft.AspNetCore.Mvc;

namespace TenantManagementService.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }

        [HttpGet]
        public IActionResult Error()
        {
            throw new NotImplementedException();
        }
    }
}

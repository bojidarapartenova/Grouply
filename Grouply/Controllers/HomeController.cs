using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Grouply.Models;
using Grouply.Data;
using Microsoft.AspNetCore.Authorization;

namespace Grouply.Controllers
{
    public class HomeController : BaseController
    {
        private readonly GrouplyDbContext context;
        public HomeController(GrouplyDbContext context)
        {
            this.context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}

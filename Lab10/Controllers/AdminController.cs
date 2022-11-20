using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab10.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            ViewData["Info"] = "Only for administrator";
            return View("Info");
        }

        public IActionResult ForAdmin()
        {
            ViewData["Info"] = "Administration page";
            return View("Info");
        }
    }
}

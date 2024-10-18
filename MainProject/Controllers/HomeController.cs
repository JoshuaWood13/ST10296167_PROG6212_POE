// Name: Joshua Wood
// Student number: ST10296167
// Group: 2

using Microsoft.AspNetCore.Mvc;
using ST10296167_PROG6212_POE.Models;
using System.Diagnostics;

namespace ST10296167_PROG6212_POE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Controller
        //------------------------------------------------------------------------------------------------------------------------------------------//
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//

        //Views
        //------------------------------------------------------------------------------------------------------------------------------------------//
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("IsLoggedIn") != 1)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//
    }
}
//--------------------------------------------------------X END OF FILE X-------------------------------------------------------------------//
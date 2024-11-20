// Name: Joshua Wood
// Student number: ST10296167
// Group: 2

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ST10296167_PROG6212_POE.Models;
using System.Diagnostics;

namespace ST10296167_PROG6212_POE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;

        // Controller
        //------------------------------------------------------------------------------------------------------------------------------------------//
        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//

        //Views
        //------------------------------------------------------------------------------------------------------------------------------------------//
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Login");
            }

            if (User.IsInRole("Human Resources"))
            {
                return RedirectToAction("Dashboard", "HR");
                //return RedirectToPage("/IndexHR");
            }
            var user = await _userManager.GetUserAsync(User);
            var userID = user.Id;

            var accountType = new[] { "Lecturer", "Academic Manager", "Programme Coordinator", "Human Resources" }
            .FirstOrDefault(role => User.IsInRole(role)) ?? "Unknown";

            ViewData["UserId"] = userID;
            ViewData["AccountType"] = accountType;
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
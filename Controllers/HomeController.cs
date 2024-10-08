using Microsoft.AspNetCore.Mvc;
using ST10296167_PROG6212_POE.Models;
using System.Diagnostics;

namespace ST10296167_PROG6212_POE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Check if the user is logged in
            if (HttpContext.Session.GetInt32("IsLoggedIn") != 1)
            {
                // If not logged in, redirect to Login
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        //public IActionResult SubmitClaim()
        //{
        //    return View();
        //}

        //public IActionResult ViewClaims()
        //{
        //    return View();
        //}
        public IActionResult UploadDocuments()
        {
            return View();
        }
        public IActionResult VerifyClaims()
        {
            return View();
        }
        public IActionResult FullClaimView()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

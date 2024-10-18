// Name: Joshua Wood
// Student number: ST10296167
// Group: 2

using Microsoft.AspNetCore.Mvc;
using ST10296167_PROG6212_POE.Models;
using ST10296167_PROG6212_POE.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ST10296167_PROG6212_POE.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        // Controller
        //------------------------------------------------------------------------------------------------------------------------------------------//
        public LoginController(AppDbContext context)
        {
            _context = context;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//

        //Views
        //------------------------------------------------------------------------------------------------------------------------------------------//
        public IActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//

        // Methods
        //------------------------------------------------------------------------------------------------------------------------------------------//
        // This method handles logging in a valid user and setting the correct session values
        [HttpPost]
        public IActionResult LoginUser(string Account, Login login)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", login); 
            }

            bool loginSuccessful = ValidateUser(Account, login.AccountID, login.Password);

            if (loginSuccessful)
            {
                // Set session variables
                HttpContext.Session.SetString("AccountType", Account);
                HttpContext.Session.SetInt32("IsLoggedIn", 1);
                HttpContext.Session.SetInt32("AccountID", login.AccountID);

                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Incorrect Account ID or password";
            ModelState.Clear();
            return View("Login");
        }

        //------------------------------------------------------------------------------------------------------------------------------------------//
        // This method validates that the account ID and password input, matches a created account
        private bool ValidateUser(string accountType, int accountID, string password)
        {

            if (accountType == "Lecturer")
            {
                var lecturer = _context.Lecturers
                    .FirstOrDefault(l => l.LecturerID == accountID && l.Password == password);
                return lecturer != null;
            }
            else if (accountType == "Academic Manager")
            {
                var academicManager = _context.AcademicManagers
                    .FirstOrDefault(am => am.AM_ID == accountID && am.Password == password);
                return academicManager != null;
            }
            else if (accountType == "Programme Coordinator")
            {
                var programmeCoordinator = _context.ProgrammeCoordinators
                    .FirstOrDefault(pm => pm.PM_ID == accountID && pm.Password == password);
                return programmeCoordinator != null;
            }

            return false; 

        }
        //------------------------------------------------------------------------------------------------------------------------------------------//
    }
}
//--------------------------------------------------------X END OF FILE X-------------------------------------------------------------------//
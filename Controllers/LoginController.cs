using Microsoft.AspNetCore.Mvc;
using ST10296167_PROG6212_POE.Models;
using ST10296167_PROG6212_POE.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace ST10296167_PROG6212_POE.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            //HttpContext.Session.SetInt32("IsLoggedIn", 0);
            //HttpContext.Session.Remove("AccountType");
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public IActionResult LoginUser(string Account, int AccountID, string Password)
        {
            // Validate user credentials
            bool loginSuccessful = ValidateUser(Account, AccountID, Password);

            if (loginSuccessful)
            {
                // Set session variable
                HttpContext.Session.SetString("AccountType", Account);
                HttpContext.Session.SetInt32("IsLoggedIn", 1);
                HttpContext.Session.SetInt32("AccountID", AccountID);

                // Redirect to a dashboard or home page
                return RedirectToAction("Index", "Home");
            }

            // If login fails, return to login view with error message
            ViewBag.ErrorMessage = "Invalid credentials. Please try again.";
            return View("Login");
        }

        private bool ValidateUser(string accountType, int accountID, string password)
        {

            if (accountType == "Lecturer")
            {
                var lecturer = _context.Lecturers
                    .FirstOrDefault(l => l.LecturerID == accountID && l.Password == password);
                return lecturer != null;
                //var sqlQuery = $"SELECT * FROM Lecturers WHERE LecturerID = {accountID} AND Password = '{password}'";
                //// Execute the SQL query
                //var user = _context.Lecturers.FromSqlRaw(sqlQuery).FirstOrDefault(); // This will fetch the user or null if not found
                //return user != null;
            }
            else if (accountType == "Academic Manager")
            {
                var academicManager = _context.AcademicManagers
                    .FirstOrDefault(am => am.AM_ID == accountID && am.Password == password);
                return academicManager != null;
                //var sqlQuery = $"SELECT * FROM AcademicManagers WHERE AM_ID = {accountID} AND Password = '{password}'";
                //// Execute the SQL query
                //var user = _context.AcademicManagers.FromSqlRaw(sqlQuery).FirstOrDefault(); // This will fetch the user or null if not found
                //return user != null;
            }
            else if (accountType == "Programme Coordinator")
            {
                var programmeCoordinator = _context.ProgrammeCoordinators
                    .FirstOrDefault(pm => pm.PM_ID == accountID && pm.Password == password);
                return programmeCoordinator != null;
                //var sqlQuery = $"SELECT * FROM ProgrammeCoordinators WHERE PD_ID = {accountID} AND Password = '{password}'";
                //// Execute the SQL query
                //var user = _context.ProgrammeCoordinators.FromSqlRaw(sqlQuery).FirstOrDefault(); // This will fetch the user or null if not found
                //return user != null;
            }

            return false; // Return true if user exists

        }
    }
}

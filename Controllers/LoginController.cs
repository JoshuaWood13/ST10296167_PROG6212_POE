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
        public IActionResult LoginUser(string Account, string AccountID, string Password)
        {
            // Validate user credentials
            bool loginSuccessful = ValidateUser(Account, AccountID, Password);

            if (loginSuccessful)
            {
                // Set session variable
                HttpContext.Session.SetString("AccountType", Account);
                HttpContext.Session.SetInt32("IsLoggedIn", 1);

                // Redirect to a dashboard or home page
                return RedirectToAction("Index", "Home");
            }

            // If login fails, return to login view with error message
            ViewBag.ErrorMessage = "Invalid credentials. Please try again.";
            return View("Login");
        }

        private bool ValidateUser(string accountType, string accountNumber, string password)
        {

            if (accountType == "Lecturer")
            {
                var sqlQuery = $"SELECT * FROM Lecturers WHERE LecturerID = {accountNumber} AND Password = '{password}'";
                // Execute the SQL query
                var user = _context.Lecturers.FromSqlRaw(sqlQuery).FirstOrDefault(); // This will fetch the user or null if not found
                return user != null;
            }
            else if (accountType == "Academic Manager")
            {
                var sqlQuery = $"SELECT * FROM AcademicManagers WHERE AM_ID = {accountNumber} AND Password = '{password}'";
                // Execute the SQL query
                var user = _context.AcademicManagers.FromSqlRaw(sqlQuery).FirstOrDefault(); // This will fetch the user or null if not found
                return user != null;
            }
            else if (accountType == "Programme Coordinator")
            {
                var sqlQuery = $"SELECT * FROM ProgrammeCoordinators WHERE PD_ID = {accountNumber} AND Password = '{password}'";
                // Execute the SQL query
                var user = _context.ProgrammeCoordinators.FromSqlRaw(sqlQuery).FirstOrDefault(); // This will fetch the user or null if not found
                return user != null;
            }

            return false; // Return true if user exists

        }
    }
}

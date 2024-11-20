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
using Microsoft.AspNetCore.Identity;

namespace ST10296167_PROG6212_POE.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        // Controller
        //------------------------------------------------------------------------------------------------------------------------------------------//
        public LoginController(AppDbContext context, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
            //HttpContext.Session.Clear();
            _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Login");
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//

        // Methods
        //------------------------------------------------------------------------------------------------------------------------------------------//
        // This method handles logging in a valid user and setting the correct session values
        //[HttpPost]
        //public IActionResult LoginUser(string Account, Login login)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View("Login", login); 
        //    }

        //    bool loginSuccessful = ValidateUser(Account, login.AccountID, login.Password);

        //    if (loginSuccessful)
        //    {
        //        // Set session variables
        //        HttpContext.Session.SetString("AccountType", Account);
        //        HttpContext.Session.SetInt32("IsLoggedIn", 1);
        //        HttpContext.Session.SetInt32("AccountID", login.AccountID);

        //        return RedirectToAction("Index", "Home");
        //    }

        //    TempData["Error"] = "Incorrect Account ID or password";
        //    ModelState.Clear();
        //    return View("Login");
        //}

        [HttpPost]
        public async Task<IActionResult> LoginUser(Login login)
        {
            //if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password))
            //{
            //    TempData["Error"] = "Please provide both User ID and password.";
            //    return View("Login");
            //}

            // Find user by UserId
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == login.AccountID.ToString());

            if (user != null)
            {
                // Check if the password is correct using UserManager
                var isPasswordValid = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

                if (isPasswordValid.Succeeded)
                {
                    // Sign in the user
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    //HttpContext.Session.SetString("AccountType", "Lecturer");
                    //HttpContext.Session.SetInt32("IsLoggedIn", 1);

                    // Redirect to the home page after successful login
                    if (await _userManager.IsInRoleAsync(user, "Human Resources"))
                    {
                        // Redirect to the HR dashboard page if the user is in the "HR" role
                        return RedirectToAction("Dashboard", "HR");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    //return RedirectToAction("Index", "Home");
                }
            }

            TempData["Error"] = "Incorrect Account ID or password";
            ModelState.Clear();
            return View("Login");

            //if (user == null)
            //{
            //    TempData["Error"] = "Account ID not found.";
            //    return View("Login");
            //}

            //// Use the found user's UserName for login
            //var result = await _signInManager.PasswordSignInAsync(user.UserName, password, isPersistent: false, lockoutOnFailure: false);

            //if (result.Succeeded)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            //TempData["Error"] = "Incorrect Account ID or password";
            //return View("Login");
        }

        //------------------------------------------------------------------------------------------------------------------------------------------//
        // This method validates that the account ID and password input, matches a created account
        //private bool ValidateUser(string accountType, int accountID, string password)
        //{

        //    if (accountType == "Lecturer")
        //    {
        //        var lecturer = _context.Lecturers
        //            .FirstOrDefault(l => l.LecturerID == accountID && l.Password == password);
        //        return lecturer != null;
        //    }
        //    else if (accountType == "Academic Manager")
        //    {
        //        var academicManager = _context.AcademicManagers
        //            .FirstOrDefault(am => am.AM_ID == accountID && am.Password == password);
        //        return academicManager != null;
        //    }
        //    else if (accountType == "Programme Coordinator")
        //    {
        //        var programmeCoordinator = _context.ProgrammeCoordinators
        //            .FirstOrDefault(pm => pm.PM_ID == accountID && pm.Password == password);
        //        return programmeCoordinator != null;
        //    }

        //    return false; 

        //}
        //------------------------------------------------------------------------------------------------------------------------------------------//
    }
}
//--------------------------------------------------------X END OF FILE X-------------------------------------------------------------------//
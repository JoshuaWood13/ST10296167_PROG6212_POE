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
            _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Login");
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//

        // This method handles logging in a users, ensuring they enter a valid ID and password combination
        [HttpPost]
        public async Task<IActionResult> LoginUser(Login login)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == login.AccountID.ToString());

            if (user != null)
            {
                var isPasswordValid = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

                if (isPasswordValid.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    if (await _userManager.IsInRoleAsync(user, "Human Resources"))
                    {
                        return RedirectToAction("Dashboard", "HR");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            TempData["Error"] = "Incorrect Account ID or password";
            ModelState.Clear();
            return View("Login");
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//
    }
}
//--------------------------------------------------------X END OF FILE X-------------------------------------------------------------------//
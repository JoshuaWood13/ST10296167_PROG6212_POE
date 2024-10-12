﻿using Microsoft.AspNetCore.Mvc;
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
            return RedirectToAction("Login", "Login");
        }

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

            // If login fails, return to login view with error message
            TempData["Error"] = "Incorrect Account ID or password";
            ModelState.Clear();
            return View("Login");
        }

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
    }
}
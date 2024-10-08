using Microsoft.AspNetCore.Mvc;
using ST10296167_PROG6212_POE.Data;
using ST10296167_PROG6212_POE.Models;
using Microsoft.EntityFrameworkCore;

namespace ST10296167_PROG6212_POE.Controllers
{
    public class ClaimController : Controller
    {
        private readonly AppDbContext _context;

        public ClaimController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult SubmitClaim()
        {
            return View();
        }
        public async Task<IActionResult> ViewClaims()
        {
            var claims = await _context.Claims.ToListAsync();
            return View(claims);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitClaim(string Month, double HourlyRate, double HoursWorked, string Notes)
        {
            double amount = HourlyRate * HoursWorked;
            var lecturerID = HttpContext.Session.GetInt32("AccountID");

            var claim = new Claims
            {
                LecturerID = (int)lecturerID,
                HourlyRate = HourlyRate,
                HoursWorked = HoursWorked,
                ClaimAmount = amount,
                ClaimMonth = Month,
                Description = Notes
            };

            // Add the claim to the context and save changes
            await _context.Claims.AddAsync(claim);
            await _context.SaveChangesAsync();

            return View("Index", "Home");
        }
    }
}

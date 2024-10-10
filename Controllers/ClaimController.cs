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
            var lecturerID = HttpContext.Session.GetInt32("AccountID");

            var claims = await _context.Claims.Where(c => c.LecturerID == lecturerID).ToListAsync();
            return View(claims);
        }

        public async Task<IActionResult> VerifyClaims()
        {
            var accountType = HttpContext.Session.GetString("AccountType");
            IEnumerable<Claims> claims;

            if (accountType == "Programme Coordinator")
            {
                claims = await _context.Claims.Where(c => c.ApprovalPC == 0).ToListAsync();
            }
            else if(accountType == "Academic Manager")
            {
                claims = await _context.Claims.Where(c => c.ApprovalPC == 1 && c.ApprovalAM == 0).ToListAsync();
            }
            else
            {
                claims = null; // Default case if needed
            }
            //var claims = await _context.Claims.ToListAsync();
            return View(claims);
        }

        public async Task<IActionResult> FullClaimView(int id)
        {
            var claim = await _context.Claims.Include(c => c.Documents).FirstOrDefaultAsync(c => c.ClaimID == id);

            return View(claim);
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

            await _context.Claims.AddAsync(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ReturnToVerifyClaims()
        {
            return RedirectToAction("VerifyClaims");
        }

        [HttpPost]
        public async Task<IActionResult> ProcessClaim(int claimID, string action)
        {
            var account = HttpContext.Session.GetString("AccountType");
            var claim = await _context.Claims.FindAsync(claimID);

            if (account == "Programme Coordinator")
            {
                if (action == "approve")
                {
                    claim.ApprovalPC = 1; 
                }
                else if (action == "deny")
                {
                    claim.ApprovalPC = 2; 
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else if (account == "Academic Manager")
            {
                if (action == "approve")
                {
                    claim.ApprovalAM = 1; 
                }
                else if (action == "deny")
                {
                    claim.ApprovalAM = 2; 
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("VerifyClaims"); 
        }
    }
}

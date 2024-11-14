// Name: Joshua Wood
// Student number: ST10296167
// Group: 2

using Microsoft.AspNetCore.Mvc;
using ST10296167_PROG6212_POE.Data;
using ST10296167_PROG6212_POE.Models;
using ST10296167_PROG6212_POE.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ST10296167_PROG6212_POE.Controllers
{
    public class ClaimController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ClaimApiService _claimApiService;

        // Controller
        //------------------------------------------------------------------------------------------------------------------------------------------//
        public ClaimController(AppDbContext context, ClaimApiService claimApiService)
        {
            _context = context;
            _claimApiService = claimApiService;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//

        // Views
        //------------------------------------------------------------------------------------------------------------------------------------------//
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

        //public async Task<IActionResult> VerifyClaims()
        //{
        //    var accountType = HttpContext.Session.GetString("AccountType");
        //    IEnumerable<Claims> claims;

        //    if (accountType == "Programme Coordinator")
        //    {
        //        claims = await _context.Claims.Where(c => c.ApprovalPC == 0).ToListAsync();
        //    }
        //    else if (accountType == "Academic Manager")
        //    {
        //        claims = await _context.Claims.Where(c => c.ApprovalPC == 1 && c.ApprovalAM == 0).ToListAsync();
        //    }
        //    else
        //    {
        //        claims = null;
        //    }

        //    return View(claims);
        //}

        public async Task<IActionResult> VerifyClaims()
        {
            var accountType = HttpContext.Session.GetString("AccountType");

            //var verifiedClaims = await _claimApiService.GetVerifiedClaimsAsync();

            SortedClaims claims;

            if (accountType == "Programme Coordinator")
            {
                claims = await _claimApiService.GetClaimsPCAsync();

            }
            else if (accountType == "Academic Manager")
            {
            claims = await _claimApiService.GetClaimsAMAsync();
            }
            else
            {
                claims = null;
            }
            return View(claims);
        }

        public async Task<IActionResult> FullClaimView(int id)
        {
            var claim = await _context.Claims.Include(c => c.Documents).FirstOrDefaultAsync(c => c.ClaimID == id);

            return View(claim);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//

        // Methods
        //------------------------------------------------------------------------------------------------------------------------------------------//
        // This method handles submitting a valid claim to the DB
        [HttpPost]
        public async Task<IActionResult> SubmitClaim(Claims model)
        {
            double amount = model.HourlyRate * model.HoursWorked;
            var lecturerID = HttpContext.Session.GetInt32("AccountID");

            var claim = new Claims
            {
                LecturerID = (int)lecturerID,
                HourlyRate = model.HourlyRate,
                HoursWorked = model.HoursWorked,
                ClaimAmount = amount,
                ClaimMonth = model.ClaimMonth,
                Description = model.Description
            };

            await _context.Claims.AddAsync(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewClaims");
        }

        // This method handles returning a user to the correct claims view
        [HttpPost]
        public IActionResult ReturnToVerifyClaims()
        {
            if (HttpContext.Session.GetString("AccountType") == "Lecturer")
            {
                return RedirectToAction("ViewClaims");
            }
            return RedirectToAction("VerifyClaims");
        }

        // This method handles assigning PC's and AM's approval or denial
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
        //------------------------------------------------------------------------------------------------------------------------------------------//
    }
}
//--------------------------------------------------------X END OF FILE X-------------------------------------------------------------------//
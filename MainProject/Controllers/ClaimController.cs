// Name: Joshua Wood
// Student number: ST10296167
// Group: 2

using Microsoft.AspNetCore.Mvc;
using ST10296167_PROG6212_POE.Data;
using ST10296167_PROG6212_POE.Models;
using ST10296167_PROG6212_POE.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ST10296167_PROG6212_POE.Migrations;

namespace ST10296167_PROG6212_POE.Controllers
{
    public class ClaimController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ClaimApiService _claimApiService;
        private readonly UserManager<User> _userManager;

        // Controller
        //------------------------------------------------------------------------------------------------------------------------------------------//
        public ClaimController(AppDbContext context, ClaimApiService claimApiService, UserManager<User> userManager)
        {
            _context = context;
            _claimApiService = claimApiService;
            _userManager = userManager;
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
            //var lecturerID = HttpContext.Session.GetInt32("AccountID");
            var user = await _userManager.GetUserAsync(User);
            var lecturerID = user.Id;

            //var user = await _userManager.GetUserAsync(User);
            //var lecturerID = user.UserId;

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
            //var accountType = HttpContext.Session.GetString("AccountType");

            var accountType = new[] { "Academic Manager", "Programme Coordinator" }
            .FirstOrDefault(role => User.IsInRole(role));

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
            var user = await _userManager.GetUserAsync(User);
            var lecturerID = user.Id;
            //var lecturerID = HttpContext.Session.GetInt32("AccountID");

            var claim = new Claims
            {
                LecturerID = lecturerID,
                HourlyRate = model.HourlyRate,
                HoursWorked = model.HoursWorked,
                ClaimAmount = amount,
                ClaimMonth = model.ClaimMonth,
                Description = model.Description
            };

            // AUTO VAL HERE
            var verifiedClaim = AutoVerify(claim);

            await _context.Claims.AddAsync(verifiedClaim);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewClaims");
        }

        // This method handles returning a user to the correct claims view
        [HttpPost]
        public IActionResult ReturnToVerifyClaims()
        {
            var accountType = new[] { "Lecturer", "Academic Manager", "Programme Coordinator", "Human Resources" }
            .FirstOrDefault(role => User.IsInRole(role)) ?? "Unknown";

            if (accountType == "Lecturer")
            {
                return RedirectToAction("ViewClaims");
            }
            return RedirectToAction("VerifyClaims");
        }

        // This method handles assigning PC's and AM's approval or denial
        [HttpPost]
        public async Task<IActionResult> ProcessClaim(int claimID, string action)
        {
            //var account = HttpContext.Session.GetString("AccountType");
            var account = new[] { "Lecturer", "Academic Manager", "Programme Coordinator", "Human Resources" }
            .FirstOrDefault(role => User.IsInRole(role)) ?? "Unknown";
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

        // This method auto verifies and approves a claim if it meets the predefined criteria
        public Claims AutoVerify(Claims claim)
        {
            if(claim.HoursWorked > 50)
            {
                return claim;
            }
            else if (claim.HourlyRate < 150)
            {
                claim.ApprovalPC = 3;
            }
            return claim;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//
    }
}
//--------------------------------------------------------X END OF FILE X-------------------------------------------------------------------//
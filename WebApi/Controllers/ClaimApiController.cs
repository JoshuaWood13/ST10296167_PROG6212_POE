using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10296167_PROG6212_POE.Data;
using ST10296167_PROG6212_POE.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ClaimApiController(AppDbContext context)
        {
            _context = context;
        }

        //[HttpGet("Verify")]
        //public IActionResult Verify()
        //{
        //    var verifiedClaims = new List<Claims>();
        //    var invalidClaims = new List<Claims>();
        //    var claims = _context.Claims.ToList();
        //    Console.WriteLine($"Number of claims retrieved: {claims.Count}"); // Debug output
                
        //    foreach (var claim in claims)
        //    {
        //        if (claim.HoursWorked <= 0 || claim.HourlyRate <= 0 || claim.ClaimAmount <= 0)
        //        {
        //            claim.ApprovalPC = 3;
        //            claim.ApprovalAM = 3;
        //            invalidClaims.Add(claim);
        //        }
        //        // I decided to include 2 policies:
        //        // 1) Lecturers must have worked 10 or more hours to submit a claim
        //        // 2) Lecturers cannot be payed more than R500 an hour
        //        else if (claim.HoursWorked < 10 || claim.HourlyRate > 500)
        //        {
        //            claim.ApprovalPC = 3;
        //            claim.ApprovalAM = 3;
        //            invalidClaims.Add(claim);
        //        }
        //        else
        //        {
        //            verifiedClaims.Add(claim);
        //        }
        //    }

        //    if (invalidClaims.Any())
        //    {
        //        _context.SaveChanges(); // Save changes for denied claims only
        //    }

        //    Console.WriteLine($"Number of verified claims: {verifiedClaims.Count}"); // Debug output
        //    return Ok(verifiedClaims);
        //}

        [HttpGet("GetClaimsPC")]
        public async Task<ActionResult<SortedClaims>> GetClaimsPC()
        {
            var claimsPC = await _context.Claims.Where(c => c.ApprovalPC == 0).ToListAsync();
            if (!claimsPC.Any())
            {
                return Ok(new SortedClaims());
            }
            else
            {
                var sortedClaims = AutoCheckClaims(claimsPC);
                return Ok(sortedClaims);
            }
        }

        [HttpGet("GetClaimsAM")]
        public async Task<ActionResult<SortedClaims>> GetClaimsAM()
        {
            var claimsAM = await _context.Claims.Where(c => c.ApprovalPC == 1 && c.ApprovalAM == 0).ToListAsync();
            if (!claimsAM.Any())
            {
                return Ok(new SortedClaims());
            }
            else
            {
                var sortedClaims = AutoCheckClaims(claimsAM);
                return Ok(sortedClaims);
            }
        }

        private SortedClaims AutoCheckClaims(List<Claims> claims)
        {
            var sortedClaims = new SortedClaims();

            foreach(var claim in claims)
            {
                if(claim.HoursWorked > 50)
                {
                    if(claim.HoursWorked > 200 || claim.HourlyRate > 500)
                    {
                        sortedClaims.FlaggedFT.Add(claim);
                    }
                    else
                    {
                        sortedClaims.FullTime.Add(claim);
                    }
                }
                else if (claim.HourlyRate > 350)
                {
                    sortedClaims.FlaggedPT.Add(claim);
                }
                else
                {
                    sortedClaims.PartTime.Add(claim);
                }
            }
            return sortedClaims;
        }

    }
}

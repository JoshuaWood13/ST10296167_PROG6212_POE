using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("Verify")]
        public IActionResult Verify()
        {
            var verifiedClaims = new List<Claims>();
            var invalidClaims = new List<Claims>();
            var claims = _context.Claims.ToList();
            Console.WriteLine($"Number of claims retrieved: {claims.Count}"); // Debug output

            foreach (var claim in claims)
            {
                if (claim.HoursWorked <= 0 || claim.HourlyRate <= 0 || claim.ClaimAmount <= 0)
                {
                    claim.ApprovalPC = 2;
                    claim.ApprovalAM = 2;
                    invalidClaims.Add(claim);
                }
                // I decided to include 2 policies:
                // 1) Lecturers must have worked 10 or more hours to submit a claim
                // 2) Lecturers cannot be payed more than R500 an hour
                else if (claim.HoursWorked < 10 || claim.HourlyRate > 500)
                {
                    claim.ApprovalPC = 2;
                    claim.ApprovalAM = 2;
                    invalidClaims.Add(claim);
                }
                else
                {
                    verifiedClaims.Add(claim);
                }
            }

            if (invalidClaims.Any())
            {
                _context.SaveChanges(); // Save changes for denied claims only
            }

            Console.WriteLine($"Number of verified claims: {verifiedClaims.Count}"); // Debug output
            return Ok(verifiedClaims);
        }
    }
}

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

        // Controller
        //------------------------------------------------------------------------------------------------------------------------------------------//
        public ClaimApiController(AppDbContext context)
        {
            _context = context;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//

        // Methods
        //------------------------------------------------------------------------------------------------------------------------------------------//
        // This method gets all claims needing PC approval, sort them, and then return them
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

        // This method gets all claims needing AM approval, sort them, and then return them
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

        // This method checks submitted claims against pre-defined criteria in order to sort them and flag claims outside the norm for closer inspection
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
        //------------------------------------------------------------------------------------------------------------------------------------------//
    }
}
//--------------------------------------------------------X END OF FILE X-------------------------------------------------------------------//
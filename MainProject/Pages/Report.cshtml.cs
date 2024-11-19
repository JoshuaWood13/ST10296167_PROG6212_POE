using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ST10296167_PROG6212_POE.Data;
using ST10296167_PROG6212_POE.Models;

namespace ST10296167_PROG6212_POE.Pages
{
    public class ReportModel : PageModel
    {
        private readonly AppDbContext _context;

        public ReportModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string LecturerID { get; set; } 

        [BindProperty(SupportsGet = true)]
        public string Month { get; set; } 

        [BindProperty(SupportsGet = true)]
        public int Range { get; set; } 

        public List<Claims> FilteredClaims { get; set; }

        public async Task OnGetAsync()
        {
            var claimsQuery = _context.Claims.AsQueryable();
            claimsQuery = claimsQuery.Where(c => c.ApprovalAM == 1 && c.ApprovalPC == 1);

            // Filter by Lecturer ID
            //if (LecturerID != "All")
            //{
            //    claimsQuery = claimsQuery.Where(c => c.LecturerID == LecturerID);
            //}

            // Filter by Month
            if (Month != "All")
            {
                claimsQuery = claimsQuery.Where(c => c.ClaimMonth == Month);
            }

            // Filter by Range
            claimsQuery = Range switch
            {
                1 => claimsQuery.Where(c => c.ClaimAmount < 5000),
                2 => claimsQuery.Where(c => c.ClaimAmount >= 5000 && c.ClaimAmount <= 20000),
                3 => claimsQuery.Where(c => c.ClaimAmount > 20000),
                0 => claimsQuery // No filtering
            };

            FilteredClaims = await claimsQuery.ToListAsync();
        }


    }
}

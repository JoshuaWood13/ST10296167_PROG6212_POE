using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10296167_PROG6212_POE.Data;
using ST10296167_PROG6212_POE.Models;

namespace ST10296167_PROG6212_POE.Controllers
{
    public class HRController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        // Controller
        //------------------------------------------------------------------------------------------------------------------------------------------//
        public HRController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//

        // Views
        //------------------------------------------------------------------------------------------------------------------------------------------//
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Report()
        {
            return View();
        }

        public IActionResult LecturerData(string id)
        {
            var lecturer = _userManager.Users.FirstOrDefault(u => u.Id == id);
            return View(lecturer);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//

        // Methods
        //------------------------------------------------------------------------------------------------------------------------------------------//
        // This method handles retrieving the required claims for a report based on the input filters
        [HttpPost]
        public async Task<IActionResult> GenerateReport(string month, int range)
        {
            var claimsQuery = _context.Claims.AsQueryable();
            claimsQuery = claimsQuery.Where(c => c.ApprovalAM == 1 && c.ApprovalPC == 1);

            if (!string.IsNullOrEmpty(month) && month != "All")
            {
                claimsQuery = claimsQuery.Where(c => c.ClaimMonth == month);
            }

            claimsQuery = range switch
            {
                1 => claimsQuery.Where(c => c.ClaimAmount <= 20000),
                2 => claimsQuery.Where(c => c.ClaimAmount > 20000 && c.ClaimAmount <= 40000),
                3 => claimsQuery.Where(c => c.ClaimAmount > 40000 && c.ClaimAmount <= 60000),
                4 => claimsQuery.Where(c => c.ClaimAmount > 60000),
                _ => claimsQuery
            };

            var filteredClaims = await claimsQuery.ToListAsync();

            double totalAmount = filteredClaims.Sum(c => c.ClaimAmount);
            double totalClaims = filteredClaims.Count;
            double totalHours = filteredClaims.Sum(c => c.HoursWorked);
            double averageClaimAmount = Math.Round(totalAmount / totalClaims, 2);
            double averageHoursWorked = Math.Round(totalHours / totalClaims, 2);
            double averageHourlyRate = Math.Round(totalAmount / totalHours, 2);

            var reportData = new Report
            {
                FilteredClaims = filteredClaims,
                TotalClaimsValue = totalAmount,
                TotalClaimsCount = totalClaims,
                TotalHoursWorked = totalHours,
                AverageHourlyRate = averageHourlyRate,
                AverageHoursWorked = averageHoursWorked,
                AverageClaimAmount = averageClaimAmount
            };

            return View("Report", reportData);
        }

        // This method handles finding the correct lecturer based on user input 
        [HttpPost]
        public async Task<IActionResult> Search(string lecturerID)
        {
            if (string.IsNullOrEmpty(lecturerID))
            {
                TempData["ErrorMessage"] = "Please enter a Lecturer ID before searching";
                return View("Dashboard");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == lecturerID);

            if (user == null || !(await _userManager.IsInRoleAsync(user, "Lecturer")))
            {
                TempData["ErrorMessage"] = "Lecturer could not be found.";
                return View("Dashboard");
            }

            return RedirectToAction("LecturerData", new { id = user.Id });
        }

        // This method handles updating the contact details of a lecturer
        [HttpPost]
        public async Task<IActionResult> UpdateLecturerData(User lecturer)
        {

            if (!ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByIdAsync(lecturer.Id);
                if (existingUser == null)
                {
                    TempData["ErrorMessage"] = "Lecturer not found";
                    return RedirectToAction("Dashboard", "HR");
                }

                return View("LecturerData",existingUser);
            }

            var userToUpdate = await _userManager.FindByIdAsync(lecturer.Id);
            if (userToUpdate == null)
            {
                TempData["ErrorMessage"] = "Lecturer not found";
                return RedirectToAction("Dashboard", "HR");
            }

            userToUpdate.PhoneNumber = lecturer.PhoneNumber;
            userToUpdate.Email = lecturer.Email;
            userToUpdate.UserName = lecturer.Email;
            userToUpdate.Address = lecturer.Address;

            var result = await _userManager.UpdateAsync(userToUpdate);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Lecturer data updated successfully!";

                return RedirectToAction("Dashboard", "HR");
            }
            else
            {
                TempData["ErrorMessage"] = "Lecturer data could not be updated";
                return RedirectToAction("Dashboard", "HR");
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//
    }
}
//--------------------------------------------------------X END OF FILE X-------------------------------------------------------------------//
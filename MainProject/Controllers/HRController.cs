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

        public HRController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
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

        // Action for handling "Generate Report" submission
        [HttpPost]
        public async Task<IActionResult> GenerateReport(string month, int range)
        {
            var claimsQuery = _context.Claims.AsQueryable();
            claimsQuery = claimsQuery.Where(c => c.ApprovalAM == 1 && c.ApprovalPC == 1);

            // Filter by Month
            if (!string.IsNullOrEmpty(month) && month != "All")
            {
                claimsQuery = claimsQuery.Where(c => c.ClaimMonth == month);
            }

            // Filter by Range
            claimsQuery = range switch
            {
                //1 => claimsQuery.Where(c => c.ClaimAmount < 5000),
                //2 => claimsQuery.Where(c => c.ClaimAmount >= 5000 && c.ClaimAmount <= 20000),
                //3 => claimsQuery.Where(c => c.ClaimAmount > 20000),
                //_ => claimsQuery // No filtering

                1 => claimsQuery.Where(c => c.ClaimAmount <= 10000),
                2 => claimsQuery.Where(c => c.ClaimAmount > 10000 && c.ClaimAmount <= 20000),
                3 => claimsQuery.Where(c => c.ClaimAmount > 20000 && c.ClaimAmount <= 30000),
                4 => claimsQuery.Where(c => c.ClaimAmount > 30000),
                _ => claimsQuery // No filtering
            };

            // Get the filtered claims
            var filteredClaims = await claimsQuery.ToListAsync();

            // Calculate total and average claim amounts
            double totalAmount = filteredClaims.Sum(c => c.ClaimAmount);
            double totalClaims = filteredClaims.Count;
            double totalHours = filteredClaims.Sum(c => c.HoursWorked);
            double averageClaimAmount = Math.Round(totalAmount / totalClaims, 2);
            double averageHoursWorked = Math.Round(totalHours / totalClaims, 2);
            double averageHourlyRate = Math.Round(totalAmount / totalHours, 2);

            // Pass the filtered claims and statistics to the Report view
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
            // Handle the report generation logic here

            //return RedirectToAction("Report", new { month = month, range = range });
        }

        [HttpPost]
        public async Task<IActionResult> Search(string lecturerID)
        {
            if (string.IsNullOrEmpty(lecturerID))
            {
                TempData["ErrorMessage"] = "Please enter a Lecturer ID before searching";
                return View("Dashboard");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == lecturerID);
            //var user = await _userManager.FindByIdAsync(lecturerID);
            if (user == null || !(await _userManager.IsInRoleAsync(user, "Lecturer")))
            {
                TempData["ErrorMessage"] = "Lecturer could not be found.";
                return View("Dashboard");
            }

            return RedirectToAction("LecturerData", new { id = user.Id });
        }

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

            //Console.WriteLine($"Lecturer Data: {lecturer.FirstName} {lecturer.LastName}, Email: {lecturer.Email}, Phone: {lecturer.PhoneNumber}, Address: {lecturer.Address}");

            //TempData["SuccessMessage"] = "Lecturer data updated successfully!";

            //return RedirectToAction("Dashboard", "HR");

            var userToUpdate = await _userManager.FindByIdAsync(lecturer.Id);
            if (userToUpdate == null)
            {
                TempData["ErrorMessage"] = "Lecturer not found";
                return RedirectToAction("Dashboard", "HR");
            }

            // Update the user's properties
            userToUpdate.PhoneNumber = lecturer.PhoneNumber;
            userToUpdate.Email = lecturer.Email;
            userToUpdate.UserName = lecturer.Email;
            userToUpdate.Address = lecturer.Address;

            // Save the changes to the user
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
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace ST10296167_PROG6212_POE.Pages
{
    public class IndexHRModel : PageModel
    {
        public IndexHRModel()
        {
            
        }
        [BindProperty]
        public string LecturerID { get; set; }

        [BindProperty]
        public string Month { get; set; }

        [BindProperty]
        public int Range { get; set; }

        [BindProperty]
        public string LecturerSelect { get; set; }


        public IActionResult OnPost()
        {
            return RedirectToPage("/Report", new { month = Month, range = Range });
        }
    }
}

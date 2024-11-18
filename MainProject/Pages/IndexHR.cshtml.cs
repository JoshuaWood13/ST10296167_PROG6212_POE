using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ST10296167_PROG6212_POE.Pages
{
    [Authorize(Roles = "Human Resources")]
    public class IndexHRModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}

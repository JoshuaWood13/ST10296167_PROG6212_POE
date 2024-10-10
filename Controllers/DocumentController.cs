using Microsoft.AspNetCore.Mvc;
using ST10296167_PROG6212_POE.Data;
using ST10296167_PROG6212_POE.Models;

namespace ST10296167_PROG6212_POE.Controllers
{
    public class DocumentController : Controller
    {
        private readonly AppDbContext _context;

        public DocumentController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult UploadDocuments()
        {
            return View();
        }

        // ref: https://www.c-sharpcorner.com/article/restrict-uploaded-file-size-in-asp-net-core2/#:~:text=In%20ASP.NET%20Core%2C%20you,programmatically%20in%20your%20controller%20actions.
        [HttpPost]
        [RequestSizeLimit(5000000)] // Limit file size to 5 MB
        public async Task<IActionResult> UploadDocs(int ClaimID, IFormFile File)
        {
            var lecturerID = HttpContext.Session.GetInt32("AccountID");

            var claim = await _context.Claims.FindAsync(ClaimID);
            if (claim == null || claim.LecturerID != lecturerID)
            {
                TempData["Error"] = $"Claim <{ClaimID}> does not exist, or you do not have permission to upload files to this Claim";
                return View("UploadDocuments");
            }

            if (File == null || File.Length <= 0)
            {
                TempData["Error"] = "Please upload a valid file.";
                return View("UploadDocuments"); 
            }

            var validFileTypes = new[] { ".pdf", ".docx", ".xlsx", ".txt" };
            var fileExtension = Path.GetExtension(File.FileName).ToLower();
            if (!validFileTypes.Contains(fileExtension))
            {
                TempData["Error"] = "Invalid file type. Please upload a .pdf, .docx, .xlsx or .txt file.";
                return View("UploadDocuments");
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await File.CopyToAsync(memoryStream);

                    var document = new Documents
                    {
                        ClaimID = ClaimID,
                        FileData = memoryStream.ToArray(),
                        FileName = File.FileName
                    };

                    await _context.Documents.AddAsync(document);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"{File.FileName} file has been successfully uploaded!";
                }
                return View("UploadDocuments");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while uploading the file.";
                return View("UploadDocuments");
            }
        }
    }
}

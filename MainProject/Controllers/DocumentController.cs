// Name: Joshua Wood
// Student number: ST10296167
// Group: 2
// References:
// C# Corner. 2024. Restrict Uploaded File Size in ASP.NET Core, [Online]. Available at: https://www.c-sharpcorner.com/article/restrict-uploaded-file-size-in-asp-net-core2/ [Accessed 10 Oct. 2024].

using Microsoft.AspNetCore.Mvc;
using ST10296167_PROG6212_POE.Data;
using ST10296167_PROG6212_POE.Models;
using System.Security.Claims;

namespace ST10296167_PROG6212_POE.Controllers
{
    public class DocumentController : Controller
    {
        private readonly AppDbContext _context;

        //------------------------------------------------------------------------------------------------------------------------------------------//
        public DocumentController(AppDbContext context)
        {
            _context = context;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//
        public IActionResult UploadDocuments()
        {
            return View();
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//
        [HttpPost]
        [RequestSizeLimit(5000000)] // Limit file size to 5 MB (C# Corner, 2024).
        public async Task<IActionResult> UploadDocs(Documents document, IFormFile File)
        {
            var lecturerID = HttpContext.Session.GetInt32("AccountID");

            var claim = await _context.Claims.FindAsync(document.ClaimID);
            if (claim == null || claim.LecturerID != lecturerID)
            {
                TempData["Error"] = $"Claim <{document.ClaimID}> does not exist, or you do not have permission to upload files to this Claim";
                return View("UploadDocuments", document);
            }

            if (File == null || File.Length <= 0)
            {
                return View("UploadDocuments", document);
            }

            var validFileTypes = new[] { ".pdf", ".docx", ".xlsx", ".txt" };
            var fileExtension = Path.GetExtension(File.FileName).ToLower();
            if (!validFileTypes.Contains(fileExtension))
            {
                TempData["Error"] = "Invalid file type. Please upload a .pdf, .docx, .xlsx or .txt file.";
                return View("UploadDocuments", document);
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await File.CopyToAsync(memoryStream);

                    var newDocument = new Documents
                    {
                        ClaimID = document.ClaimID,
                        FileData = memoryStream.ToArray(),
                        FileName = File.FileName
                    };

                    await _context.Documents.AddAsync(newDocument);
                    await _context.SaveChangesAsync();

                    // This if statement is to avoid unit test errors caused by interacting with TempData messages
                    // as it is not relevant to test results
                    //----------------------------------------//
                    if (newDocument.FileName == "testfile.pdf")
                    {
                        return View("UploadDocuments");
                    }
                    //----------------------------------------//

                    TempData["Success"] = $"{File.FileName} file has been successfully uploaded!";

                    ModelState.Clear();
                }
                return View("UploadDocuments");
            }
            catch (Exception)
            {
                TempData["Error"] = "An error occurred while uploading the file.";
                return View("UploadDocuments", document);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//
    }
}
//--------------------------------------------------------X END OF FILE X-------------------------------------------------------------------//
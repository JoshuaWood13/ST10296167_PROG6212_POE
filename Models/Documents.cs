using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ST10296167_PROG6212_POE.Models
{
    public class Documents
    {
        [Key]
        public int DocumentID { get; set; }

        [ForeignKey("Claims")]
        [Required(ErrorMessage = "Please enter a Claim ID")]
        public int ClaimID { get; set; }

        [Required(ErrorMessage = "Please select a file to upload ")]
        public byte[] FileData { get; set; }  // Store the file data as a byte array

        [Required]
        [StringLength(100)]  // Limiting the length of the file name
        public string FileName { get; set; }  // The original name of the uploaded file

        public Claims Claims { get; set; }  // Navigation property back to Claims
    }
}

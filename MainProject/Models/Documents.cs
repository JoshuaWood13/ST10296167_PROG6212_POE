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
        public byte[] FileData { get; set; }  

        [Required]
        [StringLength(100)]  
        public string FileName { get; set; }  

        public Claims Claims { get; set; }  // Navigation property back to Claims
    }
}

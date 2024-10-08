using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10296167_PROG6212_POE.Models
{
    public class Claims
    {
        [Key]
        public int ClaimID { get; set; }

        [ForeignKey("Lecturers")]
        public int LecturerID { get; set; }

        [Required]
        [Range(0, double.MaxValue)]  
        public double HourlyRate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double HoursWorked { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double ClaimAmount { get; set; }

        [Required]
        public string ClaimMonth { get; set; }

        public string Description { get; set; }

        [Range(0,1)]
        public int ApprovalPC { get; set; }

        [Range(0, 1)]
        public int ApprovalAM { get; set; }

        public Lecturers Lecturers { get; set; }  // Navigation property to the Lecturers model

    }
}

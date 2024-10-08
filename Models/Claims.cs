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


        public string Status
        {
            get
            {
                if (ApprovalPC == 0 && ApprovalAM == 0)
                    return "Pending (0/2)";
                else if (ApprovalPC == 2)
                    return "Rejected";
                else if (ApprovalPC == 1 && ApprovalAM == 0)
                    return "Pending (1/2)";
                else if (ApprovalPC == 1 && ApprovalAM == 1)
                    return "Approved";
                else if (ApprovalPC == 1 && ApprovalAM == 2)
                    return "Rejected";
                else
                    return "Error"; // Handle any unexpected cases
            }
        }
    }
}

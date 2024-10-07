using System.ComponentModel.DataAnnotations;

namespace ST10296167_PROG6212_POE.Models
{
    public class ProgrammeCoordinator
    {
        [Key]
        public int PM_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }
    }
}

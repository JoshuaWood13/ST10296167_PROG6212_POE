using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10296167_PROG6212_POE.Models
{
    public class Lecturers
    {
        [Key]
        public int LecturerID { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        //[Required]
        //[StringLength(100)]
        //public string Name { get; set; }

        //[Required]
        //[StringLength(100)]
        //public string Surname { get; set; }

        //[Required]
        //[StringLength(100)]
        //public string Email { get; set; }
    }
}

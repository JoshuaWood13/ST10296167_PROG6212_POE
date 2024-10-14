// Name: Joshua Wood
// Student number: ST10296167
// Group: 2

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

    }
}
//--------------------------------------------------------X END OF FILE X-------------------------------------------------------------------//
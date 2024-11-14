// Name: Joshua Wood
// Student number: ST10296167
// Group: 2

using System.ComponentModel.DataAnnotations;

namespace ST10296167_PROG6212_POE.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Please enter your account ID")]
        [StringLength(10, ErrorMessage = "Account ID cannot exceed 10 characters")]
        public string AccountID { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }
    }
}
//--------------------------------------------------------X END OF FILE X-------------------------------------------------------------------//
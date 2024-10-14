// Name: Joshua Wood
// Student number: ST10296167
// Group: 2

using System.ComponentModel.DataAnnotations;

namespace ST10296167_PROG6212_POE.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Please enter your username")]
        [Range(1, int.MaxValue, ErrorMessage = "Enter a valid account ID above 0")]
        public int AccountID { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }
    }
}
//--------------------------------------------------------X END OF FILE X-------------------------------------------------------------------//
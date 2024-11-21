using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ST10296167_PROG6212_POE.Models
{
    public class User : IdentityUser
    {
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression(@"^[^@\s]+@example\.com$", ErrorMessage = "The email must end with '@email.com'.")]
        public override string Email { get; set; }

        [Phone]
         [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be 10 digits.")]
        public override string PhoneNumber { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        public string? UserId { get; set; }

    }
}
//--------------------------------------------------------X END OF FILE X-------------------------------------------------------------------//
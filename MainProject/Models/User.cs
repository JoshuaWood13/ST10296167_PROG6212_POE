using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ST10296167_PROG6212_POE.Models
{
    public class User : IdentityUser
    {
        //[Required]
        //[StringLength(10)]
        //public string UserId { get; set; }

        // Full Name (First Name)
        [StringLength(100)]
        public string FirstName { get; set; }

        // Surname
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        // Email (Already included in IdentityUser, but you can also make it a required field)
        [Required]
        [EmailAddress]
        public override string Email { get; set; }

        // Phone Number (Already included in IdentityUser)
        [Phone]
        public override string PhoneNumber { get; set; }

        // Address
        [StringLength(500)]
        public string Address { get; set; }

        public string? UserId { get; set; }

    }
}

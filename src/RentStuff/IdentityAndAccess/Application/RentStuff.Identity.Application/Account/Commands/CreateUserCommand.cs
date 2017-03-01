using System.ComponentModel.DataAnnotations;

namespace RentStuff.Identity.Application.Account.Commands
{
    public class CreateUserCommand
    {
        [Required]
        [StringLength(19, ErrorMessage = "The {0} must be upto {1} characters long.")]
        [Display(Name = "FullName")]
        public string FullName { get; set; }
        
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
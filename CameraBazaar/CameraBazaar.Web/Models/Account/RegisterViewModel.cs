namespace CameraBazaar.Web.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterViewModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        [RegularExpression(@"^([A-Za-z]+)$", ErrorMessage = "The {0} must have only letters.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [RegularExpression(@"^\+\d{10,12}$", ErrorMessage = "The {0} must start with a '+' sign and contain between 10 and 12 digits.")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

    }
}

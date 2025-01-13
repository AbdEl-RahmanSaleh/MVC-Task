using System.ComponentModel.DataAnnotations;

namespace Task.Models
{
    public class SignUpViewModel
    {

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 charachters")]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Password Mismatch")]
        public string ConfirmPassword { get; set; }

    }
}

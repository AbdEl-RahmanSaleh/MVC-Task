using System.ComponentModel.DataAnnotations;

namespace Task.Models
{
    public class LogInViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace SpudPI.Shared
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Please enter a password")]
        [MinLength(6, ErrorMessage = "Your password must be at least 6 characters long")]
        [MaxLength(100, ErrorMessage = "Your password must not be longer than 100 characters")]
        public string Password { get; set; } = string.Empty;
    }
}

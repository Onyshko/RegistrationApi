using System.ComponentModel.DataAnnotations;

namespace RegApi.Services.Models
{
    public class UserRegistrationModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "The password and confirmation password do not match")]
        public string? ConfirmPassword { get; set; }

        public string? ClientUri { get; set; }
    }
}

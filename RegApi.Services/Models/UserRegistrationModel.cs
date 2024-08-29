using System.ComponentModel.DataAnnotations;

namespace RegApi.Services.Models
{
    public class UserRegistrationModel
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "The password and confirmation password do not match")]
        public required string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "ClientUri is required")]
        public required string ClientUri { get; set; }
    }
}

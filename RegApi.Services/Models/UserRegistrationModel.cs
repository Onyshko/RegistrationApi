using System.ComponentModel.DataAnnotations;

namespace RegApi.Services.Models
{
    public class UserRegistrationModel
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string ConfirmPassword { get; set; }

        public required string ClientUri { get; set; }
    }
}

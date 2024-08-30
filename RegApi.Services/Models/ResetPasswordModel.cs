using System.ComponentModel.DataAnnotations;

namespace RegApi.Services.Models
{
    public class ResetPasswordModel
    {
        public required string Password { get; set; }

        public required string ConfirmPassword { get; set; }

        public required string Email { get; set; }

        public required string Token { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace RegApi.Services.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public required string Email { get; set; }

        [Required(ErrorMessage = "ClientUri is required")]
        public required string ClientUri { get; set; }
    }
}

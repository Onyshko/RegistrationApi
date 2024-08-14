using System.ComponentModel.DataAnnotations;

namespace RegApi.Services.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? ClientUri { get; set; }
    }
}

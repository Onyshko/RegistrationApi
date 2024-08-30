using System.ComponentModel.DataAnnotations;

namespace RegApi.Services.Models
{
    public class ForgotPasswordModel
    {
        public required string Email { get; set; }

        public required string ClientUri { get; set; }
    }
}

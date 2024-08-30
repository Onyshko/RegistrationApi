using System.ComponentModel.DataAnnotations;

namespace RegApi.Services.Models
{
    public class UserAuthenticationModel
    {
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace RegApi.Services.Models
{
    /// <summary>
    /// Represents a model for user authentication.
    /// </summary>
    public class UserAuthenticationModel
    {
        /// <summary>
        /// Gets or sets the email of the user for authentication.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user for authentication.
        /// </summary>
        public required string Password { get; set; }
    }

}

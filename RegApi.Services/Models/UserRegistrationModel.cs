using System.ComponentModel.DataAnnotations;

namespace RegApi.Services.Models
{
    /// <summary>
    /// Represents a model for user registration.
    /// </summary>
    public class UserRegistrationModel
    {
        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the email of the user for registration.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user for registration.
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirmation password for the user registration.
        /// </summary>
        public required string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the client URI for redirecting after registration.
        /// </summary>
        public required string ClientUri { get; set; }
    }

}

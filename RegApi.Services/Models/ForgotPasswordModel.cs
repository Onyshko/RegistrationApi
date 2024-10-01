using System.ComponentModel.DataAnnotations;

namespace RegApi.Services.Models
{
    /// <summary>
    /// Represents the model for the "Forgot Password" functionality, containing the user's email and client URI.
    /// </summary>
    public class ForgotPasswordModel
    {
        /// <summary>
        /// The email address of the user requesting a password reset.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// The URI to which the user will be redirected after resetting their password.
        /// </summary>
        public required string ClientUri { get; set; }
    }

}

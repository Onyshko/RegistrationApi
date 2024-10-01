using System.ComponentModel.DataAnnotations;

namespace RegApi.Services.Models
{
    /// <summary>
    /// Represents the model for resetting a user's password.
    /// </summary>
    public class ResetPasswordModel
    {
        /// <summary>
        /// Gets or sets the new password for the user.
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirmation of the new password.
        /// </summary>
        public required string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user requesting the password reset.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the token used for validating the password reset request.
        /// </summary>
        public required string Token { get; set; }
    }

}

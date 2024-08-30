using Microsoft.AspNetCore.Identity;
using RegApi.Services.Models;

namespace RegApi.Services.Interfaces
{
    /// <summary>
    /// Defines a contract for user-related operations such as registration, authentication, and password management.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Registers a new user asynchronously by creating an account and sending a confirmation email.
        /// </summary>
        /// <param name="user">The user registration details including email, password, and client URI.</param>
        Task RegistrateAsync(UserRegistrationModel user);

        /// <summary>
        /// Checks if the user exists, has confirmed their email, and if the provided password is correct.
        /// </summary>
        /// <param name="userAuthenticationModel">The user authentication details including email and password.</param>
        Task CheckOfUserAsync(UserAuthenticationModel userAuthenticationModel);

        /// <summary>
        /// Verifies the user's email using a confirmation token asynchronously.
        /// </summary>
        /// <param name="email">The email address of the user to confirm.</param>
        /// <param name="token">The email confirmation token.</param>
        Task EmailCheckAsync(string email, string token);

        /// <summary>
        /// Initiates the forgot password process by generating a password reset token and sending it to the user's email asynchronously.
        /// </summary>
        /// <param name="forgotPasswordModel">The forgot password details including email and client URI.</param>
        Task ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel);

        /// <summary>
        /// Resets the user's password using a reset token and new password asynchronously.
        /// </summary>
        /// <param name="resetPasswordModel">The reset password details including email, token, and new password.</param>
        Task ResetPasswordAsync(ResetPasswordModel resetPasswordModel);

        /// <summary>
        /// Deletes the user account with the specified ID asynchronously.
        /// </summary>
        /// <param name="accountId">The unique identifier of the user account to be deleted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAccountAsync(string accountId);
    }

}

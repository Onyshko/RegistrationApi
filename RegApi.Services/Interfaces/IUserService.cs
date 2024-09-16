using Microsoft.AspNetCore.Authentication;
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

        /// <summary>
        /// Retrieves an existing user by email from the Google authentication result, or creates a new user if one is not found.
        /// </summary>
        /// <param name="response">The Google authentication result that includes user claims such as email and name.</param>
        /// <returns>The email address of the user found or created.</returns>
        /// <exception cref="NullReferenceException">Thrown if the authentication response or email claim is null.</exception>
        /// <exception cref="IdentityException">Thrown if user registration fails.</exception>
        /// <remarks>
        /// This method searches for a user based on the email address obtained from the Google authentication response.
        /// If the user does not exist in the system, a new user is created with the provided email and name. The new user is then
        /// assigned the "Visitor" role, and all changes are committed to the database. The method returns the email address of the user.
        /// </remarks>
        Task<string> FindOrCreateGoogleAsync(AuthenticateResult response);

    }

}

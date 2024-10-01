using Microsoft.AspNetCore.Identity;
using RegApi.Domain.Entities;

namespace RegApi.Repository.Interfaces
{
    /// <summary>
    /// Defines methods for user account management, including registration, role assignment, and password handling.
    /// </summary>
    public interface IUserAccountRepository
    {
        /// <summary>
        /// Registers a new user with an optional password.
        /// </summary>
        /// <param name="user">The user to be registered.</param>
        /// <param name="password">The password for the new user. If null, the user will be registered without a password.</param>
        /// <returns>A task representing the asynchronous operation, containing an IdentityResult indicating the outcome of the registration.</returns>
        Task<IdentityResult> RegisterAsync(User user, string? password = null);


        /// <summary>
        /// Generates an email confirmation token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <returns>A string representing the email confirmation token.</returns>
        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        /// <summary>
        /// Adds the specified user to the given role.
        /// </summary>
        /// <param name="user">The user to add to the role.</param>
        /// <param name="role">The role to assign to the user.</param>
        /// <returns>An IdentityResult indicating the success or failure of the role assignment.</returns>
        Task<IdentityResult> AddToRoleAsync(User user, string role);

        /// <summary>
        /// Finds a user by their email (used as the username).
        /// </summary>
        /// <param name="email">The email (username) of the user to find.</param>
        /// <returns>The user associated with the specified email.</returns>
        Task<User> FindByNameAsync(string email);

        /// <summary>
        /// Checks if the specified user's email is confirmed.
        /// </summary>
        /// <param name="user">The user whose email confirmation status is to be checked.</param>
        /// <returns>A boolean indicating whether the user's email is confirmed.</returns>
        Task<bool> IsEmailConfirmed(User user);

        /// <summary>
        /// Verifies whether the specified password is valid for the given user.
        /// </summary>
        /// <param name="user">The user whose password is to be checked.</param>
        /// <param name="password">The password to verify.</param>
        /// <returns>A boolean indicating whether the password is correct.</returns>
        Task<bool> CheckPassword(User user, string password);

        /// <summary>
        /// Retrieves the roles assigned to the specified user.
        /// </summary>
        /// <param name="user">The user whose roles are to be retrieved.</param>
        /// <returns>A list of role names assigned to the user.</returns>
        Task<IList<string>> GetRolesAsync(User user);

        /// <summary>
        /// Finds a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to find.</param>
        /// <returns>The user associated with the specified email, or null if no user is found.</returns>
        Task<User?> FindByEmailAsync(string email);

        /// <summary>
        /// Confirms the email address of the specified user using the provided token.
        /// </summary>
        /// <param name="user">The user whose email is to be confirmed.</param>
        /// <param name="token">The token used for email confirmation.</param>
        /// <returns>An IdentityResult indicating the success or failure of the email confirmation.</returns>
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        /// <summary>
        /// Generates a password reset token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to generate the password reset token.</param>
        /// <returns>A string representing the password reset token.</returns>
        Task<string> GeneratePasswordResetTokenAsync(User user);

        /// <summary>
        /// Resets the password for the specified user using the provided token and new password.
        /// </summary>
        /// <param name="user">The user whose password is to be reset.</param>
        /// <param name="token">The token used for password reset.</param>
        /// <param name="password">The new password to set for the user.</param>
        /// <returns>An IdentityResult indicating the success or failure of the password reset.</returns>
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);

        /// <summary>
        /// Deletes the specified user account asynchronously.
        /// </summary>
        /// <param name="user">The user account to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IdentityResult"/> indicating the outcome of the delete operation.</returns>
        Task<IdentityResult> DeleteAsync(User user);

        /// <summary>
        /// Finds a user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to be found.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="User"/> object if found; otherwise, <c>null</c>.</returns>
        Task<User> FindByIdAsync(string userId);

        /// <summary>
        /// Updates the specified user in the database.
        /// </summary>
        /// <param name="user">The user entity to be updated.</param>
        /// <returns>A task that represents the asynchronous update operation. The task result contains <see cref="IdentityResult"/>, indicating the success or failure of the operation.</returns>
        Task<IdentityResult> UpdateAsync(User user);
    }
}

using Microsoft.AspNetCore.Identity;
using RegApi.Domain.Entities;
using RegApi.Repository.Interfaces;
using System.Security.Claims;

namespace RegApi.Repository.Implementations
{
    /// <summary>
    /// Provides implementation for managing user accounts, including registration, role assignment, and password handling.
    /// </summary>
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly UserManager<User> _userManager;

        public UserAccountRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Registers a new user with the specified password.
        /// </summary>
        /// <param name="user">The user to register.</param>
        /// <param name="password">The password for the new user.</param>
        /// <returns>An IdentityResult indicating the success or failure of the registration.</returns>
        public async Task<IdentityResult> RegisterAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        /// <summary>
        /// Generates an email confirmation token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <returns>A string representing the email confirmation token.</returns>
        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        /// <summary>
        /// Adds the specified user to the given role.
        /// </summary>
        /// <param name="user">The user to add to the role.</param>
        /// <param name="role">The role to assign to the user.</param>
        /// <returns>An IdentityResult indicating the success or failure of the role assignment.</returns>
        public async Task<IdentityResult> AddToRoleAsync(User user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        /// <summary>
        /// Finds a user by their email (used as the username).
        /// </summary>
        /// <param name="email">The email (username) of the user to find.</param>
        /// <returns>The user associated with the specified email.</returns>
        public async Task<User> FindByNameAsync(string email)
        {
            return (await _userManager.FindByNameAsync(email))!;
        }

        /// <summary>
        /// Checks if the specified user's email is confirmed.
        /// </summary>
        /// <param name="user">The user whose email confirmation status is to be checked.</param>
        /// <returns>A boolean indicating whether the user's email is confirmed.</returns>
        public async Task<bool> IsEmailConfirmed(User user)
        {
            return await _userManager.IsEmailConfirmedAsync(user);
        }

        /// <summary>
        /// Verifies whether the specified password is valid for the given user.
        /// </summary>
        /// <param name="user">The user whose password is to be checked.</param>
        /// <param name="password">The password to verify.</param>
        /// <returns>A boolean indicating whether the password is correct.</returns>
        public async Task<bool> CheckPassword(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        /// <summary>
        /// Retrieves the roles assigned to the specified user.
        /// </summary>
        /// <param name="user">The user whose roles are to be retrieved.</param>
        /// <returns>A list of role names assigned to the user.</returns>
        public async Task<IList<string>> GetRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        /// <summary>
        /// Finds a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to find.</param>
        /// <returns>The user associated with the specified email, or null if no user is found.</returns>
        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Confirms the email address of the specified user using the provided token.
        /// </summary>
        /// <param name="user">The user whose email is to be confirmed.</param>
        /// <param name="token">The token used for email confirmation.</param>
        /// <returns>An IdentityResult indicating the success or failure of the email confirmation.</returns>
        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        /// <summary>
        /// Generates a password reset token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to generate the password reset token.</param>
        /// <returns>A string representing the password reset token.</returns>
        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        /// <summary>
        /// Resets the password for the specified user using the provided token and new password.
        /// </summary>
        /// <param name="user">The user whose password is to be reset.</param>
        /// <param name="token">The token used for password reset.</param>
        /// <param name="password">The new password to set for the user.</param>
        /// <returns>An IdentityResult indicating the success or failure of the password reset.</returns>
        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }

        /// <summary>
        /// Deletes the specified user account asynchronously.
        /// </summary>
        /// <param name="user">The user account to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IdentityResult"/> indicating the outcome of the delete operation.</returns>
        public async Task<IdentityResult> DeleteAsync(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

        /// <summary>
        /// Finds a user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to be found.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="User"/> object if found; otherwise, <c>null</c>.</returns>
        public async Task<User> FindByIdAsync(string userId)
        {
            return (await _userManager.FindByIdAsync(userId))!;
        }
    }
}

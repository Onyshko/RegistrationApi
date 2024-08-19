using Microsoft.AspNetCore.Identity;
using RegApi.Domain.Entities;

namespace RegApi.Repository.Interfaces
{
    public interface IUserAccountRepository
    {
        Task<IdentityResult> RegisterAsync(User user, string password);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<IdentityResult> AddToRoleAsync(User user, string role);
        Task<User> FindByNameAsync(string email);
        Task<bool> IsEmailConfirmed(User user);
        Task<bool> CheckPassword(User user, string password);
        Task<IList<string>> GetRolesAsync(User user);
        Task<User?> FindByEmailAsync(string email);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);
    }
}

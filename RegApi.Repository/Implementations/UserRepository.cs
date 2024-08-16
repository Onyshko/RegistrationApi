using Microsoft.AspNetCore.Identity;
using RegApi.Domain.Entities;
using RegApi.Repository.Context;
using RegApi.Repository.Interfaces;

namespace RegApi.Repository.Implementations
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager, DatabaseContext context) : base(context)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> RegisterAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<IdentityResult> AddToRoleAsync(User user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<User> FindByNameAsync(string email)
        {
            return (await _userManager.FindByNameAsync(email))!;
        }

        public async Task<bool> IsEmailConfirmed(User user)
        {
            return await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<bool> CheckPassword(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public  async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }
    }
}

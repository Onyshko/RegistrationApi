using Microsoft.AspNetCore.Identity;
using RegApi.Domain.Entities;
using RegApi.Repository.Interfaces;

namespace RegApi.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> RegisterAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
    }
}

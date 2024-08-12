using Microsoft.AspNetCore.Identity;
using RegApi.Domain.Entities;

namespace RegApi.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> RegisterAsync(User user, string password);
        Task<User> FindByNameAsync(string email);
        Task<bool> CheckPassword(User user, string password);
    }
}

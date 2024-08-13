using Microsoft.AspNetCore.Identity;
using RegApi.Services.Models;

namespace RegApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<IList<string>> RegistrateAsync(UserRegistrationModel user);
        Task<string> CheckOfUserAsync(UserAuthenticationModel userAuthenticationModel);
        Task<string> EmailCheckAsync(string email, string token);
    }
}

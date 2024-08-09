using Microsoft.AspNetCore.Identity;
using RegApi.Services.Models;

namespace RegApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegistrateAsync(UserRegistrationModel user);
    }
}

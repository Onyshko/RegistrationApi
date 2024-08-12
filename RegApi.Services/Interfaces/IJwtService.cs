using RegApi.Services.Models;

namespace RegApi.Services.Interfaces
{
    public interface IJwtService
    {
        Task<string> CreateToken(UserAuthenticationModel userAuthenticationModel);
    }
}

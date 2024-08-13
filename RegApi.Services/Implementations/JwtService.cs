using RegApi.Repository.Handlers;
using RegApi.Repository.Interfaces;
using RegApi.Services.Interfaces;
using RegApi.Services.Models;

namespace RegApi.Services.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly JwtHandler _jwtHandler;
        private readonly IUserRepository _userRepo;

        public JwtService(JwtHandler jwtHandler, IUserRepository userRepo)
        {
            _jwtHandler = jwtHandler;
            _userRepo = userRepo;
        }

        public async Task<string> CreateToken(UserAuthenticationModel userAuthenticationModel)
        {
            var user = await _userRepo.FindByNameAsync(userAuthenticationModel.Email!);
            var roles = await _userRepo.GetRolesAsync(user);

            return _jwtHandler.CreateToken(user, roles);
        }
    }
}

using RegApi.Repository.Handlers;
using RegApi.Repository.Interfaces;
using RegApi.Services.Interfaces;
using RegApi.Services.Models;

namespace RegApi.Services.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly JwtHandler _jwtHandler;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepo;

        public JwtService(JwtHandler jwtHandler, IUserService userService, IUserRepository userRepo)
        {
            _jwtHandler = jwtHandler;
            _userRepo = userRepo;
        }

        public async Task<string> CreateToken(UserAuthenticationModel userAuthenticationModel)
        {
            return _jwtHandler.CreateToken(await _userRepo.FindByNameAsync(userAuthenticationModel.Email!));
        }
    }
}

using RegApi.Repository.Handlers;
using RegApi.Repository.Interfaces;
using RegApi.Services.Interfaces;
using RegApi.Services.Models;

namespace RegApi.Services.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly JwtHandler _jwtHandler;
        private readonly IUnitOfWork _unitOfWork;

        public JwtService(JwtHandler jwtHandler, IUnitOfWork unitOfWork)
        {
            _jwtHandler = jwtHandler;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> CreateToken(UserAuthenticationModel userAuthenticationModel)
        {
            var user = await _unitOfWork.UserAccountRepository().FindByNameAsync(userAuthenticationModel.Email!);
            var roles = await _unitOfWork.UserAccountRepository().GetRolesAsync(user);

            return _jwtHandler.CreateToken(user, roles);
        }
    }
}

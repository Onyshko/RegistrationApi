using RegApi.Repository.Handlers;
using RegApi.Repository.Interfaces;
using RegApi.Services.Interfaces;
using RegApi.Services.Models;

namespace RegApi.Services.Implementations
{
    /// <summary>
    /// Provides operations for creating JWT tokens.
    /// </summary>
    public class JwtService : IJwtService
    {
        private readonly JwtHandler _jwtHandler;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtService"/> class.
        /// </summary>
        /// <param name="jwtHandler">The JWT handler for creating tokens.</param>
        /// <param name="unitOfWork">The unit of work for interacting with the repository.</param>
        public JwtService(JwtHandler jwtHandler, IUnitOfWork unitOfWork)
        {
            _jwtHandler = jwtHandler;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Creates a JWT token for the specified user asynchronously.
        /// </summary>
        /// <param name="userAuthenticationModel">The user authentication details including email.</param>
        /// <returns>A JWT token for the authenticated user.</returns>
        public async Task<string> CreateToken(UserAuthenticationModel userAuthenticationModel)
        {
            var user = await _unitOfWork.UserAccountRepository().FindByNameAsync(userAuthenticationModel.Email!);
            var roles = await _unitOfWork.UserAccountRepository().GetRolesAsync(user);

            return _jwtHandler.CreateToken(user, roles);
        }
    }

}

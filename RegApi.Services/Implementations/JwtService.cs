using RegApi.Repository.Handlers;
using RegApi.Repository.Interfaces;
using RegApi.Services.Interfaces;

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
        /// Asynchronously creates a JWT token for the specified user based on their email.
        /// </summary>
        /// <param name="email">The email address of the user for whom the token is being created.</param>
        /// <returns>A task representing the asynchronous operation, containing the generated JWT token for the user.</returns>
        public async Task<string> CreateToken(string email)
        {
            var user = await _unitOfWork.UserAccountRepository().FindByEmailAsync(email);
            var roles = await _unitOfWork.UserAccountRepository().GetRolesAsync(user!);

            return _jwtHandler.CreateToken(user!, roles);
        }

    }

}

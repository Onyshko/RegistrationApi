using RegApi.Services.Models;

namespace RegApi.Services.Interfaces
{
    /// <summary>
    /// Defines a contract for operations related to JWT tokens, such as creating tokens.
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Creates a JWT token for the specified user.
        /// </summary>
        /// <param name="userAuthenticationModel">The user authentication details including email.</param>
        /// <returns>A JWT token for the authenticated user.</returns>
        Task<string> CreateToken(UserAuthenticationModel userAuthenticationModel);
    }

}

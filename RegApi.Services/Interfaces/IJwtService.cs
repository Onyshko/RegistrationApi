using RegApi.Services.Models;

namespace RegApi.Services.Interfaces
{
    /// <summary>
    /// Defines a contract for operations related to JWT tokens, such as creating tokens.
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Generates a JWT token for the specified user's email.
        /// </summary>
        /// <param name="email">The email address of the user for whom the token is being created.</param>
        /// <returns>A task representing the asynchronous operation, containing the JWT token for the user.</returns>
        Task<string> CreateToken(string email);
    }

}

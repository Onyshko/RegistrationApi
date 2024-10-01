using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RegApi.Domain.Entities;
using RegApi.Repository.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RegApi.Repository.Handlers
{
    public class JwtHandler
    {
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtHandler"/> class with the specified configuration and JWT settings.
        /// </summary>
        /// <param name="configuration">The configuration object.</param>
        /// <param name="jwtSettings">The JWT settings object containing security key, issuer, audience, and expiry details.</param>
        public JwtHandler(IConfiguration configuration, JwtSettings jwtSettings)
        {
            _configuration = configuration;
            _jwtSettings = jwtSettings;
        }

        /// <summary>
        /// Creates a JWT token for the specified user and their roles.
        /// </summary>
        /// <param name="user">The user for whom the token is being generated.</param>
        /// <param name="roles">The list of roles assigned to the user.</param>
        /// <returns>A JWT token as a string.</returns>
        public string CreateToken(User user, IList<string> roles)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = GetClaims(user, roles);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        /// <summary>
        /// Retrieves the signing credentials using the security key from the JWT settings.
        /// </summary>
        /// <returns>A <see cref="SigningCredentials"/> object with the security key and HMAC SHA-256 algorithm.</returns>
        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey!);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        /// <summary>
        /// Generates a list of claims for the specified user and their roles.
        /// </summary>
        /// <param name="user">The user whose claims are being created.</param>
        /// <param name="roles">The list of roles assigned to the user.</param>
        /// <returns>A list of claims containing the user's ID, username, and roles.</returns>
        private List<Claim> GetClaims(User user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        /// <summary>
        /// Generates the token options including issuer, audience, claims, expiration, and signing credentials.
        /// </summary>
        /// <param name="signingCredentials">The credentials used to sign the JWT token.</param>
        /// <param name="claims">The list of claims for the token.</param>
        /// <returns>A <see cref="JwtSecurityToken"/> object with the configured options.</returns>
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings.ValidIssuer,
                audience: _jwtSettings.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.ExpiryInMinutes)),
                signingCredentials: signingCredentials
                );

            return tokenOptions;
        }
    }
}

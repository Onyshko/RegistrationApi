namespace RegApi.Repository.Models
{
    /// <summary>
    /// Represents the settings for JWT (JSON Web Token) used for authentication and authorization.
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Gets or sets the valid issuer for the JWT.
        /// </summary>
        public string? ValidIssuer { get; set; }

        /// <summary>
        /// Gets or sets the valid audience for the JWT.
        /// </summary>
        public string? ValidAudience { get; set; }

        /// <summary>
        /// Gets or sets the security key used for signing the JWT.
        /// </summary>
        public string? SecurityKey { get; set; }

        /// <summary>
        /// Gets or sets the token expiry time in minutes.
        /// </summary>
        public string? ExpiryInMinutes { get; set; }
    }
}
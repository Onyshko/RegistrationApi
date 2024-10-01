namespace RegApi.Services.Models
{
    /// <summary>
    /// Represents the response model for user authentication, containing the results of the authentication attempt.
    /// </summary>
    public class AuthenticationResponsModel
    {
        /// <summary>
        /// Indicates whether the authentication was successful.
        /// </summary>
        public bool IsAuthSuccessful { get; set; }

        /// <summary>
        /// Contains an error message if the authentication failed; otherwise, it is null.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// The JWT token issued upon successful authentication.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// The SAS token generated for accessing the user's avatar, if applicable.
        /// </summary>
        public string? SasToken { get; set; }
    }

}

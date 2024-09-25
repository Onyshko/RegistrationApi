namespace RegApi.Repository.Models
{
    public class JwtSettings
    {
        public string? ValidIssuer { get; set; }
        public string? ValidAudience { get; set; }
        public string? SecurityKey { get; set; }
        public string? ExpiryInMinutes { get; set; }

    }
}

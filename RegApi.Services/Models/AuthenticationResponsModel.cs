namespace RegApi.Services.Models
{
    public class AuthenticationResponsModel
    {
        public bool IsAuthSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Token { get; set; }
    }
}

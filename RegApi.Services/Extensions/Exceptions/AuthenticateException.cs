namespace RegApi.Services.Extensions.Exceptions
{
    public class AuthenticateException : Exception
    {
        public AuthenticateException(string message) 
            : base(message) 
        {
        }
    }
}

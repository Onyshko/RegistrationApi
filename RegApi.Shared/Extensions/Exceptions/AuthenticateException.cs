namespace RegApi.Shared.Extensions.Exceptions
{
    public class AuthenticateException : Exception
    {
        public AuthenticateException(string message) 
            : base(message) 
        {
        }
    }
}

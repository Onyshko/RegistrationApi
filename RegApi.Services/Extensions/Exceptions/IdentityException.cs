namespace RegApi.Services.Extensions.Exceptions
{
    public class IdentityException : Exception
    {
        public IEnumerable<string> Errors { get; set; }

        public IdentityException(IEnumerable<string> errors)
            : base()
        {
            Errors = errors;
        }
    }
}

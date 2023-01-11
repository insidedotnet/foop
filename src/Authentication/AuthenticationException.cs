namespace Authentication
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException(string error, string errorDescription)
            : base(errorDescription)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }

        public string Error { get; init; }
        public string ErrorDescription { get; init; }
    }
}
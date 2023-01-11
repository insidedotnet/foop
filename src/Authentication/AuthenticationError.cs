using LanguageExt;

namespace Authentication
{
    public class AuthenticationError : Record<AuthenticationError>
    {
        public AuthenticationError(string error, string description)
        {
            Error = error;
            Description = description;
        }

        public readonly string Error;
        public readonly string Description;
    }
}
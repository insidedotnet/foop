using LanguageExt;
using NodaTime;

namespace Authentication
{
    public class ExpiringRefreshToken : Record<ExpiringRefreshToken>
    {
        public ExpiringRefreshToken(Token identity,
                                    Token accessToken,
                                    Token refreshToken,
                                    int expiresIn,
                                    LocalDateTime expiration)
        {
            Identity = identity;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpiresIn = expiresIn;
            Expiration = new Expiration(expiration);
        }

        public Token Identity { get; }
        public Token AccessToken { get; }
        public Token RefreshToken { get; }
        public int ExpiresIn { get; } // QUESTION: [rlittlesii: January 07, 2023] Should we change this type?
        public Expiration Expiration { get; }
    }
}
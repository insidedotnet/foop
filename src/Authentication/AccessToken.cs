using LanguageExt;
using NodaTime;
using NodaTime.Extensions;

namespace Authentication
{
    public class AccessToken : Record<AccessToken>
    {
        public AccessToken(string accessToken)
            : this(new Token(accessToken)) { }

        public AccessToken(Token accessToken)
            : this(accessToken,
                   DateTimeOffset.Now.ToZonedDateTime()
                                 .LocalDateTime.Plus(Period.FromDays(1))) { }

        public AccessToken(string accessToken, DateTimeOffset expiration)
            : this(new Token(accessToken), expiration) { }

        public AccessToken(Token accessToken, DateTimeOffset expiration)
            : this(accessToken,
                   expiration.ToZonedDateTime()
                             .LocalDateTime) { }

        public AccessToken(Token accessToken, LocalDateTime expiration)
        {
            Token = accessToken;
            Expiration = new Expiration(expiration);
        }

        public readonly Token Token;
        public readonly Expiration Expiration;
    }
}
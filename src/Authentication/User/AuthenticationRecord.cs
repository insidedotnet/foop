using System.Security.Claims;
using LanguageExt;
using NodaTime;
using NodaTime.Extensions;

namespace Authentication.User
{
    public class AuthenticationRecord : Record<AuthenticationRecord>
    {
        public AuthenticationRecord(
            ClaimsPrincipal user,
            AccessToken accessToken,
            Token identity,
            Token refreshToken,
            DateTimeOffset? authenticationTime
        )
        {
            AccessToken = accessToken;
            Identity = identity;
            RefreshToken = refreshToken;
            Claims = new Lst<Claim>(user.Identities.SelectMany(x => x.Claims));
            AuthenticationTime = authenticationTime?.ToZonedDateTime()
                                                   .LocalDateTime;
        }

        public static AuthenticationRecord Default { get; } = new AuthenticationRecord(
            new DefaultClaimsPrincipal(),
            new AccessToken(new Token()),
            new Token(),
            new Token(),
            DateTimeOffset.Now
        );

        public readonly AccessToken AccessToken;
        public readonly Token Identity;
        public readonly Token RefreshToken;
        public readonly Lst<Claim> Claims;
        public readonly LocalDateTime? AuthenticationTime;
    }

    public class DefaultClaimsPrincipal : ClaimsPrincipal
    {
        public DefaultClaimsPrincipal()
            : this(new List<ClaimsIdentity>
                   {
                       new ClaimsIdentity(new[]
                                          {
                                              new Claim("sid",
                                                        Guid.NewGuid()
                                                            .ToString())
                                          })
                   }) { }

        public DefaultClaimsPrincipal(IEnumerable<ClaimsIdentity> claimsIdentities)
            : base(claimsIdentities) { }
    }
}
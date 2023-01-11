using System.Security.Claims;

namespace Authentication
{
    public record AuthenticationResult(
        ClaimsPrincipal User,
        string AccessToken,
        string IdentityToken,
        string? RefreshToken,
        DateTimeOffset AccessTokenExpiration,
        DateTimeOffset? AuthenticationTime
    )
    {
        public static AuthenticationResult Default { get; } = new AuthenticationResult(
            default,
            string.Empty,
            string.Empty,
            string.Empty,
            DateTimeOffset.UnixEpoch,
            DateTimeOffset.UnixEpoch
        );
    }
}
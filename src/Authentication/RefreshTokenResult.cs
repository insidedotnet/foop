namespace Authentication
{
    public record RefreshTokenResult(string IdentityToken,
                                     string AccessToken,
                                     string RefreshToken,
                                     int ExpiresIn,
                                     DateTimeOffset AccessTokenExpiration);
}
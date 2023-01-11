using System.Security.Claims;
using Rocket.Surgery.Extensions.Testing.Fixtures;

namespace Authentication.Tests
{
    internal sealed class AuthenticationResultFixture : ITestFixtureBuilder
    {
        public static implicit operator AuthenticationResult(AuthenticationResultFixture fixture) => fixture.Build();

        public AuthenticationResultFixture WithAccessToken() => WithAccessToken("ACCESS_TOKEN");
        public AuthenticationResultFixture WithAccessToken(string accessToken) => this.With(ref _accessToken, accessToken);
        public AuthenticationResultFixture WithRefreshToken() => WithRefreshToken(Guid.NewGuid().ToString());
        public AuthenticationResultFixture WithRefreshToken(string refreshToken) => this.With(ref _refreshToken, refreshToken);
        private AuthenticationResult Build() => new(_user,
                                                    _accessToken,
                                                    _identityToken,
                                                    _refreshToken,
                                                    _accessTokenExpiration,
                                                    _authenticationTime);

        private readonly ClaimsPrincipal _user = new(new[]
                                                     {
                                                         new ClaimsIdentity(new[]
                                                                            {
                                                                                new Claim("email", "auth@zero.com"),
                                                                                new Claim("sid", Guid.NewGuid().ToString())
                                                                            })
                                                     });

        private string _accessToken = "ACCESS_TOKEN";
        private string _identityToken = "IDENTIFY_TOKEN";
        private string? _refreshToken = string.Empty;
        private DateTimeOffset _accessTokenExpiration = DateTimeOffset.MaxValue;
        private DateTimeOffset? _authenticationTime = null;
    }
}
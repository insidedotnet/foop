using System.Security.Claims;
using Authentication.User;
using Rocket.Surgery.Extensions.Testing.Fixtures;

namespace Authentication.Tests
{
    internal sealed class AuthenticationRecordFixture : ITestFixtureBuilder
    {
        public static implicit operator AuthenticationRecord(AuthenticationRecordFixture fixture) => fixture.Build();

        public AuthenticationRecordFixture WithAccessToken(string accessToken) => this.With(ref _accessToken, new AccessToken(accessToken));
        private AuthenticationRecord Build() => new(_user, _accessToken, _token, _refreshToken, _authenticatedAt);

        private ClaimsPrincipal _user = new(new List<ClaimsIdentity>{ new(new []{ new Claim("sid", Guid.NewGuid().ToString())})});
        private AccessToken _accessToken = new AccessToken("ACCESS_TOKEN");
        private Token _token = new Token("IDENTITY_TOKEN");
        private Token _refreshToken = new Token(string.Empty);
        private DateTimeOffset? _authenticatedAt = null;
    }
}
using NSubstitute;
using Rocket.Surgery.Airframe.Settings;
using Rocket.Surgery.Extensions.Testing.Fixtures;

namespace Authentication.Tests
{
    internal sealed class AuthenticatorFixture : ITestFixtureBuilder
    {
        public static implicit operator Authenticator(AuthenticatorFixture fixture) => fixture.Build();

        public AuthenticatorFixture WithClient(IAuthenticationClient client) => this.With(ref _client, client);

        public AuthenticatorFixture WithSettings(ISettingsProvider settings) => this.With(ref _settings, settings);

        public IAuthenticator AsInterface() => Build();

        private Authenticator Build() => new Authenticator(_client, _settings);

        private ISettingsProvider _settings = Substitute.For<ISettingsProvider>();
        private IAuthenticationClient _client = Substitute.For<IAuthenticationClient>();
    }
}
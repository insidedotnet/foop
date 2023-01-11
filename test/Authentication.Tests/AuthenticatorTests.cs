using Authentication.User;
using FluentAssertions;
using LanguageExt;
using Rocket.Surgery.Airframe.Settings;
using Unit = System.Reactive.Unit;

namespace Authentication.Tests
{
    public class AuthenticatorTests
    {
        [Theory]
        [ClassData(typeof(LoginTestCases))]
        public void GivenClient_WhenLogin_ThenLoginCompleted(IAuthenticationClient authenticationClient)
        {
            // Given
            var sut = new AuthenticatorFixture().WithClient(authenticationClient)
                                                .AsInterface();

            // When
            Unit? result = null;
            using (var _ = sut.Login()
                              .Subscribe(next => result = next)) { }

            // Then
            result.Should()
                  .NotBeNull();
        }

        [Theory]
        [ClassData(typeof(CurrentUserIdTestCases))]
        public void GivenClient_WhenCurrentUserId_ThenUserReturned(ISettingsProvider settings, bool expected)
        {
            // Given
            var sut = new AuthenticatorFixture().WithSettings(settings)
                                                .AsInterface();

            // When
            var result = Option<UserAccount>.None;

            using (var _ = sut.CurrentUser()
                              .Subscribe(next => result = next)) { }

            // Then
            result
                .IsSome
                .Should()
                .Be(expected);
        }
    }
}
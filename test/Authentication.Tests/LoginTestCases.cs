using System.Collections;
using NSubstitute;

namespace Authentication.Tests
{
    internal class LoginTestCases : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { GetSuccessfulClientResult() };
        }

        private IAuthenticationClient GetSuccessfulClientResult()
        {
            var client = Substitute.For<IAuthenticationClient>();
            AuthenticationResult authenticationResult = new AuthenticationResultFixture();

            client.Login(Arg.Any<CancellationToken>())
                  .Returns(Task.FromResult(new LanguageExt.Common.Result<AuthenticationResult>(authenticationResult)));

            return client;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
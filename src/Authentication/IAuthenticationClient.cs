using LanguageExt.Common;

namespace Authentication
{
    public interface IAuthenticationClient
    {
        /// <summary>
        /// Login the current user and receive an authentication result containing the state of the request.
        /// </summary>
        /// <returns>The result of the authentication.</returns>
        Task<Result<AuthenticationResult>> Login() => Login(null, default);

        /// <summary>
        /// Login the current user and receive an authentication result containing the state of the request.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result of the authentication.</returns>
        Task<Result<AuthenticationResult>> Login(CancellationToken cancellationToken) => Login(null, cancellationToken);

        /// <summary>
        /// Login the current user and receive an authentication result containing the state of the request.
        /// </summary>
        /// <param name="request">The authentication request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result of the authentication.</returns>
        Task<Result<AuthenticationResult>> Login(AuthenticationRequest? request, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the claims for the provided users access token.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The users claims.</returns>
        Task<UserClaimsResponse> GetClaims(string accessToken, CancellationToken cancellationToken);

        /// <summary>
        /// Logout the currently authenticated user.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The logout result.</returns>
        Task<BrowserResultType> Logout(CancellationToken cancellationToken);

        /// <summary>
        /// Refreshes the users authentication token with the provided refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A refresh token response.</returns>
        Task<Result<RefreshTokenResult>> RefreshToken(string refreshToken, CancellationToken cancellationToken);
    }
}
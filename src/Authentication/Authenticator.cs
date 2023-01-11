using System.Reactive.Linq;
using System.Security.Claims;
using Authentication.User;
using LanguageExt;
using Rocket.Surgery.Airframe.Settings;
using static LanguageExt.Prelude;
using static Authentication.Validators;

namespace Authentication
{
    public class Authenticator : IAuthenticator
    {
        public Authenticator(IAuthenticationClient authenticationClient, ISettingsProvider settings)
        {
            _authenticationClient = authenticationClient;
            _settings = settings;
        }

        IObservable<Either<Error, UserAccountId>> IAuthenticator.GetUserAccountId(string userId)
        {
            return _settings.Observe<UserAccountId>(nameof(UserAccountId))
                            .Select(setting => ToEither(setting));

            Either<Error, UserAccountId> ToEither(ISetting<UserAccountId>? value)
                => !value.IsNull() && !value.Value.IsNull() && !value.Value.IsDefault()
                       ? Either<Error, UserAccountId>.Right(value.Value)
                       : Either<Error, UserAccountId>.Left(new Error());
        }

        IObservable<Either<Error, UserAccount>> IAuthenticator.GetUserAccount(UserAccountId userAccountId) => null;
        IObservable<Either<Error, AuthenticationRecord>> IAuthenticator.ValidateResult(AuthenticationResult result) { return null; }

        IObservable<Either<Error, UserAccountId>> IAuthenticator.GetCurrentUserId()
        {
            bool NotBad(ISetting<UserAccountId>? value) { return !value.IsNull() || !value.Value.IsNull() || !value.Value.IsDefault(); }
            return _settings
                   .Observe<UserAccountId>(nameof(UserAccountId))
                   .Select(setting => ToEither(setting));

            // TODO: [rlittlesii: January 03, 2023] Make this Validation<A, B>
            Either<Error, UserAccountId> ToEither(ISetting<UserAccountId>? value)
                => NotBad(value)
                       ? Right(value!.Value)
                       : Left(new Error());
        }

        IObservable<Either<Error, UserAccount>> IAuthenticator.Login(AuthenticationRequest request) =>
            Observable
                .FromAsync(async token => await _authenticationClient.Login(token))
                // TODO: [rlittlesii: January 04, 2023] Deconstruct this further using proper monadic types.
                .Select(result => result.Match(
                            Succ: authenticationResult =>
                                ValidateAuthenticationRecord(ValidateRecord(authenticationResult)),
                            Fail: _ => Either<Error, UserAccount>.Left(new Error())));

        private static UserAccount PersistRecord(AuthenticationRecord record) =>
            IAuthenticator.CreateUserAccount(record);

        private static Either<Error, UserAccount> ValidateAuthenticationRecord(Validation<Error, AuthenticationRecord> authenticationRecord) =>
            // TODO: [rlittlesii: January 03, 2023] This is where we left off.  We need the types to match properly.
            authenticationRecord
                .ToEither()
                .Map(record => PersistRecord(record))
                .MapLeft(seq => seq.Last);

        private static Validation<Error, ClaimsPrincipal> ValidateClaims(AuthenticationResult result) =>
            HasClaims(result.User)
                .Bind(identity => Optional(identity)
                          .ToValidation(new Error()));
        private static Validation<Error, Token> ValidateRefresh(AuthenticationResult result) =>
            Optional(result.RefreshToken)
                   .Map(token => new Token(token))
                   .ToValidation(new Error());

        private static Validation<Error, AccessToken> ValidateAccessToken(AuthenticationResult result) =>
            Optional(result)
                   // .Where(authenticationResult => !string.IsNullOrWhiteSpace(authenticationResult.AccessToken))
                   .Map(authenticationResult => new AccessToken(authenticationResult.AccessToken, authenticationResult.AccessTokenExpiration))
                   .ToValidation(new Error());

        private static Validation<Error, Token> ValidateIdentityToken(AuthenticationResult result) =>
            NotEmpty(result.IdentityToken)
                .Bind(identity => NotLongerThan()(identity));

        private static Validation<Error, AuthenticationRecord> ValidateRecord(AuthenticationResult result) =>
            (ValidateClaims(result), ValidateAccessToken(result), ValidateIdentityToken(result), ValidateRefresh(result))
            .Apply((principal, access, identity, refresh) =>
                       new AuthenticationRecord(principal,
                                                access,
                                                identity,
                                                refresh,
                                                result.AuthenticationTime));

        private readonly IAuthenticationClient _authenticationClient;
        private readonly ISettingsProvider _settings;
    }
}
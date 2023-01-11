using System.Security.Claims;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;
using static Authentication.Validators;

namespace Authentication.User
{
    public class UserAccount : Record<UserAccount>
    {
        public UserAccount(UserAccountId accountId,
                           AuthenticationRecord? authenticationRecord = null,
                           ExpiringRefreshToken? expiringRefreshToken = null)
        {
            AccountId = accountId;
            LastLogin = authenticationRecord;
            RefreshToken = expiringRefreshToken;
        }

        public readonly UserAccountId AccountId;
        public readonly AuthenticationRecord? LastLogin;
        public readonly ExpiringRefreshToken? RefreshToken;

        public static readonly UserAccount New = new UserAccount(new UserAccountId(Guid.NewGuid()
                                                                     .ToString()));

        public Either<Error, UserAccount> CreateAccount(string userAccountId) =>
            string.IsNullOrWhiteSpace(userAccountId)
                ? Left(new Error())
                : Right(With(new UserAccountId(userAccountId)));

        public Either<Error, UserAccount> AddAccessToken(Result<AuthenticationResult> authenticationResult)
        {
            var lastLogin =
                authenticationResult
                    .Match(Succ: result => Optional(result)
                                           .Map(x => ValidateRecord(x))
                                           .ToValidation(new Error()),
                           Fail: exception => new Error())
                    .Bind(validationData => validationData.Success)
                    .Map(validationData => validationData.Success)
                    .Last();

            return authenticationResult is { IsFaulted: true }
                       ? Left(new Error())
                       : Right(With(AccountId, lastLogin));
        }
        //
        // private static Either<Error, UserAccount> ValidateAuthenticationRecord(Validation<Error, AuthenticationRecord> authenticationRecord) =>
        //     // TODO: [rlittlesii: January 03, 2023] This is where we left off.  We need the types to match properly.
        //     authenticationRecord
        //         .ToEither()
        //         .Map(record => record)
        //         .MapLeft(seq => seq.Last);

        private static Validation<Error, ClaimsPrincipal> ValidateClaims(AuthenticationResult result) =>
            HasClaims(result.User)
                .Bind(identity => Optional(identity)
                          .ToValidation(new Error()));

        private static Validation<Error, Token> ValidateRefresh(AuthenticationResult result) =>
            Optional(result.RefreshToken)
                .Filter(x => !string.IsNullOrWhiteSpace(x))
                .Match(Some: token => new Token(token),
                       None: () => new Token());

        private static Validation<Error, AccessToken> ValidateAccessToken(AuthenticationResult result) =>
            Optional(result)
                // .Where(authenticationResult => !string.IsNullOrWhiteSpace(authenticationResult.AccessToken))
                .Map(authenticationResult =>
                         new AccessToken(authenticationResult.AccessToken, authenticationResult.AccessTokenExpiration))
                .ToValidation(new Error());

        private static Validation<Error, Token> ValidateIdentityToken(AuthenticationResult result) =>
            NotEmpty(result.IdentityToken)
                .Bind(identity => NotLongerThan()(identity));

        private static Validation<Error, AuthenticationRecord> ValidateRecord(AuthenticationResult result)
        {
            var valueTuple = (ValidateClaims(result), ValidateAccessToken(result), ValidateIdentityToken(result),
                              ValidateRefresh(result));
            return valueTuple
                .Apply((principal,
                        access,
                        identity,
                        refresh) =>
                           new AuthenticationRecord(principal,
                                                    access,
                                                    identity,
                                                    refresh,
                                                    result.AuthenticationTime));
        }

        /// <summary>
        /// Record 'mutation'
        /// </summary>
        UserAccount With(
            UserAccountId accountId,
            AuthenticationRecord? lastLogin = null,
            ExpiringRefreshToken? account = null) =>
            new UserAccount(
                accountId,
                lastLogin,
                account);
    }
}
using System.Reactive.Linq;
using Authentication.User;
using LanguageExt;
using ReactiveMarbles.Extensions;

namespace Authentication
{
    public interface IAuthenticator
    {
        IObservable<System.Reactive.Unit> Login() =>
            Login(new AuthenticationRequest())
                .Select(either => either.ToOption())
                .Where(option => option.IsSome)
                .AsSignal();

        IObservable<Option<UserAccount>> CurrentUser() =>
            GetCurrentUserId()
                .Select(x => new Option<UserAccount>());

        IObservable<Either<Error, UserAccountId>> GetUserAccountId(string userId);
        IObservable<Either<Error, UserAccount>> GetUserAccount(UserAccountId userAccountId);
        IObservable<Either<Error, AuthenticationRecord>> ValidateResult(AuthenticationResult result);
        IObservable<Either<Error, UserAccountId>> GetCurrentUserId();
        IObservable<Either<Error, UserAccount>> Login(AuthenticationRequest request);

        protected static AuthenticationRecord CreateAccessRecord(AuthenticationResult result) =>
            new AuthenticationRecord(result.User,
                                     new AccessToken(result.AccessToken, result.AccessTokenExpiration),
                                     new Token(result.IdentityToken),
                                     new Token(result.RefreshToken ?? string.Empty),
                                     result?.AuthenticationTime);

        protected static UserAccount CreateUserAccount(AuthenticationRecord record) =>
            new UserAccount(new UserAccountId(record.Claims.FirstOrDefault(x => x.Type == "sid")
                                                    .Value));
    }
}
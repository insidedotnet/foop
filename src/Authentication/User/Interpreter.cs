using LanguageExt;
using static LanguageExt.Prelude;

namespace Authentication.User
{
    public static class Interpreter
    {
        public static Either<Error, (A, UserAccount)> Interpret<A>(UserAccountOperations<A> dsl,
                                                                   UserAccount userAccount) =>
            dsl switch
            {
                UserAccountOperations<A>.Return @return =>
                    Right<Error, (A, UserAccount)>((@return.Value, userAccount)),

                UserAccountOperations<A>.CreateAccount createAccount =>
                    CreateAccountFromId(createAccount, userAccount),

                UserAccountOperations<A>.AddAccessToken createAccount =>
                    AddAccessToken(createAccount, userAccount),

                _ => throw new ArgumentOutOfRangeException(nameof(dsl))
            };

        private static Either<Error, (A, UserAccount)> CreateAccountFromId<A>(
            UserAccountOperations<A>.CreateAccount action,
            UserAccount userAccount) =>
            userAccount.CreateAccount(action.UserId)
                       .Bind(account => Interpret(action.Next(action.UserId), account));

        private static Either<Error, (A, UserAccount)> AddAccessToken<A>(
            UserAccountOperations<A>.AddAccessToken action,
            UserAccount userAccount) =>
            userAccount.AddAccessToken(action.AccessResult)
                       .Bind(account => Interpret(action.Next(action.AccessResult), account));
    }
}
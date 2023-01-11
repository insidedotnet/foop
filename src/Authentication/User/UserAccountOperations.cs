namespace Authentication.User
{
    public static class UserAccountOperations
    {
        public static UserAccountOperations<A> Return<A>(A value) => new UserAccountOperations<A>.Return(value);

        public static UserAccountOperations<string> CreateAccount(string userId) =>
            new UserAccountOperations<string>.CreateAccount(userId, Return);

        public static UserAccountOperations<AuthenticationResult> AddAccessToken(AuthenticationResult authenticationResult) =>
            new UserAccountOperations<AuthenticationResult>.AddAccessToken(authenticationResult, Return);
    }

    public abstract class UserAccountOperations<A>
    {
        public static UserAccountOperations<A> Returns(A value) => new Return(value);

        /// <summary>
        /// Identity type - simply returns the value provided
        /// </summary>
        public class Return : UserAccountOperations<A>
        {
            public readonly A Value;

            public Return(A value) =>
                Value = value;
        }

        /// <summary>
        /// Represents an operation that creates a new user account
        /// </summary>
        public class CreateAccount : UserAccountOperations<A>
        {
            public CreateAccount(string userId, Func<string, UserAccountOperations<A>> next)
            {
                UserId = userId;
                Next = next;
            }

            public readonly string UserId;
            public readonly Func<string, UserAccountOperations<A>> Next;
        }

        /// <summary>
        /// Represents an operation that adds an access token to the user account.
        /// </summary>
        public class AddAccessToken : UserAccountOperations<A>
        {
            public AddAccessToken(AuthenticationResult accessResult,
                                  Func<AuthenticationResult, UserAccountOperations<A>> next)
            {
                AccessResult = accessResult;
                Next = next;
            }

            public readonly AuthenticationResult AccessResult;
            public readonly Func<AuthenticationResult, UserAccountOperations<A>> Next;
        }
    }
}
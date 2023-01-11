namespace Authentication.User
{
    public static class UserAccountOperationsExtensions
    {
        public static UserAccountOperations<B> Bind<A, B>(this UserAccountOperations<A> ma,
                                                          Func<A, UserAccountOperations<B>> function) =>
            ma switch
            {
                UserAccountOperations<A>.Return @return => function(@return.Value),
                UserAccountOperations<A>.CreateAccount createAccount =>
                    new UserAccountOperations<B>.CreateAccount(createAccount.UserId,
                                                                     accountId => createAccount.Next(accountId)
                                                                         .Bind(function)),
                UserAccountOperations<A>.AddAccessToken addAccessToken =>
                    new UserAccountOperations<B>.AddAccessToken(addAccessToken.AccessResult,
                                                                     accountId => addAccessToken.Next(accountId)
                                                                         .Bind(function)),
                _ => throw new ArgumentOutOfRangeException(nameof(ma))
            };

        public static UserAccountOperations<B> Select<A, B>(this UserAccountOperations<A> ma, Func<A, B> f) =>
            ma.Bind(a => UserAccountOperations.Return(f(a)));

        public static UserAccountOperations<C> SelectMany<A, B, C>(this UserAccountOperations<A> ma,
                                                                   Func<A, UserAccountOperations<B>> bind,
                                                                   Func<A, B, C> project) =>
            ma.Bind(a => bind(a)
                        .Select(b => project(a, b)));
    }
}
using LanguageExt;
using LanguageExt.ClassInstances.Const;
using LanguageExt.ClassInstances.Pred;

namespace Authentication.User
{
    public class UserAccountId : NewType<UserAccountId, string, StrLen<I1, I50>>
    {
        public UserAccountId() : this (Guid.NewGuid().ToString()) { }

        public UserAccountId(string value)
            : base(value) { }

        public static readonly UserAccountId Default = new UserAccountId("donotreply@syndicatefinder.com");
    }
}
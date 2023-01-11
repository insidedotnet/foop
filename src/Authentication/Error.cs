using System.Runtime.CompilerServices;
using LanguageExt;

namespace Authentication
{
    public class Error : NewType<Error, string>
    {
        public Error([CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
            : this(callerMemberName) { }

        public Error(string value)
            : base(value) { }
    }
}
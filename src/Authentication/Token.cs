using System.Runtime.Serialization;
using LanguageExt;
using LanguageExt.ClassInstances.Const;
using LanguageExt.ClassInstances.Pred;
using NodaTime;
using NodaTime.Extensions;

namespace Authentication
{
    public class Token : NewType<Token, string, StrLen<I1, I50>>
    {
        public Token() : this(nameof(Token)) {}

        public Token(string value)
            : base(string.IsNullOrWhiteSpace(value) ? nameof(Token) : value) { }

        public bool IsDefault() => Value == nameof(Token);
    }

    public class Expiration : NewType<Expiration, LocalDateTime>
    {
        public Expiration(DateTimeOffset value)
            : this(value.ToZonedDateTime()
                        .LocalDateTime) { }

        public Expiration(LocalDateTime value)
            : base(value) { }

        public Expiration(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public LocalDateTime TimeToExpire(LocalDateTime localDateTime) => Value;
    }
}
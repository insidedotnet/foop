using System.Security.Claims;
using LanguageExt;

namespace Authentication
{
    public static class Validators
    {
        public static Func<string, Validation<Error, Token>> NotLongerThan() => str =>
            OptionalString(str)
                .Where(s => s.Length > 0)
                .Map(x => Token.New(x))
                .ToValidation(new Error($"{str} must not be longer than"));

        public static Func<string, Validation<Error, string>> NotLongerThan(int maxLength) => str =>
            OptionalString(str)
                .Where(s => s.Length <= maxLength)
                .ToValidation(new Error($"{str} must not be longer than {maxLength}"));

        public static Validation<Error, ClaimsPrincipal> HasClaims(ClaimsPrincipal principal) =>
            Prelude.Optional(principal)
                   .Where(claimsPrincipal => claimsPrincipal.Claims.Any())
                   .ToValidation(new Error($"{principal} must not be longer than"));

        public static Validation<Error, string> NotEmpty(string str) =>
            OptionalString(str)
                .ToValidation(new Error("Empty string"));

        private static Option<string> OptionalString(string str) =>
            Prelude.Optional(str)
                   .Where(s => !string.IsNullOrWhiteSpace(s));
    }
}
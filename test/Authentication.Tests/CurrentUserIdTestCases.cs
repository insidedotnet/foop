using System.Collections;
using System.ComponentModel;
using System.Reactive.Linq;
using Authentication.User;
using Rocket.Surgery.Airframe.Settings;
using NSubstitute;

namespace Authentication.Tests
{
    internal class CurrentUserIdTestCases : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { GenerateSettings(new UserAccountId(Guid.NewGuid().ToString())), true };
            yield return new object[] { GenerateSettings(new UserAccountId("donotreply@syndicatefinder.com")), true };
            yield return new object[] { GenerateSettings(Empty), false };
        }

        private static ISettingsProvider GenerateSettings(UserAccountId record)
        {
            var settings = Substitute.For<ISettingsProvider>();
            settings.Observe<UserAccountId>(nameof(UserAccountId))
                    .Returns(Observable.Return(new Setting<UserAccountId>(record)));
            return settings;
        }

        private static UserAccountId Empty { get; } = new UserAccountId();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal record Setting<T> : ISetting<T>
    {
        public Setting(T record)
        {
            Key = typeof(T).Name;
            Value = record;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public string Key { get; }
        public T Value { get; set; }

        object ISetting.Value
        {
            get => Value;
            set => Value = (T) value;
        }
    }
}
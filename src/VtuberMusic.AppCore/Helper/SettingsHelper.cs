using Windows.Storage;

namespace VtuberMusic.AppCore.Helper {
    public class SettingsHelper {
        public static string RefreshToken {
            get => GetSetting<string>(nameof(RefreshToken));
            set => SetSetting(nameof(RefreshToken), value);
        }

        public static void SetSetting(string key, object value) {
            ApplicationData.Current.LocalSettings.Values[key] = value;
        }

        public static T GetSetting<T>(string key) {
            return (T)ApplicationData.Current.LocalSettings.Values[key];
        }

        public static object GetSetting(string key) {
            return ApplicationData.Current.LocalSettings.Values[key];
        }
    }
}

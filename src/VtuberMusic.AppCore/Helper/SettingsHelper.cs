using System.Runtime.CompilerServices;
using VtuberMusic.AppCore.Enums;
using Windows.Storage;

namespace VtuberMusic.AppCore.Helper;
public class SettingsHelper {
    public static string RefreshToken {
        get => GetSetting<string>();
        set => SetSetting(value);
    }

    public static DefaultNavigationPage DefaultNavigationPage {
        get => GetSetting<DefaultNavigationPage>(DefaultNavigationPage.Home);
        set => SetSetting(value);
    }

    public static void SetSetting(object value , [CallerMemberName] string key = "") => ApplicationData.Current.LocalSettings.Values[key] = value;

    public static T GetSetting<T>(T defaultValue , [CallerMemberName] string key = "") {
        if (ApplicationData.Current.LocalSettings.Values[key] is T data) {
            return data;
        } else {
            return defaultValue;
        }
    }

    public static T GetSetting<T>([CallerMemberName] string key = "") => (T)ApplicationData.Current.LocalSettings.Values[key];

    public static object GetSetting([CallerMemberName] string key = "") => ApplicationData.Current.LocalSettings.Values[key];
}

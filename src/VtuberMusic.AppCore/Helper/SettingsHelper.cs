using Newtonsoft.Json;
using System;
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

    public static void SetSetting(object value , [CallerMemberName] string key = "") {
        if (value is Enum) {
            ApplicationData.Current.LocalSettings.Values[key] = (int)value;
            return;
        }

        ApplicationData.Current.LocalSettings.Values[key] = value;
    }

    public static T GetSetting<T>(T defaultValue , [CallerMemberName] string key = "") {
        var raw = ApplicationData.Current.LocalSettings.Values[key];
        switch (raw) {
            case T:
                return (T)raw;
            case int:
                if (typeof(T).BaseType == typeof(Enum))
                    return (T)raw;
                break;
        }

        return default;
    }

    public static T GetSetting<T>([CallerMemberName] string key = "") => (T)ApplicationData.Current.LocalSettings.Values[key];

    public static object GetSetting([CallerMemberName] string key = "") => ApplicationData.Current.LocalSettings.Values[key];
}

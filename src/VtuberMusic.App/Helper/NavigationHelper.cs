using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using VtuberMusic.App.Services;

namespace VtuberMusic.App.Helper;
public class NavigationHelper {
    private static INavigationService _navigationService = Ioc.Default.GetRequiredService<INavigationService>();

    public static void Navigate(Type pageType, object arg) =>
        _navigationService.Navigate(pageType, arg);

    public static void Navigate<T>(object arg = null) =>
        _navigationService.Navigate<T>(arg);

    public static void RequestGoBack() =>
        _navigationService.RequestGoBack();
}

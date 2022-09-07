using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.Pages;
using VtuberMusic.App.Services;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.AppCore.Helper;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.Helper;
public class NavigationHelper {
    private static INavigationService _navigationService = Ioc.Default.GetRequiredService<INavigationService>();
    private static IVtuberMusicService _vtuberMusicService = Ioc.Default.GetRequiredService<IVtuberMusicService>();

    public static void Navigate(Type pageType, object arg) =>
        _navigationService.Navigate(pageType, arg);

    public static void Navigate<T>(object arg = null) =>
        _navigationService.Navigate<T>(arg);

    public static void ClearHistory() =>
        _navigationService.BackStack.Clear();

    public static void RequestGoBack() =>
        _navigationService.RequestGoBack();

    public static async void GoToHome() {
        switch (SettingsHelper.DefaultNavigationPage) {
            case DefaultNavigationPage.Home:
                _navigationService.Navigate<Discover>();
                break;
            case DefaultNavigationPage.Library:
                _navigationService.Navigate<Library>();
                break;
            case DefaultNavigationPage.LikeMusic:
                var playlist = await _vtuberMusicService.GetFavouriteMusicsPlaylist();
                _navigationService.Navigate<PlaylistPage>(new PlaylistPageArg { Playlist = playlist.Data.playlist, PlaylistType = PlaylistType.LikeMusics });
                break;
            default:
                _navigationService.Navigate<Discover>();
                break;
        }
    }
}

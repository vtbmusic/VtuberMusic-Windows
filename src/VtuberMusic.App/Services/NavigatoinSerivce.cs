using Microsoft.AppCenter.Analytics;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.Pages;
using VtuberMusic.Core.Models;

namespace VtuberMusic.App.Services;
public class NavigatoinSerivce : INavigationService {
    public Frame Frame { get; private set; }
    public bool CanGoBack => this.Frame.CanGoBack;
    public IList<PageStackEntry> BackStack => this.Frame.BackStack;

    public event NavigatedEventHandler Navigated;
    public event NavigatingCancelEventHandler Navigating;
    public event NavigationFailedEventHandler NavigationFailed;
    public event NavigationStoppedEventHandler NavigationStopped;

    public void RequestGoBack() => this.Frame.GoBack();

    public void Navigate(Type pageType, object arg) => this.Frame.Navigate(pageType, arg);

    public void Navigate<T>(object arg = null) => this.Frame.Navigate(typeof(T), arg);

    public void SetContentFrame(Frame frame) {
        this.Frame = frame;
        frame.Navigated += Frame_Navigated;
        frame.Navigating += Frame_Navigating;
        frame.NavigationFailed += Frame_NavigationFailed;
        frame.NavigationStopped += Frame_NavigationStopped;
    }

    private void Frame_NavigationStopped(object sender, NavigationEventArgs e) => NavigationStopped?.Invoke(this, e);

    private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e) => NavigationFailed?.Invoke(this, e);

    private void Frame_Navigating(object sender, NavigatingCancelEventArgs e) => Navigating?.Invoke(this, e);

    private void Frame_Navigated(object sender, NavigationEventArgs e) {
        Navigated?.Invoke(this, e);
        switch (e.Content) {
            case Discover:
                Analytics.TrackEvent("浏览发现页");
                break;
            case PlaylistPage:
                var playlistPageArg = e.Parameter as PlaylistPageArg;
                Analytics.TrackEvent("浏览歌单", new Dictionary<string, string> {
                    { "playlist_id", playlistPageArg.Playlist.id },
                    { "type", playlistPageArg.PlaylistType.ToString() }
                });

                break;
            case ArtistPage:
                var artistPageArg = e.Parameter as ArtistPageArg;
                Analytics.TrackEvent("浏览歌手", new Dictionary<string, string> {
                    { "artist_id", artistPageArg.Artist.id }
                });

                break;
            case Friends:
                var friendsPageArg = e.Parameter as FriendsPageArg;
                Analytics.TrackEvent("浏览好友页", new Dictionary<string, string> {
                    { "user_id", friendsPageArg.Profile.userId },
                    { "type", friendsPageArg.Type.ToString() }
                });

                break;
            case Library:
                Analytics.TrackEvent("浏览资料库");
                break;
            case CommentPage:
                var commentPageArg = e.Parameter as CommentPageArg;
                var commentCollectData = new Dictionary<string, string>();

                if (commentPageArg.Music is Music commentMusic)
                    commentCollectData.Add("music_id", commentMusic.id);
                if (commentPageArg.Playlist is Playlist commentPlaylist)
                    commentCollectData.Add("playlist_id", commentPlaylist.id);

                Analytics.TrackEvent("浏览评论", commentCollectData);
                break;
            case ProfilePage:
                var profilePageArg = e.Parameter as ProfilePageArg;
                Analytics.TrackEvent("浏览用户资料", new Dictionary<string, string> {
                    { "profile_id", profilePageArg.Profile.userId }
                });

                break;
            case Search:
                var searchPageArg = e.Parameter as SearchPageArg;
                Analytics.TrackEvent("搜索", new Dictionary<string, string> {
                    { "keyword", searchPageArg.Keyword }
                });

                break;
        }
    }
}

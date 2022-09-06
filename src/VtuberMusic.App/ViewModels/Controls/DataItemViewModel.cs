using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using VtuberMusic.App.Helper;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.Pages;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.AppCore.Services;
using VtuberMusic.Core.Models;
using Windows.ApplicationModel.DataTransfer;

namespace VtuberMusic.App.ViewModels.Controls;
public partial class DataItemViewModel : ObservableObject {
    private readonly IMediaPlayBackService _mediaPlaybackService;

    public DataItemViewModel(IMediaPlayBackService mediaPlaybackService) {
        _mediaPlaybackService = mediaPlaybackService;
    }

    [RelayCommand]
    public void NavigateToPlaylist(Playlist playlist) =>
    NavigationHelper.Navigate<PlaylistPage>(new PlaylistPageArg { Playlist = playlist, PlaylistType = PlaylistType.Playlist });

    [RelayCommand]
    public void NavigateToArtist(Artist artist) =>
        NavigationHelper.Navigate<ArtistPage>(new ArtistPageArg { Artist = artist });

    [RelayCommand]
    public void NavigateToProfile(Profile profile) =>
        NavigationHelper.Navigate<ProfilePage>(new ProfilePageArg { Profile = profile });

    [RelayCommand]
    public void SetMusic(Music music) => _mediaPlaybackService.SetMusic(music);

    [RelayCommand]
    public void SetNextMusic(Music music) => _mediaPlaybackService.SetNextMusic(music);

    [RelayCommand]
    public void SetCollection(IEnumerable<Music> collection) {
        _mediaPlaybackService.SetCollection(collection);
    }

    [RelayCommand]
    public void CopyLink(object arg) {
        DataPackage data = new();
        if (arg is Music) {
            data.SetText($"https://vtbmusic.com/song?id={(arg as Music).id}");
        } else if (arg is Artist) {
            data.SetText($"https://vtbmusic.com/vtuber?id={(arg as Artist).id}");
        } else if (arg is Playlist) {
            data.SetText($"https://vtbmusic.com/songlist?id={(arg as Playlist).id}");
        }

        Clipboard.SetContent(data);
    }

    [RelayCommand]
    public void Share(object arg) {
        if (arg is Music) {
            ShareHelper.ShareMusic(arg as Music);
        } else if (arg is Artist) {
            ShareHelper.ShareArtist(arg as Artist);
        } else if (arg is Playlist) {
            ShareHelper.SharePlaylist(arg as Playlist);
        }
    }
}

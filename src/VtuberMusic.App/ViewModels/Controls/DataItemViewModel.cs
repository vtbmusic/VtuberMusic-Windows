using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VtuberMusic.App.Helper;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.Pages;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.AppCore.Services;
using VtuberMusic.Core.Enums;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;
using Windows.ApplicationModel.DataTransfer;

namespace VtuberMusic.App.ViewModels.Controls;
public partial class DataItemViewModel : ObservableObject {
    private readonly IMediaPlayBackService _mediaPlaybackService;
    private readonly IVtuberMusicService _vtuberMusicService;
    private readonly IAuthorizationService _authorizationService;

    [ObservableProperty]
    private bool canEdit;

    public DataItemViewModel(IMediaPlayBackService mediaPlaybackService, IVtuberMusicService vtuberMusicService, IAuthorizationService authorizationService) {
        _mediaPlaybackService = mediaPlaybackService;
        _vtuberMusicService = vtuberMusicService;
        _authorizationService = authorizationService;
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

    public async Task RemoveMusicFromPlaylist(string playlistId, string[] musicIds, bool isFavoriteMusic = false) {
        if (isFavoriteMusic) {
            Array.ForEach(musicIds, async (item) => await _vtuberMusicService.LikeMusic(item, false));
        } else {
            await _vtuberMusicService.TrackMusic(playlistId, TrackType.del, musicIds);
        }
    }

    public bool UpdatePlaylistCanEdit(Playlist playlist, PlaylistType type) {
        if (playlist != null) {
            this.CanEdit = type == PlaylistType.Playlist && playlist.creator.userId == _authorizationService.Account.id;
        } else {
            this.CanEdit = false;
        }

        return this.CanEdit;
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.App.Helper;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.Pages;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.AppCore.Helper;
using VtuberMusic.AppCore.Services;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;
using Windows.ApplicationModel.DataTransfer;

namespace VtuberMusic.App.ViewModels;
public partial class PlaylistPageViewModel : ObservableObject {
    private readonly IVtuberMusicService _vtuberMusicService = Ioc.Default.GetService<IVtuberMusicService>();
    private readonly IMediaPlayBackService _mediaPlayBackService = Ioc.Default.GetService<IMediaPlayBackService>();

    [ObservableProperty]
    private Playlist playlist;

    public PlaylistType PlaylistType { get; set; }
    [ObservableProperty]
    private ObservableCollection<Music> playlistMusics = new();

    public PlaylistPageViewModel() {
    }

    [RelayCommand]
    public async Task Load() {
        PlaylistMusicsResponse playlistResponse = null;
        switch (this.PlaylistType) {
            case PlaylistType.Playlist:
                playlistResponse = (await _vtuberMusicService.GetPlaylistMusics(this.Playlist.id)).Data;
                break;
            case PlaylistType.LikeMusics:
                playlistResponse = (await _vtuberMusicService.GetFavouriteMusicsPlaylist("1", this.Playlist.id)).Data;
                break;
            case PlaylistType.Personalized:
                playlistResponse = (await _vtuberMusicService.GetDailyPersonalizedMusic()).Data;
                break;
        }

        this.Playlist = playlistResponse.playlist;
        this.PlaylistMusics.Clear();
        foreach (var item in playlistResponse.songs) {
            this.PlaylistMusics.Add(item);
        }
    }

    [RelayCommand]
    public void SetCollection(IEnumerable<Music> collection) => _mediaPlayBackService.SetCollection(collection);

    [RelayCommand]
    public void CopyLink(object arg) {
        DataPackage dataPackage = new();
        DataPackage data = dataPackage;
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

    [RelayCommand]
    public void NavigateToProfile(Profile profile) =>
        NavigationHelper.Navigate<ProfilePage>(new ProfilePageArg { Profile = profile });
}

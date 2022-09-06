using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.Pages;
public partial class ProfilePageViewModel : ObservableObject {
    private readonly IVtuberMusicService _vtuberMusicService;

    [ObservableProperty]
    private ObservableCollection<Playlist> createPlaylist = new();
    [ObservableProperty]
    private ObservableCollection<Playlist> subPlaylist = new();

    [ObservableProperty]
    private Profile profile;

    [ObservableProperty]
    private Playlist favouritePlaylist;

    [ObservableProperty]
    private bool isFollwed;

    public ProfilePageViewModel(IVtuberMusicService vtuberMusicService) {
        _vtuberMusicService = vtuberMusicService;
    }

    [RelayCommand]
    private async Task Load() {
        this.Profile = (await _vtuberMusicService.GetProfile(this.Profile.userId)).Data.profile;
        this.IsFollwed = this.Profile.followed;

        var favouritePlaylistResponse = await _vtuberMusicService.GetFavouriteMusicsPlaylist("song", this.Profile.userId);
        var subPlaylistResponse = await _vtuberMusicService.GetSubPlaylist(this.Profile.userId);
        var createPlaylistResponse = await _vtuberMusicService.GetCreatePlaylist(this.Profile.userId);

        this.SubPlaylist.Clear();
        foreach (var item in subPlaylistResponse.Data) {
            this.SubPlaylist.Add(item);
        }

        this.CreatePlaylist.Clear();
        foreach (var item in createPlaylistResponse.Data) {
            this.CreatePlaylist.Add(item);
        }

        this.FavouritePlaylist = favouritePlaylistResponse.Data.playlist;
    }

    [RelayCommand]
    private async Task Follow() {
        if (this.Profile.followed) {
            await _vtuberMusicService.Follow(this.Profile.userId, "d");
            this.Profile.followed = false;
        } else {
            await _vtuberMusicService.Follow(this.Profile.userId);
            this.Profile.followed = true;
        }

        this.IsFollwed = this.Profile.followed;
    }
}

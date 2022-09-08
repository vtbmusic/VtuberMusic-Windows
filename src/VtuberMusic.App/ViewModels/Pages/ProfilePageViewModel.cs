using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.App.Messages;
using VtuberMusic.Core.Enums;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.Pages;
public partial class ProfilePageViewModel : ObservableRecipient {
    private readonly IVtuberMusicService _vtuberMusicService;
    private readonly IAuthorizationService _authorizationService;

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
    [ObservableProperty]
    private bool isAllFollwed;

    [ObservableProperty]
    private bool isMan;
    [ObservableProperty]
    private bool isWoman;
    [ObservableProperty]
    private bool isUnknowGender;

    public ProfilePageViewModel(IVtuberMusicService vtuberMusicService, IAuthorizationService authorizationService) {
        _vtuberMusicService = vtuberMusicService;
        _authorizationService = authorizationService;
    }

    protected override void OnActivated() {
        if (this.Profile.userId == _authorizationService.Account.id)
            WeakReferenceMessenger.Default.Register(this, async (object sender, UserPlaylistsChangedMessage message) => await Load());
    }

    [RelayCommand]
    private async Task Load() {
        this.Profile = (await _vtuberMusicService.GetProfile(this.Profile.userId)).Data.profile;
        this.IsAllFollwed = this.Profile.allfollowed;
        this.IsFollwed = this.Profile.followed;
        this.IsMan = this.Profile.gender == ProfileGender.Man;
        this.IsWoman = this.Profile.gender == ProfileGender.Woman;
        this.IsUnknowGender = this.Profile.gender == ProfileGender.Unknow;

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
        if (!this.Profile.followed) {
            await _vtuberMusicService.Follow(this.Profile.userId, "d");
            this.Profile.followed = true;
            this.IsAllFollwed = this.Profile.allfollowed;
        } else {
            await _vtuberMusicService.Follow(this.Profile.userId);
            this.Profile.followed = false;
            this.IsAllFollwed = false;
        }

        this.IsFollwed = this.Profile.followed;
    }
}

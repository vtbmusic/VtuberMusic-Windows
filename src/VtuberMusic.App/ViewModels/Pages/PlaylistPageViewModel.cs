using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.App.Helper;
using VtuberMusic.App.Messages;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.Pages;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.AppCore.Services;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;
using Windows.ApplicationModel.DataTransfer;

namespace VtuberMusic.App.ViewModels.Pages;
public partial class PlaylistPageViewModel : ObservableRecipient {
    private readonly IVtuberMusicService _vtuberMusicService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IMediaPlayBackService _mediaPlayBackService;

    [ObservableProperty]
    private Playlist playlist;

    [ObservableProperty]
    private bool canRemoveMusic;

    [ObservableProperty]
    private bool canEdit;

    [ObservableProperty]
    private bool canSubscribe;

    [ObservableProperty]
    private bool subscribed;

    public PlaylistType PlaylistType { get; set; }
    [ObservableProperty]
    private ObservableCollection<Music> playlistMusics = new();

    public PlaylistPageViewModel(IVtuberMusicService vtuberMusicService, IMediaPlayBackService mediaPlayBackService, IAuthorizationService authorizationService) {
        _vtuberMusicService = vtuberMusicService;
        _mediaPlayBackService = mediaPlayBackService;
        _authorizationService = authorizationService;
    }

    protected override void OnActivated() {
        WeakReferenceMessenger.Default.Register(this, async (object sender, PlaylistMusicChangedMessage message) => {
            await Task.Delay(500); // 等个 500ms，因为后端更新歌单有延迟
            await Load();
        });

        WeakReferenceMessenger.Default.Register(this, async (object sender, UserPlaylistsChangedMessage message) => {
            try {
                await Load();
            } catch (InvalidOperationException) {
                NavigationHelper.GoToHome();
                NavigationHelper.ClearHistory();
            }
        });
    }

    protected override void OnDeactivated() => WeakReferenceMessenger.Default.UnregisterAll(this);

    [RelayCommand]
    public async Task Load() {
        this.PlaylistMusics.CollectionChanged -= PlaylistMusics_CollectionChanged;

        ApiResponse<PlaylistMusicsResponse> playlistResponse = null;
        switch (this.PlaylistType) {
            case PlaylistType.Playlist:
                playlistResponse = (await _vtuberMusicService.GetPlaylistMusics(this.Playlist.id));
                break;
            case PlaylistType.LikeMusics:
                playlistResponse = (await _vtuberMusicService.GetFavouriteMusicsPlaylist("1", this.Playlist.id));
                break;
            case PlaylistType.Personalized:
                playlistResponse = (await _vtuberMusicService.GetDailyPersonalizedMusic());
                break;
        }

        if (!playlistResponse.Success || playlistResponse.Data.playlist == null) {
            throw new InvalidOperationException("歌单不存在");
        }

        this.Playlist = playlistResponse.Data.playlist;
        this.Subscribed = playlistResponse.Data.playlist.like;
        this.CanRemoveMusic = this.Playlist.creator.userId == _authorizationService.Account.id;
        this.CanEdit = this.Playlist.creator.userId == _authorizationService.Account.id && this.PlaylistType == PlaylistType.Playlist;
        this.CanSubscribe = this.PlaylistType == PlaylistType.Playlist;
        this.PlaylistMusics.Clear();
        foreach (var item in playlistResponse.Data.songs) {
            this.PlaylistMusics.Add(item);
        }

        this.PlaylistMusics.CollectionChanged += PlaylistMusics_CollectionChanged;
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

    [RelayCommand]
    public async Task SubscribePlaylist() {
        await _vtuberMusicService.SubscribePlaylist(this.Playlist.id, !this.Playlist.like);
        WeakReferenceMessenger.Default.Send(new UserPlaylistsChangedMessage());
    }

    public async Task UpdateOrder() {
        var musicIds = new List<string>();
        foreach (var item in this.PlaylistMusics) {
            musicIds.Add(item.id);
        }

        var response = await _vtuberMusicService.ReorderPlaylistMusic(this.Playlist.id, musicIds.ToArray());
    }

    private async void PlaylistMusics_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) =>
        await UpdateOrder();
}

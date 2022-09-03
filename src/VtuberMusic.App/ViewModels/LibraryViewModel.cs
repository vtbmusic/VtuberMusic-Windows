using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels;
public partial class LibraryViewModel : AppViewModel {
    private readonly IVtuberMusicService _vtuberMusicService = Ioc.Default.GetService<IVtuberMusicService>();
    private readonly IAuthorizationService _authorizationService = Ioc.Default.GetService<IAuthorizationService>();

    public IAsyncRelayCommand LoadCommand { get; }

    public ObservableCollection<Music> PersonalizedMusic = new();
    public ObservableCollection<Playlist> CreatePlaylist = new();
    public ObservableCollection<Playlist> SubPlaylist = new();

    public Playlist DailyRecommenderPlaylist { get => dailyRecommenderPlaylist; set => SetProperty(ref dailyRecommenderPlaylist, value); }
    private Playlist dailyRecommenderPlaylist;
    public Playlist FavouritePlaylist { get => favouritePlaylist; set => SetProperty(ref favouritePlaylist, value); }
    private Playlist favouritePlaylist;

    public Music PersonalizedFirstMusic { get => personalizedFirstMusic; set => SetProperty(ref personalizedFirstMusic, value); }
    private Music personalizedFirstMusic;

    [ObservableProperty]
    private Profile profile;

    public LibraryViewModel() {
        LoadCommand = new AsyncRelayCommand(LoadDataAsync);
    }

    private async Task LoadDataAsync() {
        try {
            var dailyRecommenderResponse = await _vtuberMusicService.GetDailyPersonalizedMusic();
            var personalizedMusicResponse = await _vtuberMusicService.GetPersonalizedMusic();
            var favouritePlaylistResponse = await _vtuberMusicService.GetFavouriteMusicsPlaylist("song");
            var subPlaylistResponse = await _vtuberMusicService.GetSubPlaylist();
            var createPlaylistResponse = await _vtuberMusicService.GetCreatePlaylist();

            PersonalizedMusic.Clear();
            foreach (var item in personalizedMusicResponse.Data) {
                PersonalizedMusic.Add(item);
            }

            SubPlaylist.Clear();
            foreach (var item in subPlaylistResponse.Data) {
                SubPlaylist.Add(item);
            }

            CreatePlaylist.Clear();
            foreach (var item in createPlaylistResponse.Data) {
                CreatePlaylist.Add(item);
            }

            PersonalizedFirstMusic = PersonalizedMusic.FirstOrDefault();
            DailyRecommenderPlaylist = dailyRecommenderResponse.Data.playlist;
            FavouritePlaylist = favouritePlaylistResponse.Data.playlist;
            Profile = _authorizationService.Profile;
        } catch {
        }
    }
}

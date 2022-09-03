using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VtuberMusic.App.Helper;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.Pages;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels;
public partial class LibraryViewModel : ObservableObject {
    private readonly IVtuberMusicService _vtuberMusicService = Ioc.Default.GetService<IVtuberMusicService>();
    private readonly IAuthorizationService _authorizationService = Ioc.Default.GetService<IAuthorizationService>();

    [ObservableProperty]
    private ObservableCollection<Music> personalizedMusic = new ObservableCollection<Music>();
    [ObservableProperty]
    private ObservableCollection<Playlist> createPlaylist = new ObservableCollection<Playlist>();
    [ObservableProperty]
    private ObservableCollection<Playlist> subPlaylist = new ObservableCollection<Playlist>();

    [ObservableProperty]
    private Playlist dailyRecommenderPlaylist;

    [ObservableProperty]
    private Playlist favouritePlaylist;

    [ObservableProperty]
    private Music personalizedFirstMusic;

    [ObservableProperty]
    private Profile profile;

    public LibraryViewModel() {
    }

    [RelayCommand]
    public void NavigateToProfile(Profile profile) =>
    NavigationHelper.Navigate<ProfilePage>(new ProfilePageArg { Profile = profile });

    [RelayCommand]
    private async Task Load() {
        try {
            var dailyRecommenderResponse = await _vtuberMusicService.GetDailyPersonalizedMusic();
            var personalizedMusicResponse = await _vtuberMusicService.GetPersonalizedMusic();
            var favouritePlaylistResponse = await _vtuberMusicService.GetFavouriteMusicsPlaylist("song");
            var subPlaylistResponse = await _vtuberMusicService.GetSubPlaylist();
            var createPlaylistResponse = await _vtuberMusicService.GetCreatePlaylist();

            this.PersonalizedMusic.Clear();
            foreach (var item in personalizedMusicResponse.Data) {
                this.PersonalizedMusic.Add(item);
            }

            this.SubPlaylist.Clear();
            foreach (var item in subPlaylistResponse.Data) {
                this.SubPlaylist.Add(item);
            }

            this.CreatePlaylist.Clear();
            foreach (var item in createPlaylistResponse.Data) {
                this.CreatePlaylist.Add(item);
            }

            this.PersonalizedFirstMusic = this.PersonalizedMusic.FirstOrDefault();
            this.DailyRecommenderPlaylist = dailyRecommenderResponse.Data.playlist;
            this.FavouritePlaylist = favouritePlaylistResponse.Data.playlist;
            this.Profile = _authorizationService.Profile;
        } catch {
        }
    }
}

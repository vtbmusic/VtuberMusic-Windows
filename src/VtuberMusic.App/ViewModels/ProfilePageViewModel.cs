using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels {
    public class ProfilePageViewModel : AppViewModel {
        private readonly IVtuberMusicService _vtuberMusicService = Ioc.Default.GetService<IVtuberMusicService>();

        public IAsyncRelayCommand LoadCommand { get; }

        public ObservableCollection<Playlist> CreatePlaylist = new ObservableCollection<Playlist>();
        public ObservableCollection<Playlist> SubPlaylist = new ObservableCollection<Playlist>();
        public Profile Profile { get => profile; set => SetProperty(ref profile, value); }
        private Profile profile;
        public Playlist FavouritePlaylist { get => favouritePlaylist; set => SetProperty(ref favouritePlaylist, value); }
        private Playlist favouritePlaylist;

        public ProfilePageViewModel() {
            LoadCommand = new AsyncRelayCommand(LoadDataAsync);
        }

        private async Task LoadDataAsync() {
            Profile = (await _vtuberMusicService.GetProfile(Profile.userId)).Data.profile;

            var favouritePlaylistResponse = await _vtuberMusicService.GetFavouriteMusicsPlaylist("song", Profile.userId);
            var subPlaylistResponse = await _vtuberMusicService.GetSubPlaylist(Profile.userId);
            var createPlaylistResponse = await _vtuberMusicService.GetCreatePlaylist(Profile.userId);

            SubPlaylist.Clear();
            foreach (var item in subPlaylistResponse.Data) SubPlaylist.Add(item);

            CreatePlaylist.Clear();
            foreach (var item in createPlaylistResponse.Data) CreatePlaylist.Add(item);

            FavouritePlaylist = favouritePlaylistResponse.Data.playlist;
        }
    }
}

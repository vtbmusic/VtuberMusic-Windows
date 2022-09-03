using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels {
    public class PlaylistPageViewModel : AppViewModel {
        private readonly IVtuberMusicService _vtuberMusicService = Ioc.Default.GetService<IVtuberMusicService>();

        public IAsyncRelayCommand LoadCommand { get; }

        public Playlist Playlist { get => playlist; set => SetProperty(ref playlist, value); }
        private Playlist playlist;

        public PlaylistType PlaylistType { get; set; }
        public ObservableCollection<Music> PlaylistMusics = new ObservableCollection<Music>();

        public PlaylistPageViewModel() {
            LoadCommand = new AsyncRelayCommand(LoadDataAsync);
        }

        private async Task LoadDataAsync() {
            PlaylistMusicsResponse playlistResponse = null;
            switch (PlaylistType) {
                case PlaylistType.Playlist:
                    playlistResponse = (await _vtuberMusicService.GetPlaylistMusics(Playlist.id)).Data;
                    break;
                case PlaylistType.LikeMusics:
                    playlistResponse = (await _vtuberMusicService.GetFavouriteMusicsPlaylist("1", Playlist.id)).Data;
                    break;
                case PlaylistType.Personalized:
                    playlistResponse = (await _vtuberMusicService.GetDailyPersonalizedMusic()).Data;
                    break;
            }

            Playlist = playlistResponse.playlist;
            PlaylistMusics.Clear();
            foreach (var item in playlistResponse.songs) PlaylistMusics.Add(item);
        }
    }
}
